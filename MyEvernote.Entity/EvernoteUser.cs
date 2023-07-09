using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entity
{
    [Table("EvernoteUsers")] // TableName
    public class EvernoteUser:MyEntityBase
    {
        [StringLength(25)]
        [DisplayName("İsim")]
        public string Name { get; set; }
        [StringLength(25)]
        [DisplayName("Soyisim")]
        public string Surname { get; set; }
        [Required,StringLength(25)]
        public string Username { get; set; }
        [StringLength(30)] //user.png
        public string ProfileImageFilename { get; set; }
        [Required, StringLength(70)]
        public string Email { get; set; }
        [Required,StringLength(8)]//MinLength(6),MaxLength(8)
        public string Password { get; set; }
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [Required]
        public Guid ActivateGuid { get; set; }
        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public EvernoteUser()
        {
            Notes = new List<Note>();
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}
