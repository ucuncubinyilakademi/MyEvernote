using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> repo = new Repository<Note>();

        public List<Note> GetAllNote()
        {
            return repo.List();
        }
        public IQueryable<Note> GetAllNoteQueryable()
        {
            return repo.ListQueryable();
        }

    }
}
