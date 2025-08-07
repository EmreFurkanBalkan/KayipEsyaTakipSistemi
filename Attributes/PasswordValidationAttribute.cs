using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LostAndFoundApp.Attributes
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            string password = value.ToString();

            // En az 6 karakter kontrolü
            if (password.Length < 6)
                return false;

            // Büyük harf kontrolü
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Şifre en az 6 karakter olmalı ve en az bir büyük harf içermelidir.";
        }
    }
}