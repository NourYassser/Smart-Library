using AuthService.Application.DependencyInjection;
using AuthService.Application.Interface;
using AuthService.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.

// DbContext
builder.Services.AddDbContext<AuthDbContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Repositories

builder.Services.AddScoped<IAuthDbContext>(sp =>
    sp.GetRequiredService<AuthDbContext>());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add services to the container.

builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
