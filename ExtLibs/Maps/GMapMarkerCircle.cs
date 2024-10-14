using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace MissionPlanner.Maps
{
    [Serializable]
    public class GMapMarkerCircle : GMapMarker
    {
        public double radius = 0; // meters
        public Color color = Color.Red;

        public GMapMarkerCircle(PointLatLng p, double radius_meters, Color color)
            : base(p)
        {
            this.radius = radius_meters;
            this.color = color;
        }

        public override void OnRender(IGraphics g)
        {
            // width of visible part of the map in meters
            double width =
                        (Overlay.Control.MapProvider.Projection.GetDistance(Overlay.Control.FromLocalToLatLng(0, 0),
                             Overlay.Control.FromLocalToLatLng(Overlay.Control.Width, 0)) * 1000.0);
            // pixels in 1 meter on the map
            double pixesInMeter = Overlay.Control.Width / width;

            Pen pen = new Pen(this.color, 2);
            double radiusInPixels = radius * pixesInMeter; // pixels

            pen.DashStyle = DashStyle.Dash;
            g.DrawEllipse(pen,
                          LocalPosition.X - (float)radiusInPixels,
                          LocalPosition.Y - (float)radiusInPixels,
                          (float)radiusInPixels * 2,
                          (float)radiusInPixels * 2);
        }
    }
}