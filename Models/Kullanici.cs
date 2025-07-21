using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Kullanici
    {
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Eposta { get; set; }
        public string Sifre { get; set; }
        public string GüvenlikSorusu { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public string Rol { get; set; }

    }
}
