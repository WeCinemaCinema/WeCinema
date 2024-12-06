using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Transaction")]
public class TransactionModel : BaseModel
{
    [PrimaryKey]
    public int Transaction_id {get;set;}

    [Column("Customer_id")]
    public int Customer_id {get;set;}

    [Column("Booking_id")]
    public int Booking_id {get;set;}
    
    
    [Column("Total_seat")]
    public int Total_seat {get;set;}

    
    [Column("Total_price")]
    public int Total_price {get;set;}

    [Column("Payment_method")]
    public string? Payment_method {get;set;}

}