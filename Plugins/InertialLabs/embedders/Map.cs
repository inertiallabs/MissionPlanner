using MissionPlanner;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using GMap.NET;
using static MAVLink;

namespace InertialLabs.Embedders
{
    public class Map : EmbedderInterface
    {
        private InertialLabsPlugin inertialLabsPluginRef;
        private Timer loadWaitTimer;
        private ToolStripMenuItem imHereToolStripMenuItem;

        public bool show_gps_raw_location = false;
        public bool show_ins_pos_estimation = false;
        public uint gcs_distance_around { get; set; }
        public bool is_gps_raw_valid = false;
        public double gps_lat_raw { get; set; }
        public double gps_lng_raw { get; set; }
        public double gps_alt_raw { get; set; }
        public double gps_track_over_ground_raw { get; set; }
        public double ins_lat_accuracy { get; set; }
        public double ins_lng_accuracy { get; set; }
        public double ins_alt_accuracy { get; set; }

        public Map(InertialLabsPlugin inertialLabsPlugin)
        {
            inertialLabsPluginRef = inertialLabsPlugin;
            loadWaitTimer = new Timer();
        }

        public override bool Init()
        {
            EmbedInUi();

            // this needs to be set per "comport" - prevent any duplicates
            MainV2.comPort.OnPacketReceived -= MavlinkMessageHandler;
            MainV2.comPort.OnPacketReceived += MavlinkMessageHandler;

            return true;
        }

        private void EmbedInUi()
        {
            imHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imHereToolStripMenuItem.Size = new System.Drawing.Size(220, 24);
            imHereToolStripMenuItem.Text = "I'm here";
            imHereToolStripMenuItem.Name = "imHereToolStripMenuItem";
            imHereToolStripMenuItem.Click += new System.EventHandler(this.imHereToolStripMenuItem_Click);

            // Wait the full loading of the original UI
            loadWaitTimer.Interval = 1000;
            loadWaitTimer.Tick += (s, e) =>
            {
                var flightData = inertialLabsPluginRef.Host.MainForm.FlightData;
                if (flightData != null)
                {
                    var menu = MissionPlanner.GCSViews.FlightData.instance.contextMenuStripMap;
                    menu.Items.Add(imHereToolStripMenuItem);

                    loadWaitTimer.Stop();
                }
            };
            loadWaitTimer.Start();
        }

        public void SendImHerePos(double latitude, double longitude)
        {
            mavlink_gps_input_t req = new mavlink_gps_input_t
            {
                ignore_flags = (ushort)(global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_ALT |
                    global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_VDOP |
                    global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_VEL_HORIZ |
                    global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_VEL_VERT |
                    global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_SPEED_ACCURACY |
                    global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_HORIZONTAL_ACCURACY |
                    global::MAVLink.GPS_INPUT_IGNORE_FLAGS.GPS_INPUT_IGNORE_FLAG_VERTICAL_ACCURACY),
                yaw = 0,
                lat = (int)(latitude * 1E7),
                lon = (int)(longitude * 1E7),
                hdop = 0.9F,
                satellites_visible = 99,
                fix_type = (byte)global::MAVLink.GPS_FIX_TYPE._3D_FIX,
            };

            MainV2.comPort.generatePacket((byte)global::MAVLink.MAVLINK_MSG_ID.GPS_INPUT,
                                          req,
                                          MainV2.comPort.MAV.sysid,
                                          MainV2.comPort.MAV.compid);
        }

        private void imHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.Show("Do you want to set this position as the current position?",
                    "Are you sure?", CustomMessageBox.MessageBoxButtons.OKCancel) !=
                    CustomMessageBox.DialogResult.OK)
            {
                return;
            }

            if (!MainV2.comPort.BaseStream.IsOpen)
            {
                CustomMessageBox.Show(Strings.PleaseConnect, Strings.ERROR);
                return;
            }


            var fieldInfo = typeof(MissionPlanner.GCSViews.FlightData)
                .GetField("MouseDownStart", BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo == null)
            {
                return;
            }

            var MouseDownStart = (PointLatLng)fieldInfo.GetValue(MissionPlanner.GCSViews.FlightData.instance);

            if (MouseDownStart.Lat == 0.0 || MouseDownStart.Lng == 0.0)
            {
                CustomMessageBox.Show(Strings.BadCoords, Strings.ERROR);
                return;
            }

            try
            {
                SendImHerePos(MouseDownStart.Lat, MouseDownStart.Lng);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
            }
        }

        private void MavlinkMessageHandler(object sender, global::MAVLink.MAVLinkMessage mavLinkMessage)
        {
            switch (mavLinkMessage.msgid)
            {
                case (uint)InertialLabs.MAVLink.MAVLINK_MSG_ID.AHRS_ADDITIONAL_RAW_INFO:
                    {
                        var ahrs_info = mavLinkMessage.ToStructure<MAVLink.mavlink_ahrs_additional_raw_info_t>();

                        gps_lat_raw = ahrs_info.lat_raw * 1.0e-7;
                        gps_lng_raw = ahrs_info.lon_raw * 1.0e-7;
                        gps_alt_raw = ahrs_info.alt_raw * 1.0e-3;
                        gps_track_over_ground_raw = ahrs_info.track_over_ground_raw * 1.0e-2;
                        is_gps_raw_valid = ahrs_info.gps_raw_status == 0;
                        ins_lat_accuracy = ahrs_info.ins_lat_accuracy * 1.0e-3;
                        ins_lng_accuracy = ahrs_info.ins_lng_accuracy * 1.0e-3;
                        ins_alt_accuracy = ahrs_info.ins_alt_accuracy * 1.0e-3;
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
