using Zajezdnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Zajezdnia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Zajezdnia_autobusow> Zajezdnie { get; set; }
    public DbSet<Autobus> Autobusy { get; set; }
    public DbSet<Kierowca> Kierowcy { get; set; }
    public DbSet<Kurs> Kursy { get; set; }
    public DbSet<Uzytkownik> Uzytkownicy { get; set; }
    
    
}