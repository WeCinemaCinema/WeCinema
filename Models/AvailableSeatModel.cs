using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Available_Seat")]
public class AvailableSeatModel : BaseModel
{
    [PrimaryKey]
    public int AvailableSeat_id {get;set;}

    [Column("Status")]
    public bool Status {get;set;}

    [Column("Price")]
    public int Price {get;set;}
    
    
    [Column("Audit_id")]
    public int Audit_id {get;set;}

    
    [Column("Show_id")]
    public int Show_id {get;set;}

    [Column("Booking_id")]
    public int Booking_id {get;set;}
}