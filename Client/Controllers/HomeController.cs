using Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // デフォルトのIndexアクション
        public IActionResult Index()
        {
            // ユーザーが認証されているか確認
            if (User.Identity.IsAuthenticated)
            {
                // ユーザー名を取得
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.Message = $"ようこそ、{userName}さん！";
            }
            else
            {
                ViewBag.Message = "ログインしていません。ログインしてください。";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ログインアクション
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // 認証のチャレンジを開始
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "OpenIdConnect");
        }

        // ログアウトアクション
        [HttpPost]
        public IActionResult Logout()
        {
            var callbackUrl = Url.Action("Index", "Home");

            return SignOut(new AuthenticationProperties
            {
                RedirectUri = callbackUrl // ログアウト後のリダイレクト先
            }, "Cookies", "OpenIdConnect");
        }
    }
}
