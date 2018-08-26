using System.IO;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Captures a screenshot of current page.</para>
    /// <para type="description">Captures a screenshot of the current page.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "WebScraperSnapshot")]
    public class GetWebScraperSnapshot : Cmdlet
    {
        /// <summary>
        /// <para type="description">Scrapper object to take screenshot from.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">Creates a pdf of the target page instead.</para>
        /// </summary>
        [Parameter]
        public SwitchParameter Pdf { get; set; }

        /// <summary>
        /// <para type="description">Optional path to store the image or pdf.</para>
        /// </summary>
        [Parameter]
        public string Path { get; set; }

        /// <summary>
        /// Main cmdlet logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (Pdf)
            {
                var pdf = Scraper.CreatePdf();

                WriteObject(pdf);

                if (Path == null)
                    return;

                var buffer = new byte[pdf.Length];
                pdf.Read(buffer, 0, buffer.Length);
                File.WriteAllBytes(Path, buffer);

                return;
            }

            var img = Scraper.SnapshotBitmap();

            WriteObject(img);

            if (Path == null)
                return;

            img.Save(Path);

        }
    }
}
