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
            var displacementOffset = stator.TopGrid.GridSizeEnum == MyCubeSize.Small ? smallDisplacementOffset : largeDisplacementOffset;
            return MatrixD.CreateTranslation(Vector3D.Up * (displacementOffset + stator.Displacement)) * MatrixD.CreateFromAxisAngle(Vector3D.Down, stator.Angle);
        }
    }
}