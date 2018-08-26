using System.Drawing;
using System.Management.Automation;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Shows image</para>
    /// <para type="description">Opens a preview window for an image object. Useful for debugging.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Show, "Image")]
    public class ShowImage : Cmdlet
    {
        /// <summary>
        /// <para type="description">Image object to preview</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            var window = new FrmImage(Image);
            window.ShowDialog();
        }
    }
}
