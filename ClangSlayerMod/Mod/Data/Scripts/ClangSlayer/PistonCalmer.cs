using System;
using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Noise;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_ExtendedPistonBase), true)]
    public class PistonCalmer : MyGameLogicComponent
    {
        private IMyExtendedPistonBase pistonBase;
        private float playerMaxImpulseAxis;
        private float overrideMaxImpulseAxis;
        private bool overriding;
        private float previousPosition;
        private bool previousPositionSet;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;

            pistonBase = Entity as IMyExtendedPistonBase;
        }

        public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
        {
            try
            {
                if (overriding)
                    pistonBase.SetValueFloat("MaxImpulseAxis", playerMaxImpulseAxis);

                return base.Serialize(copy);
            }
            finally
            {
                if (overriding)
                    pistonBase.SetValueFloat("MaxImpulseAxis", overrideMaxImpulseAxis);
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

            if (!pistonBase.IsWorking)
            {
                if (!overriding)
                    return;

                MyLog.Default.WriteLineAndConsole($"{pistonBase.CustomName ?? pistonBase.DisplayName ?? "Unnamed Piston"}: BROKEN, DETACHED");

                pistonBase.Detach();
                pistonBase.Velocity = -pistonBase.Velocity;
                pistonBase.SetValueFloat("MaxImpulseAxis", 100);
                overriding = false;
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
                if (!overriding)
                    return;

                pistonBase.SetValueFloat("MaxImpulseAxis", playerMaxImpulseAxis);
                overriding = false;

                // MyLog.Default.WriteLineAndConsole($"{pistonBase.CustomName ?? "?"}: position = {pistonBase.CurrentPosition:0.000}, velocity = {velocitySetting:0.000}, effectiveVelocity = {measuredVelocity:0.000}, playerMaxImpulseAxis = {playerMaxImpulseAxis}, FINISHED");

                return;
            }

            var currentMaxImpulseAxis = pistonBase.GetValueFloat("MaxImpulseAxis");
            if (!overriding || Math.Abs(currentMaxImpulseAxis - overrideMaxImpulseAxis) > currentMaxImpulseAxis * 1e-5)
                playerMaxImpulseAxis = currentMaxImpulseAxis;

            overrideMaxImpulseAxis = (float)Math.Max(100, playerMaxImpulseAxis * Math.Abs(measuredVelocity) / Math.Abs(velocitySetting));

            // if(!overriding)
            //     MyLog.Default.WriteLineAndConsole($"{pistonBase.CustomName ?? "?"}: position = {pistonBase.CurrentPosition:0.000}, velocity = {velocitySetting:0.000}, effectiveVelocity = {measuredVelocity:0.000}, playerMaxImpulseAxis = {playerMaxImpulseAxis}, overrideMaxImpulseAxis = {overrideMaxImpulseAxis}");

            overriding = true;
            pistonBase.SetValueFloat("MaxImpulseAxis", overrideMaxImpulseAxis);
        }
    }
}