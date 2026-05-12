using LibrarySystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LibrarySystem.Infrastructure
{
    public class LibraryContext : DbContext
    {
        // Таблиці нашої бази даних
        public DbSet<LibraryItemModel> LibraryItems { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<MagazineModel> Magazines { get; set; }
        public DbSet<PublisherModel> Publishers { get; set; }
        public DbSet<MemberModel> Members { get; set; }
        public DbSet<LibraryCardModel> LibraryCards { get; set; }
        public DbSet<GenreModel> Genres { get; set; }

        // Налаштування підключення до SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // База даних буде створена у файлі library.db в папці запуску програми
            optionsBuilder.UseSqlite("Data Source=library.db");
        }

        // Налаштування зв'язків через Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Реалізація Table-per-Type (TPT)
            // Вказуємо EF створювати окремі таблиці для нащадків
            modelBuilder.Entity<LibraryItemModel>().ToTable("LibraryItems");
            modelBuilder.Entity<BookModel>().ToTable("Books");
            modelBuilder.Entity<MagazineModel>().ToTable("Magazines");

            // 2. Зв'язок 1-до-1 (Member <-> LibraryCard)
            modelBuilder.Entity<MemberModel>()
                .HasOne(m => m.LibraryCard)
                .WithOne(c => c.Member)
                .HasForeignKey<LibraryCardModel>(c => c.MemberId);

            // 3. Зв'язок 1-до-багатьох (Publisher -> LibraryItems)
            modelBuilder.Entity<PublisherModel>()
                .HasMany(p => p.PublishedItems)
                .WithOne(i => i.Publisher)
                .HasForeignKey(i => i.PublisherId);

            // 4. Зв'язок багато-до-багатьох (Book <-> Genre)
            // EF Core 8 автоматично створить проміжну таблицю
            modelBuilder.Entity<BookModel>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books);

            base.OnModelCreating(modelBuilder);
        }
    }
}