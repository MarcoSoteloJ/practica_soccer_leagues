using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("League")]

public class League
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Country { get; set; } = "";
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public bool Enabled { get; set; } = true;
}