using MissionPlanner;
using System;
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

        private void BUT_sendtoahrs_Click(object sender, EventArgs e)
        {
            /* send mavlink commands */
            try
            {
                if (externalPositionCheckBox.Checked)
                {
                    MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                MainV2.comPort.MAV.compid,
                                                (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_POSITION,
                                                (float)latencyValueExtPos.Value,
                                                (float)latitudeStdValueExtPos.Value,
                                                (float)longitudeStdValueExtPos.Value,
                                                (float)altitudeStdValueExtPos.Value,
                                                (int)((float)latitudeValueExtPos.Value * 1e7),
                                                (int)((float)longitudeValueExtPos.Value * 1e7),
                                                (float)altitudeValueExtPos.Value);
                }

                if (externalHorizontalPositionCheckBox.Checked)
                {
                    MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                MainV2.comPort.MAV.compid,
                                                (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_HORIZONTAL_POSITION,
                                                (float)latitudeStdValueExtHorPos.Value,
                                                (float)longitudeStdValueExtHorPos.Value,
                                                (float)latencyValueExtHorPos.Value,
                                                0,
                                                (int)((float)latitudeValueExtHorPos.Value * 1e7),
                                                (int)((float)longitudeValueExtHorPos.Value * 1e7),
                                                0);
                }

                if (altitudeExternalCheckBox.Checked)
                {
                    MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                MainV2.comPort.MAV.compid,
                                                (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_ALTITUDE,
                                                (float)altitudeValueStdAltExt.Value,
                                                0, 0, 0, 0, 0,
                                                (float)altitudeValueAltExt.Value);
                }

                if (windDataCheckBox.Checked)
                {
                    MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                MainV2.comPort.MAV.compid,
                                                (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_WIND,
                                                (float)WindDirectionValueWindData.Value,
                                                (float)WindSpeedValueWindData.Value,
                                                (float)WindSpeedStdValueWindData.Value,
                                                0, 0, 0, 0);
                }

                if (ambientAirDataCheckBox.Checked)
                {
                    MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                MainV2.comPort.MAV.compid,
                                                (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_AMBIENT_AIR,
                                                (float)temperatureValueAmbientAirData.Value,
                                                (float)pressureValueAmbientAirData.Value,
                                                0, 0, 0, 0,
                                                (float)altitudeValueAmbientAirData.Value);
                }

                if (headingExternalCheckBox.Checked)
                {
                    MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                MainV2.comPort.MAV.compid,
                                                (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_HEADING,
                                                (float)headingValueHeadingExternal.Value,
                                                (float)headingStdValueHeadingExternal.Value,
                                                (float)latencyValueHeadingExternal.Value,
                                                0, 0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
            }
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
        }
    }
}