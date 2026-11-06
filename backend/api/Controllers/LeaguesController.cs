using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly SoccerLeagueDbContext _context;
    public LeaguesController(SoccerLeagueDbContext context)
    {
        _context = context;
    }

    
    
    // REQUESTS
    
    
    [HttpGet]
    public async Task<List<League>> GetAllLeagues()
    {
        return await _context.Leagues.ToListAsync();
    }
    

    
    [HttpGet("{id}")]
    public async Task<ActionResult<League>> GetLeagueById(int id)
    {
        var league = await _context.Leagues.FindAsync(id);

        if (league == null)
        {
            return NotFound();
        }

        return league;
    }

    [HttpPost]
    public async Task<ActionResult<League>> CreateLeague(League league)
    {
        
        if (league.StartDate > league.EndDate)
        {
            return BadRequest("StartDate cannot be after EndDate");
        }
        
        _context.Leagues.Add(league);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetLeagueById", new { id = league.Id }, league);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeague(int id, League updated)
    {
        
        if (updated.StartDate > updated.EndDate)
        {
            return BadRequest("StartDate cannot be after EndDate");
        }
        
        var league = await _context.Leagues.FindAsync(id);

        if (league == null)
        {
            return NotFound();
        }

        league.Name = updated.Name;
        league.Country = updated.Country;
        league.StartDate = updated.StartDate;
        league.EndDate = updated.EndDate;
        league.Enabled= updated.Enabled;
        await _context.SaveChangesAsync();
        
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeague(int id)
    {
        var league = await _context.Leagues.FindAsync(id);
        
        if (league == null)
        {
            return NotFound();
        }

        _context.Leagues.Remove(league);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpGet("{id}/teams")]
    public async Task<List<Team>> GetLeagueTeams(int id)
    {
        return await (
            from leagueTeam in _context.LeagueTeams
            join team in _context.Teams on leagueTeam.TeamId equals team.Id
            where leagueTeam.LeagueId == id
            select team
        ).ToListAsync();
    }

    [HttpPost("{id}/teams/{teamId}")]
    public async Task<IActionResult> AddTeamToLeague(int id, int teamId)
    {
        var leagueExists = await _context.Leagues.AnyAsync(league => league.Id == id);
        var teamExists = await _context.Teams.AnyAsync(team => team.Id == teamId);

        if (!leagueExists || !teamExists)
        {
            return NotFound();
        }

        var alreadyInLeague = await _context.LeagueTeams.FindAsync(id, teamId);

        if (alreadyInLeague != null)
        {
            return Conflict("The team is already in this league");
        }

        _context.LeagueTeams.Add(new LeagueTeam { LeagueId = id, TeamId = teamId });
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}/teams/{teamId}")]
    public async Task<IActionResult> RemoveTeamFromLeague(int id, int teamId)
    {
        var leagueTeam = await _context.LeagueTeams.FindAsync(id, teamId);

        if (leagueTeam == null)
        {
            return NotFound();
        }

        _context.LeagueTeams.Remove(leagueTeam);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}