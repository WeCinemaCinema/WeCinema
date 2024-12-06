using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Ms_Cinema")]

public class MsCinemaModels: BaseModel
{
    [PrimaryKey]
    public int Cinema_id {get;set;}
    
    [Column("Location")]
    public string? Location {get;set;}
    
    [Column("Name")]
    public string? Name {get;set;}

    [Column("Price")]
    public int Price {get;set;}
}