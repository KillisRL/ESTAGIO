using UsersInfraestrutura;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var connectionString = "Server=localhost;Database=BarbeariaDB;User Id=sa;Password=SuaSenhaAqui;TrustServerCertificate=True;";
//builder.Services.AddDbContext<UserDBContext>();

//INSTALAR ISSO PARA O MARIADB - dotnet add package Pomelo.EntityFrameworkCore.MySql
var connectionString = builder.Configuration.GetConnectionString("MariaDb");

builder.Services.AddDbContext<UserDBContext>(options =>
{
    // O Pomelo requer que a string de conexão e a versão sejam passadas
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


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

app.UseAuthorization();

app.MapControllers();

app.Run();
