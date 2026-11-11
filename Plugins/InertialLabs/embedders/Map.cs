using MissionPlanner;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using GMap.NET;
using GMap.NET.WindowsForms;
using static MAVLink;
using MissionPlanner.Controls;
using System.Threading;
using InertialLabs.Maps;

namespace InertialLabs.Embedders
{
    public class Map : EmbedderInterface
    {
        private InertialLabsPlugin inertialLabsPluginRef;
        private System.Windows.Forms.Timer loadWaitTimer;

        private ToolStripMenuItem imHereToolStripMenuItem;
        private Label gpsPositionLabel;
        private Label antennaDistanceLabel;

        private GMapOverlay markersOverlay = null;
        private System.Windows.Forms.Timer markersUpdateTimer;
        private global::MAVLink.MAV_TYPE lastAptype = 0;
        private GMapMarkerBase gpsMarker = null;
        private GMapMarkerCircle vehiclePosEstimationCircleMarker = null;
        private GMapMarkerCircle gcsSignalAreaCircleMarker = null;

        public Map(InertialLabsPlugin inertialLabsPlugin)
        {
            inertialLabsPluginRef = inertialLabsPlugin;
            loadWaitTimer = new System.Windows.Forms.Timer();
        }

        public override bool Init()
        {
            EmbedInUi();
            return true;
        }

        public override void Exit()
        {
            loadWaitTimer.Dispose();
            markersUpdateTimer.Dispose();
        }

        private void EmbedInUi()
        {
            imHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imHereToolStripMenuItem.Size = new System.Drawing.Size(220, 24);
            imHereToolStripMenuItem.Text = "I'm here";
            imHereToolStripMenuItem.Name = "imHereToolStripMenuItem";
            imHereToolStripMenuItem.Click += new System.EventHandler(this.ImHereToolStripMenuItem_Click);

            // Map agenda labels
            gpsPositionLabel = new Label
            {
                Name = "gpsPositionLabel",
                Tag = "custom",
                Text = "GPS Position",
                BackColor = Color.Black,
                ForeColor = Color.Blue,
                Anchor = (AnchorStyles.Bottom | AnchorStyles.Left),
                AutoSize = true,
                Size = new Size(86, 16),
                ImeMode = ImeMode.NoControl
            };

            antennaDistanceLabel = new Label
            {
                Name = "antennaDistanceLabel",
                Tag = "custom",
                Text = "Antenna distance",
                BackColor = Color.Black,
                ForeColor = Color.Purple,
                Anchor = (AnchorStyles.Bottom | AnchorStyles.Left),
                AutoSize = true,
                Size = new Size(110, 16),
                ImeMode = ImeMode.NoControl
            };

            markersOverlay = new GMapOverlay("InertialLabsMarkers");
            markersUpdateTimer = new System.Windows.Forms.Timer();
            markersUpdateTimer.Interval = 100;
            markersUpdateTimer.Tick += (s, e) =>
            {
                UpdateGPSMarker();
                UpdateVehiclePosEstimation();
                UpdateGpsDistanceAround();
            };

            // Wait the full loading of the original UI
            loadWaitTimer.Interval = 1000;
            loadWaitTimer.Tick += (s, e) =>
            {
                loadWaitTimer.Stop();

                // Agenda Labels
                var bottomLabelPanel = MissionPlanner.GCSViews.FlightData.instance
                                       .Controls
                                       .Find("splitContainer1", true)
                                       .FirstOrDefault() is SplitContainer split
                                       ? split.Panel2
                                       : null;
                if (bottomLabelPanel == null)
                {
                    Console.WriteLine("splitContainer1 not found in GCSViews.FlightData");
                }
                else
                {
                    int desiredYOffsetFromBottom = 15;
                    int labelsY = bottomLabelPanel.Height - gpsPositionLabel.Height - desiredYOffsetFromBottom;
                    bottomLabelPanel.Controls.Add(gpsPositionLabel);
                    bottomLabelPanel.Controls.Add(antennaDistanceLabel);
                    gpsPositionLabel.Location = new Point(435, labelsY);
                    antennaDistanceLabel.Location = new Point(510, labelsY);
                    bottomLabelPanel.Controls.SetChildIndex(gpsPositionLabel, 6);
                    bottomLabelPanel.Controls.SetChildIndex(antennaDistanceLabel, 6);
                }

                // Menu "I'm here"
                var flightData = inertialLabsPluginRef.Host.MainForm.FlightData;
                if (flightData != null)
                {
                    var menu = MissionPlanner.GCSViews.FlightData.instance.contextMenuStripMap;
                    menu.Items.Add(imHereToolStripMenuItem);
                }

                // GPS Markers
                gpsMarker = GetGPSMarker();
                gpsMarker.IsVisible = false;

                vehiclePosEstimationCircleMarker = new GMapMarkerCircle(MainV2.comPort.MAV.cs.Location, 0, Color.Red);
                vehiclePosEstimationCircleMarker.IsVisible = false;

                gcsSignalAreaCircleMarker = new GMapMarkerCircle(MainV2.comPort.MAV.cs.HomeLocation, 0, Color.Purple);
                gcsSignalAreaCircleMarker.IsVisible = false;

                markersOverlay.Markers.Add(gpsMarker);
                markersOverlay.Markers.Add(vehiclePosEstimationCircleMarker);
                markersOverlay.Markers.Add(gcsSignalAreaCircleMarker);
                MissionPlanner.GCSViews.FlightData.instance.gMapControl1.Overlays.Add(markersOverlay);
                markersUpdateTimer.Start();
            };
            loadWaitTimer.Start();
        }

        private void UpdateGPSMarker()
        {
            if (!PluginState.show_gps_raw_location || !PluginState.is_gps_raw_valid || !PluginState.IsAhrsAdditionalRawInfoFresh)
            {
                gpsMarker.IsVisible = false;
                return;
            }
            if (PluginState.gps_lat_raw == 0 && PluginState.gps_lng_raw == 0)
            {
                gpsMarker.IsVisible = false;
                return;
            }

            /// Check GPSMarker type change
            if (lastAptype != MainV2.comPort.MAV.aptype)
            {
                ReplaceGPSMarker();
                lastAptype = MainV2.comPort.MAV.aptype;
            }

            PointLatLng location = MainV2.comPort.MAV.cs.Location;
            location.Lat = PluginState.gps_lat_raw;
            location.Lng = PluginState.gps_lng_raw;
            gpsMarker.Position = location;
            gpsMarker.heading = (float)PluginState.gps_track_over_ground_raw;
            gpsMarker.IsVisible = true;
        }

        private void UpdateVehiclePosEstimation()
        {
            if (!PluginState.show_ins_pos_estimation || !PluginState.IsAhrsAdditionalRawInfoFresh)
            {
                vehiclePosEstimationCircleMarker.IsVisible = false;
                return;
            }

            double radius_m = Math.Max(PluginState.ins_lat_accuracy, PluginState.ins_lng_accuracy);
            if (radius_m < 10)
            {
                vehiclePosEstimationCircleMarker.IsVisible = false;
                return;
            }

            var pos = MainV2.comPort.MAV.cs.Location;
            vehiclePosEstimationCircleMarker.Position = pos;
            vehiclePosEstimationCircleMarker.radius = radius_m;
            vehiclePosEstimationCircleMarker.IsVisible = true;
        }

        private void UpdateGpsDistanceAround()
        {
            if (!PluginState.show_gcs_distance_around || PluginState.gcs_distance_around == 0)
            {
                gcsSignalAreaCircleMarker.IsVisible = false;
                return;
            }

            var pos = MainV2.comPort.MAV.cs.HomeLocation;
            var radius = PluginState.gcs_distance_around;

            gcsSignalAreaCircleMarker.Position = pos;
            gcsSignalAreaCircleMarker.radius = radius;
            gcsSignalAreaCircleMarker.IsVisible = true;
        }

        private GMapMarkerBase GetGPSMarker()
        {
            PointLatLng location = MainV2.comPort.MAV.cs.Location;
            location.Lat = PluginState.gps_lat_raw;
            location.Lng = PluginState.gps_lng_raw;

            if (MainV2.comPort.MAV.aptype == global::MAVLink.MAV_TYPE.GROUND_ROVER)
            {
                return new Maps.GMapMarkerRover(
                    location,
                    (float)PluginState.gps_track_over_ground_raw)
                {
                    Tag = MainV2.comPort.MAV
                };
            }
            else if (MainV2.comPort.MAV.cs.firmware == MissionPlanner.ArduPilot.Firmwares.ArduCopter2 ||
                MainV2.comPort.MAV.aptype == global::MAVLink.MAV_TYPE.QUADROTOR)
            {
                return new Maps.GMapMarkerQuad(
                    location,
                    (float)PluginState.gps_track_over_ground_raw,
                    MainV2.comPort.MAV.sysid)
                {
                    Tag = MainV2.comPort.MAV
                };
            }
            else if (MainV2.comPort.MAV.aptype == global::MAVLink.MAV_TYPE.SURFACE_BOAT)
            {
                return new Maps.GMapMarkerBoat(
                    location,
                    (float)PluginState.gps_track_over_ground_raw)
                {
                    Tag = MainV2.comPort.MAV
                };
            }
            else
            {
                return new Maps.GMapMarkerPlane(
                    location,
                    (float)PluginState.gps_track_over_ground_raw)
                {
                    Tag = MainV2.comPort.MAV
                };
            }
        }

        private void ReplaceGPSMarker()
        {
            markersOverlay.Markers.Remove(gpsMarker);
            gpsMarker = GetGPSMarker();
            markersOverlay.Markers.Add(gpsMarker);
        }

        private void SendImHerePos(double latitude, double longitude)
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

        private void ImHereToolStripMenuItem_Click(object sender, EventArgs e)
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
            catch (System.Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
            }
        }
    }
}
