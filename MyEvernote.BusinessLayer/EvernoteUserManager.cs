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
using System.Xml;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager:ManagerBase<EvernoteUser>
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
                    ProfileImageFilename="user.png",
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
        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser model)
        {
            EvernoteUser db_user = repo.Find(x => x.Id == model.Id && (x.Username == model.Username || x.Email == model.Email));

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            if(db_user!=null && db_user.Id != model.Id)
            {
                if (db_user.Username == model.Username)
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");

                if (db_user.Email == model.Email)
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı");

                return res;
            }

            res.result = repo.Find(x => x.Id == model.Id);
            res.result.Email = model.Email;
            res.result.Name = model.Name;
            res.result.Surname = model.Surname;
            res.result.Username = model.Username;
            res.result.Password = model.Password;

            if (string.IsNullOrEmpty(model.ProfileImageFilename) == false)
            {
                res.result.ProfileImageFilename = model.ProfileImageFilename;
            }

            if (repo.Update(res.result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi");
            }

            return res;
        }
        public BusinessLayerResult<EvernoteUser> RemoveUserById(int Id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            EvernoteUser user = repo.Find(x => x.Id == Id);

            if (user != null)
            {
                if (repo.Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }

            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadi");
            }

            return res;
        }

        public new BusinessLayerResult<EvernoteUser> Insert(EvernoteUser data)
        {
            EvernoteUser user = repo.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            res.result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı");
                }
            }

            else
            {

                res.result.ProfileImageFilename = "user.png";
                res.result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<EvernoteUser> Update(EvernoteUser model)
        {
            EvernoteUser db_user = repo.Find(x => x.Id == model.Id && (x.Username == model.Username || x.Email == model.Email));

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            res.result = model;

            if (db_user != null && db_user.Id != model.Id)
            {
                if (db_user.Username == model.Username)
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");

                if (db_user.Email == model.Email)
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı");

                return res;
            }

            res.result = repo.Find(x => x.Id == model.Id);
            res.result.Email = model.Email;
            res.result.Name = model.Name;
            res.result.Surname = model.Surname;
            res.result.Username = model.Username;
            res.result.Password = model.Password;
            res.result.IsActive = model.IsActive;
            res.result.IsAdmin = model.IsAdmin;

            if (repo.Update(res.result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi");
            }

            return res;
        }
    }
}
