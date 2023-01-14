using System;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;

namespace ClangSlayer
{
    public class PropertyOverride
    {
        public readonly IMyTerminalBlock Block;
        public readonly string Name;
        public readonly float Epsilon;

        public bool Overriding { get; private set; }

        private float originalValue;
        private float overrideValue;

        public PropertyOverride(IMyTerminalBlock block, string name, float epsilon)
        {
            Block = block;
            Name = name;
            Epsilon = epsilon;
        }

        public float Value => Block.GetValueFloat(Name);

        private void Set(float value)
        {
            Block.SetValueFloat(Name, value);
        }

        public void Override(float value)
        {
            var currentValue = Value;
            if (!Overriding || Math.Abs(currentValue - overrideValue) >= Epsilon)
                originalValue = currentValue;

            overrideValue = value;
            Overriding = true;

            Set(value);
        }

        public void Reset()
        {
            if (!Overriding)
                return;

            if (Math.Abs(Value - overrideValue) < Epsilon)
                Set(originalValue);

            Overriding = false;
        }

        public void Abandon(float value)
        {
            Set(value);

            Overriding = false;
        }
    }
}