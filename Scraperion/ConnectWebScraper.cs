using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using ScraperionFramework;

namespace Scraperion
{

    /// <summary>
    /// <para type="synopsis">This cmdlet creates a WebScrapper object.</para>
    /// <para type="description">This cmdlet creates a WebScrapper object and connects a chromium instance to it.</para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Connect, "WebScraper")]
    public class ConnectWebScraper : Cmdlet
    {
        /// <summary>
        /// <para type="description">Credentials to use to connect to page.</para>
        /// </summary>
        [Parameter]
        public PSCredential Credential { get; set; }

        /// <summary>
        /// <para type="description">Width of web page in pixels.</para>
        /// </summary>
        [Parameter]
        public int Width { get; set; } = 1024;

        /// <summary>
        /// <para type="description">Height of web page in pixels.</para>
        /// </summary>
        [Parameter]
        public int Height { get; set; } = 768;

        /// <summary>
        /// <para type="description">Initial url to connect to.</para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public string Url { get; set; }

        /// <summary>
        /// <para type="description">Pass switch to show chromium browser window. Useful for debugging.</para>
        /// </summary>
        [Parameter]
        public SwitchParameter ShowUI { get; set; }

        /// <summary>
        /// <para type="description">Browser agent to use when browsing pages.</para>
        /// </summary>
        [Parameter]
        public string Agent { get; set; } = WebScraper.DefaultAgent;
        
        protected override void ProcessRecord()
        {
            var scrapper = new WebScraper(!ShowUI, Agent);
            
            if(Credential != null)
                scrapper.SetAuth(Credential.UserName, SecureStringToString(Credential.Password));

            scrapper.SetViewPort(Width, Height);

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
