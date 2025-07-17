using Microsoft.AspNetCore.Mvc;

namespace SessionState.Controllers
{
    public class TestSessionController : Controller
    {
        public IActionResult Index()
        {
            int stepNum = HttpContext.Session.GetInt32("CurrentStep") ?? 0;
            stepNum += 1;
            HttpContext.Session.SetInt32("CurrentStep", stepNum);

            // Change the type of 'letters' to nullable string
            string? letters = HttpContext.Session.GetString("Letters");

            if (string.IsNullOrEmpty(letters))
            {
                letters = "A"; // First visit
            }
            else
            {
                // Get the last character and determine the next one
                char lastChar = letters[^1]; // ^1 = last index
                if (lastChar < 'Z') // Limit to A–Z
                {
                    char nextChar = (char)(lastChar + 1);
                    letters += nextChar;
                }
            }

            HttpContext.Session.SetString("Letters", letters);

            // Pass both values to the view (optional)
            //ViewBag.Num = num;
           // ViewBag.Letters = letters;

            return View();
        }

        // POST: /TestSession/ClearNum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearNum()
        {
            // Remove only the "num" key from session
            HttpContext.Session.Remove("CurrentStep");

            // Optional: Use TempData to show a confirmation message
            TempData["Message"] = "\"num\" session variable has been cleared.";

            // Redirect to Index to show message or updated UI
            return RedirectToAction("Index");
        }

        // POST: /TestSession/ClearSession
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear(); // Clear all session data
            TempData["Message"] = "Session cleared successfully.";
            return RedirectToAction("Index");
        }

    }
}
