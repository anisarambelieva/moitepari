using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoitePari.Web.Models;

namespace MoitePari.Web.Controllers
{
    /// <summary>
    /// Default controller for basic navigation and error handling in the web application.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class with logging support.
        /// </summary>
        /// <param name="logger">An instance of a logger for tracking application behavior.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the home page of the application.
        /// </summary>
        /// <returns>The main index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>The privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Handles errors and displays an error page with diagnostic information.
        /// </summary>
        /// <returns>A view showing the current request's error information.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
