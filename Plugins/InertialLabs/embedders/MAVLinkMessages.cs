using System.Linq;
using System.Windows.Forms;
using static InertialLabs.MAVLink;


namespace InertialLabs.Embedders
{
    public class MAVLinkMessages : EmbedderInterface
    {
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

            return true;
        }
    }
}
