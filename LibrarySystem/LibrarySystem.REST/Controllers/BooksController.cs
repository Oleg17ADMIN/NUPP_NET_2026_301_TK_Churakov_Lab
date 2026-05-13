using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Services;
using LibrarySystem.REST.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Тільки для авторизованих користувачів
    public class BooksController : ControllerBase
    {
        private readonly LibraryServiceAsync<BookModel> _bookService;

        public BooksController(LibraryServiceAsync<BookModel> bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [AllowAnonymous] // Дозволяємо перегляд без логіну
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookService.ReadAllAsync();
            var dtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PageCount = b.PageCount,
                YearPublished = b.YearPublished,
                ISBN = b.ISBN
            });
            return Ok(dtos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Тільки Адмін може додавати
        public async Task<ActionResult<BookDto>> PostBook(BookDto bookDto)
        {
            var book = new BookModel
            {
                Id = Guid.NewGuid(),
                Title = bookDto.Title,
                Author = bookDto.Author,
                PageCount = bookDto.PageCount,
                YearPublished = bookDto.YearPublished,
                ISBN = bookDto.ISBN
            };

            await _bookService.CreateAsync(book);
            bookDto.Id = book.Id;
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, bookDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Тільки Адмін може видаляти
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _bookService.ReadAsync(id);
            if (book == null) return NotFound();

            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}