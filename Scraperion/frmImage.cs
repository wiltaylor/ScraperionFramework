using System.Drawing;
using System.Windows.Forms;

namespace Scraperion
{
    /// <inheritdoc />
    /// <summary>
    /// Class used to preview image files by Show-Image cmdlet.
    /// </summary>
    public partial class FrmImage : Form
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="img">Image to preview.</param>
        public FrmImage(Image img)
        {
            InitializeComponent();

            if (img.Width < Size.Width || img.Height < Size.Height)
            {
                Size = new Size(img.Width, img.Height);
            }

            ImageBox.Image = img;

            
        }
    }
}
