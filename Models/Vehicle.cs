using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LostAndFoundApp.Models;


namespace LostAndFoundApp.Models
{
    public class Vehicle
    { 
        public int Id { get; set; }

        [Required(ErrorMessage = "Plaka alanı zorunludur.")]
        public string PlateNumber { get; set; }

        public string LineCode { get; set; }

        public ICollection<LostItem>? LostItems { get; set; }
    }
}
