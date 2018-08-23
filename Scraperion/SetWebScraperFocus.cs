using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Select, "WebScraperFocus")]
    public class SetWebScraperFocus : Cmdlet
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Target { get; set; }

        protected override void ProcessRecord()
        {
            Scraper.Focus(Target);
        }
    }
}
