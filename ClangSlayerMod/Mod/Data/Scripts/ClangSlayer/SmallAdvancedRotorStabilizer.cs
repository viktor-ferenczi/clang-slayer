using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, "SmallAdvancedStator", "SmallAdvancedStatorSmall")]
    public class SmallAdvancedRotorStabilizer : BaseRotorStabilizer
    {
        public SmallAdvancedRotorStabilizer() : base(0.108834 / -0.989409, 0.108834 / -0.989409)
        {
        }
    }
}