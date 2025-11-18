using Microsoft.EntityFrameworkCore;
using SmartLibrary.Api.Application.Commands.AddBook;
using SmartLibrary.Api.Domain.Repositories;
using SmartLibrary.Api.Infrastructure;
using SmartLibrary.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
        typeof(AddBookHandler).Assembly,
        typeof(GetBookByIdQuery).Assembly,
        typeof(BookRepository).Assembly));


// DbContext
builder.Services.AddDbContext<LibraryDbContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
