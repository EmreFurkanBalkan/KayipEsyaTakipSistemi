using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LostAndFoundApp.Models;

namespace LostAndFoundApp.Models
{
    public class Logs
    { 
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string LogType { get; set; }

        [Required]
        [MaxLength(100)]
        public string Executor { get; set; }

        [Required]
        [MaxLength(500)]
        public string LogContent { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

        