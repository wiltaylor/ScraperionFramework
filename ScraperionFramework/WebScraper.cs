using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace ScraperionFramework
{
    /// <summary>
    /// Web scraper class.
    /// This class handles all interaction with chromium.
    /// </summary>
    public class WebScraper : IDisposable
    {
        private readonly Browser m_browser;
        private readonly Page m_page;
        private decimal m_MouseX = 0;
        private decimal m_MouseY = 0;

        /// <summary>
        /// Defualt agent string this library uses. Simulates Chrome installed on windows 10.
        /// </summary>
        public static readonly string DefaultAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="headless">Set to false to show chromium window.</param>
        /// <param name="agent">Agent to use when accessing pages. Uses DefaultAgent if non is set.</param>
        public WebScraper(bool headless = true, string agent = "")
        {

            if (agent == "")
                agent = DefaultAgent;


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

        /// <summary>
        /// Set username and password to authenticate against web pages with.
        /// </summary>
        /// <param name="username">Username to authenticate with</param>
        /// <param name="password">Password to autrhenticate with.</param>
        public void SetAuth(string username, string password)
        {
            SetAuthAsync(username, password).Wait();
        }

        private async Task SetAuthAsync(string username, string password)
        {
            await m_page.AuthenticateAsync(new Credentials {Username = username, Password = password});
        }

        /// <summary>
        /// Sets the view port size of the page.
        /// </summary>
        /// <param name="width">Width of the page in pixels.</param>
        /// <param name="height">Height of page in pixels.</param>
        public void SetViewPort(int width, int height)
        {
            SetViewPortAsync(width, height).Wait();
        }

        private async Task SetViewPortAsync(int width, int height)
        {
            await m_page.SetViewportAsync(new ViewPortOptions
            {
                Width = width,
                Height = height
            });
        }

        /// <summary>
        /// Gets or sets the url the page is currently at.
        /// </summary>
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

        /// <summary>
        /// Executes a javascript expression on page.
        /// This is simuilar to typing a command in the java console.
        /// </summary>
        /// <param name="script">Expression to run.</param>
        /// <returns>Json of executed result.</returns>
        public string Exec(string script)
        {
            var result = ExecAsync(script);
            result.Wait();

            return result.Result;
        }

        private async Task<string> ExecAsync(string script)
        {

           var data = await m_page.EvaluateExpressionAsync(script);

            return (string)data.ToString();

        }
        /// <summary>
        /// Takes a screenshot of the target page.
        /// </summary>
        /// <returns>Bitmap image containing screenshot.</returns>
        public Bitmap SnapshotBitmap()
        {
            var result = SnapshotBitmapAsync();
            result.Wait();
            return result.Result;
        }

        private async Task<Bitmap> SnapshotBitmapAsync()
        {
            var data = await m_page.ScreenshotStreamAsync();
            var image = new Bitmap(data);
            data.Dispose();

            return image;
        }

        /// <summary>
        /// Simulates key presses on page.
        /// </summary>
        /// <param name="text">Text to send to page.</param>
        public void SendKeys(string text)
        {
            SendKeysAsync(text).Wait();
        }

        private async Task SendKeysAsync(string text)
        {
            await m_page.Keyboard.TypeAsync(text);
        }

        /// <summary>
        /// Simulates moving the mouse on the page.
        ///
        /// Note: this does not move the system mouse.
        /// </summary>
        /// <param name="x">X coordinates to move mouse to.</param>
        /// <param name="y">Y coordinates to move mouse to.</param>
        public void MoveMouse(decimal x, decimal y)
        {
            MoveMouseAsync(x, y).Wait();
        }

        private async Task MoveMouseAsync(decimal x, decimal y)
        {
            await m_page.Mouse.MoveAsync(x, y);
            m_MouseX = x;
            m_MouseY = y;
        }

        /// <summary>
        /// Simulates a mouse click on page.
        /// </summary>
        /// <param name="button">Mouse button to simulate.</param>
        public void MouseClick(MouseButton button)
        {
            MouseClickAsync(button).Wait();
        }

        private async Task MouseClickAsync(MouseButton button)
        {
            await m_page.Mouse.ClickAsync(m_MouseX, m_MouseY, new ClickOptions{ Button = button == MouseButton.Left ? PuppeteerSharp.Input.MouseButton.Left : PuppeteerSharp.Input.MouseButton.Right });
        }

        /// <summary>
        /// Simulates a mouse up event on page.
        /// </summary>
        /// <param name="button">Mouse button to simulate.</param>
        public void MouseUp(MouseButton button)
        {
            MouseUpAsync(button).Wait();
            
        }

        private async Task MouseUpAsync(MouseButton button)
        {
            await m_page.Mouse.UpAsync(new ClickOptions { Button = button == MouseButton.Left ? PuppeteerSharp.Input.MouseButton.Left : PuppeteerSharp.Input.MouseButton.Right });
        }

        /// <summary>
        /// Simulates a mouse down event on page.
        /// </summary>
        /// <param name="button">Mouse button to simulate.</param>
        public void MouseDown(MouseButton button)
        {
            MouseDownAsync(button).Wait();
        }

        private async Task MouseDownAsync(MouseButton button)
        {
            await m_page.Mouse.DownAsync(new ClickOptions { Button = button == MouseButton.Left ? PuppeteerSharp.Input.MouseButton.Left : PuppeteerSharp.Input.MouseButton.Right });

        }

        /// <summary>
        /// Simulates a touch tap on a page.
        /// </summary>
        /// <param name="target">Javascript selector for element to tap on.</param>
        public void TapScreen(string target)
        {
            TapScreenAsync(target).Wait();
        }

        private async Task TapScreenAsync(string target)
        {
            await m_page.TapAsync(target);
        }

        /// <summary>
        /// Generates a pdf of the page.
        /// </summary>
        /// <returns>Stream containing the pdf data.</returns>
        public Stream CreatePdf()
        {
            var data = CreatePdfAsync();
            data.Wait();
            return data.Result;

        }

        private async Task<Stream> CreatePdfAsync()
        {
            return await m_page.PdfStreamAsync();
        }

        /// <summary>
        /// Waits for expression to be to be true.
        /// </summary>
        /// <param name="expression">Expression to wait on.</param>
        public void WaitOnScript(string expression)
        {
            WaitOnScriptAsync(expression).Wait();
        }

        private async Task WaitOnScriptAsync(string expression)
        {
            await m_page.WaitForExpressionAsync(expression);
        }

        /// <summary>
        /// Selects element on page to have focus.
        /// </summary>
        /// <param name="target">Javascript selector to make have focus.</param>
        public void Focus(string target)
        {
            FocusAsync(target).Wait();
        }

        private async Task FocusAsync(string target)
        {
            await m_page.FocusAsync(target);
        }

        /// <summary>
        /// Clicks on target element on page.
        /// </summary>
        /// <param name="target">Javascript selector of element to click on.</param>
        public void Click(string target)
        {
            ClickAsync(target).Wait();
        }

        private async Task ClickAsync(string target)
        {
            await m_page.ClickAsync(target);
        }

        /// <summary>
        /// Html content of page. Useful for scraping the html directly.
        /// </summary>
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

        /// <inheritdoc />
        /// <summary>
        /// Dispose method.
        /// This will close out chromium session.
        /// </summary>
        public void Dispose()
        {
            m_browser?.Dispose();
            m_page?.Dispose();
        }
    }
}
