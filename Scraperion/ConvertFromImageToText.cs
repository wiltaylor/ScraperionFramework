using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsData.ConvertFrom, "ImageToText")]
    public class ConvertFromImageToText : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();
            WriteObject(ss.OCR(Image));
        }
    }
}
