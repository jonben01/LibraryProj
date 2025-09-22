using DotNetEnv;
using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString"));
});

var app = builder.Build();

app.MapGet("/", ([FromServices]MyDbContext dbContext) =>
{
    Console.WriteLine("Hello World!");
});

app.Run();
