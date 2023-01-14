using Sandbox.Common.ObjectBuilders;
using VRage.Game.Components;

namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_ExtendedPistonBase), true, "LargePistonBase")]
    public class LargePistonStabilizer : BasePistonStabilizer
    {
        public LargePistonStabilizer()
        {
        }
    }
}