using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entity
{
    internal class Category:MyEntityBase
    {              
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual List<Note> Notes { get; set; }
    }
}
