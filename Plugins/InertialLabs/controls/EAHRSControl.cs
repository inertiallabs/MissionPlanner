using log4net;
using MissionPlanner;
using MissionPlanner.Utilities;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace InertialLabs.Controls
{
    public partial class EAHRSControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private InertialLabs.Forms.AidingData aidingDataDialog = new InertialLabs.Forms.AidingData();

        public EAHRSControl()
        {
            InitializeComponent();
            this.VisibleChanged += EAHRSControl_VisibleChanged;
            this.Disposed += (s, e) => aidingDataDialog.Dispose();
        }

        private void EAHRSControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                updateTimer.Start();
                updateTimer_Tick(this, EventArgs.Empty);
            }
            else
            {
                updateTimer.Stop();
            }
        }

        private void BUT_externalAHRS_gnss_enable_Click(object sender, EventArgs e)
        {
            SendCommand(InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_ENABLE_GNSS);
        }

        private void BUT_externalAHRS_gnss_disable_Click(object sender, EventArgs e)
        {
            SendCommand(InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_DISABLE_GNSS);
        }

        private void BUT_externalAHRS_vg3dclb_flight_start_Click(object sender, EventArgs e)
        {
            SendCommand(InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_START_VG3D_CALIBRATION_IN_FLIGHT);
        }

        private void BUT_externalAHRS_vg3dclb_flight_stop_Click(object sender, EventArgs e)
        {
            SendCommand(InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_STOP_VG3D_CALIBRATION_IN_FLIGHT);
        }

        private void BUT_externalAHRS_start_Click(object sender, EventArgs e)
        {
            if (ConfirmCommand("External AHRS will be started."))
            {
                SendCommand(InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_START_UDD);
            }
        }

        private void BUT_externalAHRS_stop_Click(object sender, EventArgs e)
        {
            if (ConfirmCommand("External AHRS will be stopped."))
            {
                SendCommand(InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_STOP);
            }
        }

        private static bool ConfirmCommand(string action)
        {
            string armedWarning = MainV2.comPort.MAV.cs.armed ? "\nThe vehicle is ARMED!" : "";
            return CustomMessageBox.Show(action + armedWarning + "\nAre you sure?",
                       "Are you sure?", CustomMessageBox.MessageBoxButtons.OKCancel) ==
                   CustomMessageBox.DialogResult.OK;
        }

        private static void SendCommand(InertialLabs.MAVLink.MAV_CMD command)
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }

            try
            {
                if (!MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                                 MainV2.comPort.MAV.compid,
                                                 (global::MAVLink.MAV_CMD)command,
                                                 0, 0, 0, 0, 0, 0, 0))
                {
                    CustomMessageBox.Show(Strings.ErrorRejectedByMAV, Strings.ERROR);
                }
            }
            catch (TimeoutException ex)
            {
                log.Error("Command " + command + ": no acknowledgement, the result is unknown", ex);
                CustomMessageBox.Show("No acknowledgement from the vehicle: the command may or may not have been executed. Check the External AHRS status.", Strings.ERROR);
            }
            catch (Exception ex)
            {
                log.Error("Command " + command + " failed", ex);
                CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_aiding_data_Click(object sender, EventArgs e)
        {
            ThemeManager.ApplyThemeTo(aidingDataDialog);
            aidingDataDialog.Show();
        }

        private void CB_gnss_position_CheckedChanged(object sender, EventArgs e)
        {
            PluginState.show_gps_raw_location = CB_gnss_position.Checked;
        }

        private void CB_ins_pos_estimation_CheckedChanged(object sender, EventArgs e)
        {
            PluginState.show_ins_pos_estimation = CB_ins_pos_estimation.Checked;
        }

        private void CB_gcs_distance_CheckedChanged(object sender, EventArgs e)
        {
            updateGcsDistanceAround();
        }

        private void NUD_gcs_distance_around_ValueChanged(object sender, EventArgs e)
        {
            updateGcsDistanceAround();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (PluginState.IsAhrsAdditionalRawInfoFresh)
            {
                if (PluginState.is_gps_raw_valid)
                {
                    calculateInsGnssPosDiff();
                }
                else
                {
                    resetInsGnssPosDiffValues();
                }

                quickView4.number = PluginState.ins_lat_accuracy;
                quickView5.number = PluginState.ins_lng_accuracy;
                quickView6.number = PluginState.ins_alt_accuracy;
            }
            else
            {
                // No fresh AHRS_ADDITIONAL_RAW_INFO: show NaN, neither the last values nor zeros are current
                resetInsGnssPosDiffValues();
                resetInsPosAccuracyValues();
            }
        }

        private void updateGcsDistanceAround()
        {
            PluginState.show_gcs_distance_around = CB_gcs_distance.Checked;
            PluginState.gcs_distance_around = (uint)NUD_gcs_distance_around.Value;
        }

        private void calculateInsGnssPosDiff()
        {
            const int r = 6371000; // constant for conversion from Spherical to Cartesian coordinates

            if (MainV2.comPort.MAV.cs.lat == 0 || MainV2.comPort.MAV.cs.lng == 0)
            {
                resetInsGnssPosDiffValues();
                return;
            }

            if (PluginState.gps_lat_raw != 0)
            {
                double delta_lat = (MainV2.comPort.MAV.cs.lat - PluginState.gps_lat_raw) * Math.PI / 180;
                quickView1.number = delta_lat * r;
            }
            else
            {
                quickView1.number = double.NaN;
            }

            if (PluginState.gps_lat_raw != 0 && PluginState.gps_lng_raw != 0)
            {
                double delta_lng = (MainV2.comPort.MAV.cs.lng - PluginState.gps_lng_raw) * Math.PI / 180;
                quickView2.number = delta_lng * r * Math.Cos(PluginState.gps_lat_raw * Math.PI / 180);
            }
            else
            {
                quickView2.number = double.NaN;
            }

            if (PluginState.gps_alt_raw != 0)
            {
                double delta_alt = MainV2.comPort.MAV.cs.altasl / CurrentState.multiplieralt - PluginState.gps_alt_raw;
                quickView3.number = -1 * delta_alt;
            }
            else
            {
                quickView3.number = double.NaN;
            }
        }

        // QuickView shows NaN as the culture's NaN symbol, i.e. "no data"
        private void resetInsGnssPosDiffValues()
        {
            quickView1.number = double.NaN;
            quickView2.number = double.NaN;
            quickView3.number = double.NaN;
        }

        private void resetInsPosAccuracyValues()
        {
            quickView4.number = double.NaN;
            quickView5.number = double.NaN;
            quickView6.number = double.NaN;
        }
    }
}
