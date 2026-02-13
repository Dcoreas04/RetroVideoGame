using System.Text.Json;
using Confluent.Kafka;
using MailKit.Net.Smtp;
using MimeKit;
using EmailWorker.Models;

string bootstrap = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? "kafka:9092";
string topic = Environment.GetEnvironmentVariable("KAFKA_EMAIL_TOPIC") ?? "emails";

string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "mailhog";
int smtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 1025;
string from = Environment.GetEnvironmentVariable("SMTP_FROM") ?? "noreply@retrogame.local";

Console.WriteLine($"[EmailWorker] Starting...");
Console.WriteLine($"[EmailWorker] Kafka: {bootstrap} topic={topic}");
Console.WriteLine($"[EmailWorker] SMTP: {smtpHost}:{smtpPort} from={from}");

var config = new ConsumerConfig
{
    BootstrapServers = bootstrap,
    GroupId = "emailworker-group",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = true
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe(topic);

while (true)
{
    try
    {
        var cr = consumer.Consume();
        Console.WriteLine($"[EmailWorker] Received: {cr.Message.Value}");

        var msg = JsonSerializer.Deserialize<EmailWorker.Models.EmailMessage>(cr.Message.Value,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        if (msg == null || string.IsNullOrWhiteSpace(msg.To))
        {
            Console.WriteLine("[EmailWorker] Invalid payload (missing 'to'). Skipping.");
            continue;
        }

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from));
        email.To.Add(MailboxAddress.Parse(msg.To));
        email.Subject = msg.Subject ?? "(no subject)";
        email.Body = new TextPart("plain") { Text = msg.Body ?? "" };

        using var smtp = new SmtpClient();
        smtp.Connect(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.None);
        smtp.Send(email);
        smtp.Disconnect(true);

        Console.WriteLine($"[EmailWorker] Sent email to {msg.To} (captured by MailHog).");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[EmailWorker] ERROR: {ex.Message}");
    }
}