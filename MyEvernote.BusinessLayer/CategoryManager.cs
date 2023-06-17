using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public Category GetCategoryById(int id)
        {
            var cat = base.ListQueryable().Where(i => i.Id == id).Include("Notes").FirstOrDefault();

            return cat;
        }

        //public override int Delete(Category cat)
        //{
        //    NoteManager noteManager = new NoteManager();
        //    LikedManager likedManager = new LikedManager();
        //    CommentManager commentManager = new CommentManager();

        //    //Kategori ile ilişkili notların silinmesi gereklidir.

        //    foreach (Note note in cat.Notes.ToList())
        //    {
        //        //Note ile ilişkili like'ların silinmesi gereklidir.

        //        foreach (Liked like in note.Likes.ToList())
        //        {
        //            likedManager.Delete(like);
        //        }

        //        //Note ile ilişkili comment'ların silinmesi gereklidir.
        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            commentManager.Delete(comment);
        //        }

        //        noteManager.Delete(note);
        //    }

        //    return base.Delete(cat);
        //}

    }
}
