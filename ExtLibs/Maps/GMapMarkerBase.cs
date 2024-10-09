using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace MissionPlanner.Maps
{
    [Serializable]
    public class GMapMarkerBase: GMapMarker
    {
        public static bool DisplayCOGSetting = true;
        public static bool DisplayHeadingSetting = true;
        public static bool DisplayNavBearingSetting = true;
        public static bool DisplayRadiusSetting = true;
        public static bool DisplayTargetSetting = true;
        public static int length = 500;
        public static InactiveDisplayStyleEnum InactiveDisplayStyle = InactiveDisplayStyleEnum.Normal;
        
        // Instance variables
        public bool IsActive = true;
        public bool DisplayLines = true;
        protected bool IsHidden => InactiveDisplayStyle == InactiveDisplayStyleEnum.Hidden && !IsActive;
        protected bool IsTransparent => InactiveDisplayStyle == InactiveDisplayStyleEnum.Transparent && !IsActive;
        protected bool DisplayCOG => DisplayCOGSetting && !IsTransparent && DisplayLines;
        protected bool DisplayHeading => DisplayHeadingSetting && !IsTransparent && DisplayLines;
        protected bool DisplayNavBearing => DisplayNavBearingSetting && !IsTransparent && DisplayLines;
        protected bool DisplayRadius => DisplayRadiusSetting && !IsTransparent && DisplayLines;
        protected bool DisplayTarget => DisplayTargetSetting && !IsTransparent && DisplayLines;
        
        public GMapMarkerBase(PointLatLng pos):base(pos)
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
