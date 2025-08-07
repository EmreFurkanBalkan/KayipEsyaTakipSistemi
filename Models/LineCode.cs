using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LostAndFoundApp.Models;

namespace LostAndFoundApp.Models
{
    public class LineCode
    { 
        public int Id { get; set; }

        public string Line { get; set; }

        public ICollection<LostItem>? LostItems { get; set; }
    }
}
