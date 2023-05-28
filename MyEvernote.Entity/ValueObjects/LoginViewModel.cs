using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entity.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Email Adresi"),Required(ErrorMessage ="{0} alanı boş geçilemez."), DataType(DataType.EmailAddress,ErrorMessage ="Email adresi hatalı")]
        public string Email { get; set; }
        [DisplayName("Şifre"),Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}