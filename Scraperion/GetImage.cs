using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Retrives an image</para>
    /// <para type="description">Retrives an image ready for use with screen scrapping cmdlets.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Image")]
    public class GetImage : Cmdlet
    {
        /// <summary>
        /// <para type="description">Path to image to load. Can be most common image formats png, bmp, jpg, etc</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PathSet")]
        public string Path { get; set; }

        /// <summary>
        /// <para type="description">Use this switch to grab a screenshot of the screen.</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "ScreenSet")]
        public SwitchParameter Screen { get; set; }

        /// <summary>
        /// <para type="description">X coordinates to being capture from</para>
        /// </summary>
        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 1)]
        public int X { get; set; }

        /// <summary>
        /// <para type="description">Y coordinates to being capture from</para>
        /// </summary>

        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 2)]
        public int Y { get; set; }

        /// <summary>
        /// <para type="description">Width of capture.</para>
        /// </summary>
        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 3)]
        public int Width { get; set; }

        /// <summary>
        /// <para type="description">Height of capture.</para>
        /// </summary>
        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 4)]
        public int Height { get; set; }

        /// <summary>
        /// <para type="description">Another image to do capture from, use X, Y, Width and Hight to select a subsection of image.</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "ImageSet")]
        public Bitmap Image { get; set; }

        /// <summary>
        /// Powershell logic
        /// </summary>
        protected override void ProcessRecord()
        {
            if (Path != null)
            {
                WriteObject(new Bitmap(Path));
                return;
            }

            if (Screen)
            {
                if (X == 0 && Y == 0 && Width == 0 && Height == 0)
                {
                    WriteObject(new ScreenScraper().CaptureScreen());
                    return;
                }
                else
                {
                    WriteObject(new ScreenScraper().CaptureArea(new Rectangle(X, Y, Width, Height)));
                    return;
                }
            }

            var result = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(Image, new Rectangle(0,0, Width, Height), X, Y, Width, Height, GraphicsUnit.Pixel);
            }

            WriteObject(result);
        }
    }
}
