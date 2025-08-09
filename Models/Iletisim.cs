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

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefon numarası yalnızca rakam olmalı ve 10-11 haneli olmalıdır.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Mesaj alanı boş bırakılamaz.")]
        public string Mesaj { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        // >>> Burayı nullable yapıyoruz; zorunluluk kontrolünü Controller’da şartlı yapacağız
        public DateTime? RandevuTarihi { get; set; }
        public TimeSpan? RandevuSaati { get; set; }

        [NotMapped]
        [Display(Name = "Güvenlik Kodu")]
        public string GirilenGuvenlikKodu { get; set; }

        // >>> Formdaki toggle için (DB’ye kaydetmeye gerek yok)
        [NotMapped]
        public bool RandevuIstiyorum { get; set; }

        [Required]
        [MaxLength(1)]
        [Column(TypeName = "char(1)")]
        [Display(Name = "Okundu")]
        public string Okundu { get; set; } = "H";
    }
}
