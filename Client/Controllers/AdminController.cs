using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Message = "You are an Admin!";
            }
            else if (User.IsInRole("Manager"))
            {
                ViewBag.Message = "You are a Manager!";
            }
            else
            {
                ViewBag.Message = "You do not have the required roles.";
            }

            return View();
        }

        // このアクションは "Admin" ロールを持つユーザーだけがアクセスできる
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return Content("Admin");
        }

        // このアクションは "Manager" ロールを持つユーザーだけがアクセスできる
        [Authorize(Roles = "Manager")]
        public IActionResult Manager()
        {
            return Content("Manager");
        }

        // このアクションは "Admin" または "Manager" ロールを持つユーザーがアクセスできる
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult AdminOrManager()
        {
            return Content("Admin, Manager");
        }
    }
}
