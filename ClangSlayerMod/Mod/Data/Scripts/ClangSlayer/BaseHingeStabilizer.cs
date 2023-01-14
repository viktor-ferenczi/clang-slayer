using System;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace ClangSlayer
{
    public class BaseHingeStabilizer : MyGameLogicComponent
    {
        protected IMyMotorStator stator;
        private Derivative angle;
        private float previousTargetVelocityRad;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            stator = Entity as IMyMotorStator;
            angle = new Derivative(stator.Angle);
        }

        public override void Close()
        {
            stator = null;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (stator?.Top == null || stator.Closed || stator.CubeGrid?.Physics == null)
            {
                return;
            }
            
            if (!stator.IsWorking || stator.RotorLock || stator.TargetVelocityRad == 0f || 
                Math.Abs(stator.TargetVelocityRad - previousTargetVelocityRad) > 1e-6 ||
                stator.TargetVelocityRad < 0f && stator.LowerLimitRad < -314 && stator.Angle <= stator.LowerLimitRad + 1e-6 ||
                stator.TargetVelocityRad > 0f && stator.UpperLimitRad > +314 && stator.Angle >= stator.UpperLimitRad - 1e-6)
            {
                angle.Reset(stator.Angle);
                previousTargetVelocityRad = stator.TargetVelocityRad;
                return;
            }
            
            angle.Update(stator.Angle);
            
            // Is the rotor/hinge stuck or forced backwards?
            if (angle.Valid && Math.Abs(stator.TargetVelocityRad) > 0.031415)
            {
                var angularVelocity = angle.Dt;
                var angularVelocityError = Math.Abs((angularVelocity - stator.TargetVelocityRad) / stator.TargetVelocityRad);
                if (angularVelocityError > 0.9)
                {
                    MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(stator)}: Rotor is stuck or forced. angularVelocityError={100.0 * angularVelocityError:0.00}%");
                    stator.RotorLock = true;
                    stator.TargetVelocityRad = 0;
                    return;
                }
            }
            
            // Is the rotor/hinge part displaced by clang forces?
            var baseToTop = GetBaseToTopMatrixD();
            var expectedTopPose = baseToTop * stator.WorldMatrix;
            var actualTopPose = stator.Top.WorldMatrix;
            var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
            if (positionError > 0.0025)
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(stator)}: Rotor part displaced by clang forces.");
                Util.LogPoseDelta(Util.DebugName(stator), ref actualTopPose, ref expectedTopPose);
                stator.TargetVelocityRad = 0;
                stator.Torque = 0;
                stator.BrakingTorque = Math.Max(100000, stator.BrakingTorque);
            }

            // Is the rotor/hinge part misaligned by clang forces? 
            var axisAlignment = actualTopPose.Up.Dot(expectedTopPose.Up);
            if (axisAlignment < 0.999)
            {
                var axisMismatch = Math.Acos(axisAlignment) * 180.0 / Math.PI;
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(stator)}: Rotor part tilted by clang forces. axisMismatch={axisMismatch:0.000} degrees");
                stator.TargetVelocityRad = 0;
                stator.Torque = 0;
                stator.BrakingTorque = Math.Max(100000, stator.BrakingTorque);
            }
            
            // Is the rotor/hinge part extremely misaligned by clang forces?
            if (positionError > 0.5 || axisAlignment < 0.5)
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(stator)}: Rotor part broke off by clang forces.");
                var axisMismatch = Math.Acos(axisAlignment) * 180.0 / Math.PI;
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(stator)}: axisMismatch={axisMismatch:0.000} degrees");
                Util.LogPoseDelta(Util.DebugName(stator), ref actualTopPose, ref expectedTopPose);
                stator.TargetVelocityRad = 0;
                stator.Detach();
                stator.Enabled = false;
            }
        }

        protected virtual MatrixD GetBaseToTopMatrixD()
        {
            return MatrixD.CreateFromAxisAngle(Vector3D.Down, stator.Angle);
        }
    }
}