using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorStator), true, "LargeStator", "SmallStator")]
    public class RotorCalmer : MyGameLogicComponent
    {
        private IMyMotorStator rotorBase;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            rotorBase = Entity as IMyMotorStator;
        }

        public override void Close()
        {
            rotorBase = null;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (rotorBase?.CubeGrid?.Physics == null || rotorBase.Closed || !rotorBase.IsWorking || rotorBase.Top == null)
                return;

            MyLog.Default.WriteLineAndConsole($"{rotorBase.CubeGrid.GridSizeEnum}: {rotorBase.CustomName ?? "?"}");

            var baseToTop = MatrixD.CreateTranslation(Vector3D.Up * (0.04 + rotorBase.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, rotorBase.Angle);

            var expectedTopPose = baseToTop * rotorBase.WorldMatrix;
            var actualTopPose = rotorBase.Top.WorldMatrix;

            var positionDelta = actualTopPose.Translation - expectedTopPose.Translation;
            MyLog.Default.WriteLineAndConsole($"{rotorBase.CubeGrid.GridSizeEnum}: positionDelta = {Utils.Format(positionDelta)}");

            var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
            var forwardError = Vector3D.DistanceSquared(actualTopPose.Forward, expectedTopPose.Forward);
            var rollError = Vector3D.DistanceSquared(actualTopPose.Up, expectedTopPose.Up);
        }
    }
}