using VRage.Game.Components;

// ReSharper disable once CheckNamespace
namespace ClangSlayer
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ClangSlayerSession : MySessionComponentBase
    {
        public static Config Cfg;
        
        public override void LoadData()
        {
            Cfg = new Config();
            base.LoadData();
        }

        protected override void UnloadData()
        {
            base.UnloadData();
            Cfg = null;
        }
    }
}