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
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_ExtendedPistonBase), false)]
    public class PistonBase : MyGameLogicComponent
    {
        private static bool initialized;

        private IMyExtendedPistonBase pistonBase;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (initialized)
                return;

            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;

            initialized = true;
        }

        public override void OnAddedToContainer()
        {
            if (!initialized)
                return;

            pistonBase = Entity as IMyExtendedPistonBase;
            if (pistonBase == null)
                return;

            MyLog.Default.WriteLineAndConsole($"Piston base added: {pistonBase.EntityId}");
        }

        public override void MarkForClose()
        {
            if (!initialized)
                return;

            if (pistonBase != null)
                MyLog.Default.WriteLineAndConsole($"Piston base closed: {pistonBase?.EntityId}");

            pistonBase = null;
        }

        public override void UpdateBeforeSimulation10()
        {
            if (!initialized || pistonBase?.CubeGrid?.Physics == null || pistonBase.Top == null || !pistonBase.IsWorking)
            {
                MyLog.Default.WriteLineAndConsole($"Piston base is not working");
                return;
            }

            MyLog.Default.WriteLineAndConsole($"Piston base is working: {pistonBase.EntityId}");
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