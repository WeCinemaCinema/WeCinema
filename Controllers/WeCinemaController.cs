using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ITDIV_TICKET.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ITDIV_TICKET.Controllers;

public class WeCinemaController : Controller
{
    private readonly MovieService _movieService;
    private readonly EmailSender _emailSender;

    public WeCinemaController(MovieService movieService, EmailSender emailSender)
    {
        _movieService = movieService;
        _emailSender = emailSender;
    }

    public async Task<IActionResult> Index()
    {

        var model = await _movieService.GetAllMovies();
        return View(model);
    }

    public IActionResult Register()
    {

        return View();
    }

    public IActionResult LoginWithGoogle()
    {
        var properties = new AuthenticationProperties { RedirectUri = "/signin-google" };
        return Challenge(properties, "Google");
    }

    public IActionResult Login()
    {
        return View();
    }
    [Authorize]
    public async Task<IActionResult> Movie(string? query)
    {
        List<MoviesModel> model;

        if (query != null)
        {
            model = await _movieService.GetMovieByTitle(query);

        }
        else
        {
            model = await _movieService.GetAllMovies();
        }

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> History()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var data = await _movieService.GetHistory(userEmail!);


        return View(data);
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public IActionResult Term()
    {
        return View();
    }

    [Authorize]
    public IActionResult ContactUs()
    {
        return View();
    }

    [Authorize]
    public IActionResult AboutUs()
    {
        return View();
    }

    [Authorize]
    public IActionResult Payment()
    {
        return View();
    }

    [Authorize]
    public IActionResult Promotions()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> OrderTicket(int id, int locationId)
    {
        var fetchData = await _movieService.GetMovie(id);

        ViewData["LocationId"] = locationId;
        ViewData["MovieId"] = id;

        return View(fetchData);
    }
    [Authorize]
    public async Task<IActionResult> Location(int id)
    {
        var fetchData = await _movieService.GetMovie(id);
        return View(fetchData);
    }

    [Authorize]
    public IActionResult Seat()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [Route("WeCinema/DoRegisterUser")]
    public async Task<IActionResult> DoRegisterUser(string name, string phoneNum, string email, string password)
    {

        if (!ModelState.IsValid)
        {
            var ErrorMessage = ModelState.Values.SelectMany(e => e.Errors).FirstOrDefault()?.ErrorMessage;
            ViewData["ErrorMessage"] = ErrorMessage;
            return View("Register");
        }

        var isRegistered = await _movieService.DoRegister(name, phoneNum, email, password);

        if (!isRegistered)
        {
            ViewData["ErrorMessage"] = "Register error";
            return View("Register");
        }
        return View("Login");


    }

    [HttpPost]
    [Route("WeCinema/DoLoginUser")]
    public async Task<IActionResult> DoLoginUser(string email, string password)
    {
        if (!ModelState.IsValid)
        {
            var ErrorMessage = ModelState.Values.SelectMany(e => e.Errors).FirstOrDefault()?.ErrorMessage;
            ViewData["ErrorMessage"] = ErrorMessage;
            return View("Login");
        }

        var isLogin = await _movieService.DoLogin(email, password, HttpContext);

        if (!isLogin)
        {
            ViewData["ErrorMessage"] = "Login error";
            return View("Login");
        }

        return RedirectToAction("Index", "WeCinema");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ContactForm(string name, string email, string message)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                await _emailSender.SendEmailAsync(userEmail!, "WeCinema Support Issue", message);

                return RedirectToAction("Index", "WeCinema");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            var ErrorMessage = ModelState.Values.SelectMany(e => e.Errors).FirstOrDefault()?.ErrorMessage;
            ViewData["ErrorMessage"] = ErrorMessage;
        }

        return View("ContactUs");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieScheme");
        TempData.Clear();
        return RedirectToAction("Login", "WeCinema");
    }

    [Authorize]
    [HttpPost]
    public IActionResult PaymentData(int CinemaId, int MovieId, string Date, string Time, string SeatList, DateTime NormalDate)
    {
        var paymentModel = new PaymentModel
        {
            CinemaId = CinemaId,
            MovieId = MovieId,
            Date = Date,
            Time = Time,
            SeatList = SeatList
        };

        return View("Payment", paymentModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> submitData(int totalPrice, string paymentMethod, int totalSeat, int showId, string seat)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var customer = await _movieService.GetUser(userEmail!);

        await _movieService.InsertData(totalPrice, paymentMethod, totalSeat, showId, seat, customer!.Customer_id);

        var model = await _movieService.GetAllMovies();
        return View("Index", model);
    }
}