using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using ScraperionFramework;

namespace Scraperion
{
    /// <summary>
    /// <para type="synopsis">Send keys to focused application.</para>
    /// <para type="description">Simulates key presses on target application. This uses the standard .net send keys syntax. Enter is {ENTER} etc.</para>
    /// <para type="description">For more information see https://docs.microsoft.com/en-us/dotnet/framework/winforms/how-to-simulate-mouse-and-keyboard-events-in-code </para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Send, "Keys")]
    public class SendKeys : Cmdlet
    {
        /// <summary>
        /// <para type="description">Text top type in .net Standard send keys syntax.</para>
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
