using Sandbox.Common.ObjectBuilders;
using VRage.Game.Components;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_ExtendedPistonBase), true, "LargePistonBase")]
    public class LargePistonStabilizer : BasePistonStabilizer
    {
        public LargePistonStabilizer() : base(-1.393968 / -0.989401)
        {
        }
    }
}