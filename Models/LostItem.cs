using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using LostAndFoundApp.Models;

namespace LostAndFoundApp.Models
{
    public class LostItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Eşya adı gereklidir")]
        [Display(Name = "Eşya Adı")]
        public string ItemName { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Bulunma tarihi gereklidir")]
        [Display(Name = "Bulunma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime FoundDate { get; set; }

        [Required(ErrorMessage = "Durum seçimi gereklidir")]
        [Display(Name = "Durum")]
        public LostItemStatus Status { get; set; }

        [Display(Name = "Hat Kodu")]
        public int? LineCodeId { get; set; }

        [ValidateNever]
        public LineCode LineCode { get; set; }

        [Required(ErrorMessage = "Araç kapı numarası gereklidir")]
        [Display(Name = "Araç Kapı Numarası")]
        public string Location { get; set; }

        [Display(Name = "Bulan Kişi")]
        public string FoundBy { get; set; }

        [Display(Name = "Kullanıcı ID")]
        public int UserId { get; set; }

        [ValidateNever]
        public User User { get; set; }
    }
}
