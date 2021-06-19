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
        private const double overrideLimit = 0.009;
        private IMyExtendedPistonBase pistonBase;
        private float playerMaxImpulseAxis;
        private float overrideMaxImpulseAxis;
        private bool overriding;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

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

        public override void UpdateBeforeSimulation100()
        {
            if (pistonBase?.CubeGrid?.Physics == null || pistonBase.Closed || !pistonBase.IsWorking || pistonBase.Top == null)
                return;

            var velocity = pistonBase.Velocity;
            if (velocity > 0 && pistonBase.CurrentPosition > pistonBase.MaxLimit - 0.001)
                velocity = 0;
            if (velocity < 0 && pistonBase.CurrentPosition < pistonBase.MinLimit + 0.001)
                velocity = 0;

            var offset = pistonBase.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 1.410 : 0.282;
            var baseToTop = MatrixD.CreateTranslation(new Vector3D(0, offset + pistonBase.CurrentPosition - velocity * 0.01666, 0));

            var expectedTopPose = baseToTop * pistonBase.WorldMatrix;
            var actualTopPose = pistonBase.Top.WorldMatrix;

            var delta = actualTopPose.Translation - expectedTopPose.Translation;

            var currentMaxImpulseAxis = pistonBase.GetValueFloat("MaxImpulseAxis");
            if (!overriding || Math.Abs(currentMaxImpulseAxis - overrideMaxImpulseAxis) > currentMaxImpulseAxis * 1e-5)
                playerMaxImpulseAxis = currentMaxImpulseAxis;

            var error = delta.Length();


            if (pistonBase.CustomName.Contains("Jumper"))
            {
                MyLog.Default.WriteLineAndConsole($"{pistonBase.CustomName ?? "?"}: position = {pistonBase.CurrentPosition:0.000}, velocity = {velocity:0.000}");
                MyLog.Default.WriteLineAndConsole($"{pistonBase.CustomName ?? "?"}: delta = {Utils.Format(delta)}, error = {error:0.000}, playerMaxImpulseAxis = {playerMaxImpulseAxis}");
            }

            if (error < overrideLimit)
            {
                if (!overriding)
                    return;

                pistonBase.SetValueFloat("MaxImpulseAxis", playerMaxImpulseAxis);
                overriding = false;
                return;
            }

            var proposedMaxImpulseAxis = (float)(100 + Utils.Rng.NextDouble());
            if (!(Math.Abs(proposedMaxImpulseAxis - currentMaxImpulseAxis) > Math.Max(100, currentMaxImpulseAxis * 0.1)))
                return;

            overriding = true;
            overrideMaxImpulseAxis = proposedMaxImpulseAxis;
            pistonBase.SetValueFloat("MaxImpulseAxis", overrideMaxImpulseAxis);

            if (pistonBase.CustomName.Contains("Jumper"))
                MyLog.Default.WriteLineAndConsole($"{pistonBase.CustomName ?? "?"}: overrideMaxImpulseAxis = {overrideMaxImpulseAxis}");
        }
    }
}