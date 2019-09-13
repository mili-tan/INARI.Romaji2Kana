using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxyListener
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Visible = false;
            Fx.EffectsWindows(Handle, 300, Fx.AW_BLEND);
        }

        class Fx
        {
            [DllImport("user32.dll", EntryPoint = "AnimateWindow")]
            public static extern bool EffectsWindows(IntPtr handle, int effectsTime, int effectsFlags);
            public const Int32 AW_HOR_POSITIVE = 0x00000001;
            public const Int32 AW_HOR_NEGATIVE = 0x00000002;
            public const Int32 AW_VER_POSITIVE = 0x00000004;
            public const Int32 AW_VER_NEGATIVE = 0x00000008;
            public const Int32 AW_CENTER = 0x00000010;
            public const Int32 AW_HIDE = 0x00010000;
            public const Int32 AW_ACTIVATE = 0x00020000;
            public const Int32 AW_SLIDE = 0x00040000;
            public const Int32 AW_BLEND = 0x00080000;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Fx.EffectsWindows(Handle, 300, Fx.AW_BLEND);
        }
    }
}
