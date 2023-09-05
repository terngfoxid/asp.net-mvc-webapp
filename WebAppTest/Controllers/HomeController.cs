using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebAppTest.Models;

namespace WebAppTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            db dbop = new db();
            HomePageData homePageData = await dbop.GetHomePageData();
            return View(homePageData);
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

        //Register
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync([Bind] User user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                if (ModelState.IsValid)
                {
                    db dbop = new db();
                    string res = await dbop.Register(user);
                    if (res != null)
                    {
                        TempData["msg"] = res;
                        if (res.Equals("Registration Completed")) {
                            HttpContext.Session.SetString("UserName", user.UserName);
                            HttpContext.Session.SetString("FirstName", user.FirstName);
                            HttpContext.Session.SetString("LastName", user.LastName);
                            HttpContext.Session.SetString("Email", user.Email);
                            HttpContext.Session.SetString("UserRole", "m");
                        }
                    }
                    else
                    {
                        TempData["msg"] = "res is null";
                    }
                }
            }
            return View(user);
        }

        //Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([Bind] User user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                if (user.UserName == null || user.Password == null) {
                    TempData["msg"] = "Please enter your Username and Password";
                    return View(user);
                }
                db dbop = new db();
                string res = await dbop.Login(user);

                if (res != null)
                {
                    TempData["msg"] = res;
                    if (res.Equals("Login Completed")) {
                        HttpContext.Session.SetString("UserName", user.UserName);
                        HttpContext.Session.SetString("FirstName", user.FirstName);
                        HttpContext.Session.SetString("LastName", user.LastName);
                        HttpContext.Session.SetString("Email", user.Email);
                        HttpContext.Session.SetString("UserRole", user.UserRole);
                    }
                }
                else
                {
                    TempData["msg"] = "res is null";
                }
            }
            return View(user);
        }

        //----------------Test template
        public IActionResult TestTemplateEditor()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult User() {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                User user = new User();
                user.UserName = HttpContext.Session.GetString("UserName");
                user.FirstName = HttpContext.Session.GetString("FirstName");
                user.LastName = HttpContext.Session.GetString("LastName");
                user.Email = HttpContext.Session.GetString("Email");
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult UserBuddy()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                User user = new User();
                user.UserName = HttpContext.Session.GetString("UserName");
                user.FirstName = HttpContext.Session.GetString("FirstName");
                user.LastName = HttpContext.Session.GetString("LastName");
                user.Email = HttpContext.Session.GetString("Email");
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //----------------

        public ActionResult Logout()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("FirstName");
            HttpContext.Session.Remove("LastName");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("UserRole");

            return RedirectToAction("Login");
        }

        //*******
        public async Task<IActionResult> Article(int? id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (id == null)
                {
                    db dbop = new db();
                    Article[] allarticle = await dbop.GetAllArticleDesc();
                    return View(allarticle);
                }

                else
                {
                    TempData["ArticleID"] = id;
                    db dbop = new db();
                    Article[] article = new Article[1];
                    article[0] = await dbop.GetArticle(id);
                    if (article[0] == null) return NotFound();
                    return View(article);
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // GET: Home/Picture/5
        public async Task<IActionResult> Picture(int? id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (id == null)
                {
                    db dbop = new db();
                    Banner[] allbanner = await dbop.GetAllBanner();
                    return View(allbanner);
                }

                else
                {
                    TempData["PictureID"] = id;
                    db dbop = new db();
                    Banner[] banner = new Banner[1];
                    banner[0] = await dbop.GetBanner(id);
                    if (banner[0] == null) return NotFound();
                    return View(banner);
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}