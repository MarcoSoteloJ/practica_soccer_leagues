using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class SoccerLeagueDbContext : DbContext
{
    public SoccerLeagueDbContext(DbContextOptions<SoccerLeagueDbContext> options):base(options)
    {
    }
    
    public DbSet<Team> Teams { get; set; }
    public DbSet<League> Leagues { get; set; }
}

