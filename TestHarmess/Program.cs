using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScraperionFramework;

namespace TestHarmess
{
    class Program
    {
        static void Main(string[] args)
        {
            ScreenScraper.SetupDPI();

            var ss = new ScreenScraper();
           
            Console.WriteLine(ss.OCR(new Bitmap(@"C:\Users\wilfr\Desktop\test.png")));

        }
    }
}
