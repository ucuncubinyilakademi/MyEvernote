using MyEvernote.Common;
using MyEvernote.Entity;
using MyEvernote.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebUI.Init
{
    public class WebCommon : ICommon
    {
        public string GetUsername()
        {
            if (CurrentSession.User != null)
            {
                EvernoteUser user = CurrentSession.Get<EvernoteUser>("login");

                return user.Username;
            }
            return "system";
        }
    }
}