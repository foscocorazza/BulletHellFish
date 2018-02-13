using Microsoft.VisualBasic.FileIO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Utils
    {
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        
        public static long millisecondsSinceEpoch()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static int IntFromTextBox(TextBox tb, int defaultValue)
        {
            int x = defaultValue;
            int.TryParse(tb.Text, out x);
            return x;
        }

        public static void fillWithDate(TextBox tb, long ms)
        {
            int seconds = (int)ms / 1000;

            int h = seconds / 3600;
            int m = (seconds - h * 3600) / 60;
            int s = seconds % 60;

            tb.Text = h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
        }

        public static string GetWindowCaption(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public static long parseHourToMilliseconds(TextBox textBox)
        {
            string[] split = textBox.Text.Split(':');

            if (split != null && split.Length == 3)
            {
                int h = 0;
                int m = 0;
                int s = 0;

                if (int.TryParse(split[0], out h))
                {
                    if (int.TryParse(split[1], out m))
                    {
                        if (int.TryParse(split[2], out s))
                        {
                            return (s + m * 60 + h * 3600) * 1000;
                        }
                    }
                }
            }
            return 0;
        }

        internal static void Abort(ref Thread t)
        {
            if (t != null)
            {
                t.Abort();
                t = null;
            }
        }
    }
}
