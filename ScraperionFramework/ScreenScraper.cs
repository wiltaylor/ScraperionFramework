using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tesseract;

namespace ScraperionFramework
{
    /// <summary>
    /// Screen scraper class
    /// This is useful for scrapping information from the screen and also controlling applications.
    ///
    /// Unlike similar frameworks this is based off images not finding controls. This allows it to do
    /// advanced things like drive applications through Citrix, VNC or RDP sessions.
    /// </summary>
    public class ScreenScraper
    {
        private enum ProcessDPIAwareness
        {
            ProcessDPIUnaware = 0,
            ProcessSystemDPIAware = 1,
            ProcessPerMonitorDPIAware = 2
        }

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(ProcessDPIAwareness value);

        [DllImport("user32.dll", EntryPoint = "mouse_event",  CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        /// <summary>
        /// This method fixes windows DPI issues which prevent the library from working properly.
        /// This must be called first thing at the start of your application before any image related methods are called.
        /// </summary>
        public static void SetupDPI()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwareness(ProcessDPIAwareness.ProcessPerMonitorDPIAware);
            }
        }
        
        /// <summary>
        /// Takes a screenshot of the screen.
        ///
        /// On multi monitor systems it will take a screenshot of all screens.
        /// </summary>
        /// <returns>Bitmap containing screenshot.</returns>
        public Bitmap CaptureScreen()
        {
            var left = 0;
            var top = 0;
            var right = 0;
            var bottom = 0;

            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Bounds.Top < top)
                    top = screen.Bounds.Top;
                if (screen.Bounds.Left < left)
                    left = screen.Bounds.Left;
                if (screen.Bounds.Right > right)
                    right = screen.Bounds.Right;
                if (screen.Bounds.Bottom > bottom)
                    bottom = screen.Bounds.Bottom;
            }

            var rect = new Rectangle(top, left, right - left, bottom - top);

            var result = new Bitmap(right - left, bottom - top);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, rect.Size);
            }

            return result;

        }

        /// <summary>
        /// Captures an area of teh screen.
        /// </summary>
        /// <param name="area">Area of screen to capture.</param>
        /// <returns>Bitmap containing captured image.</returns>
        public Bitmap CaptureArea(Rectangle area)
        {
            var result = new Bitmap(area.Width, area.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, area.Size);
            }

            return result;
        }

        /// <summary>
        /// Finds an image in another image.
        /// </summary>
        /// <param name="sourceImage">Image to search</param>
        /// <param name="targetImage">Image to find.</param>
        /// <param name="stride">When searching how many pixels should be compared. Lower the number the more acurate the search.</param>
        /// <returns>Rectangle with coordinates of found image.</returns>
        public Rectangle Find(Bitmap sourceImage, Bitmap targetImage, int stride = 4)
        {
            for (int x = 0; x < sourceImage.Width - targetImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height - targetImage.Height; y++)
                {
                    if (sourceImage.GetPixel(x, y) == targetImage.GetPixel(0, 0) &&
                        sourceImage.GetPixel(x + targetImage.Width - 1, y) == targetImage.GetPixel(targetImage.Width - 1, 0) &&
                        sourceImage.GetPixel(x, y + targetImage.Height - 1) == targetImage.GetPixel(0, targetImage.Height - 1) &&
                        sourceImage.GetPixel(x + targetImage.Width - 1, y + targetImage.Height - 1) == targetImage.GetPixel(targetImage.Width - 1, targetImage.Height - 1))// &&
                    {
                        bool anyMiss = false;

                        for (int tx = 0; tx < targetImage.Width; tx += stride)
                        {
                            if (anyMiss)
                                break;

                            for (int ty = 0; ty < targetImage.Height; ty += stride)
                            {
                                if (sourceImage.GetPixel(x + tx, y + ty) != targetImage.GetPixel(tx, ty))
                                {
                                    anyMiss = true;
                                    break;
                                }
                            }
                        }

                        if (!anyMiss)
                            return new Rectangle(x, y, targetImage.Width, targetImage.Height);
                    }
                }
            }

            return new Rectangle(-1, -1, -1, -1);
        }

        /// <summary>
        /// Find all instances of image in target image.
        /// </summary>
        /// <param name="sourceImage">Image to search in.</param>
        /// <param name="targetImage">Image to search for.</param>
        /// <param name="stride">When searching how many pixels should be compared. Lower the number the more acurate the search.</param>
        /// <returns>IEnumerable of rectangles containing all the locations the image was found.</returns>
        public IEnumerable<Rectangle> FindAll(Bitmap sourceImage, Bitmap targetImage, int stride = 4)
        {
            var result = new List<Rectangle>();

            for (int x = 0; x < sourceImage.Width - targetImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height - targetImage.Height; y++)
                {
                    if (sourceImage.GetPixel(x, y) == targetImage.GetPixel(0, 0) &&
                        sourceImage.GetPixel(x + targetImage.Width, y) ==
                        targetImage.GetPixel(targetImage.Width - 1, 0) &&
                        sourceImage.GetPixel(x, y + targetImage.Height) ==
                        targetImage.GetPixel(0, targetImage.Height - 1) &&
                        sourceImage.GetPixel(x + targetImage.Width, y + targetImage.Height) ==
                        targetImage.GetPixel(targetImage.Width - 1, targetImage.Height - 1) &&
                        sourceImage.GetPixel(x + targetImage.Width / 2, y + targetImage.Height / 2) ==
                        targetImage.GetPixel(targetImage.Width / 2 - 1, targetImage.Height / 2 - 1))
                    {
                        bool anyMiss = false;

                        for (int tx = 0; tx < targetImage.Width; tx += stride)
                        {
                            if (anyMiss)
                                break;

                            for (int ty = 0; ty < targetImage.Height; ty += stride)
                            {
                                if (sourceImage.GetPixel(x + tx, y + ty) != targetImage.GetPixel(tx, ty))
                                {
                                    anyMiss = true;
                                    break;
                                }
                            }
                        }

                        if (!anyMiss)
                            result.Add(new Rectangle(x, y, targetImage.Width, targetImage.Height));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Simulates key presses on the active application.
        /// </summary>
        /// <param name="keys">Keys to simulate.</param>
        public void TypeKeys(string keys)
        {
            SendKeys.SendWait(keys);
        }

        /// <summary>
        /// Move the mouse to target location.
        /// </summary>
        /// <param name="x">x coordinate to move mouse to.</param>
        /// <param name="y">y coordinate to move mouse to.</param>
        public void MoveMouse(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }

        /// <summary>
        /// Simulates a mouse down and then mouse up.
        /// </summary>
        /// <param name="button">Mouse button to use.</param>
        public void MouseClick(MouseButton button)
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            if (button == MouseButton.Left)
                MouseEvent(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            else
                MouseEvent(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        /// <summary>
        /// Simulates a mouse down event.
        /// </summary>
        /// <param name="button">Mouse button to do.</param>
        public void MouseDown(MouseButton button)
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            MouseEvent(button == MouseButton.Left ?
                (uint)MOUSEEVENTF_LEFTDOWN :
                (uint)MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
        }


        /// <summary>
        /// Simulate a mouse up event.
        /// </summary>
        /// <param name="button">Mouse button to use.</param>
        public void MouseUp(MouseButton button)
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            MouseEvent(button == MouseButton.Left ?
                (uint)MOUSEEVENTF_LEFTUP :
                (uint)MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        /// <summary>
        /// Run OCR over an image.
        /// </summary>
        /// <param name="image">Image to run OCR over.</param>
        /// <returns>Text result from OCR.</returns>
        public string OCR(Bitmap image)
        {
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            using (var engine = new TesseractEngine(assemblyFolder + "\\tessdata", "eng", EngineMode.Default))
            {
                using (var page = engine.Process(image, PageSegMode.Auto))
                {
                    return page.GetText();
                }
            }
        }
    }

}
