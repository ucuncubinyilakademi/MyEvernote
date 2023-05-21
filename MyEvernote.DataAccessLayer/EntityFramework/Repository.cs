using MyEvernote.DataAccessLayer.Abstract;
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
    public class Repository<T> : RepositoryBase,IRepository<T> where T:class
    {
        //  private DatabaseContext db = new DatabaseContext(); //Singleton Pattern
               
        private DbSet<T> _objectSet;

        public Repository()
        {          
            _objectSet = db.Set<T>();
        }
        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public List<T> List(Expression<Func<T,bool>> filter)
        {
            return _objectSet.Where(filter).ToList();
        }

        public T Find(Expression<Func<T, bool>> filter)
        {
            return _objectSet.FirstOrDefault(filter);
        }

        public int Insert(T obj)
        {
           _objectSet.Add(obj);
            return Save();
        }

        public int Update(T obj)
        {            
            return Save();
        }
        public int Delete(T obj)
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
