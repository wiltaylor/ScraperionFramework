using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommunications.Send, "WebScraperKeys")]
    public class SendWebScraperKeys : Cmdlet
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "TextSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Text { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "SecureTextSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public SecureString SecureText { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "CredentialSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

        protected override void ProcessRecord()
        {
            if (Text != null)
            {
                Scraper.SendKeys(Text);
                return;
            }

            if (SecureText != null)
            {
                Scraper.SendKeys(SecureStringToString(SecureText));
            }

            if (Credential != null)
            {
                Scraper.SendKeys(Credential.UserName);
                Scraper.SendKeys("{tab}");
                Scraper.SendKeys(Credential.GetNetworkCredential().Password);
            }
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
