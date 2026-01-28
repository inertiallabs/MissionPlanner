using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerBase : GMapMarker
    {
        public float heading = 0;
        public static InactiveDisplayStyleEnum InactiveDisplayStyle = InactiveDisplayStyleEnum.Normal;

        // Instance variables
        public bool IsActive = true;
        protected bool IsHidden => InactiveDisplayStyle == InactiveDisplayStyleEnum.Hidden && !IsActive;
        protected bool IsTransparent => InactiveDisplayStyle == InactiveDisplayStyleEnum.Transparent && !IsActive;

        public GMapMarkerBase(PointLatLng pos) : base(pos)
        {
        }

        public enum InactiveDisplayStyleEnum
        {
            Normal,
            Transparent,
            Hidden
        }
    }
}
