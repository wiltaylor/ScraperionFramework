using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Set element to have focus in web page.</para>
    /// <para type="description">Set element in chromium to have focus.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Select, "WebScraperFocus")]
    public class SetWebScraperFocus : Cmdlet
    {
        /// <summary>
        /// <para type="description">Scraper to set focus on.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">Target to select using javascript selector. For more info see: https://www.w3schools.com/jsref/met_document_queryselector.asp </para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public string Target { get; set; }

        /// <summary>
        /// Powershell logic
        /// </summary>
        protected override void ProcessRecord()
        {
            Scraper.Focus(Target);
        }
    }
}
