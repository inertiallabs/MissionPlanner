using MissionPlanner;
using System;
using System.Windows.Forms;

namespace InertialLabs
{
    // Passes a handler the MAVLink packets of the current vehicle, MainV2.comPort.MAV. It's the vehicle that Flight Data
    // shows and the plugin sends its commands to. The host replaces MainV2.comPort when the user selects a vehicle
    // on another link or when the main link closes. Because the switching raises no event a timer here checks this switches.
    internal class CurrentVehiclePackets : IDisposable
    {
        private readonly Action<MAVState, global::MAVLink.MAVLinkMessage> handler;
        private readonly Timer comPortCheckTimer = new Timer { Interval = 500 };
        private MAVLinkInterface subscribedPort;

        // Create it on the UI thread.
        public CurrentVehiclePackets(Action<MAVState, global::MAVLink.MAVLinkMessage> handler)
        {
            this.handler = handler;
            SubscribeToComPort();
            comPortCheckTimer.Tick += (s, e) => SubscribeToComPort();
            comPortCheckTimer.Start();
        }

        public void Dispose()
        {
            comPortCheckTimer.Dispose();
            subscribedPort.OnPacketReceived -= OnPacketReceived;
        }

        private void SubscribeToComPort()
        {
            var port = MainV2.comPort;
            if (port == subscribedPort)
            {
                return;
            }

            if (subscribedPort != null)
            {
                subscribedPort.OnPacketReceived -= OnPacketReceived;
            }

            subscribedPort = port;
            subscribedPort.OnPacketReceived += OnPacketReceived;
        }

        // Called on the thread that read the packet, which can be any thread
        private void OnPacketReceived(object sender, global::MAVLink.MAVLinkMessage message)
        {
            var vehicle = MainV2.comPort.MAV;
            if (sender == vehicle.parent && message.sysid == vehicle.sysid && message.compid == vehicle.compid)
            {
                handler(vehicle, message);
            }
        }
    }
}
