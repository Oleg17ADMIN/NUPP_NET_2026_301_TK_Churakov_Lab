using LibrarySystem.Common;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Services
{
    public class LibraryServiceAsync<T> : ICrudServiceAsync<T> where T : class, IEntity
    {
        private readonly IRepository<T> _repository;
        private readonly LibraryContext _context;

        public LibraryServiceAsync(IRepository<T> repository, LibraryContext context)
        {
            _repository = repository;
            _context = context;
        }

        // Створення
        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            await _context.SaveChangesAsync();
            return true;
        }

        // Читання одного об'єкта за ID
        public async Task<T?> ReadAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // Читання всіх об'єктів
        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Читання з пагінацією (для списків)
        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var items = await _repository.GetAllAsync();
            return items.Skip((page - 1) * amount).Take(amount).ToList();
        }

        // Оновлення
        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.Update(element);
            await _context.SaveChangesAsync();
            return true;
        }

        // Видалення за об'єктом
        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.Delete(element);
            await _context.SaveChangesAsync();
            return true;
        }

        // НОВИЙ МЕТОД: Видалення за ID (потрібен для REST API)
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            await _repository.Delete(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Заглушки для збереження/завантаження (якщо вони потрібні за інтерфейсом)
        public Task<bool> SaveAsync(string path) => Task.FromResult(true);
        public Task<bool> LoadAsync(string path) => Task.FromResult(true);

        // Реалізація IEnumerable для зручного перебору
        public IEnumerator<T> GetEnumerator() =>
            _repository.GetAllAsync().GetAwaiter().GetResult().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}