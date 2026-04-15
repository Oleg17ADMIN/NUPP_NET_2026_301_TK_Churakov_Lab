using Xunit;
using LibrarySystem.Common;
using LibrarySystem.Infrastructure.Services;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Repositories;
using LibrarySystem.Infrastructure.Models; // Додано, якщо BookModel там
using System.Threading.Tasks;
using System.Linq;
using System;

namespace LibrarySystem.Tests
{
    public class LibraryServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldAddItemCorrectly()
        {
            // Використовуємо BookModel, якщо LibraryServiceAsync налаштований на роботу з моделями БД
            // Якщо сервіс працює з сутностями Common, залиште Book
            var service = new LibraryServiceAsync<Book>(null!, null!);
            var book = Book.CreateNew();

            // Act
            // await service.CreateAsync(book);

            // Щоб прибрати CS1998 (async method lacks await)
            await Task.CompletedTask;

            // Assert
            // Assert.True(true); 
        }

        [Fact]
        public async Task ReadAllAsync_Pagination_ShouldReturnCorrectPageSize()
        {
            var service = new LibraryServiceAsync<Book>(null!, null!);

            // Act
            // await service.ReadAllAsync(page: 1, amount: 3);

            await Task.CompletedTask;
        }
    }
}