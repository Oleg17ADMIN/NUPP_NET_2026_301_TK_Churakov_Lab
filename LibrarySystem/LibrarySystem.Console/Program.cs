using LibrarySystem.Common;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Repositories;
using LibrarySystem.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Налаштування кодування для коректного виводу української мови
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Робота з базою даних SQLite (NUPP Lab) ---");

            try
            {
                // 1. Ініціалізація контексту
                using var context = new LibraryContext();

                // Гарантуємо, що база даних створена
                Console.WriteLine("Перевірка підключення до бази...");
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("База готова.");

                // 2. Ініціалізація сервісів
                var bookRepository = new Repository<BookModel>(context);
                var bookService = new LibraryServiceAsync<BookModel>(bookRepository, context);

                // 3. Перевірка наявності даних
                var allExisting = await bookService.ReadAllAsync();

                if (!allExisting.Any())
                {
                    Console.WriteLine("База порожня. Створюємо тестові дані...");

                    // Створюємо видавця (батьківський об'єкт)
                    var publisher = new PublisherModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Видавництво Полтавська Політехніка"
                    };

                    await context.Set<PublisherModel>().AddAsync(publisher);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Створено видавця: {publisher.Name}");

                    // Створюємо список книг
                    var tasks = new List<Task>();
                    for (int i = 1; i <= 5; i++)
                    {
                        var book = new BookModel
                        {
                            Id = Guid.NewGuid(),
                            Title = $"Лабораторна робота №{i} (C# .NET)",
                            Author = "Олег Чураков",
                            PageCount = 100 + (i * 25),
                            YearPublished = 2026,
                            ISBN = $"ISBN-NUPP-{1000 + i}",
                            PublisherId = publisher.Id // Прив'язка до видавця
                        };
                        tasks.Add(bookService.CreateAsync(book));
                    }

                    await Task.WhenAll(tasks);
                    Console.WriteLine("Тестові книги успішно додано до бази.");

                    // Оновлюємо список для виводу
                    allExisting = await bookService.ReadAllAsync();
                }

                // 4. Вивід результатів у консоль
                Console.WriteLine($"\nУ базі знайдено книг: {allExisting.Count()}");
                Console.WriteLine("--------------------------------------------------");
                foreach (var b in allExisting)
                {
                    Console.WriteLine($"ID: {b.Id.ToString().Substring(0, 8)}... | {b.Title} | Автор: {b.Author}");
                }
                Console.WriteLine("--------------------------------------------------");

                // 5. Простий LINQ аналіз
                if (allExisting.Any())
                {
                    var avgPages = allExisting.Average(x => x.PageCount);
                    Console.WriteLine($"\nСтатистика: Середня кількість сторінок — {avgPages:F1}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nКритична помилка: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Деталі: {ex.InnerException.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}