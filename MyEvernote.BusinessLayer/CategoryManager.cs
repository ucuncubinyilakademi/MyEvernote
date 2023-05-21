using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager
    {
        Repository<Category> repo = new Repository<Category>();
        
        public List<Category> Liste()
        {
            return repo.List();
        }
        
    }
}
