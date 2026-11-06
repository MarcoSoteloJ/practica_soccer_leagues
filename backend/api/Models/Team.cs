using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Team")]

public class Team
{
   public int Id { get; set; }
   
   public string Name { get; set; } = "";
   
   public string Country { get; set; } = "";
   
   public int PlayersQuantity { get; set; }

   public bool Enabled { get; set; } = true;
}