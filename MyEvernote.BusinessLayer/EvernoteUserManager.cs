using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entity;
using MyEvernote.Entity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> repo = new Repository<EvernoteUser>();
        public EvernoteUser RegisterUser(RegisterViewModel data)
        {
            // Kullanıcı Email kontrolü...
            // Kullanıcı Username kontrolü...
            // Kayıt İşlemi...
            // Aktivasyon Mail gönderimi...

            EvernoteUser user = repo.Find(x => x.Username == data.Username || x.Email == data.Email);

            if (user != null)
            {
                throw new Exception("Kayıtlı kullanıcı adı yada email adresi");
            }
            return user;
        }
    }
}
