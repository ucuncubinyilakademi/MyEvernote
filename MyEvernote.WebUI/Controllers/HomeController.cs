using MyEvernote.BusinessLayer;
using MyEvernote.Entity;
using MyEvernote.Entity.ValueObjects;
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
        // GET: Home
        public ActionResult Index()
        {
            
            //return View(noteManager.GetAllNote().OrderByDescending(x=> x.ModifiedOn));
            return View(noteManager.GetAllNoteQueryable().OrderByDescending(x=> x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(i => i.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.GetAllNoteQueryable().OrderByDescending(i => i.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }
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