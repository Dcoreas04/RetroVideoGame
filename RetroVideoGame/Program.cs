using System.Text.Json.Serialization;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITradeOfferService, TradeOfferService>();

// AI was used for this
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var bootstrap = config["KAFKA_BOOTSTRAP_SERVERS"] ?? "kafka:9092";

    var producerConfig = new ProducerConfig
    {
        BootstrapServers = bootstrap
    };

    return new ProducerBuilder<Null, string>(producerConfig).Build();
});

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

