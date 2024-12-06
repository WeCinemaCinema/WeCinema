using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Ms_Customer")]
public class MsCustomerModel : BaseModel
{
    [PrimaryKey]
    public int Customer_id {get;set;}

    [Column("Username")]
    public string? Username {get;set;}

    [Column("Email")]
    public string? Email {get;set;}
    
    
    [Column("Password")]
    public string? Password {get;set;}

    
    [Column("Phone_number")]
    public string? Phone_number {get;set;}
}