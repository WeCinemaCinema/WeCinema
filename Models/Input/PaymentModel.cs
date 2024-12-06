namespace ITDIV_TICKET.Models;

public class PaymentModel
{
    public int CinemaId {get; set;}
    
    public int MovieId {get; set;}
    
    public string? Date {get; set;}
    
    public string? Time {get; set;}
    
    public string? SeatList {get; set;}

}