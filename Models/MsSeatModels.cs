using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;


[Table("Ms_Seat")]

public class MsSeatModels: BaseModel
{
    [PrimaryKey]
    public int Seat_id {get;set;}
    
    [Column("Seat_row")]
    public string? Seat_row {get;set;}

    [Column("Seat_column")]
    public int Seat_column {get;set;}
}