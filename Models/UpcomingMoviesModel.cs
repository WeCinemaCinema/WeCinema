using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITDIV_TICKET.Models;

[Table("UpcomingMovie")]
public class UpcomingMoviesModel : BaseModel
{
    [PrimaryKey]
    public int Movie_id { get; set; }

    [Column("Title")]
    public string? Title { get; set; }

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Genre")]
    public string? Genre { get; set; }

    [Column("Release_date")]
    public DateTime Release_date { get; set; }

    [Column("Director")]

    public string? Director { get; set; }

    [Column("Duration")]
    public int Duration { get; set; }


    [Column("Rating")]
    public string? Rating { get; set; }


    [Column("ImageUrl")]
    public string? ImageUrl { get; set; }

    [Column("Cast")]
    public string? Cast { get; set; }
}