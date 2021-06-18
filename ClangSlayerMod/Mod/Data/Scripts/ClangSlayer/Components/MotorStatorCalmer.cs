using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
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
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorStator), true)]
    public class MotorStatorCalmer : MyGameLogicComponent
    {
        // private static volatile bool initialized;
        //
        // private IMyMechanicalConnectionBlock block;
        // private bool isHinge;
        //
        // public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        // {
        //     base.Init(objectBuilder);
        //
        //     MyLog.Default.WriteLineAndConsole("Init");
        //
        //     if (initialized)
        //         return;
        //
        //     initialized = MyAPIGateway.Multiplayer.IsServer;
        //
        //     MyLog.Default.WriteLineAndConsole("Initialized");
        // }
        //
        // public override void OnAddedToContainer()
        // {
        //     base.OnAddedToContainer();
        //
        //     MyLog.Default.WriteLineAndConsole($"OnAddedToContainer: {Entity.GetType().Name} {Entity.EntityId}");
        //
        //     block = Entity as IMyMechanicalConnectionBlock;
        //     if (block == null)
        //         return;
        //
        //     isHinge = false; //block.BlockDefinition.SubtypeName.EndsWith("Hinge");
        //
        //     MyLog.Default.WriteLineAndConsole($"Block: {block.GetType().Name} {block.EntityId}");
        //
        //     NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        // }
        //
        // public override void MarkForClose()
        // {
        //     block = null;
        //
        //     base.MarkForClose();
        // }
        //
        // public override void UpdateOnceBeforeFrame()
        // {
        //     if (!initialized || block?.CubeGrid?.Physics == null || block.Top == null || !block.IsWorking)
        //         return;
        //
        //     var stator = block as IMyMotorStator;
        //     if (stator != null)
        //     {
        //         if (isHinge)
        //             SuppressHinge(stator);
        //         else
        //             SuppressRotor(stator);
        //         return;
        //     }
        //
        //     var piston = block as IMyPistonBase;
        //     if (piston != null)
        //         SuppressPiston(piston);
        // }
        //
        // private void SuppressHinge(IMyMotorStator stator)
        // {
        //     MatrixD baseToTop;
        //     CalculateHingeBaseToTopTransform(stator, out baseToTop);
        //
        //     var expectedTopPose = block.WorldMatrix * baseToTop;
        //     var actualTopPose = block.Top.WorldMatrix;
        //
        //     var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
        //     var forwardError = Vector3D.DistanceSquared(actualTopPose.Forward, expectedTopPose.Forward);
        //     var rollError = Vector3D.DistanceSquared(actualTopPose.Up, expectedTopPose.Up);
        // }
        //
        // private void SuppressRotor(IMyMotorStator stator)
        // {
        //     MatrixD baseToTop;
        //     CalculateRotorBaseToTopTransform(stator, out baseToTop);
        //
        //     var expectedTopPose = block.WorldMatrix * baseToTop;
        //     var actualTopPose = block.Top.WorldMatrix;
        //
        //     var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
        //     var forwardError = Vector3D.DistanceSquared(actualTopPose.Forward, expectedTopPose.Forward);
        //     var rollError = Vector3D.DistanceSquared(actualTopPose.Up, expectedTopPose.Up);
        // }
        //
        // private void SuppressPiston(IMyPistonBase piston)
        // {
        //     MatrixD baseToTop;
        //     CalculatePistonBaseToTopTransform(piston, out baseToTop);
        //
        //     var expectedTopPose = block.WorldMatrix * baseToTop;
        //     var actualTopPose = block.Top.WorldMatrix;
        //
        //     var positionDelta = actualTopPose.Translation - expectedTopPose.Translation;
        //     MyLog.Default.WriteLineAndConsole($"positionDelta = {Format(positionDelta)}");
        //
        //     var forwardDelta = positionDelta.Length() * Vector3D.Dot(positionDelta, expectedTopPose.Forward);
        //     MyLog.Default.WriteLineAndConsole($"forwardDelta = {Format(forwardDelta)}");
        //
        //     var positionError = Vector3D.DistanceSquared(actualTopPose.Translation, expectedTopPose.Translation);
        //     var forwardError = Vector3D.DistanceSquared(actualTopPose.Forward, expectedTopPose.Forward);
        //     var rollError = Vector3D.DistanceSquared(actualTopPose.Up, expectedTopPose.Up);
        // }
        //
        // private static void CalculateHingeBaseToTopTransform(IMyMotorStator hinge, out MatrixD tr)
        // {
        //     tr = MatrixD.CreateFromAxisAngle(Vector3D.Down, hinge.Angle);
        // }
        //
        // private static void CalculateRotorBaseToTopTransform(IMyMotorStator rotor, out MatrixD tr)
        // {
        //     tr = MatrixD.CreateTranslation(Vector3D.Up * (0.2 + rotor.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, rotor.Angle);
        // }
        //
        // private static void CalculatePistonBaseToTopTransform(IMyPistonBase piston, out MatrixD tr)
        // {
        //     var offset = piston.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 1.4 : 1.4;
        //     tr = MatrixD.CreateTranslation(Vector3D.Up * (offset + piston.CurrentPosition));
        // }
        //
        // public static string Format(float v)
        // {
        //     return $"{v:0.000}";
        // }
        //
        // public static string Format(double v)
        // {
        //     return $"{v:0.000}";
        // }
        //
        // public static string Format(Vector3I v)
        // {
        //     return $"[{v.X}, {v.Y}, {v.Z}]";
        // }
        //
        // public static string Format(Vector3D v)
        // {
        //     return $"[{v.X:0.000}, {v.Y:0.000}, {v.Z:0.000}]";
        // }
        //
        // public static string Format(MatrixD m)
        // {
        //     return $"\r\n  T: {Format(m.Translation)}\r\n  F: {Format(m.Forward)}\r\n  U: {Format(m.Up)}\r\n  S: {Format(m.Scale)}";
        // }
    }
}