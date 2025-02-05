using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnakeForm.Models;
using SnakeForum.Data;

namespace SnakeForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly SnakeForumContext _context;

        public CommentsController(SnakeForumContext context)
        {
            _context = context;
        }

        // ✅ GET: Comments/CreateComment
        public IActionResult Create(int Id)
        {
            var comment = new Comment
            {
                DiscussionId = Id
            };

            return View(comment);
        }

        // ✅ POST: Comments/CreateComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Content, Author, DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CommentDate = DateTime.Now;

                // ✅ Ensure you're using the correct DbSet
                _context.Comment.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }

            return View(comment);
        }
    }
}
