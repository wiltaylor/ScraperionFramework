using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Select, "Image")]
    public class SelectImage : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        [Parameter(Position = 1)]
        [ValidateSet("Left", "Right")]
        public string Button { get; set; } = "Left";

        [Parameter]
        public int XOffset { get; set; } = 0;

        [Parameter]
        public int YOffset { get; set; } = 0;

        [Parameter(Mandatory = true, ParameterSetName = "ClickSet")]
        public SwitchParameter Click { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "UpSet")]
        public SwitchParameter Up { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "DownSet")]
        public SwitchParameter Down { get; set; }

        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();

            var pos = ss.Find(ss.CaptureScreen(), Image);

            if (pos.Right == -1 && pos.Left == -1)
                throw new ApplicationException("Can't find image on screen!");

            ss.MoveMouse(pos.X + XOffset, pos.Y + YOffset);

            if (Click)
                ss.MouseClick(Button == "Left" ? MouseButton.Left : MouseButton.Right);
            else if (Up)
                ss.MouseUp(Button == "Left" ? MouseButton.Left : MouseButton.Right);
            else if(Down)
                ss.MouseDown(Button == "Left" ? MouseButton.Left : MouseButton.Right);
        }
    }
}
