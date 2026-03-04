using System;

namespace LibrarySystem.Common
{
    // Інтерфейс для забезпечення наявності Id у всіх сутностей
    public interface IEntity
    {
        Guid Id { get; set; }
    }

    // Делегати та Події [Конструкція з вимог]
    public delegate void ItemActionHandler(string message);

    public abstract class LibraryItem : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int YearPublished { get; set; }

        // Статичні поля [Конструкція з вимог]
        public static int TotalItemsCreated;

        // Статичні конструктори [Конструкція з вимог]
        static LibraryItem()
        {
            TotalItemsCreated = 0;
        }

        // Конструктори [Конструкція з вимог]
        protected LibraryItem(string title, int year)
        {
            Id = Guid.NewGuid();
            Title = title;
            YearPublished = year;
            TotalItemsCreated++;
        }

        // Події [Конструкція з вимог]
        public static event ItemActionHandler OnItemCreated;

        // Методи [Конструкція з вимог]
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Item: {Title}, Year: {YearPublished}");
            OnItemCreated?.Invoke($"Created: {Title}");
        }

        // Статичний метод [Конструкція з вимог]
        public static int GetTotalCount()
        {
            return TotalItemsCreated;
        }
    }

    public class Book : LibraryItem
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string ISBN { get; set; }

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
    }

    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }

        public Magazine(string title, int year, int issueNumber, string publisher, string category)
            : base(title, year)
        {
            IssueNumber = issueNumber;
            Publisher = publisher;
            Category = category;
        }
    }

    public class Member : IEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Member()
        {
            Id = Guid.NewGuid();
            RegistrationDate = DateTime.Now;
        }
    }

    // Метод розширення [Конструкція з вимог]
    public static class StringExtensions
    {
        public static string ToConsoleFormat(this string str)
        {
            return $"---> {str} <---";
        }
    }
}