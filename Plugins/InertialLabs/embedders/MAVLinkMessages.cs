using MissionPlanner;
using System.Linq;
using System.Windows.Forms;
using static InertialLabs.MAVLink;


namespace InertialLabs.Embedders
{
    public class MAVLinkMessages : EmbedderInterface
    {
        private CurrentVehiclePackets currentVehiclePackets;

        public MAVLinkMessages()
        {
        }

        public override bool Init()
        {
            // msgid, name, crc, minlength, length, type
            global::MAVLink.MAVLINK_MESSAGE_INFOS =
                global::MAVLink.MAVLINK_MESSAGE_INFOS.Concat(new[]
                {
                    new global::MAVLink.message_info(699, "AHRS_ADDITIONAL_RAW_INFO", 59, 27, 27, typeof( mavlink_ahrs_additional_raw_info_t )),
                    new global::MAVLink.message_info(700, "EAHRS_STATUS_INFO", 231, 10, 10, typeof( mavlink_eahrs_status_info_t ))
                }).ToArray();

            currentVehiclePackets = new CurrentVehiclePackets(MavlinkMessageHandler);

            return true;
        }

        private void MavlinkMessageHandler(MAVState vehicle, global::MAVLink.MAVLinkMessage mavLinkMessage)
        {
            switch (mavLinkMessage.msgid)
            {
                case (uint)InertialLabs.MAVLink.MAVLINK_MSG_ID.AHRS_ADDITIONAL_RAW_INFO:
                    {
                        var ahrs_info = mavLinkMessage.ToStructure<MAVLink.mavlink_ahrs_additional_raw_info_t>();

                        PluginState.gps_lat_raw = ahrs_info.lat_raw * 1.0e-7;
                        PluginState.gps_lng_raw = ahrs_info.lon_raw * 1.0e-7;
                        PluginState.gps_alt_raw = ahrs_info.alt_raw * 1.0e-3;
                        PluginState.gps_track_over_ground_raw = ahrs_info.track_over_ground_raw * 1.0e-2;
                        PluginState.is_gps_raw_valid = ahrs_info.gps_raw_status == 0;
                        PluginState.ins_lat_accuracy = ahrs_info.ins_lat_accuracy * 1.0e-3;
                        PluginState.ins_lng_accuracy = ahrs_info.ins_lng_accuracy * 1.0e-3;
                        PluginState.ins_alt_accuracy = ahrs_info.ins_alt_accuracy * 1.0e-3;
                        PluginState.SaveAhrsAdditionalRawInfoTimestamp(vehicle);
                    }
                    break;
                case (uint)InertialLabs.MAVLink.MAVLINK_MSG_ID.EAHRS_STATUS_INFO:
                    {
                        var status = mavLinkMessage.ToStructure<MAVLink.mavlink_eahrs_status_info_t>();
                        PluginState.eahrsStatusValue1 = status.status1;
                        PluginState.eahrsStatusValue2 = status.status2;
                        PluginState.eahrsStatusValue3 = status.status3;
                        PluginState.eahrsStatusValue4 = status.status4;
                        PluginState.eahrsStatusValue5 = status.status5;
                        PluginState.SaveEahrsStatusInfoTimestamp(vehicle);
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }
    }
}
