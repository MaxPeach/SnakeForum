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

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            var comment = new Comment
            {
                DiscussionId = discussionId
            };

            return View(comment);
        }


        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Author,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CommentDate = DateTime.Now; // Set the timestamp
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to the Get Discussion page
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }
            return View(comment);
        }
    }
}
