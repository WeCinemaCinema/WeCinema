namespace ITDIV_TICKET.Models;

public class HistoryModel
{
    public int Transaction_id { get; set; }
    public int Booking_id { get; set; }
    public string? Seat_number { get; set; }
    public DateTime Booking_timestamp { get; set; }
    public DateTime Show_date_time { get; set; }
    public string? Cinema_name { get; set; }
    public string? Movie_title { get; set; }  
    public string? Movie_image_url { get; set; }
    public int TotalPrice { get; set; }      
}