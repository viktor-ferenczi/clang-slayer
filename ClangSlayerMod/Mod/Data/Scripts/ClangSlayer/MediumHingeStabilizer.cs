using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    // ReSharper disable once UnusedType.Global
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), true, "MediumHinge")]
    public class MediumHingeStabilizer : BaseHingeStabilizer
    {
        public MediumHingeStabilizer()
        {
        }
    }
}