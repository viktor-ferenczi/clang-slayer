using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorStator), true, "LargeStator")]
    public class LargeRotorStabilizer : BaseRotorStabilizer
    {
        public LargeRotorStabilizer(): base(-0.416403 / -0.989401, -0.416402 / -0.989401)
        {
        }
    }
}