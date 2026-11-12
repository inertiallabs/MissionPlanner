using System;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;
using MissionPlanner.Utilities;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerRover : GMapMarkerBase
    {
        static readonly System.Drawing.Size SizeSt =
            new System.Drawing.Size(global::MissionPlanner.Maps.Resources.rover.Width,
                global::MissionPlanner.Maps.Resources.rover.Height);

        static readonly Bitmap icon = Resources.roverBlue;

        public GMapMarkerRover(PointLatLng p, float heading)
            : base(p)
        {
            this.heading = heading;
            Size = SizeSt;
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
#if NET472_OR_GREATER
            using (var ia = new System.Drawing.Imaging.ImageAttributes())
            {
                if (IsTransparent)
                {
                    // Draw image with transparency using a color matrix
                    var cm = new System.Drawing.Imaging.ColorMatrix { Matrix33 = 0.39f };
                    ia.SetColorMatrix(cm, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                }
                g.DrawImage(icon, new Rectangle(-icon.Width / 2, -icon.Width / 2, icon.Width, icon.Height), 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, ia);
            }
#else
            g.DrawImageUnscaled(global::MissionPlanner.Maps.Resources.rover,
                Size.Width / -2,
                Size.Height / -2);
#endif
            g.Transform = temp;
            temp.Dispose();
        }
    }
}