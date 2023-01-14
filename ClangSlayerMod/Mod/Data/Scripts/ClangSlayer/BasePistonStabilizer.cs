using System;
using VRage.Game.Components;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    public class BasePistonStabilizer: MyGameLogicComponent
    {
        private readonly Config cfg = Util.Cfg;
        private IMyExtendedPistonBase piston;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!MyAPIGateway.Multiplayer.IsServer)
                return;
            
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;

            piston = Entity as IMyExtendedPistonBase;
        }

        public override void Close()
        {
            piston = null;
        }

        public override void UpdateBeforeSimulation10()
        {
            if (piston?.Top == null || piston.Closed || piston.CubeGrid?.Physics == null)
            {
                return;
            } 
            
            if (!piston.IsWorking || piston.Velocity == 0f || 
                piston.Velocity > 0f && piston.CurrentPosition >= piston.MaxLimit - 1e-6f ||
                piston.Velocity < 0f && piston.CurrentPosition <= piston.MinLimit + 1e-6f)
            {
                return;
            }
            
            // Calculate the error in the top part's pose
            var offset = piston.TopGrid.GridSizeEnum == MyCubeSize.Large ? -1.393968 / -0.989401 : -0.279103 / -0.989410;
            var baseToTop = MatrixD.CreateTranslation(Vector3D.Up * (offset + piston.CurrentPosition));  // + piston.Velocity / 60.0 
            var expectedTopPose = baseToTop * piston.WorldMatrix;
            var actualTopPose = piston.Top.WorldMatrix;
            var positionError = Vector3D.Distance(actualTopPose.Translation, expectedTopPose.Translation);
            var axisAlignment = Math.Max(0f, Math.Min(1f, actualTopPose.Up.Dot(expectedTopPose.Up)));
            var axisError = Math.Acos(axisAlignment) * 180.0 / Math.PI;

            // Util.LogPoseDelta(Util.DebugName(piston), ref actualTopPose, ref expectedTopPose);
            // MyLog.Default.WriteLineAndConsole($"  positionError={positionError:0.000} m");
            // MyLog.Default.WriteLineAndConsole($"      axisError={axisError:0.000} degrees");
            
            // Is the head slightly displaced by clang forces?
            var modified = false;
            if (positionError > cfg.PistonDeactivateAtPositionError || axisError > cfg.PistonDeactivateAtAxisError)
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(piston)}: Deactivated because of clang forces");
                piston.SetValueFloat("Velocity", 0f);
                piston.SetValueFloat("MaxImpulseAxis", 100f);
                piston.SetValueFloat("MaxImpulseNonAxis", 100f);
                modified = true;
            }

            // Is the head extremely misaligned by clang forces?
            if (positionError > cfg.PistonDetachAtPositionError || axisError > cfg.PistonDetachAtAxisError)
            {
                MyLog.Default.WriteLineAndConsole($"ClangSlayer: {Util.DebugName(piston)}: Detached top part due to heavy clang forces");
                piston.Detach();
                piston.Enabled = false;
                modified = true;
            }

            if (modified && Util.Debug)
            {
                Util.LogPoseDelta(Util.DebugName(piston), ref actualTopPose, ref expectedTopPose);
                MyLog.Default.WriteLineAndConsole($"  positionError={positionError:0.000} m");
                MyLog.Default.WriteLineAndConsole($"      axisError={axisError:0.000} degrees");
            }
        }
    }
}