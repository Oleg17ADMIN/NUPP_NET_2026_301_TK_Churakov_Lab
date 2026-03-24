using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Common
{
    public interface ICrudServiceAsync<T> : IEnumerable<T> where T : IEntity
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(Guid id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<IEnumerable<T>> ReadAllAsync(int page, int amount); // Пагінація
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync(string filePath);
        Task<bool> LoadAsync(string filePath);
    }
}