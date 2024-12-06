using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Booking")]
public class BookingModel : BaseModel
{
    [PrimaryKey]
    public int Booking_id {get;set;}

    [Column("Timestamp")]
    public DateTime Timestamp {get;set;}
    
    
    [Column("Show_id")]
    public int Show_id {get;set;}
}