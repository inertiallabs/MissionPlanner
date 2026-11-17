using MissionPlanner;
using System.Diagnostics;

namespace InertialLabs
{
    public abstract class ReceivedMessage
    {
        private readonly long timestamp;
        private readonly MAVState vehicle;

        // For the "nothing received yet" instances: timestamp 0, so never fresh
        protected ReceivedMessage()
        {
        }

        protected ReceivedMessage(MAVState vehicle)
        {
            timestamp = Stopwatch.GetTimestamp();
            this.vehicle = vehicle;
        }

        public bool IsFresh
        {
            get
            {
                const int DataTimeoutSeconds = 3;
                return timestamp != 0 && Stopwatch.GetTimestamp() - timestamp < DataTimeoutSeconds * Stopwatch.Frequency &&
                       vehicle == MainV2.comPort.MAV;
            }
        }
    }

    // AHRS_ADDITIONAL_RAW_INFO (699)
    public sealed class AhrsAdditionalRawInfo : ReceivedMessage
    {
        public AhrsAdditionalRawInfo()
        {
        }

        public AhrsAdditionalRawInfo(MAVState vehicle, double gps_lat_raw, double gps_lng_raw, double gps_alt_raw,
                                     double gps_track_over_ground_raw, bool is_gps_raw_valid,
                                     double ins_lat_accuracy, double ins_lng_accuracy, double ins_alt_accuracy)
            : base(vehicle)
        {
            this.gps_lat_raw = gps_lat_raw;
            this.gps_lng_raw = gps_lng_raw;
            this.gps_alt_raw = gps_alt_raw;
            this.gps_track_over_ground_raw = gps_track_over_ground_raw;
            this.is_gps_raw_valid = is_gps_raw_valid;
            this.ins_lat_accuracy = ins_lat_accuracy;
            this.ins_lng_accuracy = ins_lng_accuracy;
            this.ins_alt_accuracy = ins_alt_accuracy;
        }

        // GPS
        public double gps_lat_raw { get; }
        public double gps_lng_raw { get; }
        public double gps_alt_raw { get; }
        public double gps_track_over_ground_raw { get; }
        public bool is_gps_raw_valid { get; }

        // INS
        public double ins_lat_accuracy { get; }
        public double ins_lng_accuracy { get; }
        public double ins_alt_accuracy { get; }
    }

    // EAHRS_STATUS_INFO (700)
    public sealed class EahrsStatusInfo : ReceivedMessage
    {
        public EahrsStatusInfo()
        {
        }

        public EahrsStatusInfo(MAVState vehicle, uint eahrsStatusValue1, uint eahrsStatusValue2, uint eahrsStatusValue3,
                               uint eahrsStatusValue4, uint eahrsStatusValue5)
            : base(vehicle)
        {
            this.eahrsStatusValue1 = eahrsStatusValue1;
            this.eahrsStatusValue2 = eahrsStatusValue2;
            this.eahrsStatusValue3 = eahrsStatusValue3;
            this.eahrsStatusValue4 = eahrsStatusValue4;
            this.eahrsStatusValue5 = eahrsStatusValue5;
        }

        public uint eahrsStatusValue1 { get; }
        public uint eahrsStatusValue2 { get; }
        public uint eahrsStatusValue3 { get; }
        public uint eahrsStatusValue4 { get; }
        public uint eahrsStatusValue5 { get; }
    }
}
