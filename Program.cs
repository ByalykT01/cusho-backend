using cusho.Contexts;
using cusho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<NetworkClient>();
builder.Services.AddSingleton<MessageFactory>();
builder.Services.AddSingleton(provider => new EmailServerSettings(Host: "smtp.gmail.com", Port: 25));

builder.Services.AddDbContext<MyAppContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();
app.UseHttpsRedirection();

app.MapGet("/", void () => Results.NotFound());

app.MapGet("/teapot", (HttpResponse res) =>
    {
        res.ContentType = "plain/text";
        res.StatusCode = 418;
        return res.WriteAsync("I\'m a teapot!!");
    }
);

LinkGenerator links = app.Services.GetRequiredService<LinkGenerator>();

app.MapGet("/product/{id}", (int id) => { Results.Ok(new { Source = "route", Value = id }); });

app.MapGet("/product", (int id) => { return $"Received {id} in route query"; });

app.MapGet("/cart/{id}", (long id, MyAppContext myAppContext) =>
{
    if (id == 1)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { "Invalid format. Id must start with 'f'" } }
        });
    }

    var cart = myAppContext.Cart.Find(id);
    if (cart == null)
    {
        return Results.Problem(statusCode: 404);
    }

    return TypedResults.Ok(cart);
});

app.MapPost("/cart", ([FromBody] Cart cart, MyAppContext myAppContext) =>
{
    myAppContext.Cart.Add(cart);
    myAppContext.SaveChanges();
});

app.MapGet("register/{username}", RegisterUser);

app.Run();

string RegisterUser(string username, IEmailSender emailSender)
{
    emailSender.SendEmail(username);
    return $"Email sent to {username}";
}

public record EmailServerSettings(string Host, int Port);

public class NetworkClient
{
    private readonly EmailServerSettings _settings;

    public NetworkClient(EmailServerSettings settings)
    {
        _settings = settings;
    }

    public void SendEmail(string email)
    {
        Console.WriteLine($"Hello, {email}! This is a test!");
    }
}

public class MessageFactory
{
    public string Create(string username)
    {
        return $"{username}@gmail.com";
    }
}

public interface IEmailSender
{
    public void SendEmail(string username);
}

public class EmailSender : IEmailSender
{
    private readonly NetworkClient _client;
    private readonly MessageFactory _factory;

    public EmailSender(MessageFactory factory, NetworkClient client)
    {
        _factory = factory;
        _client = client;
    }

    public void SendEmail(string username)
    {
        var email = _factory.Create(username);
        _client.SendEmail(email);
        Console.WriteLine($"Email sent to {username}");
    }
}