using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, "SmallAdvancedStator")]
    public class SmallAdvanced3X3RotorStabilizer : BaseRotorStabilizer
    {
        public SmallAdvanced3X3RotorStabilizer() : base(0.108834 / -0.989409, 0.108834 / -0.989409)
        {
        }
    }
}