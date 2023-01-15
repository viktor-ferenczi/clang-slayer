using Sandbox.ModAPI;
using VRage.Utils;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    public static class Util
    {
        public static Config Cfg => ClangSlayerSession.Cfg;
        
        public static string DebugName(IMyTerminalBlock block)
        {
            return block?.CubeGrid == null ? "?" : $"{Name(block.CubeGrid)}/{Name(block)}";
        }

        public static string Name(VRage.Game.ModAPI.IMyCubeGrid grid)
        {
            return grid.CustomName ?? grid.DisplayName ?? grid.Name;
        }

        public static string Name(IMyTerminalBlock block)
        {
            return block.CustomName ?? block.DisplayName ?? block.Name;
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
            return $"[T: {Format(m.Translation)}, F: {Format(m.Forward)}, U: {Format(m.Up)}, S: {Format(m.Scale)}]";
        }

        public static void LogPoseDelta(string name, ref MatrixD actualTopPose, ref MatrixD expectedTopPose)
        {
            var positionDelta = actualTopPose.Translation - expectedTopPose.Translation;
            
            MyLog.Default.WriteLineAndConsole(
                $"ClangSlayer: {name}:\r\n" +
                $"expectedTopPose={Format(expectedTopPose)};\r\n" +
                $"  actualTopPose={Format(actualTopPose)};\r\n" +
                $"  positionDelta={Format(positionDelta)};");
        }
    }
}