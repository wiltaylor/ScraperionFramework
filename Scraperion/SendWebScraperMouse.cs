using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommunications.Send, "WebScraperMouse")]
    public class SendWebScraperMouse : Cmdlet
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        [Parameter]
        [ValidateSet("Left", "Right")]
        public string Button { get; set; } = "Left";

        [Parameter(Mandatory = true, ParameterSetName = "ClickSet")]
        public SwitchParameter Click { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "UpSet")]
        public SwitchParameter Up { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "DownSet")]
        public SwitchParameter Down { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "TargetSet")]
        public string Target { get; set; }

        [Parameter(ParameterSetName = "TargetSet")]
        public SwitchParameter Tap { get; set; }

        protected override void ProcessRecord()
        {
            if(Target != null)
                if(Tap)
                    Scraper.TapScreen(Target);
                else
                    Scraper.Click(Target);
            else if (Click)
                Scraper.MouseClick(Button == "Left" ? MouseButton.Left : MouseButton.Right);
            else if(Up)
                Scraper.MouseUp(Button == "Left" ? MouseButton.Left : MouseButton.Right);
            else if(Down)
                Scraper.MouseDown(Button == "Left" ? MouseButton.Left : MouseButton.Right);

        }
    }
}
