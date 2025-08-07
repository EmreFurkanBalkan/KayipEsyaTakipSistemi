using System.ComponentModel.DataAnnotations;
using LostAndFoundApp.Attributes;

namespace LostAndFoundApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        [PasswordValidation(ErrorMessage = "Şifre en az 6 karakter olmalı ve en az bir büyük harf içermelidir.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Rol { get; set; }
    }
}
