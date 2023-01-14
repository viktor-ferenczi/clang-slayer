// ReSharper disable once CheckNamespace

using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Utils;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    public class Config: BaseConfig
    {
        private const string ConfigFileName = "ClangSlayer.ini";
        private static readonly Type ModType = typeof(Config);

        public Config()
        {
            string text = null;
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage(ConfigFileName, ModType))
            {
                using (var f = MyAPIGateway.Utilities.ReadFileInWorldStorage(ConfigFileName, ModType))
                {
                    text = f.ReadToEnd();
                }
            }
            
            var errors = new List<string>();
            if (string.IsNullOrEmpty(text) || TryParse(text, Defaults, errors))
            {
                using (var f = MyAPIGateway.Utilities.WriteFileInWorldStorage(ConfigFileName, ModType))
                {
                    text = FormatIni();
                    f.Write(text);
                }
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: Saved default configuration file to the world's Storage folder: {ConfigFileName}");
            }
            else
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: Failed to parse configuration file from the world's Storage folder: {ConfigFileName}");
                foreach (var error in errors)
                {
                    MyLog.Default.WriteLineAndConsole($"ClangSlayer: {error}");
                }
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: Starting with default configuration");
            }
        }

        protected override void AddOptions()
        {
            Defaults["Debug"] = false;
            
            // Piston error thresholds
            Defaults["PistonDeactivateAtPositionError"] = 0.03f;  // m
            Defaults["PistonDeactivateAtAxisError"] = 0.5f;  // degrees
            Defaults["PistonDetachAtPositionError"] = 0.15f;  // m
            Defaults["PistonDetachAtAxisError"] = 2.5f;  // degrees
            
            // Rotor and hinge error thresholds
            Defaults["RotorDeactivateAtPositionError"] = 0.03f;  // m
            Defaults["RotorDeactivateAtAxisError"] = 0.5f;  // degrees
            Defaults["RotorDetachAtPositionError"] = 0.15f;  // m
            Defaults["RotorDetachAtAxisError"] = 2.5f;  // degrees
        }

        public bool Debug => (bool)this["Debug"];
        public float PistonDeactivateAtPositionError => (float)this["PistonDeactivateAtPositionError"];
        public float PistonDeactivateAtAxisError => (float)this["PistonDeactivateAtAxisError"];
        public float PistonDetachAtPositionError => (float)this["PistonDetachAtPositionError"];
        public float PistonDetachAtAxisError => (float)this["PistonDetachAtAxisError"];
        public float RotorDeactivateAtPositionError => (float)this["RotorDeactivateAtPositionError"];
        public float RotorDeactivateAtAxisError => (float)this["RotorDeactivateAtAxisError"];
        public float RotorDetachAtPositionError => (float)this["RotorDetachAtPositionError"];
        public float RotorDetachAtAxisError => (float)this["RotorDetachAtAxisError"];
    }
}