using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Disconnects Web scrapper</para>
    /// <para type="description">Disconnects from chromium and closes it.</para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Disconnect, "WebScraper")]
    public class DisconnectWebScraper : Cmdlet
    {
        /// <summary>
        /// <para type="description">Instance of web scrapper to close.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            Scraper.Dispose();
        }
    }
}
