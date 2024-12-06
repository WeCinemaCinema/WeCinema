using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;


[Table("Reserved_Seat")]

public class ReservedSeatModel: BaseModel
{
    [PrimaryKey]
    public int ReservedSeat_id {get;set;}


    [Column("Seat_id")]
    public int Seat_id {get;set;}


    [Column("Booking_id")]
    public int Booking_id {get;set;}


}