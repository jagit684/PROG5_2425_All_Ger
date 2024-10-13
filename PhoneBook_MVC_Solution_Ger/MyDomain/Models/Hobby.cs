using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyDomain.Models
{

    [Index(nameof(Hobby.Titel), IsUnique = true)]
    public class Hobby
    {
 
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage ="Geef de hobby een Titel.")]

        public string Titel { get; set; }

        public byte[]? HobbyImage { get; set; }

        public virtual ICollection<Person> Participants { get; set; } = null!;

    }
}
