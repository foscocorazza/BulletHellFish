using EyeOpen.Imaging.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellFish
{
    class ScreenshotManager
    {
        private static readonly string EXT = "png";
        private static readonly string ActualScreenPath = "actual_screen."+EXT;
        private static readonly Rectangle NullRectangle = new Rectangle(-1,-1,-1,-1);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private static Rectangle RectByHandle(IntPtr handle)
        {
            RECT rct;

            if (!GetWindowRect(new HandleRef(handle, handle), out rct))
            {
                return new Rectangle();
            }

            Rectangle myRect = new Rectangle();
            myRect.X = rct.Left;
            myRect.Y = rct.Top;
            myRect.Width = rct.Right - rct.Left + 1;
            myRect.Height = rct.Bottom - rct.Top + 1;
            return myRect;
        }

        public static bool ThisScreenPresent(string folder, string thisPath, IntPtr emuHandle, Rectangle bounder, double similarity = 0.8 )
        {
            SaveScreenAs(folder, ActualScreenPath, emuHandle, bounder);

            string FullActual = ProperFolder(folder) + ProperName(ActualScreenPath);
            string FullThis = ProperFolder(folder) + ProperName(thisPath);

            try { 
                ComparableImage actualScreen = new ComparableImage(new FileInfo(FullActual));
                ComparableImage thisScreen = new ComparableImage(new FileInfo(FullThis));

                if (thisScreen.CalculateSimilarity(actualScreen) > similarity) return true;
            }
            catch (Exception e) { }

            return false;
        }

        /*public static bool StartScreenPresent(IntPtr emuHandle)
        {
            SaveScreenAs(ActualScreenPath, emuHandle, NullRectangle);
            ComparableImage actualScreen = new ComparableImage(new FileInfo(ActualScreenPath));

            foreach (string startScreenPath in EveryStartScreen())
            {
                ComparableImage startScreen = new ComparableImage(new FileInfo(startScreenPath));
                if (startScreen.CalculateSimilarity(actualScreen) > 0.9) return true;
            }
            return false;
        }*/

        internal static bool ThisScreenPresent(string folder, string thisPath, IntPtr emuHandle, double similarity = 0.8)
        {
            return ThisScreenPresent(folder, thisPath, emuHandle, NullRectangle);
        }

        public static void SaveScreenAs(string path, string name, IntPtr emuHandle) {
            SaveScreenAs(path, name, emuHandle, NullRectangle);
        }

        public static void SaveScreenAs(string folder, string name, IntPtr emuHandle, Rectangle bounder)
        {
            name = ProperName(name);
            folder = ProperFolder(folder);

            Rectangle bounds = RectByHandle(emuHandle);
            if (bounder == NullRectangle) {
                bounder = new Rectangle(0,0,bounds.Width, bounds.Height);
            }

            using (Bitmap bitmap = new Bitmap(bounder.Width, bounder.Height, PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    
                    g.CopyFromScreen(bounds.Left + bounder.Left, bounds.Top + bounder.Top, 0, 0, bounder.Size);
                    g.Dispose();
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                try
                {
                    if (File.Exists(folder + name)) File.Delete(folder + name);
                    bitmap.Save(folder + name, ImageFormat.Png);
                }


                catch (Exception e) { }

                bitmap.Dispose();
            }
        }

        private static string ProperFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                folder = ".\\";
            }

            if (!folder.EndsWith("\\"))
            {
                folder += "\\";
            }

            return folder;
        }

        private static string ProperName(string name)
        {
            string[] splits = name.Split('.');

            string nameonly = splits[0];
            string extension = splits.Length > 1 ? splits[1] : EXT;

            return nameonly + "."+extension;
        }

        public static string GenerateScreenPath(string folder, string basename)
        {
            int count = 1;
            string propername = ProperName(basename);

            string fileNameOnly = Path.GetFileNameWithoutExtension(propername);
            string extension = Path.GetExtension(propername);
            string path = ProperFolder(folder);

            while (File.Exists(path + propername))
            {
                string tempFileName = string.Format("{0}_{1}", fileNameOnly, count++);
                propername = ProperName(tempFileName + extension);
            }
            return propername;
        }


        private static string[] EveryScreenNamed(string folder, string basename)
        {
            string[] splits = ProperName(basename).Split('.');

            string nameonly = splits[0];
            string extension = splits.Length > 1 ? splits[1] : EXT;
            string filesToDelete = nameonly + "*."+ extension;
            return Directory.GetFiles(ProperFolder(folder), filesToDelete);
        }

        internal static void ClearScreens(string folder, string basename)
        {
            foreach (string file in EveryScreenNamed(folder, basename))
            {
                File.Delete(file);
            }
        }
    }
    
}
