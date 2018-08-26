using System;
using System.Drawing;
using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Finds an image on the screen and clicks it.</para>
    /// <para type="description">Finds an image on the screen and clicks it.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Select, "Image")]
    public class SelectImage : Cmdlet
    {
        /// <summary>
        /// <para type="description">Image to search for on the screen.</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// <para type="description">Mouse button to use.</para>
        /// </summary>
        [Parameter(Position = 1)]
        [ValidateSet("Left", "Right")]
        public string Button { get; set; } = "Left";

        /// <summary>
        /// <para type="description">Offset to click on image when found in X axis.</para>
        /// </summary>
        [Parameter]
        public int XOffset { get; set; } = 0;

        /// <summary>
        /// <para type="description">Offset to click on image when found in Y axis.</para>
        /// </summary>
        [Parameter]
        public int YOffset { get; set; } = 0;

        /// <summary>
        /// <para type="description">Click on target image (mouse down then mouse up).</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "ClickSet")]
        public SwitchParameter Click { get; set; }

        /// <summary>
        /// <para type="description">Mouse up on target.</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "UpSet")]
        public SwitchParameter Up { get; set; }

        /// <summary>
        /// <para type="description">Mouse down on target.</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "DownSet")]
        public SwitchParameter Down { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
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
