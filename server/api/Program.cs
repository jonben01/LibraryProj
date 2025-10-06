using api;
using api.Services;
using efscaffold;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app);

app.Run();

public partial class Program
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnectionString");
        
        services.AddDbContext<MyDbContext>(conf => conf.UseNpgsql(connectionString));
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IGenreService, GenreService>();
        
        services.AddControllers();
        services.AddOpenApiDocument();
        services.AddProblemDetails();
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddCors();
    }

    public static void Configure(WebApplication app)
    {
        app.UseExceptionHandler();
        
        app.UseCors(config => config
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .SetIsOriginAllowed(x => true));

        if (app.Environment.IsDevelopment())
        {
               app.UseOpenApi();
               app.UseSwaggerUi();
               app.GenerateApiClientsFromOpenApi("/../../client/src/generated-ts-client.ts")
                   .GetAwaiter().GetResult();
        }
        
        app.MapControllers();
    }
}