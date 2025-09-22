using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");

builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(connectionString);
});

var app = builder.Build();

app.MapGet("/", ([FromServices]MyDbContext dbContext) =>
{
    return dbContext.Books.ToList();
});

app.Run();
