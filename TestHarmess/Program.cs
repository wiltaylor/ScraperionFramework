using ScraperionFramework;

namespace TestHarmess
{
    class Program
    {
        static void Main(string[] args)
        {

            //AppDomain.CurrentDomain.UnhandledException += (s, a) =>
            //{
            //    Console.WriteLine("Crashing exception!" + a.ToString());

            //};


            //Application.ThreadException += (s, a) =>
            //{
            //    Console.WriteLine("Thread exception!");
           // };


            WebScraper scrapper = new WebScraper(false);

            scrapper.Url = "http://www.weatherzone.com.au/";

            scrapper.Dispose();

        }
    }
}
