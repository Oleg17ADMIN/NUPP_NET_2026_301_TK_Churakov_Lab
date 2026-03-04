using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json; // Потрібно для збереження у файл

namespace LibrarySystem.Common
{
    // Описуємо інтерфейс згідно з вимогами лабораторної 
    public interface ICrudService<T> where T : IEntity
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);

        // Додаткове завдання 
        void Save(string filePath);
        void Load(string filePath);
    }

    // Реалізація сервісу
    public class CrudService<T> : ICrudService<T> where T : IEntity
    {
        // Внутрішня колекція для зберігання даних [cite: 44]
        private List<T> _collection = new List<T>();

        public void Create(T element)
        {
            _collection.Add(element);
        }

        public T Read(Guid id)
        {
            return _collection.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> ReadAll()
        {
            return _collection;
        }

        public void Update(T element)
        {
            var existing = Read(element.Id);
            if (existing != null)
            {
                var index = _collection.IndexOf(existing);
                _collection[index] = element;
            }
        }

        public void Remove(T element)
        {
            var existing = Read(element.Id);
            if (existing != null)
            {
                _collection.Remove(existing);
            }
        }

        // Реалізація додаткового завдання (Save/Load) 
        public void Save(string filePath)
        {
            // Перетворюємо список об'єктів у текст (JSON)
            var json = JsonSerializer.Serialize(_collection, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                // Завантажуємо дані з тексту назад у список
                _collection = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }
    }
}