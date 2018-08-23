using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommunications.Connect, "WebScraper")]
    public class ConnectWebScraper : Cmdlet
    {
        [Parameter]
        public PSCredential Credential { get; set; }

        [Parameter]
        public int ViewPortWidth { get; set; } = 1024;

        [Parameter]
        public int ViewPortHeight { get; set; } = 768;

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public string Url { get; set; }

        protected override void ProcessRecord()
        {
            var scrapper = new WebScraper();
            
            if(Credential != null)
                scrapper.SetAuth(Credential.UserName, SecureStringToString(Credential.Password));

            scrapper.SetViewPort(ViewPortWidth, ViewPortHeight);

            scrapper.Url = Url;

            WriteObject(scrapper);
        }



        private string SecureStringToString(SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }


}
