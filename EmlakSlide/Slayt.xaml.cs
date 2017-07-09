using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Configuration;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace EmlakSlide
{
    /// <summary>
    /// Slayt.xaml etkileşim mantığı
    /// </summary>
    public partial class Slayt : BasePage
    {
      
        private DispatcherTimer timerImageChange;
        private System.Windows.Controls.Image[] ImageControls;

        private List<string> Images = new List<string>();
        private static string[] ValidImageExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
        private static string[] TransitionEffects = new[] { "Fade" };
        private string TransitionType, strImagePath = "";
        private int CurrentSourceIndex, CurrentCtrlIndex, EffectIndex = 0, IntervalTimer = 1;
        public static List<İlan> ilanlar = new List<İlan>();
        public static int mevcutilan;
        private void sliderpage_Loaded(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show(ilanlar.Count().ToString() );
        }
       
       
        public Slayt()
        {
            InitializeComponent();
            //Initialize Image control, Image directory path and Image timer.
            IntervalTimer = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalTime"]);
            strImagePath = ConfigurationManager.AppSettings["ImagePath"];
            ImageControls = new[] { myImage, myImage2 };
          
        }

        private void myImage2_Drop(object sender, DragEventArgs e)
        {
           
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadImageFolder(files);
                MessageBox.Show(files.Count() + " adet fotoğraf eklendi");
                //PlaySlideShow();
               

                ////do whatever you need to do with the folder path
                //timerImageChange.IsEnabled = true;
            
               
            }
        }

        private void newbutton_Click(object sender, RoutedEventArgs e)
        {
            Images.Clear();
            try
            {
                timerImageChange.Stop();
            }
            catch
            {


            }
            aciklamatxt.Text = "yeni ilan";
            this.NavigationService.Refresh();
         
        }
    

        private List<İlan> ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            List<İlan> obj = (List<İlan>)binForm.Deserialize(memStream);

            return obj;
        }
        private void savtebtn_Click(object sender, RoutedEventArgs e)
        {
            //burda kaldık
            string filename = "";
            foreach (var item in ilanlar)
            {
                MessageBox.Show(item.Resimler.Count().ToString());
            }
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Slaytlar"; // Default file name
            dlg.DefaultExt = ".slt"; // Default file extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.Filter = "Emlak Slider dosyasi (.slt)|*.slt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filename = dlg.FileName;

               // Save(JsonConvert.SerializeObject(ilanlar), filename);
                //File.WriteAllBytes(filename, ObjectToByteArray(ilanlar));

                BinaryFormatter b = new BinaryFormatter();
                StreamWriter sw = new StreamWriter(filename);
                b.Serialize(sw.BaseStream, ilanlar);
                sw.Close();
            }


        }

        private void saveilan_Click(object sender, RoutedEventArgs e)
        {
            İlan yeni = new İlan();
            yeni.aciklama = aciklamatxt.Text;
            yeni.alan = alantxt.Text;
            yeni.baslik = basliktxt.Text;
            yeni.ceptel = cepteltxt.Text;
            yeni.emlaktipi = emlaktipitxt.Text;
            yeni.firmaadi = firmatxt.Text;
            yeni.fiyat = fiyattxt.Text;
            yeni.il = iltxt.Text;
            yeni.ilanno = ilannotxt.Text;
            yeni.ilantarihi = tarihtxt.Text;
            yeni.ilçe = ilçetxt.Text;
            yeni.mahalle = mahalletxt.Text;
            yeni.odasayisi = odatxt.Text;
            yeni.Resimler = Images;
            yeni.sabittel = sabitteltxt.Text;

            if (ilanlar.Any(a=>a.ilanno==ilannotxt.Text))
            {
                ilanlar[mevcutilan] = yeni;
            }
            else
            {
                ilanlar.Add(yeni);
                mevcutilan = ilanlar.Count() - 1;
            }
           
        }

        private void openbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Emlak Slider dosyasi (.slt)|*.slt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
               
                foreach (string filename in openFileDialog.FileNames)
                {

                    List<İlan> gln = ByteArrayToObject(File.ReadAllBytes(filename));

                    foreach (var item in gln)
                    {
                        ilanlar.Add(item);
                    }
                }
                İlan yeni = ilanlar.Last();
                aciklamatxt.Text = yeni.aciklama;
                alantxt.Text = yeni.alan;
                basliktxt.Text = yeni.baslik;
                cepteltxt.Text = yeni.ceptel;
                emlaktipitxt.Text = yeni.emlaktipi;
                firmatxt.Text = yeni.firmaadi;
                fiyattxt.Text = yeni.fiyat;
                iltxt.Text = yeni.il;
                ilannotxt.Text = yeni.ilanno;
                tarihtxt.Text = yeni.ilantarihi;
                ilçetxt.Text = yeni.ilçe;
                mahalletxt.Text = yeni.mahalle;
                odatxt.Text = yeni.odasayisi;
                Images = yeni.Resimler;
                sabitteltxt.Text = yeni.sabittel;


            }
            mevcutilan = ilanlar.Count - 1;
            slaybaslat();


        }

        private void LoadImageFolder(string[] folder)
        {
            ErrorText.Visibility = Visibility.Collapsed;
            var sw = System.Diagnostics.Stopwatch.StartNew();
        
            Images.Clear();

            foreach (var item in folder)
            {             
                Images.Add(imagetostring(item));
             
            }         
          
            sw.Stop();           
            Console.WriteLine("Total time to load {0} images: {1}ms", Images.Count, sw.ElapsedMilliseconds);
          
        }
        private void slaybaslat()
        {
            timerImageChange = new DispatcherTimer();
            timerImageChange.Interval = new TimeSpan(0, 0, IntervalTimer);
            timerImageChange.Tick += new EventHandler(timerImageChange_Tick);
            PlaySlideShow();

            //do whatever you need to do with the folder path
            timerImageChange.IsEnabled = true;
            

        }

        private void nextbtn_Click(object sender, RoutedEventArgs e)
        {
            if (ilanlar.Count>1)
            {
                //0<2
                try
                {
                    timerImageChange.Stop();
                }
                catch
                {


                }
                if (mevcutilan<ilanlar.Count-1)
                {

                    İlan yeni = ilanlar[mevcutilan+1];
                    aciklamatxt.Text = yeni.aciklama;
                    alantxt.Text = yeni.alan;
                    basliktxt.Text = yeni.baslik;
                    cepteltxt.Text = yeni.ceptel;
                    emlaktipitxt.Text = yeni.emlaktipi;
                    firmatxt.Text = yeni.firmaadi;
                    fiyattxt.Text = yeni.fiyat;
                    iltxt.Text = yeni.il;
                    ilannotxt.Text = yeni.ilanno;
                    tarihtxt.Text = yeni.ilantarihi;
                    ilçetxt.Text = yeni.ilçe;
                    mahalletxt.Text = yeni.mahalle;
                    odatxt.Text = yeni.odasayisi;
                    Images = yeni.Resimler;
                    sabitteltxt.Text = yeni.sabittel;
                    mevcutilan += 1;
                    slaybaslat();
                }
              

            }
           
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {

            if (ilanlar.Count > 1)
            {
                //0<2
                try
                {

              
                timerImageChange.Stop();
                }
                catch 
                {

                   
                }
                if (mevcutilan >0)
                {

                    İlan yeni = ilanlar[mevcutilan -1];
                    aciklamatxt.Text = yeni.aciklama;
                    alantxt.Text = yeni.alan;
                    basliktxt.Text = yeni.baslik;
                    cepteltxt.Text = yeni.ceptel;
                    emlaktipitxt.Text = yeni.emlaktipi;
                    firmatxt.Text = yeni.firmaadi;
                    fiyattxt.Text = yeni.fiyat;
                    iltxt.Text = yeni.il;
                    ilannotxt.Text = yeni.ilanno;
                    tarihtxt.Text = yeni.ilantarihi;
                    ilçetxt.Text = yeni.ilçe;
                    mahalletxt.Text = yeni.mahalle;
                    odatxt.Text = yeni.odasayisi;
                    Images = yeni.Resimler;
                    sabitteltxt.Text = yeni.sabittel;
                    mevcutilan -= 1;
                    slaybaslat();
                }


            }
        }

        private void delete_ilan_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                timerImageChange.Stop();
            }
            catch
            {


            }
            ilanlar.RemoveAt(mevcutilan);
            mevcutilan = ilanlar.Count - 1;
            
            İlan yeni = ilanlar[mevcutilan - 1];
            aciklamatxt.Text = yeni.aciklama;
            alantxt.Text = yeni.alan;
            basliktxt.Text = yeni.baslik;
            cepteltxt.Text = yeni.ceptel;
            emlaktipitxt.Text = yeni.emlaktipi;
            firmatxt.Text = yeni.firmaadi;
            fiyattxt.Text = yeni.fiyat;
            iltxt.Text = yeni.il;
            ilannotxt.Text = yeni.ilanno;
            tarihtxt.Text = yeni.ilantarihi;
            ilçetxt.Text = yeni.ilçe;
            mahalletxt.Text = yeni.mahalle;
            odatxt.Text = yeni.odasayisi;
            Images = yeni.Resimler;
            sabitteltxt.Text = yeni.sabittel;
            mevcutilan -= 1;
            this.NavigationService.Refresh();
            slaybaslat();
        }

        private void videobtn_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            int sure = 0;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Videolar"; // Default file name
            dlg.DefaultExt = ".avi"; // Default file extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.Filter = "Video dosyasi (.avi)|*.avi"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {

                filename = dlg.FileName;

                foreach (var item in ilanlar)
                {
                    sure += item.Resimler.Count();
                }
                sure = sure * 7;
                Process.Start("ScreenCapture.exe" , filename+" "+sure.ToString());
                slaybaslat();
            }
       
            // int dpi = 96;
            // int fps = 24;
            // int anim_length_in_secs = 15;
            // int num_total_frames = fps * anim_length_in_secs;

            // var secs = Enumerable.Range(0, num_total_frames).Select(t => (((double)t) / fps));
            // var aviManager = new AviFile.AviManager(filename, false);
            // AviFile.VideoStream aviStream = null;
            // foreach (var sec in secs)
            // {

            //     var proc = Process.GetProcessesByName("EmlakSlide.vshost")[0];
            //     var rect = new User32.Rect();
            //     User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            //     int width = rect.right - rect.left;
            //     int height = rect.bottom - rect.top;

            //     var bmp = new Bitmap(width, height);
            //     Graphics graphics = Graphics.FromImage(bmp);
            //     graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);

            //     bmp.Save("test.png", ImageFormat.Png);

            // string temp_bitmap = "test.png";
            ////     await  ConvertToBitmapSource(canvas1);

            //     System.Drawing.Bitmap bm = new System.Drawing.Bitmap(temp_bitmap);
            //     if (aviStream == null)
            //     {
            //         aviStream = aviManager.AddVideoStream(true, fps, bm);
            //     }
            //     else
            //     {
            //         aviStream.AddFrame(bm);

            //     }
            //     bm.Dispose();

            // }
            // aviManager.Close();
        }
        public async Task ConvertToBitmapSource(UIElement element)
        {
            var target = new RenderTargetBitmap(
                (int)element.RenderSize.Width, (int)element.RenderSize.Height,
                96, 96, PixelFormats.Pbgra32);
            target.Render(element);

            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(target);
            encoder.Frames.Add(outputFrame);

            using (var file = File.OpenWrite("TestImage.png"))
            {
                encoder.Save(file);
            }
        }

        private string imagetostring(string file)
        {
            
              
                Bitmap bitmap = new Bitmap(file);
                // Convert the image to byte[]
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = stream.ToArray();
            //Convert the image to base64 string
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
           
        }

        private static BitmapImage StringToImage(string imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(imageData);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageBytes))
            {
                mem.Position = 0;                
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void timerImageChange_Tick(object sender, EventArgs e)
        {
            PlaySlideShow();
        }
        public static int ilansayici = 0;
        private void  PlaySlideShow()
        {
            try
            {
                
                if (Images.Count == 0)
                    return;
                
                var oldCtrlIndex = CurrentCtrlIndex;
                CurrentCtrlIndex = (CurrentCtrlIndex + 1) % 2;
                CurrentSourceIndex = (CurrentSourceIndex + 1) % Images.Count;

                System.Windows.Controls.Image imgFadeOut = ImageControls[oldCtrlIndex];
                System.Windows.Controls.Image imgFadeIn = ImageControls[CurrentCtrlIndex];
                ImageSource newSource = StringToImage(Images[CurrentSourceIndex]);
                imgFadeIn.Source = newSource;
             
                TransitionType = TransitionEffects[EffectIndex].ToString();

                Storyboard StboardFadeOut = (Resources[string.Format("{0}Out", TransitionType.ToString())] as Storyboard).Clone();
                StboardFadeOut.Begin(imgFadeOut);
                Storyboard StboardFadeIn = Resources[string.Format("{0}In", TransitionType.ToString())] as Storyboard;
                StboardFadeIn.Begin(imgFadeIn);
                if (CurrentSourceIndex==0)
                {
                    ilansayici += 1;
                    timerImageChange.Stop();
                    StboardFadeOut.Begin(basliktxt);
                    if (0<= ilanlar.Count() - ilansayici)
                    {
                        basliktxt.Text=ilanlar[ilanlar.Count() - ilansayici].baslik;
                        Images = ilanlar[ilanlar.Count() - ilansayici].Resimler;
                        StboardFadeIn.Begin(basliktxt);
                        slaybaslat();

                    }



                  

                    //bir önceki slaytı aç

                }
              
          
            }
            catch 
            {
              
            }
        }
    }
}
