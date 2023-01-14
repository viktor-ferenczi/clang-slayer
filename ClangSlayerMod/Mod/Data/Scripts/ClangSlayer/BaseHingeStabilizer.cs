using System;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    public class BaseHingeStabilizer : MyGameLogicComponent
    {
        private readonly Config cfg = Util.Cfg;
        protected IMyMotorStator Stator;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;

            Stator = Entity as IMyMotorStator;
        }

        public override void Close()
        {
            Stator = null;
        }

        public override void UpdateBeforeSimulation10()
        {
            if (Stator?.Top == null || Stator.Closed || Stator.CubeGrid?.Physics == null)
            {
                return;
            }
            
            if (!Stator.IsWorking || Stator.RotorLock || Stator.TargetVelocityRad == 0f || 
                Stator.TargetVelocityRad < 0f && Stator.LowerLimitRad < -31.415f && Stator.Angle <= Stator.LowerLimitRad + 1e-6f ||
                Stator.TargetVelocityRad > 0f && Stator.UpperLimitRad > +31.415f && Stator.Angle >= Stator.UpperLimitRad - 1e-6f)
            {
                return;
            }

            // Calculate the error in the top part's pose
            var baseToTop = GetBaseToTopMatrixD();
            var expectedTopPose = baseToTop * Stator.WorldMatrix;
            var actualTopPose = Stator.Top.WorldMatrix;
            var positionError = Vector3D.Distance(actualTopPose.Translation, expectedTopPose.Translation);
            var axisAlignment = Math.Max(0f, Math.Min(1f, actualTopPose.Up.Dot(expectedTopPose.Up)));
            var axisError = Math.Acos(axisAlignment) * 180.0 / Math.PI;
            
            // Util.LogPoseDelta(Util.DebugName(Stator), ref actualTopPose, ref expectedTopPose);
            // MyLog.Default.WriteLineAndConsole($"  positionError={positionError:0.000} m");
            // MyLog.Default.WriteLineAndConsole($"      axisError={axisError:0.000} degrees");
            
            // Is the head slightly displaced by clang forces?
            var modified = false;
            if (positionError > cfg.RotorDeactivateAtPositionError || axisError > cfg.RotorDeactivateAtAxisError)
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(Stator)}: Deactivated because of clang forces");
                Stator.SetValueFloat("Torque", 0f);
                Stator.SetValueFloat("BrakingTorque", Math.Max(1000000, Stator.BrakingTorque));
                Stator.SetValueFloat("Velocity", 0f);
                modified = true;
            }

            // Is the head extremely misaligned by clang forces?
            if (positionError > cfg.RotorDetachAtPositionError || axisError > cfg.RotorDetachAtAxisError)
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(Stator)}: Detached top part due to heavy clang forces");
                Stator.Detach();
                Stator.Enabled = false;
                modified = true;
            }
            
            if (modified && Util.Debug)
            {
                Util.LogPoseDelta(Util.DebugName(Stator), ref actualTopPose, ref expectedTopPose);
                MyLog.Default.WriteLineAndConsole($"  positionError={positionError:0.000} m");
                MyLog.Default.WriteLineAndConsole($"      axisError={axisError:0.000} degrees");
            }
        }

        protected virtual MatrixD GetBaseToTopMatrixD()
        {
            return MatrixD.CreateFromAxisAngle(Vector3D.Down, Stator.Angle);
        }
    }
}