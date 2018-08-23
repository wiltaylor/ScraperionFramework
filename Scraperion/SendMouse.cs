using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommunications.Send, "Mouse")]
    public class SendMouse: Cmdlet
    {
        [Parameter]
        [ValidateSet("Left", "Right")]
        public string Button { get; set; } = "Left";

        [Parameter(Mandatory = false, ParameterSetName = "ClickSet")]
        public SwitchParameter Click { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "UpSet")]
        public SwitchParameter Up { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "DownSet")]
        public SwitchParameter Down { get; set; }

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
