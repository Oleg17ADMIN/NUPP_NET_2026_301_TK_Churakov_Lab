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

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> ReadAsync(Guid id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<T>> ReadAllAsync() => await _repository.GetAllAsync();

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var items = await _repository.GetAllAsync();
            return items.Skip((page - 1) * amount).Take(amount).ToList();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.Update(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.Delete(element);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> SaveAsync(string path) => Task.FromResult(true);
        public Task<bool> LoadAsync(string path) => Task.FromResult(true);

        public IEnumerator<T> GetEnumerator() =>
            _repository.GetAllAsync().GetAwaiter().GetResult().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}