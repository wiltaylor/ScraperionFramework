using System.IO;
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

        [Parameter]
        public string Path { get; set; }

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
