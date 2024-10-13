using Microsoft.EntityFrameworkCore;
using MyDomain.Models;

namespace PhoneBookWebApp.DbAccess

{
    public class PhoneBookContext : DbContext
    {

        private readonly ImageConverter _imageConverter = new ImageConverter();

        private readonly string picLocation = "./wwwroot/Content/pics/";


        public PhoneBookContext()
        {
        }

        public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Hobby> Hobbies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hobby>().HasData(
               new Hobby { Id = 1, Titel = "Wiskunde",
                   HobbyImage = _imageConverter.FilePNGToByteArray(
                        picLocation + "Wiskunde.png")
               },
               new Hobby { Id = 2, Titel = "Zingen",
                   HobbyImage = _imageConverter.FilePNGToByteArray(
                        picLocation + "Zingen.png")
               },
               new Hobby { Id = 3, Titel = "Paardrijden",
                   HobbyImage = _imageConverter.FilePNGToByteArray(
                        picLocation + "Paardrijden.png")
               },
               new Hobby { Id = 4, Titel = "Zwemmen",
                   HobbyImage = _imageConverter.FilePNGToByteArray(
                        picLocation + "Zwemmen.png")
               },
               new Hobby { Id = 5, Titel = "Dansen",
                   HobbyImage = _imageConverter.FilePNGToByteArray(
                        picLocation + "Dansen.png")
               }
           );

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, FirstName = "John", LastName = "Drummajor", Birthdate = new DateTime(1943, 3, 29) },
                new Person { Id = 2, FirstName = "Blaise", LastName = "Pascal", Birthdate = new DateTime(1623, 6, 19) },
                new Person { Id = 3, FirstName = "Peter", LastName = "Gabriel", Birthdate = new DateTime(1950, 2, 13) },
                new Person { Id = 4, FirstName = "Mister", LastName = "Eduarte", Birthdate = new DateTime(1949, 1, 1) },
                new Person { Id = 5, FirstName = "Mister", LastName = "Zorro", Birthdate = new DateTime(1957, 10, 1) },
                new Person { Id = 6, FirstName = "Paul", LastName = "McCartney", Birthdate = new DateTime(1942, 6, 18) },
                new Person { Id = 7, FirstName = "Ad", LastName = "Baantjer", Birthdate = new DateTime(1923, 9, 16) },
                new Person { Id = 8, FirstName = "Bea", LastName = "Knol", Birthdate = new DateTime(1989, 9, 12) },
                new Person { Id = 9, FirstName = "Celine", LastName = "Stakkeren", Birthdate = new DateTime(1989, 9, 12) }
            );

            // Seeding the relationships
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Hobbies)
                .WithMany(h => h.Participants)
                .UsingEntity(j => j.HasData(
                    new { HobbiesId = 1, ParticipantsId = 2 },
                    new { HobbiesId = 3, ParticipantsId = 3 },
                    new { HobbiesId = 1, ParticipantsId = 4 },
                    new { HobbiesId = 5, ParticipantsId = 5 },
                    new { HobbiesId = 3, ParticipantsId = 6 },
                    new { HobbiesId = 4, ParticipantsId = 8 },
                    new { HobbiesId = 5, ParticipantsId = 8 },
                    new { HobbiesId = 4, ParticipantsId = 9 },
                    new { HobbiesId = 5, ParticipantsId = 9 }
                ));


        }
    }
}

