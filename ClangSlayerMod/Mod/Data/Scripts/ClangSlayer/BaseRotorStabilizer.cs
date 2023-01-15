using VRage.Game;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    public class BaseRotorStabilizer : BaseHingeStabilizer
    {
        protected readonly double SmallDisplacementOffset;
        protected readonly double LargeDisplacementOffset;

        public BaseRotorStabilizer(double smallDisplacementOffset, double largeDisplacementOffset)
        {
            SmallDisplacementOffset = smallDisplacementOffset;
            LargeDisplacementOffset = largeDisplacementOffset;
        }

        protected override MatrixD GetBaseToTopMatrixD()
        {
            var displacementOffset = Stator.TopGrid.GridSizeEnum == MyCubeSize.Large ? LargeDisplacementOffset : SmallDisplacementOffset;
            return MatrixD.CreateTranslation(Vector3D.Up * (displacementOffset + Stator.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, Stator.Angle);
        }
    }
}