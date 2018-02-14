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
        private static readonly string ActualScreenPath = "actual_screen.jpg";
        private static readonly string StartScreenPath = "start_screen.jpg";
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

        public static bool ThisScreenPresent(string thisPath, IntPtr emuHandle, Rectangle bounder, double similarity = 0.8 )
        {
            SaveScreenAs(ActualScreenPath, emuHandle, bounder);

            try { 
                ComparableImage actualScreen = new ComparableImage(new FileInfo(ActualScreenPath));
                ComparableImage thisScreen = new ComparableImage(new FileInfo(thisPath));

                if (thisScreen.CalculateSimilarity(actualScreen) > similarity) return true;
            }
            catch (Exception e) { }

            return false;
        }

        public static bool StartScreenPresent(IntPtr emuHandle)
        {
            SaveScreenAs(ActualScreenPath, emuHandle, NullRectangle);
            ComparableImage actualScreen = new ComparableImage(new FileInfo(ActualScreenPath));

            foreach (string startScreenPath in EveryStartScreen())
            {
                ComparableImage startScreen = new ComparableImage(new FileInfo(startScreenPath));
                if (startScreen.CalculateSimilarity(actualScreen) > 0.9) return true;
            }
            return false;
        }

        internal static bool ThisScreenPresent(string v, IntPtr emuHandle, double similarity = 0.8)
        {
            return ThisScreenPresent(v, emuHandle, NullRectangle);
        }

        public static void SaveScreenAs(string path, IntPtr emuHandle) {
            SaveScreenAs(path, emuHandle, NullRectangle);
        }

        public static void SaveScreenAs(string path, IntPtr emuHandle, Rectangle bounder)
        {

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
                    if (File.Exists(path)) File.Delete(path);
                    bitmap.Save(path, ImageFormat.Png);
                }


                catch (Exception e) { }

                bitmap.Dispose();
            }
        }
        public static string GenerateStartScreenPath()
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(StartScreenPath);
            string extension = Path.GetExtension(StartScreenPath);
            string path = Path.GetDirectoryName(StartScreenPath);
            string newFullPath = StartScreenPath;

            while (System.IO.File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}_{1}", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }


        private static string[] EveryStartScreen()
        {
            string rootFolderPath = "./";
            string filesToDelete = "start_screen*.jpg";
            return Directory.GetFiles(rootFolderPath, filesToDelete);
        }

        internal static void CleanStartScreens()
        {
            foreach (string file in EveryStartScreen())
            {
                File.Delete(file);
            }
        }
    }
    
}
