using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly SoccerLeagueDbContext _context;
    public TeamsController(SoccerLeagueDbContext context)
    {
        _context = context;
    }

    
    
    //REQUESTS 
    
    
    [HttpGet]
    public async Task<List<Team>> GetAllTeams()
    {
        return await _context.Teams.ToListAsync();
    }
    

    
    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> GetTeamById(int id)
    {
        var team = await _context.Teams.FindAsync(id);

        if (team == null)
        {
            return NotFound();
        }

        return team;
    }

    [HttpPost]
    public async Task<ActionResult<Team>> CreateTeam(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("GetTeamById", new { id = team.Id }, team);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(int id, Team updated)
    {
        var team = await _context.Teams.FindAsync(id);

        if (team == null)
        {
            return NotFound();
        }

        team.Name = updated.Name;
        team.Country = updated.Country;
        team.PlayersQuantity = updated.PlayersQuantity;
        team.Enabled= updated.Enabled;
        await _context.SaveChangesAsync();
        
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        
        if (team == null)
        {
            return NotFound();
        }

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpGet("{id}/leagues")]
    public async Task<List<League>> GetTeamLeagues(int id)
    {
        return await (
            from leagueTeam in _context.LeagueTeams
            join league in _context.Leagues on leagueTeam.LeagueId equals league.Id
            where leagueTeam.TeamId == id
            select league
        ).ToListAsync();
    }
}

