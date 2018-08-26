using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Simulates moving the mouse to a location on the web page.</para>
    /// <para type="description">Simulates moving the mouse to a location on the web page.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Move, "WebScraperMouse")]
    public class MoveWebScraperMouse :Cmdlet 
    {
        /// <summary>
        /// <para type="description">Web scrapper object to move mouse on.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">X coordinate to move mouse to on page</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public decimal X { get; set; }


        /// <summary>
        /// <para type="description">Y coordinate to move mouse to on page</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 2)]
        public decimal Y { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            Scraper.MoveMouse(X, Y);
        }
    }
}
