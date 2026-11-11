using MissionPlanner;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace InertialLabs.Embedders
{
    public class EahrsHudStatus : EmbedderInterface
    {
        internal static readonly Color NoDataColor = Color.Gray;

        private InertialLabsPlugin inertialLabsPluginRef;
        private Timer loadWaitTimer;
        private Timer statusUpdateTimer;
        private System.Windows.Forms.Label eahrsLabel;
        private Font eahrsLabelFont;

        InertialLabs.Forms.EAHRSStatus eahrsStatus;

        public EahrsHudStatus(InertialLabsPlugin inertialLabsPlugin)
        {
            inertialLabsPluginRef = inertialLabsPlugin;
            loadWaitTimer = new Timer();
            statusUpdateTimer = new Timer();
        }

        public override bool Init()
        {
            EmbedInUi();
            return true;
        }

        public override void Exit()
        {
            loadWaitTimer.Dispose();
            statusUpdateTimer.Dispose();
            MissionPlanner.GCSViews.FlightData.myhud.Resize -= Hud_Resize;

            eahrsStatus?.Dispose();

            eahrsLabel.Dispose();
            eahrsLabelFont?.Dispose();
        }

        private void EmbedInUi()
        {
            eahrsLabel = new System.Windows.Forms.Label
            {
                Name = "eahrsLabel",
                Tag = "custom",
                Text = "EAHRS",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = NoDataColor,
                ForeColor = Color.White,
                AutoSize = true,
                ImeMode = ImeMode.NoControl,
                Cursor = Cursors.Hand,
            };

            eahrsLabel.Click += (obj, eventArgs) =>
            {
                if (eahrsStatus == null || eahrsStatus.IsDisposed)
                {
                    eahrsStatus = new InertialLabs.Forms.EAHRSStatus();
                    eahrsStatus.Show();
                }
                else
                {
                    eahrsStatus.BringToFront();
                }
            };

            // Wait the full loading of the original UI
            loadWaitTimer.Interval = 1000;
            loadWaitTimer.Tick += (s, e) =>
            {
                var flightData = inertialLabsPluginRef.Host.MainForm.FlightData;
                if (flightData != null)
                {
                    loadWaitTimer.Stop();

                    var hud = MissionPlanner.GCSViews.FlightData.myhud;

                    UpdateEahrsLabelLayout(hud);
                    hud.Resize += Hud_Resize;

                    hud.Controls.Add(eahrsLabel);
                    eahrsLabel.BringToFront();
                }
            };
            loadWaitTimer.Start();

            statusUpdateTimer.Interval = 500;
            statusUpdateTimer.Tick += (s, e) => UpdateEahrsStatus();
            statusUpdateTimer.Start();
        }

        private void Hud_Resize(object sender, EventArgs e)
        {
            UpdateEahrsLabelLayout((Control)sender);
        }

        private void UpdateEahrsLabelLayout(Control hud)
        {
            int fontsize = Math.Max(1, hud.Height / 30);
            int fontoffset = fontsize - 10;

            var oldFont = eahrsLabelFont;
            eahrsLabelFont = new Font(eahrsLabel.Font.FontFamily, fontsize, FontStyle.Bold);
            eahrsLabel.Font = eahrsLabelFont;
            oldFont?.Dispose();

            eahrsLabel.Location = new Point(
                (hud.Width - eahrsLabel.PreferredWidth) / 2,
                hud.Height - eahrsLabel.PreferredHeight - 4 * fontsize - 3 * fontoffset
            );
        }

        private void UpdateEahrsLabelColor(EahrsSeverity mostSevere)
        {
            if (mostSevere >= EahrsSeverity.Fail)
            {
                eahrsLabel.BackColor = Color.DarkRed;
            }
            else if (mostSevere >= EahrsSeverity.Warning)
            {
                eahrsLabel.BackColor = Color.Orange;
            }
            else if (mostSevere >= EahrsSeverity.Info)
            {
                eahrsLabel.BackColor = Color.DarkCyan;
            }
            else
            {
                eahrsLabel.BackColor = Color.Green;
            }
        }

        public void UpdateEahrsStatus()
        {
            bool hasStatus;
            try
            {
                hasStatus = PluginState.IsEahrsStatusInfoFresh &&
                            MainV2.comPort.MAV.param.ContainsKey("EAHRS_TYPE") &&
                            MainV2.comPort.MAV.param["EAHRS_TYPE"].Value == 5 /*Inertial Labs*/;
            }
            catch
            {
                hasStatus = false;
            }

            if (!hasStatus)
            {
                eahrsLabel.BackColor = NoDataColor;
                return;
            }

            var mostSevere = EahrsSeverity.None;
            foreach (var row in EahrsStatusTable.GetStatusRows())
            {
                if (row.Severity > mostSevere)
                {
                    mostSevere = row.Severity;
                }
            }

            UpdateEahrsLabelColor(mostSevere);
        }
    }
}
