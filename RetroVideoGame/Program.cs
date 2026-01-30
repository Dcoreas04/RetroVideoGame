using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddControllers();

builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.MapGet("/instance", () =>
{
    Console.WriteLine($"Handled by {Environment.MachineName}");
    return Results.Ok(new { instance = Environment.MachineName });
});

app.Run();
