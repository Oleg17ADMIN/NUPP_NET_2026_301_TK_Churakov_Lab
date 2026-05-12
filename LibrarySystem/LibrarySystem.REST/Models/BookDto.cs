using System;

namespace LibrarySystem.REST.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int PageCount { get; set; }
        public int YearPublished { get; set; }
        public string ISBN { get; set; } = null!;
    }
}