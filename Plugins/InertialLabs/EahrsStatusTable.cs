using System;
using System.Collections.Generic;

namespace InertialLabs
{
    // Order is important! From least to most severe. This is used for sorting the rows in the status table.
    internal enum EahrsSeverity
    {
        None,
        Good,
        Info,
        Warning,
        Fail,
    }

    internal struct EahrsStatusRow
    {
        public string Name;
        public uint Value;
        public EahrsSeverity Severity;
    }

    // For EAHRS_STATUS_INFO message
    internal static class EahrsStatusTable
    {
        private struct Flag
        {
            public uint Bit;
            public string Name;
            public EahrsSeverity Severity;
        }

        private static Flag F(Enum flag, EahrsSeverity severity)
        {
            return new Flag
            {
                Bit = Convert.ToUInt32(flag),
                Name = flag.ToString().Replace("EAHRS_", "").ToLowerInvariant(),
                Severity = severity,
            };
        }

        private static readonly Flag[] UswFlags =
        {
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INITIAL_ALIGNMENT, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_SOFTWARE_STATUS, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GYROSCOPE_UNIT, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ACCELEROMETER_UNIT, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_MAGNETOMETER_UNIT, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ELECTRONICS, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_GNSS_RECEIVER, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATION, EahrsSeverity.Info),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_LOW_POWER_SUPPLY, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_INCORRECT_HIGH_POWER_SUPPLY, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_X_ANGULAR_RATE_EXCEEDING_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Y_ANGULAR_RATE_EXCEEDING_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_Z_ANGULAR_RATE_EXCEEDING_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_LARGE_MAGNETIC_FIELD_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ENVIRONMENTAL_TEMPERATURE, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS.EAHRS_ON_THE_FLY_CALIBRATED, EahrsSeverity.Good),
        };

        private static readonly Flag[] Usw2Flags =
        {
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_X_ACCELERATION_EXCEEDING_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Y_ACCELERATION_EXCEEDING_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_Z_ACCELERATION_EXCEEDING_DETECT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_BARO_ALTIMETER, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_SENSOR, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_2D_MAGNETOMETERS_CALIBRATION, EahrsSeverity.Info),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_AUTOMATIC_3D_MAGNETOMETERS_CALIBRATION, EahrsSeverity.Info),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_RECEIVER_INPUT_TO_THE_INS_ALGORITHM, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_DIFFERENTIAL_PRESSURE_INPUT_TO_THE_INS_ALGORITHM, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_STATUS_FLAGS2.EAHRS_GNSS_POSITION_VALIDITY, EahrsSeverity.Warning),
        };

        private static readonly Flag[] AduFlags =
        {
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_INITIALIZATION, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_INITIALIZATION, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_SENSOR_STATUS, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_SENSOR_STATUS, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_STATIC_PRESSURE_MEASUREMENT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_DIFFERENTIAL_PRESSURE_MEASUREMENT, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_PRESSURE_ALTITUDE, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED, EahrsSeverity.Fail),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_AIR_SPEED_BELOW_THRESHOLD, EahrsSeverity.Warning),
            F(MAVLink.ILABS_EAHRS_ADU_STATUS_FLAGS.EAHRS_ADU_BAROMETRIC_TEMPERATURE, EahrsSeverity.Warning),
        };

        public static List<EahrsStatusRow> GetStatusRows()
        {
            var rows = new List<EahrsStatusRow>();
            AddRowsFromFlags(rows, UswFlags, PluginState.eahrsStatusValue1);
            AddRowsFromFlags(rows, Usw2Flags, PluginState.eahrsStatusValue2);
            AddRowsFromFlags(rows, AduFlags, PluginState.eahrsStatusValue3);

            uint gpsFix = PluginState.eahrsStatusValue4;
            rows.Add(new EahrsStatusRow { Name = "gps_fix", Value = gpsFix, Severity = GpsFixSeverity(gpsFix) });

            uint gpsSpoofing = PluginState.eahrsStatusValue5;
            rows.Add(new EahrsStatusRow { Name = "gps_spoofing_indicated", Value = gpsSpoofing, Severity = GpsSpoofingSeverity(gpsSpoofing) });
            return rows;
        }

        private static void AddRowsFromFlags(List<EahrsStatusRow> rows, Flag[] flags, uint statusBitsValue)
        {
            foreach (var flag in flags)
            {
                bool isFlagSet = (statusBitsValue & flag.Bit) != 0;
                rows.Add(new EahrsStatusRow { Name = flag.Name,
                                              Value = isFlagSet ? 1u : 0u,
                                              Severity = isFlagSet ? flag.Severity : EahrsSeverity.None });
            }
        }

        private static EahrsSeverity GpsFixSeverity(uint value)
        {
            switch ((MAVLink.ILABS_EAHRS_GPS_FIX_STATUS)value)
            {
                case MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.NO:
                    return EahrsSeverity.Warning;
                case MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_2D:
                case MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.FIX_3D:
                case MAVLink.ILABS_EAHRS_GPS_FIX_STATUS.OTHER:
                    return EahrsSeverity.Good;
                default:
                    return EahrsSeverity.Fail;
            }
        }

        private static EahrsSeverity GpsSpoofingSeverity(uint value)
        {
            switch ((MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS)value)
            {
                case MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.INDICATED:
                case MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.MULTIPLE_INDICATIONS:
                    return EahrsSeverity.Warning;
                case MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.UNKNOWN_OR_DEACTIVATED:
                case MAVLink.ILABS_EAHRS_GPS_SPOOFING_INDICATED_STATUS.NO_INDICATED:
                    return EahrsSeverity.Good;
                default:
                    return EahrsSeverity.Fail;
            }
        }
    }
}
