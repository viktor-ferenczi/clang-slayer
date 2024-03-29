using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_ExtendedPistonBase), true, "SmallPistonBase")]
    public class SmallPistonStabilizer : BasePistonStabilizer
    {
        public SmallPistonStabilizer() : base(-0.279103 / -0.989410)
        {
        }
    }
}