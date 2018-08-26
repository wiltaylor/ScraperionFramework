using System.Drawing;
using System.Management.Automation;

namespace Scraperion
{
    [Cmdlet(VerbsCommon.Show, "Image")]
    public class ShowImage : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        protected override void ProcessRecord()
        {
            var window = new frmImage(Image);
            window.ShowDialog();
        }
    }
}
