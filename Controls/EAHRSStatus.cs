using System;
using System.Drawing;
using System.Windows.Forms;
using static MAVLink;

namespace MissionPlanner.Controls
{
    public partial class EAHRSStatus : Form
    {
        private double eahrsType;

        public EAHRSStatus(double eahrsType)
        {
            this.eahrsType = eahrsType;

            InitializeComponent();

            Utilities.ThemeManager.ApplyThemeTo(this);

            timer1.Start();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // restore colours
            Utilities.ThemeManager.ApplyThemeTo(this);

            if (this.eahrsType != 5 /*Inertial Labs EAHRS*/)
            {
                return;
            }

            int idx = 0;

            // ILabs USW flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED; bitvalue = bitvalue << 1)
            {
                uint currentbit = (MainV2.comPort.MAV.cs.eahrsStatusValue1 & bitvalue);
                var currentflag = (MAVLink.ILABS_EAHRS_STATUS_FLAGS)Enum.Parse(typeof(MAVLink.ILABS_EAHRS_STATUS_FLAGS), bitvalue.ToString());

                if (currentflag.ToString().StartsWith("EAHRS_RESERVED_BIT"))
                {
                    continue;
                }

                if (flowLayoutPanel1.Controls.Count <= idx)
                {
                    flowLayoutPanel1.Controls.Add(new Label() { Height = 13, Width = flowLayoutPanel1.Width });
                }

                flowLayoutPanel1.Controls[idx].Text = currentflag.ToString().Replace("EAHRS_", "").ToLower() + " " +
                                                         (currentbit != 0 ? "1" : "0") + "\r\n";

                flowLayoutPanel1.Controls[idx].ForeColor = ForeColor;

                if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Green;
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ENVIRONMENTAL_TEMPERATURE ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_LARGE_MAGNETIC_FIELD_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Z_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Y_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_X_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_HIGH_POWER_SUPPLY ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_LOW_POWER_SUPPLY) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Orange;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATION && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.DarkCyan;
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GNSS_RECEIVER ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ELECTRONICS ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_MAGNETOMETER_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ACCELEROMETER_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GYROSCOPE_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_SOFTWARE_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INITIAL_ALIGNMENT) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Red;
                }

                idx++;
            }

            // ILabs USW2 flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY; bitvalue = bitvalue << 1)
            {
                uint currentbit = (MainV2.comPort.MAV.cs.eahrsStatusValue2 & bitvalue);
                var currentflag = (MAVLink.ILABS_EAHRS_STATUS_FLAGS2)Enum.Parse(typeof(MAVLink.ILABS_EAHRS_STATUS_FLAGS2), bitvalue.ToString());

                if (currentflag.ToString().StartsWith("EAHRS_RESERVED_BIT"))
                {
                    continue;
                }

                if (flowLayoutPanel1.Controls.Count <= idx)
                {
                    flowLayoutPanel1.Controls.Add(new Label() { Height = 13, Width = flowLayoutPanel1.Width });
                }

                flowLayoutPanel1.Controls[idx].Text = currentflag.ToString().Replace("EAHRS_", "").ToLower() + " " +
                                                         (currentbit > 0 ? "1" : "0") + "\r\n";

                flowLayoutPanel1.Controls[idx].ForeColor = ForeColor;

                if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_X_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Y_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Z_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_RECEIVER_INPUT_TO_THE_INS_ALGORITHM ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_INPUT_TO_THE_INS_ALGORITHM ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Orange;
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_2D_MAGNETOMETERS_CALIBRATION ||
                    currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_3D_MAGNETOMETERS_CALIBRATION) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.DarkCyan;
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_BARO_ALTIMETER ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_SENSOR) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Red;
                }

                idx++;
            }

            // ILabs EAHRS ADU flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD; bitvalue = bitvalue << 1)
            {
                uint currentbit = (MainV2.comPort.MAV.cs.eahrsStatusValue3 & bitvalue);
                var currentflag = (MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS)Enum.Parse(typeof(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS), bitvalue.ToString());

                if (currentflag.ToString().StartsWith("EAHRS_ADU_RESERVED_BIT"))
                {
                    continue;
                }

                if (flowLayoutPanel1.Controls.Count <= idx)
                {
                    flowLayoutPanel1.Controls.Add(new Label() { Height = 13, Width = flowLayoutPanel1.Width });
                }

                flowLayoutPanel1.Controls[idx].Text = currentflag.ToString().Replace("EAHRS_", "").ToLower() + " " +
                                                         (currentbit > 0 ? "1" : "0") + "\r\n";

                flowLayoutPanel1.Controls[idx].ForeColor = ForeColor;

                if ((currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_MEASUREMENT ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_MEASUREMENT ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_BAROMETRIC_TEMPERATURE) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Orange;
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_INITIALIZATION ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_INITIALIZATION ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_PRESSURE_ALTITUDE ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED) && currentbit != 0)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Red;
                }

                idx++;
            }

            // ILabs EAHRS GPS fix flag
            uint statusValue = MainV2.comPort.MAV.cs.eahrsStatusValue4;
            if (flowLayoutPanel1.Controls.Count <= idx)
            {
                flowLayoutPanel1.Controls.Add(new Label() { Height = 13, Width = flowLayoutPanel1.Width });
            }
            flowLayoutPanel1.Controls[idx].Text = "gps_fix " + statusValue + "\r\n";
            flowLayoutPanel1.Controls[idx].ForeColor = ForeColor;

            if (statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.NO)
            {
                flowLayoutPanel1.Controls[idx].ForeColor = Color.Orange;
            }
            else if (statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_2D ||
                 statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_3D ||
                 statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.OTHER)
            {
                flowLayoutPanel1.Controls[idx].ForeColor = Color.Green;
            }
            else
            {
                flowLayoutPanel1.Controls[idx].ForeColor = Color.Red;
            }

            idx++;

            // ILabs EAHRS GPS spoofing flag
            statusValue = MainV2.comPort.MAV.cs.eahrsStatusValue5;
            if (flowLayoutPanel1.Controls.Count <= idx)
            {
                flowLayoutPanel1.Controls.Add(new Label() { Height = 13, Width = flowLayoutPanel1.Width });
            }
            flowLayoutPanel1.Controls[idx].Text = "gps_spoofing_indicated " + statusValue + "\r\n";
            flowLayoutPanel1.Controls[idx].ForeColor = ForeColor;

            if (statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.INDICATED ||
                statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.MULTIPLE_INDICATIONS)
            {
                flowLayoutPanel1.Controls[idx].ForeColor = Color.Orange;
            }
            else if (statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.UNKNOWN_OR_DEACTIVATED ||
                 statusValue == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.NO_INDICATED)
            {
                flowLayoutPanel1.Controls[idx].ForeColor = Color.Green;
            }
            else
            {
                flowLayoutPanel1.Controls[idx].ForeColor = Color.Red;
            }

            idx++;
        }
    }
}