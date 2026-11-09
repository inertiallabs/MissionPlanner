using MissionPlanner;
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
        private static MAVState eahrsStatusInfoVehicle;
        public static void SaveEahrsStatusInfoTimestamp(MAVState vehicle) => SaveTimestamp(ref eahrsStatusInfoTimestamp, ref eahrsStatusInfoVehicle, vehicle);
        public static bool IsEahrsStatusInfoFresh => IsFresh(ref eahrsStatusInfoTimestamp, ref eahrsStatusInfoVehicle);

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
        private static MAVState ahrsAdditionalRawInfoVehicle;
        public static void SaveAhrsAdditionalRawInfoTimestamp(MAVState vehicle) => SaveTimestamp(ref ahrsAdditionalRawInfoTimestamp, ref ahrsAdditionalRawInfoVehicle, vehicle);
        public static bool IsAhrsAdditionalRawInfoFresh => IsFresh(ref ahrsAdditionalRawInfoTimestamp, ref ahrsAdditionalRawInfoVehicle);

        public static bool show_gps_raw_location = false;
        public static bool is_gps_raw_valid = false;
        public static double gps_lat_raw { get; set; }
        public static double gps_lng_raw { get; set; }
        public static double gps_alt_raw { get; set; }
        public static double gps_track_over_ground_raw { get; set; }

        // Saves when and from which vehicle a message was received. Called on the MAVLink reader thread,
        // after the message values are stored.
        private static void SaveTimestamp(ref long timestamp, ref MAVState savedVehicle, MAVState vehicle)
        {
            Volatile.Write(ref savedVehicle, vehicle);
            Interlocked.Exchange(ref timestamp, Stopwatch.GetTimestamp());
        }

        // A message is current if it was received in the last 3 s from the vehicle selected now (MainV2.comPort.MAV):
        // after the user selects another vehicle or link, the previous vehicle's data is not shown.
        private static bool IsFresh(ref long timestamp, ref MAVState vehicle)
        {
            const int DataTimeoutSeconds = 3;
            long last = Interlocked.Read(ref timestamp);
            return last != 0 && Stopwatch.GetTimestamp() - last < DataTimeoutSeconds * Stopwatch.Frequency &&
                   Volatile.Read(ref vehicle) == MainV2.comPort.MAV;
        }
    }
}
