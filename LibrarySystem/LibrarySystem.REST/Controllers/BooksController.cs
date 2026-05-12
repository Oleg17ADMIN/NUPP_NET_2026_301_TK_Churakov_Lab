using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Services;
using LibrarySystem.REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Шлях до API: api/books
    public class BooksController : ControllerBase
    {
        private readonly LibraryServiceAsync<BookModel> _bookService;

        public BooksController(LibraryServiceAsync<BookModel> bookService)
        {
            _bookService = bookService;
        }

        // 1. Отримати всі книги (READ ALL)
        [HttpGet]
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

            return Ok(dtos); // Повертає 200 OK
        }

        // 2. Отримати книгу за ID (READ ONE)
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(Guid id)
        {
            var book = await _bookService.ReadAsync(id);

            if (book == null)
            {
                return NotFound(); // Повертає 404 Not Found
            }

            var dto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PageCount = book.PageCount,
                YearPublished = book.YearPublished,
                ISBN = book.ISBN
            };

            return Ok(dto);
        }

        // 3. Створити нову книгу (CREATE)
        [HttpPost]
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
            // Повертає 201 Created і посилання на метод GetBook
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
        }

        // 4. Видалити книгу (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _bookService.ReadAsync(id);
            if (book == null) return NotFound();

            await _bookService.DeleteAsync(id);
            return NoContent(); // Повертає 204 No Content
        }
    }
}