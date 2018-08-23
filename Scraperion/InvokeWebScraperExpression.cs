using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsLifecycle.Invoke, "WebScraperExpression")]
    public class InvokeWebScraperExpression : Cmdlet
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 1)]
        public string Expression { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Scraper.ExecAsync(Expression));
        }
    }
}
