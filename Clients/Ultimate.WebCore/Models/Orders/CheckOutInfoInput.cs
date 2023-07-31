using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.WebCore.Models.Orders
{
    public class CheckOutInfoInput
    {
        [Display(Name ="İl")]
        public string Province { get; set; }
        [Display(Name = "İlçe")]
        public string District { get; set; }
        [Display(Name = "Cadde")]
        public string Street { get; set; }
        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }
        [Display(Name = "Adres")]
        public string Line { get; set; }
        [Display(Name = "Kart İsmi")]
        public string CardName { get; set; }
        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }
        [Display(Name = "SKT")]
        public string Expiration { get; set; }
        [Display(Name = "CCV Numarası")]
        public string CVV { get; set; }
        [Display(Name = "Toplam Tutar")]
        public decimal TotalPrice { get; set; }
    }
}
