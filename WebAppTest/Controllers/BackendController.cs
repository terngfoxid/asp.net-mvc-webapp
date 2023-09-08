using MessagePack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.Models;

namespace WebAppTest.Controllers
{
    public class BackendController : Controller
    {
        // GET: BackendController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a")) {
                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }

        //Picture----------------------------------------------------
        public async Task<IActionResult> PictureManager(int? id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing
                    if (id == null)
                    {
                        db dbop = new db();
                        Banner[] allbanner = await dbop.GetAllBanner();
                        return View(allbanner);
                    }
                    else
                    {
                        db dbop = new db();
                        Banner[] banner = new Banner[1];
                        TempData["PictureManagerID"] = id;
                        banner[0] = await dbop.GetBanner(id);
                        if (banner[0] == null) return NotFound();
                        return View(banner);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //Picture-Banner------------------------------------------------------

        public ActionResult BannerCreate() {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing
                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BannerCreateAsync(BannerCreate banner)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (ModelState.IsValid)
                    {
                        //db connect and insert
                        db dbop = new db();
                        string res = await dbop.InsertNewBanner(banner);
                        if (res != null)
                        {
                            TempData["BannerCreate"] = res;
                            if (res.Equals("Create Banner Completed"))
                            {
                                return View();
                            }
                            else return View(banner);
                        }
                        else
                        {
                            TempData["BannerCreate"] = "res is null";
                            return View(banner);
                        }
                    }
                    else
                    {

                        //if (Image == null || Image.Length == 0) TempData["BannerCreate"] = "Please input upload file";
                        return View(banner);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<ActionResult> BannerEdit(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        BannerEdit banner = await dbop.GetBannerEdit(id);
                        TempData["BannerManagerID"] = id;
                        if (banner == null) return NotFound();
                        return View(banner);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BannerEditAsync(int id, BannerEdit banner)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            db dbop = new db();
                            if (await dbop.UpdateBanner(id, banner))
                            {
                                TempData["BannerUpdate"] = "Banner update success";

                            }
                            else
                            {
                                TempData["BannerUpdate"] = "Banner update Failed";
                            }
                            TempData["BannerManagerID"] = id;
                            return View(banner);
                        }
                        else
                        {
                            db dbop = new db();
                            banner = await dbop.GetBannerEdit(id);
                            return View(banner);
                        }
                        /*db dbop = new db();
                        Article article = await dbop.GetArticle(id);
                        TempData["ArticleManagerID"] = id;
                        if (article == null) return NotFound();
                        return View(article);*/
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<ActionResult> BannerDelete(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        Banner banner = await dbop.GetBanner(id);
                        TempData["BannerManagerID"] = id;
                        if (banner == null) return NotFound();
                        return View(banner);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BannerDeleteAsync(int id, Banner banner)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        if (await dbop.DeleteBanner(id))
                        {
                            TempData["BannerDelete"] = "Delete banner success";                          
                        }
                        else
                        {
                            TempData["BannerDelete"] = "Delete banner failed";
                        }
                        return View(banner);

                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        //------------Sub-Picture-------------------------------------

        public async Task<IActionResult> SubPictureManager(int? id) {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        Banner banner = await dbop.GetBanner(id);
                        if (banner == null) return NotFound();
                        TempData["PictureManagerID"] = id;
                        return View(banner);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubPictureCreate(int? id, IFormFile? image) {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        if (image == null || image.Length == 0) {
                            TempData["SubPictureMSG"] = "Create sub picture failed [No input file]";
                            TempData["nextroute"] = id;
                            return View();
                        }
                        db dbop = new db();
                        string res = await dbop.InsertNewSubPicture(id,image);
                        if (!res.Equals("Create Sub Picture Completed")) {
                            TempData["SubPictureMSG"] = res;
                            TempData["nextroute"] = id;
                            return View();
                        }
                        return RedirectToAction("SubPictureManager", "Backend", new { id = id });
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> SubPictureDelete(int? id,int? nextId) {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        if (await dbop.DeleteSubPicture(id))
                        {
                            return RedirectToAction("SubPictureManager", "Backend", new { id = nextId });
                        }
                        else
                        {
                            @TempData["nextroute"] = nextId;
                            TempData["SubPictureMSG"] = "Delete Sub picture failed";
                            return View();
                        }
                        
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //Article------------------------------------------------------
        public async Task<IActionResult> ArticleManager(int? id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing
                    if (id == null)
                    {
                        db dbop = new db();
                        Article[] allarticle = await dbop.GetAllArticle();
                        return View(allarticle);
                    }
                    else
                    {
                        db dbop = new db();
                        Article[] article = new Article[1];
                        TempData["ArticleManagerID"] = id;
                        article[0] = await dbop.GetArticle(id);
                        if (article[0] == null) return NotFound();
                        return View(article);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ArticleCreate() {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ArticleCreateAsync(Article article){
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (ModelState.IsValid)
                    {
                        //db connect and insert
                        db dbop = new db();
                        string res = await dbop.InsertNewArticle(article);
                        TempData["ArticleCreate"] = "ok";
                        if (res != null)
                        {
                            TempData["ArticleCreate"] = res;
                            if (res.Equals("Create Article Completed"))
                            {
                                return View();
                            }
                            else return View(article);
                        }
                        else
                        {
                            TempData["ArticleCreate"] = "res is null";
                            return View(article);
                        }
                    }
                    else
                    {
                        return View(article);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<ActionResult> ArticleEdit(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                            db dbop = new db();
                            Article article = await dbop.GetArticle(id);
                            TempData["ArticleManagerID"] = id;
                            if (article == null) return NotFound();
                            return View(article);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ArticleEditAsync(int id,Article article)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            db dbop = new db();
                            if (await dbop.UpdateArticle(id, article))
                            {
                                TempData["ArticleUpdate"] = "Article update success";

                            }
                            else
                            {
                                TempData["ArticleUpdate"] = "Article update Failed";
                            }
                            TempData["ArticleManagerID"] = id;
                            return View(article);
                        }
                        else { 
                            return View(article);
                        }
                            /*db dbop = new db();
                            Article article = await dbop.GetArticle(id);
                            TempData["ArticleManagerID"] = id;
                            if (article == null) return NotFound();
                            return View(article);*/
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<ActionResult> ArticleDelete(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        Article article = await dbop.GetArticle(id);
                        TempData["ArticleManagerID"] = id;
                        if (article == null) return NotFound();
                        return View(article);
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ArticleDeleteAsync(int id, Article article)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if (HttpContext.Session.GetString("UserRole").Equals("a"))
                {
                    //do some thing            
                    if (id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        db dbop = new db();
                        if (await dbop.DeleteArticle(id))
                        {
                            TempData["ArticleDelete"] = "Delete article success";
                            return View(article);
                        }
                        else {
                            TempData["ArticleDelete"] = "Delete article failed";
                            return View(article);
                        }
                        
                    }
                }
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        
    }
}
