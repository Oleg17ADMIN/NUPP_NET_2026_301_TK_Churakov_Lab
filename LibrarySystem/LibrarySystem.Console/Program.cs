using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Common;

namespace LibrarySystem.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Налаштування кодування для української мови (як ми робили в Лабі 1)
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Створюємо екземпляр нашого асинхронного сервісу для книг
            var bookService = new LibraryServiceAsync<Book>();

            Console.WriteLine("--- Старт багатопотокового створення даних ---");

            // 2. БАГАТОПОТОКОВІСТЬ: Створюємо 1000 книг одночасно
            // Parallel.For розпаралелює задачу на всі ядра вашого процесора
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(bookService.CreateAsync(Book.CreateNew())); // Використовуємо твій метод CreateNew
            }

            // Чекаємо завершення всіх потоків
            await Task.WhenAll(tasks);

            Console.WriteLine($"Успішно створено об'єктів: {(await bookService.ReadAllAsync()).Count()}");

            // 3. LINQ АНАЛІЗ (Вимога завдання: мін/макс/середнє)
            var allBooks = await bookService.ReadAllAsync();

            // Знаходимо книгу з найбільшою кількістю сторінок
            var maxPages = allBooks.Max(b => b.PageCount);
            // Знаходимо середню кількість сторінок
            var avgPages = allBooks.Average(b => b.PageCount);
            // Знаходимо найдавнішу книгу
            var oldestYear = allBooks.Min(b => b.YearPublished);

            Console.WriteLine("\n--- Аналіз даних через LINQ ---");
            Console.WriteLine($"Максимальна кількість сторінок: {maxPages}");
            Console.WriteLine($"Середня кількість сторінок: {avgPages:F2}");
            Console.WriteLine($"Найдавніший рік видання: {oldestYear}");

            // 4. ПАГІНАЦІЯ (Вивід по 5 елементів)
            Console.WriteLine("\n--- Тест пагінації (Сторінка 1, перші 5 книг) ---");
            var page1 = await bookService.ReadAllAsync(page: 1, amount: 5);
            foreach (var book in page1)
            {
                Console.WriteLine($"ID: {book.Id.ToString().Substring(0, 8)}... | Назва: {book.Title} | Сторінок: {book.PageCount}");
            }

            // 5. ЗБЕРЕЖЕННЯ ТА ЗАВАНТАЖЕННЯ (Асинхронно)
            string path = "library_data.json";
            Console.WriteLine($"\nЗбереження даних у файл {path}...");
            await bookService.SaveAsync(path);

            Console.WriteLine("Завантаження даних назад...");
            await bookService.LoadAsync(path);

            Console.WriteLine("\nПрограма завершила роботу. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}