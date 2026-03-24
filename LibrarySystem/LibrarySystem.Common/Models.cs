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

        // Додаємо об'єкт Random для генерації даних
        private static readonly Random _random = new Random();

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

        // Статичний метод для створення об'єкта зі згенерованими даними
        public static Book CreateNew()
        {
            return new Book(
                $"Книга_Автогенерація_{_random.Next(1, 10000)}",
                _random.Next(1900, 2024),
                $"Автор_{_random.Next(1, 100)}",
                _random.Next(50, 1000), // Це цифрове значення ми потім будемо аналізувати через LINQ
                $"ISBN-{_random.Next(1000, 9999)}"
            );
        }
    }

    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }

        private static readonly Random _random = new Random();

        public Magazine(string title, int year, int issueNumber, string publisher, string category)
            : base(title, year)
        {
            IssueNumber = issueNumber;
            Publisher = publisher;
            Category = category;
        }

        // Статичний метод для створення об'єкта зі згенерованими даними
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