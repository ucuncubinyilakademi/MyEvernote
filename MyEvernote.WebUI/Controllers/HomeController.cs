using MyEvernote.BusinessLayer;
using MyEvernote.Entity;
using MyEvernote.Entity.Messages;
using MyEvernote.Entity.ValueObjects;
using MyEvernote.WebUI.Filters;
using MyEvernote.WebUI.Models;
using MyEvernote.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyEvernote.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteuserManager = new EvernoteUserManager();
        // GET: Home
        public ActionResult Index()
        {
            
            //return View(noteManager.List().OrderByDescending(x=> x.ModifiedOn));
            return View(noteManager.ListQueryable().Where(i=> i.IsDraft==false).OrderByDescending(x=> x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = categoryManager.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(i => i.ModifiedOn).Where(i => i.IsDraft == false).ToList());
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().Where(i => i.IsDraft == false).OrderByDescending(i => i.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }
        [Auth]
        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.result);
        }

        [HttpPost]
        [Auth]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            if(ProfileImage != null && 
                ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png")
            {
                string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                ProfileImage.SaveAs(Server.MapPath($"~/Content/images/{filename}"));
                model.ProfileImageFilename = filename;
            }

            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.UpdateProfile(model);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {

                    Title = "Profil Güncellenemdi",
                    Items = res.Errors,
                    RedirectingUrl="/Home/EditProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session["login"] = res.result; //profile güncellendiğinde oturum güncellenecek.
            return RedirectToAction("ShowProfile");
        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            EvernoteUser currentuser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.RemoveUserById(currentuser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    RedirectingUrl="/Home/ShowProfile",
                    Title = "Profil Silinemedi",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();
            return RedirectToAction("Index");
        }
               
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Giriş kontrolü ve yönlendirme...
                BusinessLayerResult<EvernoteUser> res = evernoteuserManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    if(res.Errors.Find(x=> x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-5678-1234";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                // Session'a kullanıcı bilgi saklama..
                CurrentSession.Set<EvernoteUser>("login", res.result);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            // Kullanıcı Email kontrolü...
            // Kullanıcı Username kontrolü...
            // Kayıt İşlemi...
            // Aktivasyon Mail gönderimi...

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = evernoteuserManager.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }


                OkViewModel notifymodel = new OkViewModel()
                {

                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"                   
                };
                notifymodel.Items.Add("Lütfen eposta adrsinize gönderilen aktivasyon linkine tıklayarak hesabınızı aktifleştiriniz. Hesabınızı aktif etmeden note yazamaz, yorum yapamazsınız.");

                return View("Ok", notifymodel);
            }


            return View(model);
        }

       
        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz işlem",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık note yazabilir ve beğeni yapabilirsiniz.");

            return View("Ok", okNotifyObj);
        }

      
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}