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
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_ExtendedPistonBase), true)]
    public class PistonBaseCalmer : MyGameLogicComponent
    {
        private IMyExtendedPistonBase pistonBase;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            pistonBase = Entity as IMyExtendedPistonBase;
        }

        public override void Close()
        {
            pistonBase = null;
        }

        public override void UpdateBeforeSimulation100()
        {
            if (pistonBase?.CubeGrid?.Physics == null || pistonBase.Closed || !pistonBase.IsWorking || pistonBase.Top == null)
                return;

            var offset = pistonBase.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 1.4 : 1.4;
            var baseToTop = MatrixD.CreateTranslation(Vector3D.Up * (offset + pistonBase.CurrentPosition));

            var expectedTopPose = pistonBase.WorldMatrix * baseToTop;
            var actualTopPose = pistonBase.Top.WorldMatrix;

            var positionDelta = actualTopPose.Translation - expectedTopPose.Translation;
            MyLog.Default.WriteLineAndConsole($"positionDelta = {Format(positionDelta)}");

            var forwardDelta = positionDelta.Length() * Vector3D.Dot(positionDelta, expectedTopPose.Forward);
            MyLog.Default.WriteLineAndConsole($"forwardDelta = {Format(forwardDelta)}");

            var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
            var forwardError = Vector3D.DistanceSquared(actualTopPose.Forward, expectedTopPose.Forward);
            var rollError = Vector3D.DistanceSquared(actualTopPose.Up, expectedTopPose.Up);
        }

        public static string Format(float v)
        {
            return $"{v:0.000}";
        }

        public static string Format(double v)
        {
            return $"{v:0.000}";
        }

        public static string Format(Vector3I v)
        {
            return $"[{v.X}, {v.Y}, {v.Z}]";
        }

        public static string Format(Vector3D v)
        {
            return $"[{v.X:0.000}, {v.Y:0.000}, {v.Z:0.000}]";
        }

        public static string Format(MatrixD m)
        {
            return $"\r\n  T: {Format(m.Translation)}\r\n  F: {Format(m.Forward)}\r\n  U: {Format(m.Up)}\r\n  S: {Format(m.Scale)}";
        }
    }
}