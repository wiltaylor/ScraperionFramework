using System.Drawing;
using System.Windows.Forms;

namespace Scraperion
{
    public partial class frmImage : Form
    {
        public frmImage(Bitmap img)
        {
            InitializeComponent();

            if (img.Width < this.Size.Width || img.Height < this.Size.Height)
            {
                this.Size = new Size(img.Width, img.Height);
            }

            ImageBox.Image = img;

            
        }
    }
}
