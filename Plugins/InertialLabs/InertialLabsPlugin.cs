using MissionPlanner;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace InertialLabs
{
    public class InertialLabsPlugin : MissionPlanner.Plugin.Plugin
    {
        private Embedders.MAVLinkMessages mavLinkMessagesEmbedder;
        private Embedders.EahrsTab eahrsTabEmbedder;
        private Embedders.EahrsHudStatus eahrsHudStatusEmbedder;
        private Embedders.Map mapEmbedder;

        public override string Name => "Inertial Labs Plugin";
        public override string Version => "0.1";
        public override string Author => "Valentin Bugrov";

        public override bool Init() => true;

        public override bool Loaded()
        {
            mavLinkMessagesEmbedder = new Embedders.MAVLinkMessages();
            mavLinkMessagesEmbedder.Init();

            eahrsTabEmbedder = new Embedders.EahrsTab(this);
            eahrsTabEmbedder.Init();

            eahrsHudStatusEmbedder = new Embedders.EahrsHudStatus(this);
            eahrsHudStatusEmbedder.Init();

            mapEmbedder = new Embedders.Map(this);
            mapEmbedder.Init();

            return true;
        }

        public override bool Loop() => true;

        public override bool Exit() => true;
    };
}
