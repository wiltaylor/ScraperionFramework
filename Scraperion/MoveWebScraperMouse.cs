using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Move, "WebScraperMouse")]
    public class MoveWebScraperMouse :Cmdlet 
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public decimal X { get; set; }

        [Parameter(Mandatory = true, Position = 2)]
        public decimal Y { get; set; }

        protected override void ProcessRecord()
        {
            Scraper.MoveMouse(X, Y);
        }
    }
}
