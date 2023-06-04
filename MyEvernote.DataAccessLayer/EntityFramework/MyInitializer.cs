using MyEvernote.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user..
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Altan Emre",
                Surname = "Demirci",
                Email = "altanemredemirci@gmail.com",
                Username = "adminUser",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                ProfileImageFilename="user.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "altanemre"
            };
            // Adding standart user..
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Uras",
                Surname = "Demirci",
                Email = "altanurasdemirci@gmail.com",
                Username = "altanuras",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                ProfileImageFilename = "user.png",
                IsAdmin = false,
                Password = "1",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "altanuras"
            };
            context.Users.Add(admin);
            context.Users.Add(standartUser);

            context.SaveChanges();

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email =FakeData.NetworkData.GetEmail(),
                    Username = $"user{i}",
                    ActivateGuid = Guid.NewGuid(),
                    ProfileImageFilename = "user.png",
                    IsActive = true,
                    IsAdmin = false,
                    Password = "123",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now.AddMinutes(5),
                    ModifiedUsername = $"user{i}"
                };
                context.Users.Add(user);
            }

            context.SaveChanges();

            // Adding fake categories
            List<EvernoteUser> userList = context.Users.ToList();

            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description=FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now.AddMinutes(5),
                    ModifiedUsername = "altanemre",
                    Notes = new List<Note>()
                };

                context.Categories.Add(cat);

                // Adding fake notes..
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    EvernoteUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername =owner.Username
                    };

                    cat.Notes.Add(note);

                    // Adding fake comments
                    for (int c = 0; c < FakeData.NumberData.GetNumber(3,5); c++)
                    {
                        EvernoteUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username
                        };

                        note.Comments.Add(comment);
                    }

                    // Adding fake likes

                   

                    for (int l = 0; l < note.LikeCount; l++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[l]
                        };

                        note.Likes.Add(liked);
                    }
                }

            }

            context.SaveChanges();    
        }
    }
}
