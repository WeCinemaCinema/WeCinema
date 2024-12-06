using System.Security.Claims;
using ITDIV_TICKET.Models;
using Microsoft.AspNetCore.Authentication;
using Supabase;
using static Supabase.Postgrest.Constants;


public class MovieService
{
    private readonly Client _supabaseClient;

    public MovieService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<List<MoviesModel>> GetAllMovies()
    {
        var data = await _supabaseClient.From<MoviesModel>().Get();
        var movies = data.Models ?? [];
        return movies;
    }


    public async Task<List<UpcomingMoviesModel>> GetUpcomingMovie()
    {
        var data = await _supabaseClient.From<UpcomingMoviesModel>().Get();
        var movies = data.Models ?? [];
        return movies;
    }

    public async Task<List<MsCinemaModels>> GetAllLocation()
    {
        var data = await _supabaseClient.From<MsCinemaModels>().Get();
        var locations = data.Models ?? [];

        return locations;
    }

    public async Task<bool> DoRegister(string name, string phoneNum, string email, string password)
    {
        try
        {
            var model = new MsCustomerModel
            {
                Username = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Phone_number = phoneNum
            };

            await _supabaseClient.From<MsCustomerModel>().Insert(model);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during registration: " + e.Message);
            return false;
        }

    }

    public async Task<bool> DoLogin(string email, string password, HttpContext httpContext)
    {
        try
        {
            var currUser = await _supabaseClient
                .From<MsCustomerModel>()
                .Match(new Dictionary<string, string>
                {
                    { "Email", email }
                })
                .Get();


            var currModel = currUser.Model;

            if (currModel == null)
            {
                return false; 
            }

            var isValid = BCrypt.Net.BCrypt.Verify(password, currModel!.Password);

            if (!isValid) return false;

            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.Email,email)
            };

            var authProp = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            var claimIdentity = new ClaimsIdentity(claimList, "CookieScheme");

            await httpContext.SignInAsync("CookieScheme", new ClaimsPrincipal(claimIdentity), authProp);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during registration: " + e.Message);
            return false;
        }

    }

    public async Task<MoviesModel?> GetMovie(int id)
    {
        var getData = await _supabaseClient
                                .From<MoviesModel>()
                                .Match(new Dictionary<string, string>
                                {
                                    {"Movie_id",id.ToString()}
                                })
                                .Get();

        var currModel = getData.Model;
        return currModel;
    }

    public async Task<MsCinemaModels?> GetLocation(int id)
    {
        var getData = await _supabaseClient
                                .From<MsCinemaModels>()
                                .Match(new Dictionary<string, string>
                                {
                                    {"Cinema_id",id.ToString()}
                                })
                                .Get();

        var currModel = getData.Model;
        return currModel;
    }
    public async Task<bool> CheckEmail(string email)
    {
        var currUser = await _supabaseClient
                .From<MsCustomerModel>()
                .Match(new Dictionary<string, string>
                {
                    { "Email", email }
                })
                .Get();



        if (currUser.Model is null) return false;
        else return true;
    }

    public async Task<MsCustomerModel?> GetUser(string email)
    {
        var getData = await _supabaseClient
                .From<MsCustomerModel>()
                .Match(new Dictionary<string, string>
                {
                    { "Email", email }
                })
                .Get();


        var currModel = getData.Model;
        return currModel;
    }

    public async Task<string> GetReservedSeatsAsync(int bookingId)
    {
        var reservedSeatsResponse = await _supabaseClient
            .From<ReservedSeatModel>()
            .Select("Seat_id")
            .Match(new Dictionary<string, string>
            {
            { "Booking_id", bookingId.ToString() }
            })
            .Get();

        var seatIds = reservedSeatsResponse.Models.Select(rs => rs.Seat_id).ToList();

        if (!seatIds.Any())
        {
            return "No reserved seats.";
        }
        var seatDetailsResponse = await _supabaseClient
            .From<MsSeatModels>()
            .Select("Seat_row, Seat_column")
            .Filter("Seat_id", Operator.In, seatIds) 
            .Get();

        var seatDetails = seatDetailsResponse.Models;

        var formattedSeats = seatDetails
            .Select(sd => $"{sd.Seat_row}{sd.Seat_column}")
            .ToList();

        return string.Join(", ", formattedSeats);
    }





    public async Task<List<HistoryModel>> GetHistory(string email)
    {
        var getData = await _supabaseClient
            .From<MsCustomerModel>()
            .Match(new Dictionary<string, string>
            {
                { "Email", email }
            })
            .Get();

        var currModel = getData.Models.FirstOrDefault();
        if (currModel == null)
        {
            Console.WriteLine("No customer found with this email.");
            return new List<HistoryModel>();
        }

        var customerId = currModel.Customer_id;
        var transactionHistoryList = new List<HistoryModel>();

        var response = await _supabaseClient
            .From<TransactionModel>()
            .Select("Transaction_id, Booking_id, Total_price")
            .Match(new Dictionary<string, string>
            {
                { "Customer_id", customerId.ToString() }
            })
            .Get();

        var transactions = response.Models;

        if (!transactions.Any())
        {
            Console.WriteLine("No transactions found for this customer.");
            return transactionHistoryList;
        }

        foreach (var transaction in transactions)
        {
            var bookingResponse = await _supabaseClient
                .From<BookingModel>()
                .Select("Booking_id, Timestamp, Show_id")
                .Match(new Dictionary<string, string>
                {
                    { "Booking_id", transaction.Booking_id.ToString() }
                })
                .Get();

            var booking = bookingResponse.Models.FirstOrDefault();
            if (booking == null) continue;

            var showResponse = await _supabaseClient
                .From<ShowModel>()
                .Select("Show_id, Date_time, Movie_id, Cinema_id")
                .Match(new Dictionary<string, string>
                {
                    { "Show_id", booking.Show_id.ToString() }
                })
                .Get();

            var show = showResponse.Models.FirstOrDefault();
            if (show == null) continue;

            var cinemaResponse = await _supabaseClient
                .From<MsCinemaModels>()
                .Select("Cinema_id, Name")
                .Match(new Dictionary<string, string>
                {
                    { "Cinema_id", show.Cinema_id.ToString() }
                })
                .Get();

            var cinema = cinemaResponse.Models.FirstOrDefault();
            if (cinema == null) continue;

            var movieResponse = await _supabaseClient
                .From<MoviesModel>()
                .Select("Title, ImageUrl")
                .Match(new Dictionary<string, string>
                {
                    { "Movie_id", show.Movie_id.ToString() }
                })
                .Get();

            var movie = movieResponse.Models.FirstOrDefault();
            if (movie == null) continue;

            var transactionHistory = new HistoryModel
            {
                Transaction_id = transaction.Transaction_id,
                Booking_id = booking.Booking_id,
                Seat_number = await GetReservedSeatsAsync(booking.Booking_id), 
                Booking_timestamp = booking.Timestamp,
                Show_date_time = show.Date,
                Cinema_name = cinema.Name,
                Movie_image_url = movie.ImageUrl,
                Movie_title = movie.Title,
                TotalPrice = transaction.Total_price
            };

            transactionHistoryList.Add(transactionHistory);
        }

        return transactionHistoryList;
    }




    public async Task<List<MoviesModel>> GetMovieByTitle(string title)
    {
        var result = await _supabaseClient
            .From<MoviesModel>()
            .Filter("Title", Operator.ILike, $"%{title}%")
            .Get();

        if (result.Models is null) return [];

        return result.Models;
    }

    public async Task<List<MsSeatModels>> GetReservedSeatsForCinema(int cinemaId, int movieId)
    {
        try
        {
            var shows = await _supabaseClient
                .From<ShowModel>()
                .Filter("Cinema_id", Operator.Equals, cinemaId)
                .Filter("Movie_id", Operator.Equals, movieId) 
                .Get();


            if (shows.Models == null || !shows.Models.Any())
            {
                return new List<MsSeatModels>();
            }

            var showIds = shows.Models.Select(show => show.Show_id).ToList();

            var bookings = await _supabaseClient
                .From<BookingModel>()
                .Filter("Show_id", Operator.In, showIds) 
                .Get();

            if (bookings.Models == null || !bookings.Models.Any())
            {
                return new List<MsSeatModels>();
            }

            var bookingIds = bookings.Models.Select(booking => booking.Booking_id).ToList();

            var reservedSeats = await _supabaseClient
                .From<ReservedSeatModel>()
                .Filter("Booking_id", Operator.In, bookingIds) 
                .Get();

            if (reservedSeats.Models == null || !reservedSeats.Models.Any())
            {
                return new List<MsSeatModels>();
            }

            var reservedSeatIds = reservedSeats.Models.Select(seat => seat.Seat_id).ToList();

            var seatData = await _supabaseClient
                .From<MsSeatModels>()
                .Filter("Seat_id", Operator.In, reservedSeatIds)
                .Get();

            if (seatData.Models == null || !seatData.Models.Any())
            {
                return new List<MsSeatModels>();
            }

            var combinedSeatData = seatData.Models.Select(seat => new MsSeatModels
            {
                Seat_id = seat.Seat_id,
                Seat_row = seat.Seat_row,
                Seat_column = seat.Seat_column
            }).ToList();

            return combinedSeatData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<MsSeatModels>();
        }
    }

    public async Task<List<ShowModel>> GetShowsForCinemaAndMovie(int cinemaId, int movieId)
    {
        try
        {
            var shows = await _supabaseClient
                .From<ShowModel>() 
                .Filter("Cinema_id", Operator.Equals, cinemaId)
                .Filter("Movie_id", Operator.Equals, movieId)
                .Get();
            return shows.Models ?? new List<ShowModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<ShowModel>();
        }
    }

    public async Task<int> GetSinglePrice(int cinemaId)
    {
        var getData = await _supabaseClient
                                        .From<MsCinemaModels>()
                                        .Match(new Dictionary<string, string>
                                        {
                                            {"Cinema_id",cinemaId.ToString()}
                                        })
                                        .Get();

        var currModel = getData.Model;

        return currModel?.Price ?? 0;
    }

    public async Task<List<ShowModel>> GetShowByCinemaMovieDate(int cinemaId, int movieId, DateTime time)
    {
        try
        {
            var utcTime = time.ToUniversalTime();
            var formattedDateTime = utcTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

            var shows = await _supabaseClient
                .From<ShowModel>() 
                .Filter("Cinema_id", Operator.Equals, cinemaId)  
                .Filter("Movie_id", Operator.Equals, movieId)   
                .Filter("Date_time", Operator.Equals, formattedDateTime)      
                .Get();

            return shows.Models ?? new List<ShowModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<ShowModel>();
        }
    }

    public async Task InsertData(int totalPrice, string paymentMethod, int totalSeat, int showId, string seat, int customerId)
    {
        var seats = seat.Split(',');

        var booking = new BookingModel
        {
            Timestamp = DateTime.UtcNow,
            Show_id = showId
        };
        var bookingResult = await _supabaseClient.From<BookingModel>().Insert(booking);
        int bookingId = bookingResult.Models.First().Booking_id;

        var transaction = new TransactionModel
        {
            Customer_id = customerId, 
            Booking_id = bookingId,
            Total_seat = totalSeat,
            Total_price = totalPrice,
            Payment_method = paymentMethod
        };

        var transactionResult = await _supabaseClient.From<TransactionModel>().Insert(transaction);

        foreach (var nseat in seats)
        {
            string seatRow = new string(nseat.TakeWhile(char.IsLetter).ToArray()); 
            int seatColumn = int.Parse(new string(nseat.SkipWhile(char.IsLetter).ToArray())); 

            var seatResult = await _supabaseClient
                .From<MsSeatModels>()
                .Filter("Seat_row", Operator.Equals, seatRow)
                .Filter("Seat_column", Operator.Equals, seatColumn)
                .Get();

            if (!seatResult.Models.Any())
            {
                throw new Exception($"Seat {nseat} not found in Ms_Seat table.");
            }

            int seatId = seatResult.Models.First().Seat_id;

            var reservedSeat = new ReservedSeatModel
            {
                Seat_id = seatId,
                Booking_id = bookingId
            };
            await _supabaseClient.From<ReservedSeatModel>().Insert(reservedSeat);
        }
    }
}