using System;
using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class EAHRSStatus : Form
    {
        public EAHRSStatus()
        {
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

            int idx = 0;

            // ILabs USW flags
            for (int bitvalue = 1; bitvalue <= (int)MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED; bitvalue = bitvalue << 1)
            {
                int currentbit = (MainV2.comPort.MAV.cs.eahrsStatus & bitvalue);

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
                                                         (currentbit > 0 ? "1" : "0") + "\r\n";

                flowLayoutPanel1.Controls[idx].ForeColor = ForeColor;

                if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = Color.Green;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ENVIRONMENTAL_TEMPERATURE ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_LARGE_MAGNETIC_FIELD_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Z_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Y_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_X_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_HIGH_POWER_SUPPLY ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_LOW_POWER_SUPPLY)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.Orange : Color.White;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATION)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.DarkCyan : Color.White;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GNSS_RECEIVER ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ELECTRONICS ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_MAGNETOMETER_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ACCELEROMETER_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GYROSCOPE_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_SOFTWARE_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INITIAL_ALIGNMENT)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.Red : Color.White;
                }

                idx++;
            }

            // ILabs USW2 flags
            for (int bitvalue = 1; bitvalue <= (int)MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY; bitvalue = bitvalue << 1)
            {
                int currentbit = (MainV2.comPort.MAV.cs.eahrsStatus2 & bitvalue);

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

                if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_X_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Y_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Z_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_RECEIVER_INPUT_TO_THE_INS_ALGORITHM ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_INPUT_TO_THE_INS_ALGORITHM ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.Orange : Color.White;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_2D_MAGNETOMETERS_CALIBRATION ||
                    currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_3D_MAGNETOMETERS_CALIBRATION)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.DarkCyan : Color.White;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_BARO_ALTIMETER ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_SENSOR)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.Red : Color.White;
                }

                idx++;
            }

            // ILabs EAHRS ADU flags
            for (int bitvalue = 1; bitvalue <= (int)MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD; bitvalue = bitvalue << 1)
            {
                int currentbit = (MainV2.comPort.MAV.cs.eahrsStatus3 & bitvalue);

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

                if (currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_MEASUREMENT ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_BAROMETRIC_TEMPERATURE)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.Orange : Color.White;
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_INITIALIZATION ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_INITIALIZATION ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_PRESSURE_ALTITUDE ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED)
                {
                    flowLayoutPanel1.Controls[idx].ForeColor = currentbit == 1 ? Color.Red : Color.White;
                }

                idx++;
            }
        }
    }
}