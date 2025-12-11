using MissionPlanner;
using MissionPlanner.Attributes;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace InertialLabs.Embedders
{
    public class EahrsHudStatus : EmbedderInterface
    {
        public uint eahrsStatusResultValue { get; set; }
        public uint eahrsStatusValue1 { get; set; }
        public uint eahrsStatusValue2 { get; set; }
        public uint eahrsStatusValue3 { get; set; }
        public uint eahrsStatusValue4 { get; set; }
        public uint eahrsStatusValue5 { get; set; }

        InertialLabs.Forms.EAHRSStatus eahrsStatus;

        public override bool Init()
        {
            // TODO: remove Hardcode 5
            eahrsStatus = new InertialLabs.Forms.EAHRSStatus(this, 5);

            // this needs to be set per "comport" - prevent any duplicates
            MainV2.comPort.OnPacketReceived -= MavlinkMessageHandler;
            MainV2.comPort.OnPacketReceived += MavlinkMessageHandler;

            return true;
        }

        private void MavlinkMessageHandler(object sender, global::MAVLink.MAVLinkMessage mavLinkMessage)
        {
            switch (mavLinkMessage.msgid)
            {
                case (uint)MAVLink.MAVLINK_MSG_ID.EAHRS_STATUS_INFO:
                    {
                        var status = mavLinkMessage.ToStructure<MAVLink.mavlink_eahrs_status_info_t>();
                        eahrsStatusValue1 = status.status1;
                        eahrsStatusValue2 = status.status2;
                        eahrsStatusValue3 = status.status3;
                        eahrsStatusValue4 = status.status4;
                        eahrsStatusValue5 = status.status5;
                        UpdateEahrsStatus();
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }

        public void UpdateEahrsStatus()
        {
            try
            {
                if (MainV2.comPort.MAV.cs.parent == null ||
                    !MainV2.comPort.MAV.cs.parent.parent.MAV.param.ContainsKey("EAHRS_TYPE") ||
                    MainV2.comPort.MAV.cs.parent.parent.MAV.param["EAHRS_TYPE"].Value != 5 /*Inertial Labs*/)
                {
                    return;
                }
            }
            catch
            {
                return;
            }

            uint calculatedStatus = (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.OK;

            // ILabs USW flags
            for (uint bitvalue = 1; bitvalue <= (uint)MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED; bitvalue = bitvalue << 1)
            {
                uint currentbit = (eahrsStatusValue1 & bitvalue);
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
                uint currentbit = (eahrsStatusValue2 & bitvalue);
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
                uint currentbit = (eahrsStatusValue3 & bitvalue);
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
            if (eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.NO)
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
            }
            else if (eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_2D ||
                 eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_3D ||
                 eahrsStatusValue4 == (uint)MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.OTHER)
            {
            }
            else
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
            }

            // ILabs EAHRS GPS spoofing flag
            if (eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.INDICATED ||
                eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.MULTIPLE_INDICATIONS)
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.WARNING);
            }
            else if (eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.UNKNOWN_OR_DEACTIVATED ||
                 eahrsStatusValue5 == (uint)MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.NO_INDICATED)
            {
            }
            else
            {
                calculatedStatus = (calculatedStatus | (uint)MAVLink.EAHRS_COMMON_STATUS_FLAGS.FAIL);
            }

            eahrsStatusResultValue = calculatedStatus;
        }
    }
}
