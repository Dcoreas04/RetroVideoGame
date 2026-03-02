using System.Text.Json;
using Confluent.Kafka;
using MailKit.Net.Smtp;
using MimeKit;
using Prometheus;
using EmailWorker.Models;

string bootstrap = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS") ?? "kafka:9092";
string topic = Environment.GetEnvironmentVariable("KAFKA_EMAIL_TOPIC") ?? "emails";

string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "mailhog";
int smtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 1025;
string from = Environment.GetEnvironmentVariable("SMTP_FROM") ?? "noreply@retrogame.local";

Console.WriteLine($"[EmailWorker] Starting...");
Console.WriteLine($"[EmailWorker] Kafka: {bootstrap} topic={topic}");
Console.WriteLine($"[EmailWorker] SMTP: {smtpHost}:{smtpPort} from={from}");

var emailsReceived = Metrics.CreateCounter("emailworker_emails_received_total", "Total email messages received from Kafka");
var emailsSent = Metrics.CreateCounter("emailworker_emails_sent_total", "Total emails sent successfully");
var emailsInvalid = Metrics.CreateCounter("emailworker_emails_invalid_total", "Total invalid email messages");
var emailsFailed = Metrics.CreateCounter("emailworker_emails_failed_total", "Total email send failures");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapMetrics("/metrics");

_ = Task.Run(() =>
{
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
            emailsReceived.Inc();

            Console.WriteLine($"[EmailWorker] Received: {cr.Message.Value}");

            var msg = JsonSerializer.Deserialize<EmailMessage>(
                cr.Message.Value,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (msg == null || string.IsNullOrWhiteSpace(msg.To))
            {
                emailsInvalid.Inc();
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

            emailsSent.Inc();
            Console.WriteLine($"[EmailWorker] Sent email to {msg.To} (captured by MailHog).");
        }
        catch (Exception ex)
        {
            emailsFailed.Inc();
            Console.WriteLine($"[EmailWorker] ERROR: {ex.Message}");
        }
    }
});

app.Run("http://0.0.0.0:9100");