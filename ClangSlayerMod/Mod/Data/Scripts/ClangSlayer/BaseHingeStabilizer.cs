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
        private IMyMotorStator hingeBase;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            hingeBase = Entity as IMyMotorAdvancedStator;
        }

        public override void Close()
        {
            hingeBase = null;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (hingeBase?.Top == null || hingeBase.Closed || hingeBase?.CubeGrid?.Physics == null)
                return;

            // Handle changes to rotorBase.IsWorking

            var baseToTop = MatrixD.CreateFromAxisAngle(Vector3D.Down, hingeBase.Angle);
            var expectedTopPose = baseToTop * hingeBase.WorldMatrix;
            var actualTopPose = hingeBase.Top.WorldMatrix;

            var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
            if (positionError > 1e-6)
            {
                Util.LogPoseDelta(Util.Name(hingeBase), ref actualTopPose, ref expectedTopPose);
            }

            // MyLog.Default.WriteLineAndConsole($"{hingeBase.CubeGrid.GridSizeEnum}: positionDelta = {Utils.Format(positionDelta)}");
        }
    }
}