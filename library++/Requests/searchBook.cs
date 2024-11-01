using System.ComponentModel.DataAnnotations;

namespace Library.Requests
{
    public class searchBook
    {
        public int Id_Books { get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public string Genre { get; set; }
        public int AvailableCopies { get; set; }
        public int YearOfPublication { get; set; }
        public string Description { get; set; }
    }
}
