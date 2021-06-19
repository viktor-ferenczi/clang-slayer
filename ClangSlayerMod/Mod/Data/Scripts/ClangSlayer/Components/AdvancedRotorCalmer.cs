using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, "LargeAdvancedStator", "SmallAdvancedStator")]
    public class AdvancedRotorCalmer : MyGameLogicComponent
    {
        private IMyMotorAdvancedStator rotorBase;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            rotorBase = Entity as IMyMotorAdvancedStator;
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

            var baseToTop = MatrixD.CreateTranslation(Vector3D.Up * (0.2 + rotorBase.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, rotorBase.Angle);

            var expectedTopPose = baseToTop * rotorBase.WorldMatrix;
            var actualTopPose = rotorBase.Top.WorldMatrix;

            var positionDelta = actualTopPose.Translation - expectedTopPose.Translation;
            MyLog.Default.WriteLineAndConsole($"{rotorBase.CubeGrid.GridSizeEnum}: positionDelta = {Format(positionDelta)}");

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