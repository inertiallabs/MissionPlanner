using System;
using System.Drawing;
using GMap.NET;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerBoat : GMapMarkerBase
    {
        static readonly System.Drawing.Size SizeSt =
            new System.Drawing.Size(global::MissionPlanner.Maps.Resources.boat.Width,
                global::MissionPlanner.Maps.Resources.boat.Height);

        static readonly Bitmap icon = Resources.boatBlue;

        public GMapMarkerBoat(PointLatLng p, float heading)
            : base(p)
        {
            this.heading = heading;
            Size = SizeSt;
        }

        public override void OnRender(IGraphics g)
        {
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
            g.DrawImage(icon, new Rectangle(-icon.Width / 2, -icon.Width / 2, icon.Width, icon.Height), 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel);
#else
            g.DrawImageUnscaled(global::MissionPlanner.Maps.Resources.boat,
                Size.Width / -2,
                Size.Height / -2);
#endif

            g.Transform = temp;
            temp.Dispose();
        }
    }
}