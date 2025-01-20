using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab5Borowy.Models
{
    public class BookAuthorViewModel
    {
        public List<Book>? Books { get; set; }
        public SelectList? Authors { get; set; }
        public string? BookAuthor {  get; set; }
        public string? SearchString { get; set; }
    }
}
