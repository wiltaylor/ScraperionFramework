using System.Management.Automation;
using System.Threading;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsLifecycle.Wait, "WebScrapExpression")]
    public class WaitWebScraperExpression : Cmdlet
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Expression { get; set; }

        protected override void ProcessRecord()
        {
            while (Scraper.Exec(Expression).ToLower() != "true")
            {
                Thread.Sleep(1000);
            }
        }
    }
}
