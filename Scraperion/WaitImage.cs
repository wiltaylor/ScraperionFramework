using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Wait for image to appear on screen.</para>
    /// <para type="description">Wait for image to appear on screen.</para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Wait, "Image")]
    public class WaitImage : Cmdlet
    {
        /// <summary>
        /// <para type="description">Image to wait for.</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Position = 0)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
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
