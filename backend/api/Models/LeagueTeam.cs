using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.Models;

[Table("LeagueTeam")]
[PrimaryKey(nameof(LeagueId), nameof(TeamId))]

public class LeagueTeam
{
    public int LeagueId { get; set; }
    public int TeamId { get; set; }
}