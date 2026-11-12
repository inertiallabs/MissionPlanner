using System;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace InertialLabs.Maps
{
    [Serializable]
    public class GMapMarkerBase : GMapMarker
    {
        public float heading = 0;

        public GMapMarkerBase(PointLatLng pos) : base(pos)
        {
        }
    }
}
