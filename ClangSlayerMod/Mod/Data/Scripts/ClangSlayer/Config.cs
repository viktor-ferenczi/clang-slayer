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
            MyLog.Default.WriteLineAndConsole($"ClangSlayer: Configuration file in the world's Storage folder: {ConfigFileName}");

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
            }
            else
            {
                MyLog.Default.WriteLineAndConsole("ClangSlayer: Failed to parse configuration file");
                foreach (var error in errors)
                {
                    MyLog.Default.WriteLineAndConsole($"ClangSlayer: {error}");
                }
                MyLog.Default.WriteLineAndConsole("ClangSlayer: Starting with default configuration");
            }
        }

        protected override void AddOptions()
        {
            Comments["Debug"] = "Enables logging of actions taken (useful to see that the mod is actually in use)";
            Defaults["Debug"] = false;

            Comments["Detail"] = "Enables logging of details of each action taken (for investigation or fine-tuning only)"; 
            Defaults["Detail"] = false;

            Comments["Trace"] = "Enables logging of details each time a mechanical base it verified (very verbose, for development only)";
            Defaults["Trace"] = false;

            Comments["PistonDeactivateAtPositionError"] = "Deactivates the piston if the position error of its head is greater than this limit [m]";
            Defaults["PistonDeactivateAtPositionError"] = 0.03f;  // m

            Comments["PistonDeactivateAtAxisError"] = "Deactivates the piston if the axis error of its head is greater than this limit [degrees]";
            Defaults["PistonDeactivateAtAxisError"] = 0.5f;  // degrees
            
            Comments["PistonDetachAtPositionError"] = "Detaches the piston if the position error of its head is greater than this limit [m]";
            Defaults["PistonDetachAtPositionError"] = 0.15f;  // m
            
            Comments["PistonDetachAtAxisError"] = "Detaches the piston if the axis error of its head is greater than this limit [degrees]";
            Defaults["PistonDetachAtAxisError"] = 2.5f;  // degrees
            
            Comments["RotorDeactivateAtPositionError"] = "Deactivates the rotor or hinge if the position error of its head is greater than this limit [m]";
            Defaults["RotorDeactivateAtPositionError"] = 0.03f;  // m
            
            Comments["RotorDeactivateAtAxisError"] = "Deactivates the rotor or hinge if the axis error of its head is greater than this limit [degrees]";
            Defaults["RotorDeactivateAtAxisError"] = 0.5f;  // degrees
            
            Comments["RotorDetachAtPositionError"] = "Detaches the rotor or hinge if the position error of its head is greater than this limit [m]";
            Defaults["RotorDetachAtPositionError"] = 0.15f;  // m
            
            Comments["RotorDetachAtAxisError"] = "Detaches the rotor or hinge if the axis error of its head is greater than this limit [degrees]";
            Defaults["RotorDetachAtAxisError"] = 2.5f;  // degrees
        }

        public bool Debug => (bool)this["Debug"];
        public bool Detail => (bool)this["Detail"];
        public bool Trace => (bool)this["Trace"];
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