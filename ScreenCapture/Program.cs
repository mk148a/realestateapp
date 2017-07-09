using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenCapture
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        /// 

        [STAThread]      
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
         
            //Application.Run(new Form1());

            try
            {


                string filename = args[0];
                
                int dpi = 96;
                int fps = 24;
                int anim_length_in_secs = int.Parse(args[1]);
                int num_total_frames = fps * anim_length_in_secs;

                var secs = Enumerable.Range(0, num_total_frames).Select(t => (((double)t) / fps));
              
                var aviManager = new AviFile.AviManager(filename, false);
              
                AviFile.VideoStream aviStream = null;
                foreach (var sec in secs)
                {


                    Process[] processes = Process.GetProcessesByName("EmlakSlide.vshost");
                  
                    Process lol = processes[0];
                 
                    IntPtr ptr = lol.MainWindowHandle;
                    
                    Rect rect = new Rect();
                    GetWindowRect(ptr, ref rect);

                    int width = rect.Right - rect.Left;
                    int height = rect.Bottom - rect.Top-57;
                  
                    var bmp = new Bitmap(width, height);
                  
                    Graphics graphics = Graphics.FromImage(bmp);
                   
                    graphics.CopyFromScreen(rect.Left, rect.Top+57, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
                 
                    bmp.Save("test.png", ImageFormat.Png);
                  
                    string temp_bitmap = "test.png";
                    //     await  ConvertToBitmapSource(canvas1);

                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(temp_bitmap);
                    if (aviStream == null)
                    {
                        aviStream = aviManager.AddVideoStream(true, fps, bm);
                    }
                    else
                    {
                        aviStream.AddFrame(bm);

                    }
                    bm.Dispose();
                    

                }
             
                aviManager.Close();
                MessageBox.Show("video kaydedildi");
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
    }
}
