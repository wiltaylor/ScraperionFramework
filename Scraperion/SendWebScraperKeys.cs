using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Send key presses to browser.</para>
    /// <para type="description">Simulate key presses in browser window.</para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Send, "WebScraperKeys")]
    public class SendWebScraperKeys : Cmdlet
    {
        /// <summary>
        /// <para type="description">Scraper object to send the key presses to.</para>
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public WebScraper Scraper { get; set; }

        /// <summary>
        /// <para type="description">Text to send to browser window.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "TextSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Text { get; set; }

        /// <summary>
        /// <para type="description">Decode a secure string and send that instead. Useful for sending passwords.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "SecureTextSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public SecureString SecureText { get; set; }

        /// <summary>
        /// <para type="description">Sends the contents of a PSCredential object. Will press tab between username and password.</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "CredentialSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

        /// <summary>
        /// Powershell logic.
        /// </summary>
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
