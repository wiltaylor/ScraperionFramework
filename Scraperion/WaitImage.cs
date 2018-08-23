using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsLifecycle.Wait, "Image")]
    public class WaitImage : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 0)]
        public Bitmap Image { get; set; }

        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();

            while (true)
            {
                var result = ss.Find(ss.CaptureScreen(), Image);

                if (result.Left != -1 && result.Right != -1)
                    break;
            }
            

        }
    }
}
