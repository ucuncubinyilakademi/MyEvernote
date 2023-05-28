using MyEvernote.BusinessLayer;
using MyEvernote.Entity;
using MyEvernote.Entity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyEvernote.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //ByCategory,NoteManager,CategoryManager-GetbyId

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            // Giriş kontrolü ve yönlendirme...
            // Session'a kullanıcı bilgi saklama...
            if (ModelState.IsValid)
            {

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
                EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
                EvernoteUser user = null;
                try
                {
                    user = evernoteUserManager.RegisterUser(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message); 
                }
            }


            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }


        public ActionResult UserActivate(Guid activate_id)
        {
            // Kullanıcı aktivasyonu sağlanacak...
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}