using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorStator), true, "SmallStator")]
    public class SmallRotorStabilizer : BaseRotorStabilizer
    {
        public SmallRotorStabilizer() : base(0.008864 / -0.989409, 0.008864 / -0.989409)
        {
        }
    }
}