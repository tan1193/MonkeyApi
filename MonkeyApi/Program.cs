using MonkeyApi.Data;
using MonkeyApi;
using Microsoft.EntityFrameworkCore;
using Application;
using Infrastruscture;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MonkeyApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MonkeyApiContext") ?? throw new InvalidOperationException("Connection string 'MonkeyApiContext' not found.")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastruscture();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddSingleton<SqlConnectionFactory>(new SqlConnectionFactory(builder.Configuration.GetConnectionString("MonkeyApiContext") ?? throw new InvalidOperationException("Connection string 'MonkeyApiContext' not found."))); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapMonkeyEndpoints();

app.Run();
