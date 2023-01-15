using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, "SmallAdvancedStatorSmall")]
    public class SmallAdvanced1X1RotorStabilizer : BaseRotorStabilizer
    {
        public SmallAdvanced1X1RotorStabilizer() : base(0.244399 / -0.989409, 0.244399 / -0.989409)
        {
        }
    }
}