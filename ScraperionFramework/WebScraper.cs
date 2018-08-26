using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace ScraperionFramework
{
    public class WebScraper : IDisposable
    {
        private readonly Browser m_browser;
        private readonly Page m_page;
        private decimal m_MouseX = 0;
        private decimal m_MouseY = 0;

        public WebScraper(bool headless = true, string agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36")
        {

            var ops = new BrowserFetcherOptions
            {
                Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\scraperion\\browser"
            };

            (new BrowserFetcher(ops).DownloadAsync(BrowserFetcher.DefaultRevision)).Wait();

            var browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = headless,
                IgnoreHTTPSErrors = true,

            });

            browser.Wait();
            m_browser = browser.Result;


            var page = m_browser.NewPageAsync();
            page.Wait();
            m_page = page.Result;

            m_page.Error += (s, e) => {
                Console.WriteLine("Error:" + e.ToString());
            };

            m_page.PageError += (s, e) =>
            {
                Console.WriteLine("Error:" + e.ToString());
            };

            m_page.Console += (s, e) => { Console.WriteLine(e.Message.Text); };

            m_page.SetUserAgentAsync(agent).Wait();
        }

        public void SetAuth(string username, string password)
        {
            SetAuthAsync(username, password).Wait();
        }

        public async Task SetAuthAsync(string username, string password)
        {
            await m_page.AuthenticateAsync(new Credentials {Username = username, Password = password});
        }

        public void SetViewPort(int width, int height)
        {
            SetViewPortAsync(width, height).Wait();
        }

        public async Task SetViewPortAsync(int width, int height)
        {
            await m_page.SetViewportAsync(new ViewPortOptions
            {
                Width = width,
                Height = height
            });
        }

        public string Url
        {
            get => m_page.Url;
            set
            {
                try
                {
                    m_page.GoToAsync(value).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public string Exec(string script)
        {
            var result = ExecAsync(script);
            result.Wait();

            return result.Result;
        }

        public async Task<string> ExecAsync(string script)
        {

           var data = await m_page.EvaluateExpressionAsync(script);

            return (string)data.ToString();

        }

        public Bitmap SnapshotBitmap()
        {
            var result = SnapshotBitmapAsync();
            result.Wait();
            return result.Result;
        }

        public async Task<Bitmap> SnapshotBitmapAsync()
        {
            var data = await m_page.ScreenshotStreamAsync();
            var image = new Bitmap(data);
            data.Dispose();

            return image;
        }

        public void SendKeys(string text)
        {
            SendKeysAsync(text).Wait();
        }

        public async Task SendKeysAsync(string text)
        {
            await m_page.Keyboard.TypeAsync(text);
        }

        public void MoveMouse(decimal x, decimal y)
        {
            MoveMouseAsync(x, y).Wait();
        }

        public async Task MoveMouseAsync(decimal x, decimal y)
        {
            await m_page.Mouse.MoveAsync(x, y);
            m_MouseX = x;
            m_MouseY = y;
        }

        public void MouseClick(MouseButton button)
        {
            MouseClickAsync(button).Wait();
        }

        public async Task MouseClickAsync(MouseButton button)
        {
            await m_page.Mouse.ClickAsync(m_MouseX, m_MouseY, new ClickOptions{ Button = button == MouseButton.Left ? PuppeteerSharp.Input.MouseButton.Left : PuppeteerSharp.Input.MouseButton.Right });
        }

        public void MouseUp(MouseButton button)
        {
            MouseUpAsync(button).Wait();
            
        }

        public async Task MouseUpAsync(MouseButton button)
        {
            await m_page.Mouse.UpAsync(new ClickOptions { Button = button == MouseButton.Left ? PuppeteerSharp.Input.MouseButton.Left : PuppeteerSharp.Input.MouseButton.Right });
        }

        public void MouseDown(MouseButton button)
        {
            MouseDownAsync(button).Wait();
        }

        public async Task MouseDownAsync(MouseButton button)
        {
            await m_page.Mouse.DownAsync(new ClickOptions { Button = button == MouseButton.Left ? PuppeteerSharp.Input.MouseButton.Left : PuppeteerSharp.Input.MouseButton.Right });

        }

        public void TapScreen(string target)
        {
            TapScreenAsync(target).Wait();
        }

        public async Task TapScreenAsync(string target)
        {
            await m_page.TapAsync(target);
        }

        public Stream CreatePdf()
        {
            var data = CreatePdfAsync();
            data.Wait();
            return data.Result;

        }

        public async Task<Stream> CreatePdfAsync()
        {
            return await m_page.PdfStreamAsync();
        }

        public void WaitOnScript(string expression)
        {
            WaitOnScriptAsync(expression).Wait();
        }

        public async Task WaitOnScriptAsync(string expression)
        {
            await m_page.WaitForExpressionAsync(expression);
        }

        public void Focus(string target)
        {
            FocusAsync(target).Wait();
        }

        public async Task FocusAsync(string target)
        {
            await m_page.FocusAsync(target);
        }

        public void Click(string target)
        {
            ClickAsync(target).Wait();
        }

        public async Task ClickAsync(string target)
        {
            await m_page.ClickAsync(target);
        }

        public string Content
        {
            get
            {
                var data = m_page.GetContentAsync();
                data.Wait();
                return data.Result;
            }
            set
            {
                var data = m_page.SetContentAsync(value);
                data.Wait();
            }
        }

        public void Dispose()
        {
            m_browser?.Dispose();
            m_page?.Dispose();
        }
    }
}
