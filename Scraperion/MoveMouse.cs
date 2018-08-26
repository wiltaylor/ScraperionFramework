using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Moves the mouse on the screen.</para>
    /// <para type="description">Moves the mouse to target location on the screen.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Move, "Mouse")]
    public class MoveMouse : Cmdlet
    {
        /// <summary>
        /// <para type="description">X coordinate to move mouse to on screen</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public int X { get; set; }

        /// <summary>
        /// <para type="description">Y coordinate to move mouse to on screen</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public int Y { get; set; }

        /// <summary>
        /// Powershell logic
        /// </summary>
        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();

            ss.MoveMouse(X, Y);
        }
    }
}
