using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using ScraperionFramework;

namespace Scraperion
{
    [Cmdlet(VerbsCommunications.Send, "Keys")]
    public class SendKeys : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "TextSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Text { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "SecureTextSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public SecureString SecureText { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "CredentialSet", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

        protected override void ProcessRecord()
        {
            var ss = new ScreenScraper();

            if (Text != null)
            {
                ss.TypeKeys(Text);
                return;
            }

            if (SecureText != null)
            {
                ss.TypeKeys(SecureStringToString(SecureText));
            }

            if (Credential != null)
            {
                ss.TypeKeys(Credential.UserName);
                ss.TypeKeys("{tab}");
                ss.TypeKeys(Credential.GetNetworkCredential().Password);
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
