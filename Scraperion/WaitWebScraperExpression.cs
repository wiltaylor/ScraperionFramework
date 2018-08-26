using System.Management.Automation;
using System.Threading;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Wait for web scrapper expression to be true.</para>
    /// <para type="description">Wait for web scrapper expression to be true.</para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "WebScraperExpression")]
    public class WaitWebScraperExpression : Cmdlet
    {
        /// <summary>
        /// <para type="description">Scraper to wait on expression to be true.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">Javascript expression to test.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public string Expression { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            while (Scraper.Exec(Expression)?.ToLower() != "true")
            {
                Thread.Sleep(1000);
            }
        }
    }
}
