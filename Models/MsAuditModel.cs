using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Ms_Audit")]
public class MsAuditModel : BaseModel
{
    [PrimaryKey]
    public int Audit_id {get;set;}

    [Column("Audit_name")]
    public string? Audit_name {get;set;}

    [Column("Total_seat")]
    public int Total_seat {get;set;}
    
    [Column("Cinema_id")]
    public int Cinema_id {get;set;}

    
    [Column("Release_date")]
    public DateTime Release_date {get;set;}

    [Column("Director")]
    public string? Director {get;set;}

    [Column("Duration")]
    public DateTime Duration {get;set;}

}