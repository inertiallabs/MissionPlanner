using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace InertialLabs {

public partial class MAVLink
{
    public class Units : Attribute
    {
        public Units(string unit)
        {
            Unit = unit;
        }

        public string Unit { get; set; }
    }

    public class Description : Attribute
    {
        public Description(string desc)
        {
            Text = desc;
        }

        public string Text { get; set; }
    }

    public class hasLocation : Attribute
    {
        public hasLocation()
        {
        }
    }

    public enum MAVLINK_MSG_ID
    {
        AHRS_ADDITIONAL_RAW_INFO = 699,
        EAHRS_STATUS_INFO = 700,
    }

    ///<summary> Flags for the EAHRS common status. </summary>
    [Flags]
	public enum EAHRS_COMMON_STATUS_FLAGS: int /*default*/
    {
        ///<summary> Ok. | </summary>
        [Description("Ok.")]
        OK=1,
        ///<summary> Info. | </summary>
        [Description("Info.")]
        INFO=2,
        ///<summary> Warning. | </summary>
        [Description("Warning.")]
        WARNING=4,
        ///<summary> Fail. | </summary>
        [Description("Fail.")]
        FAIL=8,
    };

    ///<summary> Flags in ILABS_EAHRS_STATUS message. </summary>
    [Flags]
	public enum ILABS_EAHRS_STATUS_FLAGS: int /*default*/
    {
        ///<summary> 0 - ok. 1 - unsuccessful initial alignment due to INS movement or large change of outer magnetic field. | </summary>
        [Description("0 - ok. 1 - unsuccessful initial alignment due to INS movement or large change of outer magnetic field.")]
        EAHRS_INITIAL_ALIGNMENT=1,
        ///<summary> 0 - ok. 1 - incorrect data appeared at calculations. | </summary>
        [Description("0 - ok. 1 - incorrect data appeared at calculations.")]
        EAHRS_SOFTWARE_STATUS=2,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_GYROSCOPE_UNIT=4,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_ACCELEROMETER_UNIT=8,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_MAGNETOMETER_UNIT=16,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_ELECTRONICS=32,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_GNSS_RECEIVER=64,
        ///<summary> 1 - during data accumulation and calculation. 0 - otherwise. | </summary>
        [Description("1 - during data accumulation and calculation. 0 - otherwise.")]
        EAHRS_ON_THE_FLY_CALIBRATION=128,
        ///<summary> 0 - ok. 1 - low supply voltage detected. | </summary>
        [Description("0 - ok. 1 - low supply voltage detected.")]
        EAHRS_INCORRECT_LOW_POWER_SUPPLY=256,
        ///<summary> 0 - ok. 1 - high supply voltage detected. | </summary>
        [Description("0 - ok. 1 - high supply voltage detected.")]
        EAHRS_INCORRECT_HIGH_POWER_SUPPLY=512,
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_X_ANGULAR_RATE_EXCEEDING_DETECT=1024,
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_Y_ANGULAR_RATE_EXCEEDING_DETECT=2048,
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_Z_ANGULAR_RATE_EXCEEDING_DETECT=4096,
        ///<summary> 0 - ok. 1 - total magnetic field limit is exceeded. | </summary>
        [Description("0 - ok. 1 - total magnetic field limit is exceeded.")]
        EAHRS_LARGE_MAGNETIC_FIELD_DETECT=8192,
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_ENVIRONMENTAL_TEMPERATURE=16384,
        ///<summary> 0 - no. 1 - successfully calibrated during current run. | </summary>
        [Description("0 - no. 1 - successfully calibrated during current run.")]
        EAHRS_ON_THE_FLY_CALIBRATED=32768,
    };

    ///<summary> Flags in ILABS_EAHRS_STATUS message. </summary>
    [Flags]
	public enum ILABS_EAHRS_STATUS_FLAGS2: int /*default*/
    {
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_X_ACCELERATION_EXCEEDING_DETECT=1,
        ///<summary> 0 - ok. 1 - 1 - out of range. | </summary>
        [Description("0 - ok. 1 - 1 - out of range.")]
        EAHRS_Y_ACCELERATION_EXCEEDING_DETECT=2,
        ///<summary> 0 - ok. 1 - 1 - out of range. | </summary>
        [Description("0 - ok. 1 - 1 - out of range.")]
        EAHRS_Z_ACCELERATION_EXCEEDING_DETECT=4,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_BARO_ALTIMETER=8,
        ///<summary> 0 - ok. 1 - failure detected. | </summary>
        [Description("0 - ok. 1 - failure detected.")]
        EAHRS_DIFFERENTIAL_PRESSURE_SENSOR=16,
        ///<summary> 0 - not active. 1 - in progress. | </summary>
        [Description("0 - not active. 1 - in progress.")]
        EAHRS_AUTOMATIC_2D_MAGNETOMETERS_CALIBRATION=32,
        ///<summary> 0 - not active. 1 - in progress. | </summary>
        [Description("0 - not active. 1 - in progress.")]
        EAHRS_AUTOMATIC_3D_MAGNETOMETERS_CALIBRATION=64,
        ///<summary> 0 - switched on. 1 - switched off. | </summary>
        [Description("0 - switched on. 1 - switched off.")]
        EAHRS_GNSS_RECEIVER_INPUT_TO_THE_INS_ALGORITHM=128,
        ///<summary> 0 - switched on. 1 - switched off. | </summary>
        [Description("0 - switched on. 1 - switched off.")]
        EAHRS_DIFFERENTIAL_PRESSURE_INPUT_TO_THE_INS_ALGORITHM=256,
        ///<summary> Reserved bit | </summary>
        [Description("Reserved bit")]
        EAHRS_RESERVED_BIT_9=512,
        ///<summary> 0 - valid. 1 - invalid. | </summary>
        [Description("0 - valid. 1 - invalid.")]
        EAHRS_GNSS_POSITION_VALIDITY=1024,
    };

    ///<summary> Flags in ADU_STATUS message. </summary>
    [Flags]
	public enum ILABS_EAHRS_ADU_STATUS_FLAGS: int /*default*/
    {
        ///<summary> 0 - ok. 1 - unsuccessful initialization. | </summary>
        [Description("0 - ok. 1 - unsuccessful initialization.")]
        EAHRS_ADU_STATIC_PRESSURE_SENSOR_INITIALIZATION=1,
        ///<summary> 0 - ok. 1 - unsuccessful initialization. | </summary>
        [Description("0 - ok. 1 - unsuccessful initialization.")]
        EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_INITIALIZATION=2,
        ///<summary> 0 - no fails. 1 - failure detected. | </summary>
        [Description("0 - no fails. 1 - failure detected.")]
        EAHRS_ADU_STATIC_PRESSURE_SENSOR_STATUS=4,
        ///<summary> 0 - no fails. 1 - failure detected. | </summary>
        [Description("0 - no fails. 1 - failure detected.")]
        EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_STATUS=8,
        ///<summary> 0 - no fails. 1 - failure detected. | </summary>
        [Description("0 - no fails. 1 - failure detected.")]
        EAHRS_ADU_STATIC_PRESSURE_MEASUREMENT=16,
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_ADU_DIFFERENTIAL_PRESSURE_MEASUREMENT=32,
        ///<summary> Reserved bit | </summary>
        [Description("Reserved bit")]
        EAHRS_ADU_RESERVED_BIT_6=64,
        ///<summary> Reserved bit | </summary>
        [Description("Reserved bit")]
        EAHRS_ADU_RESERVED_BIT_7=128,
        ///<summary> 0 - ok. 1 - incorrect. | </summary>
        [Description("0 - ok. 1 - incorrect.")]
        EAHRS_ADU_PRESSURE_ALTITUDE=256,
        ///<summary> 0 - ok. 1 - incorrect. | </summary>
        [Description("0 - ok. 1 - incorrect.")]
        EAHRS_ADU_AIR_SPEED=512,
        ///<summary> 0 - ok. 1 - below the threshold. | </summary>
        [Description("0 - ok. 1 - below the threshold.")]
        EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD=1024,
        ///<summary> 0 - ok. 1 - out of range. | </summary>
        [Description("0 - ok. 1 - out of range.")]
        EAHRS_ADU_BAROMETRIC_TEMPERATURE=2048,
    };

    ///<summary>  </summary>
    public enum ILABS_EAHRS_GPS_FIX_STATUS: int /*default*/
    {
        ///<summary> No fix. | </summary>
        [Description("No fix.")]
        NO=0,
        ///<summary> 2D fix. | </summary>
        [Description("2D fix.")]
        FIX_2D=1,
        ///<summary> 3D fix. | </summary>
        [Description("3D fix.")]
        FIX_3D=2,
        ///<summary> Other fix. | </summary>
        [Description("Other fix.")]
        OTHER=3,
    };

    ///<summary>  </summary>
    public enum ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS: int /*default*/
    {
        ///<summary> Unknown or deactivated spoofing. | </summary>
        [Description("Unknown or deactivated spoofing.")]
        UNKNOWN_OR_DEACTIVATED=0,
        ///<summary> No spoofing indicated. | </summary>
        [Description("No spoofing indicated.")]
        NO_INDICATED=1,
        ///<summary> Spoofing indicated. | </summary>
        [Description("Spoofing indicated.")]
        INDICATED=2,
        ///<summary> Multiple spoofing indicated. | </summary>
        [Description("Multiple spoofing indicated.")]
        MULTIPLE_INDICATIONS=3,
    };

    ///<summary>  </summary>
    public enum MAV_CMD: int /*default*/
    {
        ///<summary> Send command 'start sending user defined data' to external AHRS |Empty.| Empty.| Empty.| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send command 'start sending user defined data' to external AHRS")]
        EXTERNAL_AHRS_START_UDD=33000,
        ///<summary> Send command 'stop sending data' to external AHRS |Empty.| Empty.| Empty.| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send command 'stop sending data' to external AHRS")]
        EXTERNAL_AHRS_STOP=33001,
        ///<summary> Send command 'enable GNSS' to external AHRS |Empty.| Empty.| Empty.| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send command 'enable GNSS' to external AHRS")]
        EXTERNAL_AHRS_ENABLE_GNSS=33002,
        ///<summary> Send command 'disable GNSS' to external AHRS |Empty.| Empty.| Empty.| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send command 'disable GNSS' to external AHRS")]
        EXTERNAL_AHRS_DISABLE_GNSS=33003,
        ///<summary> Send command 'start vg3d calibration in flight' to external AHRS |Empty.| Empty.| Empty.| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send command 'start vg3d calibration in flight' to external AHRS")]
        EXTERNAL_AHRS_START_VG3D_CALIBRATION_IN_FLIGHT=33004,
        ///<summary> Send command 'stop vg3d calibration in flight' to external AHRS |Empty.| Empty.| Empty.| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send command 'stop vg3d calibration in flight' to external AHRS")]
        EXTERNAL_AHRS_STOP_VG3D_CALIBRATION_IN_FLIGHT=33005,
        ///<summary> Send external position aiding data to AHRS |Latency| Latitude STD| Longitude STD| Altitude STD| Latitude| Longitude| Altitude|  </summary>
        [Description("Send external position aiding data to AHRS")]
        [hasLocation()]
        EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_POSITION=33006,
        ///<summary> Send external horizontal position aiding data to AHRS |Latitude STD| Longitude STD| Latency| Empty.| Latitude| Longitude| Empty.|  </summary>
        [Description("Send external horizontal position aiding data to AHRS")]
        [hasLocation()]
        EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_HORIZONTAL_POSITION=33007,
        ///<summary> Send external altitude aiding data to AHRS |Altitude STD| Empty.| Empty.| Empty.| Empty.| Empty.| Altitude|  </summary>
        [Description("Send external altitude aiding data to AHRS")]
        EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_ALTITUDE=33008,
        ///<summary> Send wind aiding data to AHRS |Direction| Speed| Speed STD| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send wind aiding data to AHRS")]
        EXTERNAL_AHRS_AIDING_DATA_WIND=33009,
        ///<summary> Send aiding data ambient air data to AHRS |Temperature| Pressure| Empty.| Empty.| Empty.| Empty.| Altitude|  </summary>
        [Description("Send aiding data ambient air data to AHRS")]
        EXTERNAL_AHRS_AIDING_DATA_AMBIENT_AIR=33010,
        ///<summary> Send aiding data external heading to AHRS |Heading| Heading STD| Latency| Empty.| Empty.| Empty.| Empty.|  </summary>
        [Description("Send aiding data external heading to AHRS")]
        EXTERNAL_AHRS_AIDING_DATA_EXTERNAL_HEADING=33011,
        ///<summary> Send aiding data DVL to AHRS |Lateral velocity| Forward velocity| Vertical velocity| Lateral velocity STD| Forward velocity STD| Vertical velocity STD| Velocity latency|  </summary>
        [Description("Send aiding data DVL to AHRS")]
        EXTERNAL_AHRS_AIDING_DATA_DVL=33012,
    };

    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=27)]
    ///<summary> Additional GPS and INS parameters. </summary>
    public struct mavlink_ahrs_additional_raw_info_t
    {
        /// packet ordered constructor
        public mavlink_ahrs_additional_raw_info_t(int lat_raw,int lon_raw,int alt_raw,uint ins_lat_accuracy,uint ins_lng_accuracy,uint ins_alt_accuracy,ushort track_over_ground_raw,byte gps_raw_status)
        {
            this.lat_raw = lat_raw;
            this.lon_raw = lon_raw;
            this.alt_raw = alt_raw;
            this.ins_lat_accuracy = ins_lat_accuracy;
            this.ins_lng_accuracy = ins_lng_accuracy;
            this.ins_alt_accuracy = ins_alt_accuracy;
            this.track_over_ground_raw = track_over_ground_raw;
            this.gps_raw_status = gps_raw_status;
        }

        /// packet xml order
        public static mavlink_ahrs_additional_raw_info_t PopulateXMLOrder(int lat_raw,int lon_raw,int alt_raw,ushort track_over_ground_raw,byte gps_raw_status,uint ins_lat_accuracy,uint ins_lng_accuracy,uint ins_alt_accuracy)
        {
            var msg = new mavlink_ahrs_additional_raw_info_t();
            msg.lat_raw = lat_raw;
            msg.lon_raw = lon_raw;
            msg.alt_raw = alt_raw;
            msg.track_over_ground_raw = track_over_ground_raw;
            msg.gps_raw_status = gps_raw_status;
            msg.ins_lat_accuracy = ins_lat_accuracy;
            msg.ins_lng_accuracy = ins_lng_accuracy;
            msg.ins_alt_accuracy = ins_alt_accuracy;

            return msg;
        }

        /// <summary>Latitude (WGS84, EGM96 ellipsoid)  [degE7] </summary>
        [Units("[degE7]")]
        [Description("Latitude (WGS84, EGM96 ellipsoid)")]
        //[FieldOffset(0)]
        public  int lat_raw;

        /// <summary>Longitude (WGS84, EGM96 ellipsoid)  [degE7] </summary>
        [Units("[degE7]")]
        [Description("Longitude (WGS84, EGM96 ellipsoid)")]
        //[FieldOffset(4)]
        public  int lon_raw;

        /// <summary>Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.  [mm] </summary>
        [Units("[mm]")]
        [Description("Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.")]
        //[FieldOffset(8)]
        public  int alt_raw;

        /// <summary>INS latitude accuracy.  [mm] </summary>
        [Units("[mm]")]
        [Description("INS latitude accuracy.")]
        //[FieldOffset(12)]
        public  uint ins_lat_accuracy;

        /// <summary>INS longitude accuracy.  [mm] </summary>
        [Units("[mm]")]
        [Description("INS longitude accuracy.")]
        //[FieldOffset(16)]
        public  uint ins_lng_accuracy;

        /// <summary>INS altitude accuracy.  [mm] </summary>
        [Units("[mm]")]
        [Description("INS altitude accuracy.")]
        //[FieldOffset(20)]
        public  uint ins_alt_accuracy;

        /// <summary>Yaw in earth frame from north. Use 0 if this GPS does not provide yaw. Use 36000 for north.  [cdeg] </summary>
        [Units("[cdeg]")]
        [Description("Yaw in earth frame from north. Use 0 if this GPS does not provide yaw. Use 36000 for north.")]
        //[FieldOffset(24)]
        public  ushort track_over_ground_raw;

        /// <summary>Status for GPS data   </summary>
        [Units("")]
        [Description("Status for GPS data")]
        //[FieldOffset(26)]
        public  byte gps_raw_status;
    };

    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=10)]
    ///<summary> Info about EAHRS status. </summary>
    public struct mavlink_eahrs_status_info_t
    {
        /// packet ordered constructor
        public mavlink_eahrs_status_info_t(ushort status1,ushort status2,ushort status3,ushort status4,ushort status5)
        {
            this.status1 = status1;
            this.status2 = status2;
            this.status3 = status3;
            this.status4 = status4;
            this.status5 = status5;
        }

        /// packet xml order
        public static mavlink_eahrs_status_info_t PopulateXMLOrder(ushort status1,ushort status2,ushort status3,ushort status4,ushort status5)
        {
            var msg = new mavlink_eahrs_status_info_t();
            msg.status1 = status1;
            msg.status2 = status2;
            msg.status3 = status3;
            msg.status4 = status4;
            msg.status5 = status5;

            return msg;
        }

        /// <summary>Status flags1.   bitmask</summary>
        [Units("")]
        [Description("Status flags1.")]
        //[FieldOffset(0)]
        public  ushort status1;

        /// <summary>Status flags2.   bitmask</summary>
        [Units("")]
        [Description("Status flags2.")]
        //[FieldOffset(2)]
        public  ushort status2;

        /// <summary>Status flags3.   bitmask</summary>
        [Units("")]
        [Description("Status flags3.")]
        //[FieldOffset(4)]
        public  ushort status3;

        /// <summary>Status flags4.   bitmask</summary>
        [Units("")]
        [Description("Status flags4.")]
        //[FieldOffset(6)]
        public  ushort status4;

        /// <summary>Status flags5.   bitmask</summary>
        [Units("")]
        [Description("Status flags5.")]
        //[FieldOffset(8)]
        public  ushort status5;
    };
}

} // namespace
