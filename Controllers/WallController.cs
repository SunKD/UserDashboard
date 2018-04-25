using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class WallController : Controller
    {
        private DashboardContext _context;

        public WallController(DashboardContext context)
        {
            _context = context;
        }
        [HttpGet]


        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            if( TempData["Error"] != null){
                ViewBag.Error= TempData["Error"];
            }

            ViewBag.AllIdea = _context.Ideas.Include(p=>p.User).OrderByDescending(e=>e.liked).ToList();
            ViewBag.AllLikes = _context.Likes.ToList();
            ViewBag.CurrentUserID = (int)HttpContext.Session.GetInt32("CurrentUserID");
            ViewBag.CurrentUser = _context.Users.SingleOrDefault(e => e.UserID == (int)HttpContext.Session.GetInt32("CurrentUserID"));
            
            return View("Dashboard");
        }

        [HttpPost]
        [Route("PostIdea")]
        public IActionResult PostIdea(string idea)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            Idea newIdea = new Idea
            {
                UserID = (int)HttpContext.Session.GetInt32("CurrentUserID"),
                UserIdea = idea
            };
            _context.Ideas.Add(newIdea);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("delete/{IdeaID}")]
        public IActionResult Delete(int IdeaID)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            Idea deleteIdea = _context.Ideas.Where(e => e.IdeaID == IdeaID).SingleOrDefault();
            if(deleteIdea.UserID != (int)HttpContext.Session.GetInt32("CurrentUserID")){
                TempData["Error"]="You can't delete that!";
                return RedirectToAction("Dashboard");
            }
            _context.Ideas.Remove(deleteIdea);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("like/{IdeaID}/{UserID}")]
        public IActionResult Like(int IdeaID, int UserID)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            Idea likingIdea = _context.Ideas.Where(e => e.IdeaID == IdeaID).SingleOrDefault();
            likingIdea.liked++;

            Like newLike = new Like
            {
                IdeaID = IdeaID,
                UserID = UserID
            };
            _context.Likes.Add(newLike);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        //
        [HttpGet]
        [Route("idea/{IdeaID}")]
        public IActionResult Idea(int IdeaID)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            Idea Idea = _context.Ideas.Where(e => e.IdeaID == IdeaID).Include(p=>p.User).SingleOrDefault();
            var allLiked = _context.Likes.Include(e => e.Idea).Include(r => r.User).Where(p => p.IdeaID == IdeaID).Distinct().ToList();
            
            ViewBag.allLiked = allLiked;
            ViewBag.Idea = Idea;
            ViewBag.User = Idea.User;
            ViewBag.CurrentUserID = (int)HttpContext.Session.GetInt32("CurrentUserID");

            return View("Idea");
        }
        [HttpGet]
        [Route("users/{UserID}")]
        public IActionResult ShowUser(int UserID)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            User user = _context.Users.SingleOrDefault(e => e.UserID == UserID);
            ViewBag.User = user;
            int likes = _context.Likes.Where(e => e.UserID == UserID).Count();
            ViewBag.Likes = likes;
            int posted = _context.Ideas.Where(e => e.UserID == UserID).Count();
            ViewBag.Posted = posted;
            ViewBag.CurrentUserID = (int)HttpContext.Session.GetInt32("CurrentUserID");

            return View("Profile");
        }


        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public bool CheckLoggedIn()
        {
            if (HttpContext.Session.GetInt32("CurrentUserID") == null)
            {
                return false;
            }
            return true;
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
