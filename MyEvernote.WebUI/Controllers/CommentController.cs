using MyEvernote.BusinessLayer;
using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(i => i.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments",note.Comments);
        }
    }
}