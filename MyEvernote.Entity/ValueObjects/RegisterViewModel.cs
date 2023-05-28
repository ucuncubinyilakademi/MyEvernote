using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEvernote.Entity.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [StringLength(20,ErrorMessage ="{0} alanı max{1} karakter olmalı")]
        public string Username { get; set; }

        [DisplayName("Email Adresi"), Required(ErrorMessage = "{0} alanı boş geçilemez."), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Şifre"), Required, DataType(DataType.Password),Compare("Password")]
        public string RePassword { get; set; }
    }
}