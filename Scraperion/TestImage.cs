using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Return if target image appears on the screen or not.</para>
    /// <para type="description">Test if image appears on the screen or not.</para>
    /// </summary>
    [Cmdlet(VerbsDiagnostic.Test, "Image")]
    public class TestImage : Cmdlet
    {
        /// <summary>
        /// <para type="description">Image to test the existance of.</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// <para type="description">Image to search in. If left blank screen is used instead.</para>
        /// </summary>
        [Parameter]
        public Bitmap SearchInImage { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
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
