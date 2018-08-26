using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Invokes a javascript command</para>
    /// <para type="description">Invokes a javascript command in the browser and returns json of what was executed.</para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "WebScraperExpression")]
    public class InvokeWebScraperExpression : Cmdlet
    {
        /// <summary>
        /// <para type="description">Scrapper to invoke javascript on.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">Expression to execute.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 1)]
        public string Expression { get; set; }

        /// <summary>
        /// Powershell cmdlet logic
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject(Scraper.Exec(Expression));
        }
    }
}
