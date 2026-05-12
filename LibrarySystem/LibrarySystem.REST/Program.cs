using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Models;
using LibrarySystem.Infrastructure.Repositories;
using LibrarySystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Реєстрація стандартних сервісів Web API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Реєстрація ваших сервісів (DI) ---

// 1. Реєструємо контекст бази даних (SQLite)
builder.Services.AddDbContext<LibraryContext>();

// 2. Реєструємо репозиторії ЧЕРЕЗ ІНТЕРФЕЙСИ (це виправить помилку запуску)
builder.Services.AddScoped<IRepository<BookModel>, Repository<BookModel>>();
builder.Services.AddScoped<IRepository<PublisherModel>, Repository<PublisherModel>>();

// 3. Реєструємо асинхронні сервіси
builder.Services.AddScoped<LibraryServiceAsync<BookModel>>();
builder.Services.AddScoped<LibraryServiceAsync<PublisherModel>>();

var app = builder.Build();

// --- Налаштування конвеєра запитів (Middleware) ---

// Swagger буде доступний за адресою: https://localhost:XXXX/swagger/index.html
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Цей метод шукає контролери в папці Controllers
app.MapControllers();

app.Run();