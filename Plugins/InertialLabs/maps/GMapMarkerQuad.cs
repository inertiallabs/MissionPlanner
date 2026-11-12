using System;
using System.Drawing;
using GMap.NET;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerQuad : GMapMarkerBase
    {
        private readonly Bitmap icon = global::MissionPlanner.Maps.Resources.quadicon;

        private static readonly Pen bluePen;
        private static readonly Pen darkBluePen;
        private static readonly SolidBrush darkBlueBrush;

        static GMapMarkerQuad()
        {
            bluePen = new Pen(MissionPlanner.Maps.ExtensionsMaps.ColorFromHex("00aeef"), 3);
            darkBluePen = new Pen(MissionPlanner.Maps.ExtensionsMaps.ColorFromHex("0000ff"), 3);
            darkBlueBrush = new SolidBrush(darkBluePen.Color);
        }

        public GMapMarkerQuad(PointLatLng p, float heading)
            : base(p)
        {
            this.heading = heading;
            Size = icon.Size;
            // for hitzone
            Offset = new Point(-icon.Width / 2, -icon.Width / 2);
        }

        public override void OnRender(IGraphics g)
        {
            var temp = g.Transform;
            g.TranslateTransform(LocalPosition.X, LocalPosition.Y);
            g.TranslateTransform(-Offset.X, -Offset.Y);
            g.RotateTransform(-Overlay.Control.Bearing);
      
            // anti NaN
            try
            {
                g.RotateTransform(heading);
            }
            catch
            {
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //motors
            g.DrawArc(darkBluePen, 35f - 10 + Offset.X, 12f - 10 + Offset.Y, 20, 20, 0, 360);
            g.DrawArc(darkBluePen, 35f - 10 + Offset.X, 57f - 10 + Offset.Y, 20, 20, 0, 360);
            g.DrawArc(darkBluePen, 57f - 10 + Offset.X, 35f - 10 + Offset.Y, 20, 20, 0, 360);
            g.DrawArc(darkBluePen, 12f - 10 + Offset.X, 35f - 10 + Offset.Y, 20, 20, 0, 360);

            g.DrawArc(darkBluePen, 35f - 2.5f + Offset.X, 12f - 2.5f + Offset.Y, 5, 5, 0, 360);
            g.DrawArc(darkBluePen, 35f - 2.5f + Offset.X, 57f - 2.5f + Offset.Y, 5, 5, 0, 360);
            g.DrawArc(darkBluePen, 57f - 2.5f + Offset.X, 35f - 2.5f + Offset.Y, 5, 5, 0, 360);
            g.DrawArc(darkBluePen, 12f - 2.5f + Offset.X, 35f - 2.5f + Offset.Y, 5, 5, 0, 360);

            g.DrawLine(bluePen, 35 + Offset.X, 12 + Offset.Y, 35 + Offset.X, 35 + Offset.Y);
            g.DrawLine(darkBluePen, 35 + Offset.X, 36 + Offset.Y, 35 + Offset.X, 57 + Offset.Y);
            g.DrawLine(darkBluePen, 57 + Offset.X, 35 + Offset.Y, 12 + Offset.X, 35 + Offset.Y);

            g.FillRectangle(darkBlueBrush, 32 + Offset.X, 30 + Offset.Y, 5, 8);

            g.Transform = temp;
            temp.Dispose();
        }
    }
}