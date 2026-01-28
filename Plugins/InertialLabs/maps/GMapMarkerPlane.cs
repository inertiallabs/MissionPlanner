using System;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;
using MissionPlanner.Utilities;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerPlane : GMapMarkerBase
    {
        private readonly Bitmap icon = global::MissionPlanner.Maps.Resources.planeicon;

        static SolidBrush shadow = new SolidBrush(Color.FromArgb(50, Color.Black));

        static Point[] plane = new Point[] {
            new Point(28,0),
            new Point(32,13),
            new Point(53,27),
            new Point(55,32),
            new Point(31,28),
            new Point(30,35),
            new Point(30,43),
            new Point(37,48),
            new Point(37,50),
            new Point(29,50),
            new Point(29,53),
            // inverse
            new Point(inv(29,28),53),
            new Point(inv(29,28),50),
            new Point(inv(37,28),50),
            new Point(inv(37,28),48),
            new Point(inv(30,28),43),
            new Point(inv(30,28),35),
            new Point(inv(31,28),28),
            new Point(inv(55,28),32),
            new Point(inv(53,28),27),
            new Point(inv(32,28),13),
            new Point(inv(28,28),0),
            };

        private static int inv(int input, int mid)
        {
            var delta = input - mid;

            return mid - delta;
        }

        public GMapMarkerPlane(PointLatLng p, float heading)
            : base(p)
        {
            this.heading = heading;
            Size = icon.Size;
        }

        public override void OnRender(IGraphics g)
        {
            if(IsHidden)
            {
                return;
            }

            var temp = g.Transform;
            g.TranslateTransform(LocalPosition.X, LocalPosition.Y);

            g.RotateTransform(-Overlay.Control.Bearing);

            try
            {
                g.RotateTransform(heading);
            }
            catch
            {
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // the shadow
            g.TranslateTransform(-26, -26);

            g.FillPolygon(shadow, plane);

            // the plane
            g.TranslateTransform(-2, -2);

            var color = Color.Blue;

            if(IsTransparent)
            {
                color = Color.FromArgb(100, color);
            }

            g.FillPolygon(new SolidBrush(color), plane);

            g.Transform = temp;
        }
    }
}