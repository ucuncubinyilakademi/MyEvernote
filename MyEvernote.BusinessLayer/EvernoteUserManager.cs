using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entity;
using MyEvernote.Entity.Messages;
using MyEvernote.Entity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> repo = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            // Kullanıcı Email kontrolü...
            // Kullanıcı Username kontrolü...
            // Kayıt İşlemi...
            // Aktivasyon Mail gönderimi...

            EvernoteUser user = repo.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists,"Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists,"Email adresi kayıtlı");
                }
            }

            else
            {
                int dbResult = repo.Insert(new EvernoteUser()
                {
                    Username=data.Username,
                    Email=data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),                   
                    IsActive=false,
                    IsAdmin=false
                });

                if (dbResult > 0)
                {
                    res.result = repo.Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.result.ActivateGuid}";
                    string body = $"Merhaba {res.result.Username};<br/><br/>Hesabınız aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız.</a>";

                    MailHelper.SendMail(body, res.result.Email, "MyEvernote Hesap Aktifleştirme");

                }
            }

            return res;
        }
        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            // Giriş Kontrolü
            // Hesap aktive edilmiş mi?
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.result = repo.Find(x => x.Email == data.Email && x.Password == data.Password);

            if (res.result != null)
            {
                if (!res.result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir. Lütfen email adresinizi kontrol ediniz");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.EmailOrPassWrong,"Email yada şifre uyuşmuyor.");
            }

            return res;

        }
        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.result = repo.Find(x => x.ActivateGuid == id);

            if(res.result != null)
            {
                if (res.result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.result.IsActive = true;
                repo.Update(res.result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.result = repo.Find(x => x.Id == id);
            if(res.result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }
    }
}
