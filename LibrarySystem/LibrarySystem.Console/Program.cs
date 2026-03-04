using System;
using System.Linq;
using LibrarySystem.Common;

namespace LibrarySystem.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Підписка на подію (вимога завдання) [cite: 36]
            LibraryItem.OnItemCreated += msg => Console.WriteLine($"[ПОДІЯ ГЕНЕРУЄТЬСЯ]: {msg}");

            // 2. Створюємо сервіс для книг [cite: 52]
            var bookService = new CrudService<Book>();

            // Виклик методу розширення [cite: 38]
            Console.WriteLine("БІБЛІОТЕЧНА СИСТЕМА ЗАПУЩЕНА".ToConsoleFormat());

            // 3. Демонстрація CREATE [cite: 46]
            var b1 = new Book("C# для початківців", 2024, "Олег Чураков", 300, "978-1-23");
            var b2 = new Book("Чистий код", 2008, "Роберт Мартін", 464, "978-0-13");

            bookService.Create(b1);
            bookService.Create(b2);

            // 4. Демонстрація READ ALL [cite: 48, 52]
            Console.WriteLine("\nСписок книг після додавання:");
            foreach (var b in bookService.ReadAll())
            {
                b.DisplayInfo(); // Виклик віртуального методу
            }

            // 5. Демонстрація UPDATE [cite: 49]
            b1.PageCount = 350;
            bookService.Update(b1);
            Console.WriteLine($"\nОновлено кількість сторінок для '{b1.Title}': {bookService.Read(b1.Id).PageCount}");

            // 6. Статичний метод (вимога завдання) [cite: 37]
            Console.WriteLine($"\nВсього об'єктів у пам'яті: {LibraryItem.GetTotalCount()}");

            // 7. ДОДАТКОВЕ ЗАВДАННЯ: Збереження та Завантаження 
            string file = "my_library.json";
            bookService.Save(file);
            Console.WriteLine($"\nДані успішно збережені у файл: {file}");

            // Створимо новий сервіс, щоб перевірити завантаження
            var newService = new CrudService<Book>();
            newService.Load(file);
            Console.WriteLine($"Завантажено з файлу книг: {newService.ReadAll().Count()}");

            Console.WriteLine("\nНатисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}