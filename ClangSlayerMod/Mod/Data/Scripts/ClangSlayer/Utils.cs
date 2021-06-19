using VRageMath;

namespace ClangSlayer
{
    public static class Utils
    {
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
            return $"\r\n  T: {Utils.Format(m.Translation)}\r\n  F: {Utils.Format(m.Forward)}\r\n  U: {Utils.Format(m.Up)}\r\n  S: {Utils.Format(m.Scale)}";
        }
    }
}