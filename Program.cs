using cusho.Contexts;
using cusho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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


app.MapGet("/cart", (MyAppContext myAppContext) => myAppContext.Cart);

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
app.Run();