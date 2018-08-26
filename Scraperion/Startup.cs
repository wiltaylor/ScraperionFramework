using System.Management.Automation;
using ScraperionFramework;

namespace Scraperion
{
    public class Startup : IModuleAssemblyInitializer
    {
        public void OnImport()
        {
            ScreenScraper.SetupDPI();
        }
    }
}
