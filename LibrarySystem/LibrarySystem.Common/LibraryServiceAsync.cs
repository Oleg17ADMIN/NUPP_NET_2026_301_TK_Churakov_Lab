using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LibrarySystem.Common
{
    public class LibraryServiceAsync<T> : ICrudServiceAsync<T> where T : class, IEntity
    {
        // Використовуємо звичайний List, але захищаємо його за допомогою SemaphoreSlim
        // SemaphoreSlim(1, 1) гарантує, що лише один потік може змінювати список одночасно
        private readonly List<T> _items = new List<T>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<bool> CreateAsync(T element)
        {
            await _semaphore.WaitAsync(); // Чекаємо дозволу на вхід
            try
            {
                _items.Add(element);
                return true;
            }
            finally
            {
                _semaphore.Release(); // Звільняємо місце для іншого потоку
            }
        }

        public async Task<T> ReadAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                return _items.FirstOrDefault(x => x.Id == id);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return _items.ToList(); // Повертаємо копію списку
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // РЕАЛІЗАЦІЯ ПАГІНАЦІЇ
        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                return _items
                    .Skip((page - 1) * amount) // Пропускаємо попередні сторінки
                    .Take(amount)               // Беремо потрібну кількість
                    .ToList();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _semaphore.WaitAsync();
            try
            {
                var index = _items.FindIndex(x => x.Id == element.Id);
                if (index != -1)
                {
                    _items[index] = element;
                    return true;
                }
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _semaphore.WaitAsync();
            try
            {
                return _items.RemoveAll(x => x.Id == element.Id) > 0;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Асинхронне збереження у JSON
        public async Task<bool> SaveAsync(string filePath)
        {
            await _semaphore.WaitAsync();
            try
            {
                using (FileStream createStream = File.Create(filePath))
                {
                    await JsonSerializer.SerializeAsync(createStream, _items);
                }
                return true;
            }
            catch { return false; }
            finally
            {
                _semaphore.Release();
            }
        }

        // Асинхронне завантаження з JSON
        public async Task<bool> LoadAsync(string filePath)
        {
            if (!File.Exists(filePath)) return false;
            await _semaphore.WaitAsync();
            try
            {
                using (FileStream openStream = File.OpenRead(filePath))
                {
                    var loadedItems = await JsonSerializer.DeserializeAsync<List<T>>(openStream);
                    if (loadedItems != null)
                    {
                        _items.Clear();
                        _items.AddRange(loadedItems);
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
            finally
            {
                _semaphore.Release();
            }
        }

        // Реалізація IEnumerable для можливості перебору сервісу циклом foreach
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}