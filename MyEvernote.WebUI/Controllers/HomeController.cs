using MyEvernote.BusinessLayer;
using MyEvernote.Entity;
using MyEvernote.Entity.Messages;
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
        private EvernoteUserManager evernoteuserManager = new EvernoteUserManager();
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

            Category cat = categoryManager.GetCategoryById(id.Value);

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

        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                // TODO : Kullanıcıyı bir hata ekranına yönlendirmek gerekiyor
            }

            return View(res.result);
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model)
        {
            return View();
        }

        public ActionResult DeleteProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteProfile(EvernoteUser model)
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
                Session["login"] = res.result;
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

                return RedirectToAction("RegisterOk");
            }


            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }


        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = evernoteuserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }

        public ActionResult UserActivateOk()
        {            
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}