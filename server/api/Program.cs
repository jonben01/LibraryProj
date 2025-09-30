using api;
using api.Services;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");

builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddCors();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors(config => config
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .SetIsOriginAllowed(x => true));

app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi();
await app.GenerateApiClientsFromOpenApi("/../../client/src/generated-ts-client.ts");


app.Run();
