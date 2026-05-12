using System;
using System.Collections.Generic;
using LibrarySystem.Common;

namespace LibrarySystem.Infrastructure.Models
{
    public abstract class LibraryItemModel : IEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public int YearPublished { get; set; }

        // Зроблено Guid? (nullable), щоб не було помилки FOREIGN KEY, якщо видавець не заданий
        public Guid? PublisherId { get; set; }
        public PublisherModel? Publisher { get; set; }
    }

    public class BookModel : LibraryItemModel
    {
        public required string Author { get; set; }
        public int PageCount { get; set; }
        public required string ISBN { get; set; }
        public List<GenreModel> Genres { get; set; } = new List<GenreModel>();
    }

    public class MagazineModel : LibraryItemModel
    {
        public int IssueNumber { get; set; }
        public required string Category { get; set; }
    }

    public class PublisherModel : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<LibraryItemModel> PublishedItems { get; set; } = new List<LibraryItemModel>();
    }

    public class MemberModel : IEntity
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public LibraryCardModel? LibraryCard { get; set; }
    }

    public class LibraryCardModel : IEntity
    {
        public Guid Id { get; set; }
        public required string CardNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public Guid MemberId { get; set; }
        public MemberModel Member { get; set; } = null!;
    }

    public class GenreModel : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<BookModel> Books { get; set; } = new List<BookModel>();
    }
}