using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace InertialLabs.Embedders
{
    public class EahrsTab : EmbedderInterface
    {
        private InertialLabsPlugin inertialLabsPluginRef;
        private Timer loadWaitTimer;
        private Timer tabUpdateTimer;

        private InertialLabs.Controls.EAHRSControl eahrsControl;
        private System.Windows.Forms.TabPage tabExternalAHRS;

        public EahrsTab(InertialLabsPlugin inertialLabsPlugin)
        {
            inertialLabsPluginRef = inertialLabsPlugin;
            loadWaitTimer = new Timer();
            tabUpdateTimer = new Timer();
        }

        public override bool Init()
        {
            EmbedInUi();
            return true;
        }

        public override void Exit()
        {
            loadWaitTimer.Dispose();
            tabUpdateTimer.Dispose();

            var tabCtrl = inertialLabsPluginRef.Host.MainForm.FlightData.tabControlactions;
            tabCtrl.ControlAdded -= TabControlActions_ControlsChanged;
            tabCtrl.ControlRemoved -= TabControlActions_ControlsChanged;

            eahrsControl.Dispose();
        }

        private void EmbedInUi()
        {
            //
            // eahrsControl
            //
            eahrsControl = new InertialLabs.Controls.EAHRSControl();
            eahrsControl.Name = "eahrsControl1";

            //
            // tabExternalAHRS
            //
            tabExternalAHRS = new System.Windows.Forms.TabPage();
            tabExternalAHRS.Controls.Add(this.eahrsControl);
            tabExternalAHRS.Name = "tabExternalAHRS";
            tabExternalAHRS.Text = "External AHRS";
            tabExternalAHRS.UseVisualStyleBackColor = true;

            eahrsControl.ResumeLayout(false);
            eahrsControl.PerformLayout();
            tabExternalAHRS.ResumeLayout(false);
            tabExternalAHRS.PerformLayout();

            // Wait the full loading of the original UI
            loadWaitTimer.Interval = 1000;
            loadWaitTimer.Tick += (s, e) =>
            {
                var tabCtrl = inertialLabsPluginRef.Host.MainForm.FlightData.tabControlactions;
                if (tabCtrl != null)
                {
                    loadWaitTimer.Stop();
                    SetupTabWatcher();
                    AddEahrsTabIfNeeded();
                }
            };
            loadWaitTimer.Start();
        }

        private void SetupTabWatcher()
        {
            // delay in tab addition. To be shure, EahrsTab will be the last.
            var tabCtrl = inertialLabsPluginRef.Host.MainForm.FlightData.tabControlactions;

            tabUpdateTimer.Interval = 100;
            tabUpdateTimer.Tick += (s, e) =>
            {
                tabUpdateTimer.Stop();
                AddEahrsTabIfNeeded();
            };

            tabCtrl.ControlAdded += TabControlActions_ControlsChanged;
            tabCtrl.ControlRemoved += TabControlActions_ControlsChanged;
        }

        private void TabControlActions_ControlsChanged(object sender, ControlEventArgs e)
        {
            tabUpdateTimer.Stop();
            tabUpdateTimer.Start();
        }

        private void AddEahrsTabIfNeeded()
        {
            var tabCtrl = inertialLabsPluginRef.Host.MainForm.FlightData.tabControlactions;

            bool isTabAdded = tabCtrl.TabPages
                .Cast<TabPage>()
                .Any(tp => tp == tabExternalAHRS || tp.Name == "tabExternalAHRS");

            if (!isTabAdded)
            {
                tabCtrl.TabPages.Add(tabExternalAHRS);
                MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(tabExternalAHRS);
            }
        }
    }
}
