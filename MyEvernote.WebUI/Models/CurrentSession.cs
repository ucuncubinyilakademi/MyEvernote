using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebUI.Models
{
    public class CurrentSession
    {
        public static EvernoteUser User 
        { 
            get 
            {
                return Get<EvernoteUser>("login");
            }
        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        } 
    }
}