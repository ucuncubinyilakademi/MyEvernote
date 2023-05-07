﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entity
{
    public class Liked
    {
        public int Id { get; set; }
        public virtual Note Note { get; set; }
        public virtual EvernoteUser LikedUser { get; set; }
    }
}
