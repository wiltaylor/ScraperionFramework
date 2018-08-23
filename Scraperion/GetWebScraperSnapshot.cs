using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Get, "WebScraperSnapshot")]
    public class GetWebScraperSnapshot : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public WebScraper Scraper { get; set; }

        [Parameter]
        public SwitchParameter Pdf { get; set; }

        protected override void ProcessRecord()
        {
            if (Pdf)
            {
                WriteObject(Scraper.CreatePdf());
            }
            
            WriteObject(Scraper.SnapshotBitmap());
        }
    }
}
