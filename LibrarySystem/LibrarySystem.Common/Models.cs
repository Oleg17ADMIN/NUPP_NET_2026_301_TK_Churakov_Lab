using System;
using System.Diagnostics.CodeAnalysis; // Додано для [SetsRequiredMembers]

namespace LibrarySystem.Common
{
    // Делегати та Події
    public delegate void ItemActionHandler(string message);

    public abstract class LibraryItem : IEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public int YearPublished { get; set; }

        public static int TotalItemsCreated;

        static LibraryItem()
        {
            TotalItemsCreated = 0;
        }

        [SetsRequiredMembers] // Повідомляємо компілятору, що конструктор встановлює required поля
        protected LibraryItem(string title, int year)
        {
            Id = Guid.NewGuid();
            Title = title;
            YearPublished = year;
            TotalItemsCreated++;
        }

        public static event ItemActionHandler? OnItemCreated;

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Item: {Title}, Year: {YearPublished}");
            OnItemCreated?.Invoke($"Created: {Title}");
        }

        public static int GetTotalCount()
        {
            return TotalItemsCreated;
        }
    }

    public class Book : LibraryItem
    {
        public required string Author { get; set; }
        public int PageCount { get; set; }
        public required string ISBN { get; set; }

        private static readonly Random _random = new Random();

        [SetsRequiredMembers]
        public Book(string title, int year, string author, int pageCount, string isbn)
            : base(title, year)
        {
            Author = author;
            PageCount = pageCount;
            ISBN = isbn;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Book: '{Title}' by {Author} ({YearPublished}), Pages: {PageCount}");
            base.DisplayInfo();
        }

        public static Book CreateNew()
        {
            return new Book(
                $"Книга_Автогенерація_{_random.Next(1, 10000)}",
                _random.Next(1900, 2024),
                $"Автор_{_random.Next(1, 100)}",
                _random.Next(50, 1000),
                $"ISBN-{_random.Next(1000, 9999)}"
            );
        }
    }

    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }
        public required string Publisher { get; set; }
        public required string Category { get; set; }

        private static readonly Random _random = new Random();

        [SetsRequiredMembers]
        public Magazine(string title, int year, int issueNumber, string publisher, string category)
            : base(title, year)
        {
            IssueNumber = issueNumber;
            Publisher = publisher;
            Category = category;
        }

        public static Magazine CreateNew()
        {
            return new Magazine(
                $"Журнал_Автогенерація_{_random.Next(1, 10000)}",
                _random.Next(2000, 2024),
                _random.Next(1, 52),
                $"Видавництво_{_random.Next(1, 50)}",
                "Технології"
            );
        }
    }

    public class Member : IEntity
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Member()
        {
            Id = Guid.NewGuid();
            RegistrationDate = DateTime.Now;
        }
    }

    public static class StringExtensions
    {
        public static string ToConsoleFormat(this string str)
        {
            return $"---> {str} <---";
        }
    }
}