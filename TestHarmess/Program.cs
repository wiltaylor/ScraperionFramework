using ScraperionFramework;

namespace TestHarmess
{
    class Program
    {
        static void Main(string[] args)
        {

            WebScraper scrapper = new WebScraper(false) {Url = "http://www.weatherzone.com.au/"};


            scrapper.Dispose();

        }
    }
}
