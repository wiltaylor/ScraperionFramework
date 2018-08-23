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

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void SetupDPI()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwareness(ProcessDPIAwareness.ProcessPerMonitorDPIAware);
            }
        }


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

        public Bitmap CaptureArea(Rectangle area)
        {
            var result = new Bitmap(area.Width, area.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, area.Size);
            }

            return result;
        }

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

        public void TypeKeys(string keys)
        {
            SendKeys.SendWait(keys);
        }

        public void MoveMouse(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }

        public void MouseClick(MouseButton button)
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            if (button == MouseButton.Left)
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            else
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        public void MouseDown(MouseButton button)
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            mouse_event(button == MouseButton.Left ?
                (uint)MOUSEEVENTF_LEFTDOWN :
                (uint)MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
        }

        public void MouseUp(MouseButton button)
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            mouse_event(button == MouseButton.Left ?
                (uint)MOUSEEVENTF_LEFTUP :
                (uint)MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        public string OCR(Bitmap image)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
