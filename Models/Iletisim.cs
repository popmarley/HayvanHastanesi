using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Iletisim
    {
        public int IletisimID { get; set; }

        [Required]
        public string Ad { get; set; }
        [Required]
        public string Soyad { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Telefon { get; set; }
        [Required]
        public string Mesaj { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        [NotMapped]
        [Display(Name = "Güvenlik Kodu")]
        public string GirilenGuvenlikKodu { get; set; } // DB’ye kaydedilmez

    }
}
