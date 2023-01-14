using VRage.Game;
using VRageMath;

namespace ClangSlayer
{
    public class BaseRotorStabilizer : BaseHingeStabilizer
    {
        private readonly double smallDisplacementOffset;
        private readonly double largeDisplacementOffset;

        public BaseRotorStabilizer(double smallDisplacementOffset, double largeDisplacementOffset)
        {
            this.smallDisplacementOffset = smallDisplacementOffset;
            this.largeDisplacementOffset = largeDisplacementOffset;
        }

        protected override MatrixD GetBaseToTopMatrixD()
        {
            var displacementOffset = Stator.TopGrid.GridSizeEnum == MyCubeSize.Large ? largeDisplacementOffset : smallDisplacementOffset;
            return MatrixD.CreateTranslation(Vector3D.Up * (displacementOffset + Stator.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, Stator.Angle);
        }
    }
}