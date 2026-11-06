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
}