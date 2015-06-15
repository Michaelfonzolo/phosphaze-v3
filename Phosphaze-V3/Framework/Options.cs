using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework.Extensions;

namespace Phosphaze_V3.Framework
{
    /// <summary>
    /// A valid screen resolution.
    /// </summary>
    public struct Resolution
    {
        public int width, height;

        public double aspectRatio { get { return width / height; } }

        public double diagonal { get { return Math.Sqrt(width * width + height * height); } }

        public double area { get { return width * height; } }

        public Resolution(int w, int h)
        {
            width = w;
            height = h;
        }

        /// <summary>
        /// The native screen resolution.
        /// </summary>
        public static Resolution native = new Resolution(Screen.PrimaryScreen.Bounds.Width, 
                                                         Screen.PrimaryScreen.Bounds.Height);
    }

    public static class Options
    {

        /// <summary>
        /// The valid standard resolutions.
        /// </summary>
        public static Resolution[] standardResolutions = {
            new Resolution(800, 600),  // 4:3
            new Resolution(960, 720),  // 4:3
            new Resolution(1024, 768), // 4:3
            new Resolution(1152, 864), // 4:3
            new Resolution(1280, 960), // 4:3
        };

        /// <summary>
        /// The vlaid widescreen resolutions.
        /// </summary>
        public static Resolution[] widescreenResolutions = {
            new Resolution(1280, 720), // 16:9
            new Resolution(1280, 768), // 5:3
            new Resolution(1280, 800), // 8:5
        };

        /// <summary>
        /// All valid resolutions.
        /// </summary>
        public static Resolution[] validResolutions =
            standardResolutions
            .Concat(widescreenResolutions)
            .Concat(new Resolution[] { Resolution.native });

        /// <summary>
        /// The index of the current resolution in validResolutions.
        /// </summary>
        public static int currentResolutionIndex = 0;

        /// <summary>
        /// The current resolution.
        /// </summary>
        public static Resolution currentResolution { get { return validResolutions[currentResolutionIndex]; } }

        /// <summary>
        /// Whether or not the display is stretched to be fullscreen.
        /// </summary>
        public static bool fullscreen = false;
    }
}
