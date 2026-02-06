using cusho.Models;
using Microsoft.EntityFrameworkCore;

namespace cusho.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}