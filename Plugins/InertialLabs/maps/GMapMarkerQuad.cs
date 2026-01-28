using System;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;
using MissionPlanner.Utilities;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerQuad : GMapMarkerBase
    {
        private readonly Bitmap icon = global::MissionPlanner.Maps.Resources.quadicon;

        private static Pen _bluePen;
        private static Pen _transparentBluePen;
        private static Pen _darkBluePen;
        private static Pen _transparentDarkBluePen;

        private static SolidBrush _darkBlueBrush;
        private static SolidBrush _transparentDarkBlueBrush;
        private static SolidBrush _textBrush;
        private static SolidBrush _transparentTextBrush;

        static Pen bluePen
        {
            set
            {
                _bluePen = value;
                _transparentBluePen = new Pen(Color.FromArgb(100, _bluePen.Color), _bluePen.Width);
            }
        }

        static Pen darkBluePen
        {
            set
            {
                _darkBluePen = value;
                _transparentDarkBluePen = new Pen(Color.FromArgb(100, _darkBluePen.Color), _darkBluePen.Width);
            }
        }

        static SolidBrush darkBlueBrush
        {
            set
            {
                _darkBlueBrush = value;
                _transparentDarkBlueBrush = new SolidBrush(Color.FromArgb(100, _darkBlueBrush.Color));
            }
        }
        static SolidBrush textBrush
        {
            set
            {
                _textBrush = value;
                _transparentTextBrush = new SolidBrush(Color.FromArgb(100, _textBrush.Color));
            }
        }

        private Pen thisBluePen
        {
            get
            {
                return IsTransparent ? _transparentBluePen : _bluePen;
            }
        }
        private Pen thisDarkBluePen
        {
            get
            {
                return IsTransparent ? _transparentDarkBluePen : _darkBluePen;
            }
        }
        private SolidBrush thisDarkBlueBrush
        {
            get
            {
                return IsTransparent ? _transparentDarkBlueBrush : _darkBlueBrush;
            }
        }
        private SolidBrush thisTextBrush
        {
            get
            {
                return IsTransparent ? _transparentTextBrush : _textBrush;
            }
        }


        static GMapMarkerQuad()
        {
            bluePen = new Pen(MissionPlanner.Maps.ExtensionsMaps.ColorFromHex("00aeef"), 3);
            darkBluePen = new Pen(MissionPlanner.Maps.ExtensionsMaps.ColorFromHex("0000ff"), 3);
            darkBlueBrush = new SolidBrush(_darkBluePen.Color);
            textBrush = new SolidBrush(Color.Red);
        }

        public GMapMarkerQuad(PointLatLng p, float heading, int sysid)
            : base(p)
        {
            this.heading = heading;
            Size = icon.Size;
            // for hitzone
            Offset = new Point(-icon.Width / 2, -icon.Width / 2);
        }

        public override void OnRender(IGraphics g)
        {
            if (IsHidden)
            {
                return;
            }

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
            g.DrawArc(thisDarkBluePen, 35f - 10 + Offset.X, 12f - 10 + Offset.Y, 20, 20, 0, 360);
            g.DrawArc(thisDarkBluePen, 35f - 10 + Offset.X, 57f - 10 + Offset.Y, 20, 20, 0, 360);
            g.DrawArc(thisDarkBluePen, 57f - 10 + Offset.X, 35f - 10 + Offset.Y, 20, 20, 0, 360);
            g.DrawArc(thisDarkBluePen, 12f - 10 + Offset.X, 35f - 10 + Offset.Y, 20, 20, 0, 360);

            g.DrawArc(thisDarkBluePen, 35f - 2.5f + Offset.X, 12f - 2.5f + Offset.Y, 5, 5, 0, 360);
            g.DrawArc(thisDarkBluePen, 35f - 2.5f + Offset.X, 57f - 2.5f + Offset.Y, 5, 5, 0, 360);
            g.DrawArc(thisDarkBluePen, 57f - 2.5f + Offset.X, 35f - 2.5f + Offset.Y, 5, 5, 0, 360);
            g.DrawArc(thisDarkBluePen, 12f - 2.5f + Offset.X, 35f - 2.5f + Offset.Y, 5, 5, 0, 360);

            g.DrawLine(thisBluePen, 35 + Offset.X, 12 + Offset.Y, 35 + Offset.X, 35 + Offset.Y);
            g.DrawLine(thisDarkBluePen, 35 + Offset.X, 36 + Offset.Y, 35 + Offset.X, 57 + Offset.Y);
            g.DrawLine(thisDarkBluePen, 57 + Offset.X, 35 + Offset.Y, 12 + Offset.X, 35 + Offset.Y);

            g.FillRectangle(thisDarkBlueBrush, 32 + Offset.X, 30 + Offset.Y, 5, 8);

            g.Transform = temp;
        }
    }
}