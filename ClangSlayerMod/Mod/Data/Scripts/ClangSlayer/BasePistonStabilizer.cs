using System;
using VRage.Game.Components;
using Sandbox.ModAPI;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace ClangSlayer
{
    public class BasePistonStabilizer: MyGameLogicComponent
    {
        private IMyExtendedPistonBase pistonBase;
        private PropertyOverride maxImpulseAxis;
        private float previousPosition;
        private bool previousPositionSet;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;

            pistonBase = Entity as IMyExtendedPistonBase;
            maxImpulseAxis = new PropertyOverride(pistonBase, "MaxImpulseAxis", 1);
        }

        public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
        {
            if (!maxImpulseAxis.Overriding)
                return base.Serialize(copy);

            var value = maxImpulseAxis.Value;

            try
            {
                maxImpulseAxis.Reset();
                return base.Serialize(copy);
            }
            finally
            {
                maxImpulseAxis.Override(value);
            }
        }

        public override void Close()
        {
            pistonBase = null;
        }

        public override void UpdateBeforeSimulation()
        {
            if (pistonBase?.Top == null || pistonBase.Closed || pistonBase?.CubeGrid?.Physics == null)
                return;

            return;
            
            if (!pistonBase.IsWorking)
            {
                if (!maxImpulseAxis.Overriding)
                    return;

                MyLog.Default.WriteLineAndConsole($"{Util.Name(pistonBase)}: BROKEN, DETACHED");

                pistonBase.Detach();

                maxImpulseAxis.Abandon(100);
                return;
            }

            if (!previousPositionSet)
            {
                previousPosition = pistonBase.CurrentPosition;
                previousPositionSet = true;
                return;
            }

            var measuredVelocity = (pistonBase.CurrentPosition - previousPosition) * 60;
            previousPosition = pistonBase.CurrentPosition;

            var velocitySetting = pistonBase.Velocity;
            if (velocitySetting > 0 && pistonBase.CurrentPosition > pistonBase.MaxLimit - 1e-4)
                velocitySetting = 0;
            if (velocitySetting < 0 && pistonBase.CurrentPosition < pistonBase.MinLimit + 1e-4)
                velocitySetting = 0;

            if (Math.Abs(velocitySetting) < 1e-4)
                return;

            var sign = Math.Sign(velocitySetting);
            if (sign * measuredVelocity > 0.5 * sign * velocitySetting)
            {
                maxImpulseAxis.Reset();
                return;
            }

            // Reduce the maximum impulse
            maxImpulseAxis.Override(Math.Max(100, maxImpulseAxis.Value * Math.Abs(measuredVelocity) / Math.Abs(velocitySetting)));
        }
    }
}