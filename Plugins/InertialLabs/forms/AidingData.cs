using MissionPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InertialLabs.Forms
{
    public partial class AidingData : Form
    {
        public AidingData()
        {
            InitializeComponent();
            MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(this);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if ((int)m.Result == HTCLIENT)
                {
                    // Click on the titleBar
                    m.Result = (IntPtr)HTCAPTION;
                    return;
                }
                return;
            }

            base.WndProc(ref m);
        }

        private void externalPositionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            externalPositionGroupBox.Enabled = externalPositionCheckBox.Checked;
        }

        private void externalHorizontalPositionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            externalHorizontalPositionGroupBox.Enabled = externalHorizontalPositionCheckBox.Checked;
        }

        private void altitudeExternalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            altitudeExternalGroupBox.Enabled = altitudeExternalCheckBox.Checked;
        }

        private void windDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            windDataGroupBox.Enabled = windDataCheckBox.Checked;
        }

        private void ambientAirDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ambientAirDataGroupBox.Enabled = ambientAirDataCheckBox.Checked;
        }

        private void headingExternalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            headingExternalGroupBox.Enabled = headingExternalCheckBox.Checked;
        }

        private void dvlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dvlGroupBox.Enabled = dvlCheckBox.Checked;
        }

        private void BUT_sendtoahrs_Click(object sender, EventArgs e)
        {
            var aiding = new List<(string Name, Func<bool> Send)>();

            if (externalPositionCheckBox.Checked)
            {
                aiding.Add((externalPositionCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_POSITION,
                    (float)latencyValueExtPos.Value,
                    (float)latitudeStdValueExtPos.Value,
                    (float)longitudeStdValueExtPos.Value,
                    (float)altitudeStdValueExtPos.Value,
                    ToDegE7(latitudeValueExtPos.Value),
                    ToDegE7(longitudeValueExtPos.Value),
                    (float)altitudeValueExtPos.Value)));
            }

            if (externalHorizontalPositionCheckBox.Checked)
            {
                aiding.Add((externalHorizontalPositionCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_HORIZONTAL_POSITION,
                    (float)latitudeStdValueExtHorPos.Value,
                    (float)longitudeStdValueExtHorPos.Value,
                    (float)latencyValueExtHorPos.Value,
                    0,
                    ToDegE7(latitudeValueExtHorPos.Value),
                    ToDegE7(longitudeValueExtHorPos.Value),
                    0)));
            }

            if (altitudeExternalCheckBox.Checked)
            {
                aiding.Add((altitudeExternalCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_ALTITUDE,
                    (float)altitudeValueStdAltExt.Value,
                    0, 0, 0, 0, 0,
                    (float)altitudeValueAltExt.Value)));
            }

            if (windDataCheckBox.Checked)
            {
                aiding.Add((windDataCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_WIND,
                    (float)WindDirectionValueWindData.Value,
                    (float)WindSpeedValueWindData.Value,
                    (float)WindSpeedStdValueWindData.Value,
                    0, 0, 0, 0)));
            }

            if (ambientAirDataCheckBox.Checked)
            {
                aiding.Add((ambientAirDataCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_AMBIENT_AIR,
                    (float)temperatureValueAmbientAirData.Value,
                    (float)pressureValueAmbientAirData.Value,
                    0, 0, 0, 0,
                    (float)altitudeValueAmbientAirData.Value)));
            }

            if (headingExternalCheckBox.Checked)
            {
                aiding.Add((headingExternalCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_HEADING,
                    (float)headingValueHeadingExternal.Value,
                    (float)headingStdValueHeadingExternal.Value,
                    (float)latencyValueHeadingExternal.Value,
                    0, 0, 0, 0)));
            }

            if (dvlCheckBox.Checked)
            {
                aiding.Add((dvlCheckBox.Text, () => SendAidingCommand(
                    InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_DVL,
                    (float)lateralVelocityDvl.Value * 1e3f,
                    (float)forwardVelocityDvl.Value * 1e3f,
                    (float)verticalVelocityDvl.Value * 1e3f,
                    (float)lateralVelocityStdDvl.Value * 1e3f,
                    (int)((float)forwardVelocityStdDvl.Value * 1e3f),
                    (int)((float)verticalVelocityStdDvl.Value * 1e3f),
                    (float)latencyDvl.Value * 1e3f)));
            }

            if (aiding.Count == 0)
            {
                return;
            }

            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }

            var report = new StringBuilder();
            bool isFailed = false;
            bool needShowResultDialog = false;
            foreach (var (name, send) in aiding)
            {
                if (isFailed)
                {
                    report.AppendLine(name + ": Skipped due to previous error");
                    continue;
                }

                if (!MainV2.comPort.BaseStream.IsOpen)
                {
                    report.AppendLine(name + ": Not sent, not connected");
                    isFailed = true;
                    needShowResultDialog = true;
                    continue;
                }

                try
                {
                    bool accepted = send();
                    report.AppendLine(name + (accepted ? ": Ok" : ": Rejected by MAV"));
                    if (!accepted)
                    {
                        needShowResultDialog = true;
                    }
                }
                catch (Exception ex)
                {
                    report.AppendLine(name + ": " + (ex is TimeoutException ? "No acknowledgement, result unknown" : ex.Message));
                    isFailed = true;
                    needShowResultDialog = true;
                }
            }

            if (needShowResultDialog)
            {
                CustomMessageBox.Show(report.ToString(), "Send to AHRS error");
            }
        }

        private static bool SendAidingCommand(InertialLabs.MAVLink.MAV_CMD command, float p1, float p2, float p3, float p4, int x, int y, float z)
        {
            return MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                               MainV2.comPort.MAV.compid,
                                               (global::MAVLink.MAV_CMD)command,
                                               p1, p2, p3, p4, x, y, z);
        }

        private static int ToDegE7(decimal degrees)
        {
            return (int)Math.Round(degrees * 10000000m, MidpointRounding.AwayFromZero);
        }

        private void BUT_close_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void BUT_uncheckall_Click(object sender, EventArgs e)
        {
            externalPositionCheckBox.Checked = false;
            externalHorizontalPositionCheckBox.Checked = false;
            altitudeExternalCheckBox.Checked = false;
            windDataCheckBox.Checked = false;
            ambientAirDataCheckBox.Checked = false;
            headingExternalCheckBox.Checked = false;
            dvlCheckBox.Checked = false;
        }
    }
}