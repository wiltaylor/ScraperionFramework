using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Simulate mouse click.</para>
    /// <para type="description">Similar to send keys but for the mouse.</para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Send, "Mouse")]
    public class SendMouse: Cmdlet
    {
        /// <summary>
        /// <para type="description">Button to simulate.</para>
        /// </summary>
        [Parameter]
        [ValidateSet("Left", "Right")]
        public string Button { get; set; } = "Left";

        /// <summary>
        /// <para type="description">Simulate a mouse click (mouse down then mouse up).</para>
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "ClickSet")]
        public SwitchParameter Click { get; set; }

        /// <summary>
        /// <para type="description">Simulates a mouse down.</para>
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "UpSet")]
        public SwitchParameter Up { get; set; }

        /// <summary>
        /// <para type="description">Simulates a mouse up.</para>
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = "DownSet")]
        public SwitchParameter Down { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();
            
            if (Click)
                ss.MouseClick(Button == "Left" ? MouseButton.Left : MouseButton.Right);
            else if (Up)
                ss.MouseUp(Button == "Left" ? MouseButton.Left : MouseButton.Right);
            else if (Down)
                ss.MouseDown(Button == "Left" ? MouseButton.Left : MouseButton.Right);
        }
    }
}
