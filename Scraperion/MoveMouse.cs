using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Move, "Mouse")]
    public class MoveMouse : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public int X { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public int Y { get; set; }

        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();

            ss.MoveMouse(X, Y);
        }
    }
}
