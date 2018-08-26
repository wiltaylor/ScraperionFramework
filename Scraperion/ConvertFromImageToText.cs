using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Converts image to Text</para>
    /// <para type="description">Runs OCR over the image and returns the text returend.</para>
    /// </summary>
    [Cmdlet(VerbsData.ConvertFrom, "ImageToText")]
    public class ConvertFromImageToText : Cmdlet
    {

        /// <summary>
        /// <para type="description">Image to run ocr over.</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// Powershell logic
        /// </summary>
        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();
            WriteObject(ss.OCR(Image));
        }
    }
}
