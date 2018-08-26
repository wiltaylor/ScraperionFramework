using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsDiagnostic.Test, "Image")]
    public class TestImage : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        [Parameter]
        public Bitmap SearchInImage { get; set; }

        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();

            if (SearchInImage == null)
                SearchInImage = ss.CaptureScreen();

            var result = ss.Find(SearchInImage, Image);


            //-1 indicates it didn't find the image.
            WriteObject(result.Left != -1 && result.Right != -1);
        }
    }
}
