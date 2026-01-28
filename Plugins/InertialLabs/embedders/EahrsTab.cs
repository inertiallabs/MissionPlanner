using BruTile.Wms;
using MissionPlanner;
using MissionPlanner.Attributes;
using System;
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

            // this needs to be set per "comport" - prevent any duplicates
            MainV2.comPort.OnPacketReceived -= MavlinkMessageHandler;
            MainV2.comPort.OnPacketReceived += MavlinkMessageHandler;

            return true;
        }

        private void EmbedInUi()
        {
            //
            // eahrsControl
            //
            eahrsControl = new InertialLabs.Controls.EAHRSControl(this);
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

            tabCtrl.ControlAdded += (_s, _e) =>
            {
                tabUpdateTimer.Stop();
                tabUpdateTimer.Start();
            };

            tabCtrl.ControlRemoved += (_s, _e) =>
            {
                tabUpdateTimer.Stop();
                tabUpdateTimer.Start();
            };
        }

        private void AddEahrsTabIfNeeded()
        {
            var tabCtrl = inertialLabsPluginRef.Host.MainForm.FlightData.tabControlactions;
            if (tabCtrl == null)
            {
                Console.WriteLine("tabControlactions not found in GCSViews.FlightData");
                return;
            }

            bool isTabAdded = tabCtrl.TabPages
                .Cast<TabPage>()
                .Any(tp => tp == tabExternalAHRS || tp.Name == "tabExternalAHRS");

            if (!isTabAdded)
            {
                tabCtrl.TabPages.Add(tabExternalAHRS);
                MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(tabExternalAHRS);
            }
        }

        private void MavlinkMessageHandler(object sender, global::MAVLink.MAVLinkMessage mavLinkMessage)
        {
            switch (mavLinkMessage.msgid)
            {
                case (uint)InertialLabs.MAVLink.MAVLINK_MSG_ID.AHRS_ADDITIONAL_RAW_INFO:
                    {
                        var ahrs_info = mavLinkMessage.ToStructure<MAVLink.mavlink_ahrs_additional_raw_info_t>();

                        PluginState.gps_lat_raw = ahrs_info.lat_raw * 1.0e-7;
                        PluginState.gps_lng_raw = ahrs_info.lon_raw * 1.0e-7;
                        PluginState.gps_alt_raw = ahrs_info.alt_raw * 1.0e-3;
                        PluginState.gps_track_over_ground_raw = ahrs_info.track_over_ground_raw * 1.0e-2;
                        PluginState.is_gps_raw_valid = ahrs_info.gps_raw_status == 0;
                        PluginState.ins_lat_accuracy = ahrs_info.ins_lat_accuracy * 1.0e-3;
                        PluginState.ins_lng_accuracy = ahrs_info.ins_lng_accuracy * 1.0e-3;
                        PluginState.ins_alt_accuracy = ahrs_info.ins_alt_accuracy * 1.0e-3;
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }
    }
}
