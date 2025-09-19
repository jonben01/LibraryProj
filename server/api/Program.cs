using DotNetEnv;
using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("CONN_STR");

builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(connectionString);
});

var app = builder.Build();

app.MapGet("/", ([FromServices]MyDbContext dbContext) =>
{
    
    var book = new Book()
    {
        Id = Guid.NewGuid().ToString(),
        Title = "Jdoas",
        Pages = 1,
        Createdat = DateTime.UtcNow,
        Authors =  null
    };
    dbContext.Books.Add(book);
    dbContext.SaveChanges();
    var objects = dbContext.Books.ToList();
    return objects;

});

app.Run();
