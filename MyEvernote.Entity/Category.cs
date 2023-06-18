using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entity
{
    public class Category:MyEntityBase
    {
        [DisplayName("Kategori Adı")]
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual List<Note> Notes { get; set; }

        public Category()
        {
            Notes = new List<Note>();
        }
    }
}
