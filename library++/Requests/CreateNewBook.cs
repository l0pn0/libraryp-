using System.ComponentModel.DataAnnotations;

namespace Library.Requests
{
    public class CreateNewBook
    {
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage = "Автор книги обязателен")]
        public int Id_Autors { get; set; }
        public int Year_public { get; set; }
        public string Description { get; set; }
        [Required]
        public int AvailableCopies { get; set; }
        [Required]
        public int Genre_Id { get; set; }
    }
    public class BookAvailableCopies
    {
        public int Id_Books { get; set; }
        public string Title { get; set; }
        public int AvailableCopies { get; set; }
    }
}
