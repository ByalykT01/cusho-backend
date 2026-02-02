using cusho.Models;
using Microsoft.EntityFrameworkCore;

namespace cusho.Contexts;

public class MyAppContext(DbContextOptions<MyAppContext> options) : DbContext(options)
{
    public DbSet<Cart> Cart { get; set; } = null!;
}

