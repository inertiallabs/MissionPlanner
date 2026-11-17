namespace InertialLabs
{
    public static class PluginState
    {
        public static bool show_gcs_distance_around = false;
        public static uint gcs_distance_around { get; set; }

        public static bool show_ins_pos_estimation = false;
        public static bool show_gps_raw_location = false;

        // Read once per update: a second read can return the next message
        public static volatile AhrsAdditionalRawInfo AhrsAdditionalRawInfo = new AhrsAdditionalRawInfo();
        public static volatile EahrsStatusInfo EahrsStatusInfo = new EahrsStatusInfo();
    }
}
