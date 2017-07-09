using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EmlakSlide
{
    [Serializable]
    public class İlan
    {
     
        public List<string> Resimler { get; set; }
        public string il;
        public string ilçe;
        public string mahalle;
        public string ilanno;
        public string emlaktipi;
        public string alan;
        public string odasayisi;
        public string ilantarihi;
        public string aciklama;
        public string fiyat;
        public string firmaadi;
        public string sabittel;
        public string ceptel;
        public string baslik;


    }
}
