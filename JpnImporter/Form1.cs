using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JpnImporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (PortIsUse(2020))
                new WebClient().DownloadString(@"http://127.0.0.1:2020/set/lyric/wq?r=1&val=" +
                                               string.Join(",", richTextBox.SelectedText.Trim().ToCharArray()));
            else
            {
                MessageBox.Show(@"Foxy Listener is not running");
            }
        }

        private void ButtonConvert_Click(object sender, EventArgs e)
        {
            var str = richTextBox.Text;
            richTextBox.Clear();
            foreach (var item in str.Split('\n'))
            {
                var ifeLang = Activator.CreateInstance(Type.GetTypeFromProgID("MSIME.Japan")) as IFELanguage;
                int hr = ifeLang.Open();
                if (hr != 0) throw Marshal.GetExceptionForHR(hr);
                hr = ifeLang.GetPhonetic(item, 1, -1, out var yomigana);
                if (hr != 0) throw Marshal.GetExceptionForHR(hr);
                richTextBox.Text += yomigana + Environment.NewLine;
            }
        }

        public static bool PortIsUse(int port)
        {
            IPEndPoint[] ipEndPointsTcp = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
            IPEndPoint[] ipEndPointsUdp = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners();

            return ipEndPointsTcp.Any(endPoint => endPoint.Port == port)
                   || ipEndPointsUdp.Any(endPoint => endPoint.Port == port);
        }
    }

    [ComImport]
    [Guid("019F7152-E6DB-11d0-83C3-00C04FDDB82E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFELanguage
    {
        int Open();
        int Close();
        int GetJMorphResult(uint dwRequest, uint dwCMode, int cwchInput, [MarshalAs(UnmanagedType.LPWStr)] string pwchInput, IntPtr pfCInfo, out object ppResult);
        int GetConversionModeCaps(ref uint pdwCaps);
        int GetPhonetic([MarshalAs(UnmanagedType.BStr)] string @string, int start, int length, [MarshalAs(UnmanagedType.BStr)] out string result);
        int GetConversion([MarshalAs(UnmanagedType.BStr)] string @string, int start, int length, [MarshalAs(UnmanagedType.BStr)] out string result);
    }
}
