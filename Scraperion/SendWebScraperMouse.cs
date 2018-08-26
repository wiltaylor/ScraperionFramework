using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Sends simulated mouse or tap event to target on page.</para>
    /// <para type="description">Simulates a finger press or mouse event on target on page.</para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Send, "WebScraperMouse")]
    public class SendWebScraperMouse : Cmdlet
    {
        /// <summary>
        /// <para type="description">Scraper object to simulat event on.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">Mouse button to simulate.</para>
        /// </summary>
        [Parameter]
        [ValidateSet("Left", "Right")]
        public string Button { get; set; } = "Left";

        /// <summary>
        /// <para type="description">Simulate a mouse click (mouse down then mouse up).</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "ClickSet")]
        public SwitchParameter Click { get; set; }

        /// <summary>
        /// <para type="description">Simulates a mouse up event.</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "UpSet")]
        public SwitchParameter Up { get; set; }

        /// <summary>
        /// <para type="description">Simulates a mouse down event.</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "DownSet")]
        public SwitchParameter Down { get; set; }

        /// <summary>
        /// <para type="description">Target to apply mouse event to. Use Target selector to specify.</para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TargetSet")]
        public string Target { get; set; }

        /// <summary>
        /// <para type="description">Javascript selector to select object to click on. For more details see https://www.w3schools.com/jsref/met_document_queryselector.asp </para>
        /// </summary>
        [Parameter(ParameterSetName = "TargetSet")]
        public SwitchParameter Tap { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
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
