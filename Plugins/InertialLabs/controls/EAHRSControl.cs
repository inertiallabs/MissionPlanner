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
        private InertialLabs.Embedders.EahrsTab eahrsTab;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private bool isEahrsStoped = false;
        private InertialLabs.Forms.AidingData aidingDataDialog = new InertialLabs.Forms.AidingData();

        public EAHRSControl(InertialLabs.Embedders.EahrsTab eahrsTabRef)
        {
            eahrsTab = eahrsTabRef;
            InitializeComponent();
            this.VisibleChanged += EAHRSControl_VisibleChanged;
        }

        private void EAHRSControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (!isEahrsStoped)
                {
                    updateTimer.Start();
                }
            }
            else
            {
                updateTimer.Stop();
                resetInsGnssPosDiffValues();
                resetInsPosAccuracyValues();
            }
        }

        private void BUT_externalAHRS_gnss_enable_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                            MainV2.comPort.MAV.compid,
                                            (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_ENABLE_GNSS,
                                            0, 0, 0, 0, 0, 0, 0);

                updateTimer.Start();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_gnss_disable_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                            MainV2.comPort.MAV.compid,
                                            (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_DISABLE_GNSS,
                                            0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_vg3dclb_flight_start_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                            MainV2.comPort.MAV.compid,
                                            (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_START_VG3D_CALIBRATION_IN_FLIGHT,
                                            0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_vg3dclb_flight_stop_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                            MainV2.comPort.MAV.compid,
                                            (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_STOP_VG3D_CALIBRATION_IN_FLIGHT,
                                            0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_start_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                            MainV2.comPort.MAV.compid,
                                            (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_START_UDD,
                                            0, 0, 0, 0, 0, 0, 0);

                resetInsGnssPosDiffValues();
                resetInsPosAccuracyValues();
                updateTimer.Start();
                isEahrsStoped = false;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_stop_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.Show("Attention! External AHRS will be stopped.\nAre you Sure?",
                    "Are you sure?", CustomMessageBox.MessageBoxButtons.OKCancel) !=
                    CustomMessageBox.DialogResult.OK)
            {
                return;
            }
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid,
                                            MainV2.comPort.MAV.compid,
                                            (global::MAVLink.MAV_CMD)InertialLabs.MAVLink.MAV_CMD.EXTERNAL_AHRS_STOP,
                                            0, 0, 0, 0, 0, 0, 0);

                updateTimer.Stop();
                resetInsGnssPosDiffValues();
                resetInsPosAccuracyValues();
                isEahrsStoped = true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_aiding_data_Click(object sender, EventArgs e)
        {
            ThemeManager.ApplyThemeTo(aidingDataDialog);
            aidingDataDialog.Show();
        }

        private void CB_gnss_position_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_gnss_position.Checked && !eahrsTab.is_gps_raw_valid)
            {
                log.Warn("Can't enable GPS position. Invalid GPS status.");
            }

            bool needEnable = CB_gnss_position.Checked && eahrsTab.is_gps_raw_valid;
            CB_gnss_position.Checked = needEnable;
            eahrsTab.show_gps_raw_location = needEnable;
        }

        private void CB_ins_pos_estimation_CheckedChanged(object sender, EventArgs e)
        {
            eahrsTab.show_ins_pos_estimation = CB_ins_pos_estimation.Checked;
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
            if (eahrsTab.is_gps_raw_valid)
            {
                calculateInsGnssPosDiff();
            }
            else
            {
                resetInsGnssPosDiffValues();
            }

            quickView4.number = eahrsTab.ins_lat_accuracy;
            quickView5.number = eahrsTab.ins_lng_accuracy;
            quickView6.number = -1 * eahrsTab.ins_alt_accuracy;

            BUT_externalAHRS_start.Enabled = !MainV2.comPort.MAV.cs.armed;
            BUT_externalAHRS_stop.Enabled = !MainV2.comPort.MAV.cs.armed;
        }

        private void updateGcsDistanceAround()
        {
            if (CB_gcs_distance.Checked)
            {
                eahrsTab.gcs_distance_around = (uint)NUD_gcs_distance_around.Value;
            }
            else
            {
                eahrsTab.gcs_distance_around = 0;
            }
        }

        private void calculateInsGnssPosDiff()
        {
            const int r = 6371000; // constant for conversion from Spherical to Cartesian coordinates

            if (eahrsTab.gps_lat_raw != 0)
            {
                double delta_lat = (MainV2.comPort.MAV.cs.lat - eahrsTab.gps_lat_raw) * Math.PI / 180;
                quickView1.number = delta_lat * r;
            }
            else
            {
                quickView1.number = 0D;
            }

            if (eahrsTab.gps_lat_raw != 0 && eahrsTab.gps_lng_raw != 0)
            {
                double delta_lng = (MainV2.comPort.MAV.cs.lng - eahrsTab.gps_lng_raw) * Math.PI / 180;
                quickView2.number = delta_lng * r * Math.Cos(eahrsTab.gps_lat_raw * Math.PI / 180);
            }
            else
            {
                quickView2.number = 0D;
            }

            if (eahrsTab.gps_alt_raw != 0)
            {
                double delta_alt = MainV2.comPort.MAV.cs.altasl - eahrsTab.gps_alt_raw;
                quickView3.number = -1 * delta_alt;
            }
            else
            {
                quickView3.number = 0D;
            }
        }

        private void resetInsGnssPosDiffValues()
        {
            quickView1.number = 0D;
            quickView2.number = 0D;
            quickView3.number = 0D;
        }

        private void resetInsPosAccuracyValues()
        {
            quickView4.number = 0D;
            quickView5.number = 0D;
            quickView6.number = 0D;
        }
    }
}
