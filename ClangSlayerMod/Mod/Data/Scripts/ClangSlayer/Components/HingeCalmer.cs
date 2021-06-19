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
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, new []{"LargeHinge", "MediumHinge", "SmallHinge"})]
    public class PistonBaseCalmer : MyGameLogicComponent
    {
        private IMyMotorAdvancedStator hingeBase;

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
            if (hingeBase?.CubeGrid?.Physics == null || hingeBase.Closed || !hingeBase.IsWorking || hingeBase.Top == null)
                return;

            MyLog.Default.WriteLineAndConsole($"{hingeBase.CubeGrid.GridSizeEnum}: {hingeBase.CustomName ?? "?"}");

            var baseToTop = MatrixD.CreateFromAxisAngle(Vector3D.Down, hingeBase.Angle);

            var expectedTopPose = baseToTop * hingeBase.WorldMatrix;
            var actualTopPose = hingeBase.Top.WorldMatrix;

            var positionDelta = actualTopPose.Translation - expectedTopPose.Translation;
            MyLog.Default.WriteLineAndConsole($"{hingeBase.CubeGrid.GridSizeEnum}: positionDelta = {Format(positionDelta)}");

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