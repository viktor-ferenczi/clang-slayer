using System;
using VRage.Game.Components;
using Sandbox.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace ClangSlayer
{
    public class BasePistonStabilizer: MyGameLogicComponent
    {
        private IMyExtendedPistonBase piston;
        private Derivative position;
        private float previousVelocity;
        // private PropertyOverride maxImpulseAxis;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;
            
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            piston = Entity as IMyExtendedPistonBase;
            position = new Derivative(piston.CurrentPosition);
            // maxImpulseAxis = new PropertyOverride(piston, "MaxImpulseAxis", 1);
        }

        // public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
        // {
        //     if (!maxImpulseAxis.Overriding)
        //         return base.Serialize(copy);
        //
        //     var value = maxImpulseAxis.Value;
        //
        //     try
        //     {
        //         maxImpulseAxis.Reset();
        //         return base.Serialize(copy);
        //     }
        //     finally
        //     {
        //         maxImpulseAxis.Override(value);
        //     }
        // }

        public override void Close()
        {
            piston = null;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (piston?.Top == null || piston.Closed || piston.CubeGrid?.Physics == null)
            {
                return;
            } 
            
            if (!piston.IsWorking || piston.Velocity == 0f || 
                Math.Abs(piston.Velocity - previousVelocity) > 1e-6 ||
                piston.Velocity > 0f && piston.CurrentPosition >= piston.MaxLimit - 1e-6 ||
                piston.Velocity < 0f && piston.CurrentPosition <= piston.MinLimit + 1e-6)
            {
                position.Reset(piston.CurrentPosition);
                previousVelocity = piston.Velocity;
                return;
            }
            
            position.Update(piston.CurrentPosition);
            if (!position.Valid)
            {
                return;
            }
            
            // Does the piston's velocity significantly differ from the configured value? 
            var velocity = position.Dt;
            var velocityError = velocity - piston.Velocity;
            if (Math.Abs(velocityError) > 0.9 * Math.Abs(piston.Velocity))
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(piston)}: Piston is stuck. expectedVelocity={piston.Velocity:0.000}; actualVelocity={velocity:0.000}; velocityError={velocityError:0.000}");
                piston.Velocity = 0;
            }
        }
    }
}