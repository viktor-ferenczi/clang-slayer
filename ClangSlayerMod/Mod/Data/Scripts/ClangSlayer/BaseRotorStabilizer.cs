using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace ClangSlayer
{
    public class BaseRotorStabilizer : MyGameLogicComponent
    {
        private IMyMotorStator rotorBase;
        private readonly double smallDisplacementOffset;
        private readonly double largeDisplacementOffset;

        protected BaseRotorStabilizer(double smallDisplacementOffset, double largeDisplacementOffset)
        {
            this.smallDisplacementOffset = smallDisplacementOffset;
            this.largeDisplacementOffset = largeDisplacementOffset;
        }

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            rotorBase = Entity as IMyMotorStator;
            
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        }

        public override void Close()
        {
            rotorBase = null;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (rotorBase?.Top == null || rotorBase.Closed || rotorBase?.CubeGrid?.Physics == null)
                return;

            // Handle changes to rotorBase.IsWorking
            var displacementOffset = rotorBase.TopGrid.GridSizeEnum == MyCubeSize.Small ? smallDisplacementOffset : largeDisplacementOffset;
            var baseToTop = MatrixD.CreateTranslation(Vector3D.Up * (displacementOffset + rotorBase.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, rotorBase.Angle);
            var expectedTopPose = baseToTop * rotorBase.WorldMatrix;
            var actualTopPose = rotorBase.Top.WorldMatrix;
            
            var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
            if (positionError > 1e-6)
            {
                Util.LogPoseDelta(Util.Name(rotorBase), ref actualTopPose, ref expectedTopPose);
            }
            
            // MyLog.Default.WriteLineAndConsole($"{rotorBase.CubeGrid.GridSizeEnum}: {rotorBase.CustomName ?? "?"}");
            // MyLog.Default.WriteLineAndConsole($"{rotorBase.CubeGrid.GridSizeEnum}: positionDelta = {Utils.Format(positionDelta)}");

            // var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
            // var forwardError = Vector3D.DistanceSquared(actualTopPose.Forward, expectedTopPose.Forward);
            // var rollError = Vector3D.DistanceSquared(actualTopPose.Up, expectedTopPose.Up);
        }
    }
}