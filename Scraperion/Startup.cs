using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// Contains code that runs when module is loaded.
    ///
    ///
    /// DPI awareness code needs to be called before any image preview code is done to make screen coordinates work properly.
    /// </summary>
    public class Startup : IModuleAssemblyInitializer
    {
        /// <summary>
        /// Module startup code
        /// </summary>
        public void OnImport()
        {
            ScreenScraper.SetupDPI();
        }
    }
}
