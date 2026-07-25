using BorrowService.Application.DependencyInjection;
using BorrowService.Application.Interface;
using BorrowService.Infrastructure;
using BorrowService.Infrastructure.Service;
using Library.BuildingBlock.RabbitMQ.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Add services to the container.

builder.Services.AddRabbitMq(builder.Configuration);

// DbContext
builder.Services.AddDbContext<BorrowDbContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBorrowDbContext>(sp =>
    sp.GetRequiredService<BorrowDbContext>());

builder.Services.AddApplication();

builder.Services.AddHttpClient<IAuthServiceClient, AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7267");
});

builder.Services.AddHttpClient<IBookServiceClient, BookServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7162");
});

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
