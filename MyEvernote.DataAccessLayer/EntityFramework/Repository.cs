using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        //  private DatabaseContext db = new DatabaseContext(); //Singleton Pattern

        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = db.Set<T>(); // db.Set<Category> ==> db.Categories
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable();
        }
        public List<T> List()
        {
            return _objectSet.ToList(); //db.Categories.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _objectSet.Where(filter).ToList();
        }

        public T Find(Expression<Func<T, bool>> filter)
        {
            return _objectSet.FirstOrDefault(filter); //db.Categories.FirstOrDefault(i=>i.Id==1);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetUsername();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetUsername();
            }
            return Save();
        }
        public virtual int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
