using Microsoft.AspNetCore.Mvc;
using SessionState.Models;
using SessionState; // For SessionExtensions

namespace SessionState.Controllers
{
    public class TestSessionController : Controller
    {
        public IActionResult Index()
        {
            int stepNum = HttpContext.Session.GetInt32("CurrentStep") ?? 0;
            stepNum += 1;
            HttpContext.Session.SetInt32("CurrentStep", stepNum);

            string? letters = HttpContext.Session.GetString("Letters");

            if (string.IsNullOrEmpty(letters))
            {
                letters = "A";
            }
            else
            {
                char lastChar = letters[^1];
                if (lastChar < 'Z')
                {
                    char nextChar = (char)(lastChar + 1);
                    letters += nextChar;
                }
            }

            HttpContext.Session.SetString("Letters", letters);

            // Car session logic: only display what's in session, or empty if not set
            var car = HttpContext.Session.GetObject<Car>("Car");
            ViewBag.Car = car ?? new Car();

            return View();
        }

        // POST: /TestSession/ResetCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetCar()
        {
            HttpContext.Session.Remove("Car");
            TempData["Message"] = "Car object has been reset.";
            return RedirectToAction("Index");
        }

        // POST: /TestSession/ClearSession
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            TempData["Message"] = "Session cleared successfully.";
            return RedirectToAction("Index");
        }

        // POST: /TestSession/SetCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetCar()
        {
            var car = new Car
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Color = "Blue"
            };
            HttpContext.Session.SetObject("Car", car);
            TempData["Message"] = "Car object has been set.";
            return RedirectToAction("Index");
        }

        // POST: /TestSession/ClearNum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearNum()
        {
            HttpContext.Session.Remove("CurrentStep");
            TempData["Message"] = "\"num\" session variable has been cleared.";
            return RedirectToAction("Index");
        }
    }
}
