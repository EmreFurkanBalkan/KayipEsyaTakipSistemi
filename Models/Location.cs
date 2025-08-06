using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using LostAndFoundApp.Models;

namespace LostAndFoundApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Hat kodu seçimi gereklidir")]
        [Display(Name = "Hat Kodu")]
        public int LineCodeId { get; set; }
        
        [ValidateNever]
        public LineCode LineCode { get; set; }

        [Display(Name = "Kayıp Eşya")]
    public int? LostItemId { get; set; }
        
        [ValidateNever]
        public LostItem LostItem { get; set; }

        [Required(ErrorMessage = "Kapı numarası gereklidir")]
        [Display(Name = "Kapı Numarası")]
        public string KapiID { get; set; }
    }
}
