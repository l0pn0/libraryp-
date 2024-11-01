using library_.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model
{
    public class Book
    {
        [Key]
        public int Id_Book { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int YearOfPublication { get; set; }
        public int AvailableCopies { get; set; }
        [Required]
        [ForeignKey("Autors")]
        public int Id_Autors { get; set; }
        public Autors Autors { get; set; }

        [ForeignKey("Genres")]
        public int Genre_Id { get; set; }
        public Genres Genres { get; set; }

    }
}
