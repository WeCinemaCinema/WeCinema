using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("Show")]
public class ShowModel : BaseModel
{
    [PrimaryKey]
    public int Show_id {get;set;}

    [Column("Date_time")]
    public DateTime Date {get;set;}

    [Column("Cinema_id")]
    public int Cinema_id {get;set;}
    
    [Column("Movie_id")]
    public int Movie_id {get;set;}
}