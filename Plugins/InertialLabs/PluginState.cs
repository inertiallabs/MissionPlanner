using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InertialLabs
{
    public static class PluginState
    {
        public static bool show_gcs_distance_around = false;
        public static uint gcs_distance_around { get; set; }

        // EAHRS Status value
        private static long eahrsStatusInfoTimestamp;
        public static void SaveEahrsStatusInfoTimestamp() => Interlocked.Exchange(ref eahrsStatusInfoTimestamp, Stopwatch.GetTimestamp());
        public static bool IsEahrsStatusInfoFresh => IsFresh(ref eahrsStatusInfoTimestamp);

        public static uint eahrsStatusResultValue { get; set; }
        public static uint eahrsStatusValue1 { get; set; }
        public static uint eahrsStatusValue2 { get; set; }
        public static uint eahrsStatusValue3 { get; set; }
        public static uint eahrsStatusValue4 { get; set; }
        public static uint eahrsStatusValue5 { get; set; }

        // INS
        public static bool show_ins_pos_estimation = false;
        public static double ins_lat_accuracy { get; set; }
        public static double ins_lng_accuracy { get; set; }
        public static double ins_alt_accuracy { get; set; }

        // GPS
        private static long ahrsAdditionalRawInfoTimestamp;
        public static void SaveAhrsAdditionalRawInfoTimestamp() => Interlocked.Exchange(ref ahrsAdditionalRawInfoTimestamp, Stopwatch.GetTimestamp());
        public static bool IsAhrsAdditionalRawInfoFresh => IsFresh(ref ahrsAdditionalRawInfoTimestamp);

        public static bool show_gps_raw_location = false;
        public static bool is_gps_raw_valid = false;
        public static double gps_lat_raw { get; set; }
        public static double gps_lng_raw { get; set; }
        public static double gps_alt_raw { get; set; }
        public static double gps_track_over_ground_raw { get; set; }

        private static bool IsFresh(ref long timestamp)
        {
            const int DataTimeoutSeconds = 3;
            long last = Interlocked.Read(ref timestamp);
            return last != 0 && Stopwatch.GetTimestamp() - last < DataTimeoutSeconds * Stopwatch.Frequency;
        }
    }
}
