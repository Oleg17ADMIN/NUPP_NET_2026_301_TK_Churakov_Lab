using LibrarySystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure
{
    public class LibraryContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryContext() : base() { }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        // Твої існуючі таблиці
        public DbSet<LibraryItemModel> LibraryItems { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<MagazineModel> Magazines { get; set; }
        public DbSet<PublisherModel> Publishers { get; set; }
        public DbSet<MemberModel> Members { get; set; }
        public DbSet<LibraryCardModel> LibraryCards { get; set; }
        public DbSet<GenreModel> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=library.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Твої налаштування зв'язків з попередніх робіт
            modelBuilder.Entity<LibraryItemModel>().ToTable("LibraryItems");
            modelBuilder.Entity<BookModel>().ToTable("Books");
            modelBuilder.Entity<MagazineModel>().ToTable("Magazines");

            modelBuilder.Entity<MemberModel>()
                .HasOne(m => m.LibraryCard)
                .WithOne(c => c.Member)
                .HasForeignKey<LibraryCardModel>(c => c.MemberId);

            modelBuilder.Entity<PublisherModel>()
                .HasMany(p => p.PublishedItems)
                .WithOne(i => i.Publisher)
                .HasForeignKey(i => i.PublisherId);

            modelBuilder.Entity<BookModel>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books);
        }
    }
}