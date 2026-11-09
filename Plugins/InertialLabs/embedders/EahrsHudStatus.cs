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
                    eahrsStatus = new InertialLabs.Forms.EAHRSStatus(this, 5);
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

        public void UpdateEahrsLabelColor()
        {
            if (PluginState.eahrsStatusResultValue >= 8)
            {
                eahrsLabel.BackColor = Color.DarkRed;
            }
            else if (PluginState.eahrsStatusResultValue >= 4)
            {
                eahrsLabel.BackColor = Color.Orange;
            }
            else if (PluginState.eahrsStatusResultValue >= 2)
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
                            MainV2.comPort.MAV.cs.parent != null &&
                            MainV2.comPort.MAV.cs.parent.parent.MAV.param.ContainsKey("EAHRS_TYPE") &&
                            MainV2.comPort.MAV.cs.parent.parent.MAV.param["EAHRS_TYPE"].Value == 5 /*Inertial Labs*/;
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

            uint calculatedStatus = (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.OK;

            // ILabs USW flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED; bitvalue = bitvalue << 1)
            {
                uint currentbit = (PluginState.eahrsStatusValue1 & bitvalue);
                var currentflag = (MAVLink.ILABS_EAHRS_STATUS_FLAGS)Enum.Parse(typeof(MAVLink.ILABS_EAHRS_STATUS_FLAGS), bitvalue.ToString());

                if (currentflag.ToString().StartsWith("EAHRS_RESERVED_BIT"))
                {
                    continue;
                }

                if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ENVIRONMENTAL_TEMPERATURE ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_LARGE_MAGNETIC_FIELD_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Z_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Y_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_X_ANGULAR_RATE_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_HIGH_POWER_SUPPLY ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_LOW_POWER_SUPPLY) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
                }
                else if (currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATION && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.INFO);
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GNSS_RECEIVER ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ELECTRONICS ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_MAGNETOMETER_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ACCELEROMETER_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GYROSCOPE_UNIT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_SOFTWARE_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INITIAL_ALIGNMENT) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
                }
            }

            // ILabs USW2 flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY; bitvalue = bitvalue << 1)
            {
                uint currentbit = (PluginState.eahrsStatusValue2 & bitvalue);
                var currentflag = (MAVLink.ILABS_EAHRS_STATUS_FLAGS2)Enum.Parse(typeof(MAVLink.ILABS_EAHRS_STATUS_FLAGS2), bitvalue.ToString());

                if (currentflag.ToString().StartsWith("EAHRS_RESERVED_BIT"))
                {
                    continue;
                }

                if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_X_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Y_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Z_ACCELERATION_EXCEEDING_DETECT ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_RECEIVER_INPUT_TO_THE_INS_ALGORITHM ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_INPUT_TO_THE_INS_ALGORITHM ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_2D_MAGNETOMETERS_CALIBRATION ||
                    currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_3D_MAGNETOMETERS_CALIBRATION) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.INFO);
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_BARO_ALTIMETER ||
                     currentflag == MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_SENSOR) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
                }
            }

            // ILabs EAHRS ADU flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD; bitvalue = bitvalue << 1)
            {
                uint currentbit = (PluginState.eahrsStatusValue3 & bitvalue);
                var currentflag = (MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS)Enum.Parse(typeof(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS), bitvalue.ToString());

                if (currentflag.ToString().StartsWith("EAHRS_ADU_RESERVED_BIT"))
                {
                    continue;
                }

                if ((currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_MEASUREMENT ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_MEASUREMENT ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_BAROMETRIC_TEMPERATURE) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
                }
                else if ((currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_INITIALIZATION ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_INITIALIZATION ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_STATUS ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_PRESSURE_ALTITUDE ||
                     currentflag == MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED) && currentbit != 0)
                {
                    calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
                }
            }

            // ILabs EAHRS GPS fix flag
            if (PluginState.eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.NO)
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
            }
            else if (PluginState.eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_2D ||
                 PluginState.eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_3D ||
                 PluginState.eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.OTHER)
            {
            }
            else
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
            }

            // ILabs EAHRS GPS spoofing flag
            if (PluginState.eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.INDICATED ||
                PluginState.eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.MULTIPLE_INDICATIONS)
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
            }
            else if (PluginState.eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.UNKNOWN_OR_DEACTIVATED ||
                 PluginState.eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.NO_INDICATED)
            {
            }
            else
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
            }

            PluginState.eahrsStatusResultValue = calculatedStatus;
            UpdateEahrsLabelColor();
        }
    }
}
