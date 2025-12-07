using Microsoft.EntityFrameworkCore;
using SmartLibrary.Api.Application.Commands.AddBook;
using SmartLibrary.Api.Application.Commands.BorrowBook;
using SmartLibrary.Api.Application.Commands.CreateUser;
using SmartLibrary.Api.Application.Queries.GetAllBooks;
using SmartLibrary.Api.Domain.Repositories;
using SmartLibrary.Api.FileHelper;
using SmartLibrary.Api.Infrastructure;
using SmartLibrary.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Add services to the container.

// DbContext
builder.Services.AddDbContext<LibraryDbContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Repositories
builder.Services.AddScoped<IUserContextProvider, UserContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
        typeof(AddBookHandler).Assembly,
        typeof(UpdateBookCommand).Assembly,
        typeof(BorrowBookCommand).Assembly,
        typeof(ReturnBookCommand).Assembly,
        typeof(GetBookByIdQuery).Assembly,
        typeof(GetTopRatedQuery).Assembly,
        typeof(CreateUserCommand).Assembly));


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
