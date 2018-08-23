using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommunications.Disconnect, "WebScraper")]
    public class DisconnectWebScraper : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public WebScraper Scraper { get; set; }

        protected override void ProcessRecord()
        {
            Scraper.Dispose();
        }
    }
}
