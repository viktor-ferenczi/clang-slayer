using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, "LargeAdvancedStator")]
    public class LargeAdvancedRotorStabilizer : BaseRotorStabilizer
    {
        public LargeAdvancedRotorStabilizer() : base(-0.216893 / -0.989401, -0.197673 / -0.989401)
        {
        }
    }
}