using Xunit; // Бібліотека для тестів
using LibrarySystem.Common; // Твої моделі та сервіс
using System.Threading.Tasks;
using System.Linq;
using System;

namespace LibrarySystem.Tests
{
    public class LibraryServiceTests
    {
        // Тест 1: Перевірка додавання книги
        [Fact] // Ця позначка каже студії, що це тест
        public async Task CreateAsync_ShouldAddItemCorrectly()
        {
            // 1. Arrange (Підготовка): створюємо сервіс та об'єкт
            var service = new LibraryServiceAsync<Book>();
            var book = Book.CreateNew();

            // 2. Act (Дія): викликаємо метод, який тестуємо
            await service.CreateAsync(book);

            // 3. Assert (Перевірка): перевіряємо, чи з'явилася книга в сервісі
            var items = await service.ReadAllAsync();
            Assert.Single(items); // Перевірка, що в списку рівно 1 елемент
            Assert.Equal(book.Title, items.First().Title); // Перевірка, що назва збігається
        }

        // Тест 2: Перевірка пагінації (сторінок)
        [Fact]
        public async Task ReadAllAsync_Pagination_ShouldReturnCorrectPageSize()
        {
            // Arrange: додаємо 10 випадкових книг
            var service = new LibraryServiceAsync<Book>();
            for (int i = 0; i < 10; i++)
            {
                await service.CreateAsync(Book.CreateNew());
            }

            // Act: просимо 1-шу сторінку, де має бути лише 3 книги
            int pageSize = 3;
            var page = await service.ReadAllAsync(page: 1, amount: pageSize);

            // Assert: перевіряємо, що повернулося саме 3 книги, а не всі 10
            Assert.Equal(pageSize, page.Count());
        }
    }
}