using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonkeyApi.Data;
using MonkeyApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MonkeyApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MonkeyApiContext") ?? throw new InvalidOperationException("Connection string 'MonkeyApiContext' not found.")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SqlConnectionFactory>(new SqlConnectionFactory(builder.Configuration.GetConnectionString("MonkeyApiContext") ?? throw new InvalidOperationException("Connection string 'MonkeyApiContext' not found."))); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMonkeyEndpoints();

app.Run();
