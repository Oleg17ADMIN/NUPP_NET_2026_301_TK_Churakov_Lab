using System;

namespace LibrarySystem.REST.Models
{
    public class PublisherDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}