using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BookOnTable.Data;
using BookOnTable.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book On Table API", Description = "Todos os seus livros num só lugar", Version = "v1" });
    }
);

var connectionString = builder.Configuration.GetConnectionString("Books") ?? "Data Source=Book.db";
builder.Services.AddSqlite<AppDbContext>(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book")
    );
}

app.UseHttpsRedirection();

app.MapGet("/Book", async (AppDbContext db) => await db.Book.ToListAsync());

app.MapGet("/Book/{id}", async (AppDbContext db, int id) => await db.Book.FindAsync(id));

app.MapPut("/Book/{id}", async (AppDbContext db, Book updatebook, int id) =>
{
    var book = await db.Book.FindAsync(id);
    if (book is null) return Results.NotFound();


    book.Title = updatebook.Title;
    book.Author = updatebook.Author;
    book.Synopsis = updatebook.Synopsis;
    book.PublishDate = updatebook.PublishDate;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/book/{id}", async (AppDbContext db, int id) =>
{
    var book = await db.Book.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    
    db.Book.Remove(book);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/book", async (AppDbContext db, Book book) =>
{
    await db.Book.AddAsync(book);
    await db.SaveChangesAsync();
    return Results.Created($"/book/{book.Id}", book);
});

app.Run();
