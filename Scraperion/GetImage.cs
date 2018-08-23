using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Get, "Image")]
    public class GetImage : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PathSet")]
        public string Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ScreenSet")]
        public SwitchParameter Screen { get; set; }


        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 1)]
        public int X { get; set; }

        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 2)]
        public int Y { get; set; }

        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 3)]
        public int Width { get; set; }

        [Parameter(ParameterSetName = "ScreenSet")]
        [Parameter(ParameterSetName = "ImageSet", Mandatory = true, Position = 4)]
        public int Height { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "ImageSet")]
        public Bitmap Image { get; set; }

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
