using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SnakeForm.Models;
using SnakeForum.Data;

[Authorize]
public class CommentsController : Controller
{
    private readonly SnakeForumContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentsController(SnakeForumContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Comments/CreateComment
    public IActionResult Create(int Id)
    {
        var comment = new Comment
        {
            DiscussionId = Id
        };

        return View(comment);
    }

    // POST: Comments/CreateComment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateComment([Bind("Content, Author, DiscussionId")] Comment comment)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User); // Get the current logged-in user's ID
            comment.ApplicationUserId = userId; // Save the user's ID with the comment
            comment.CommentDate = DateTime.Now;

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
        }

        return View(comment);
    }
}
