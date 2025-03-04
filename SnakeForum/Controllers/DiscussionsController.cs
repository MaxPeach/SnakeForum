using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnakeForm.Models;
using SnakeForum.Data;

[Authorize]
public class DiscussionsController : Controller
{
    private readonly SnakeForumContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DiscussionsController(SnakeForumContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Discussions
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User); // Get the current logged-in user's ID
        var discussions = await _context.Discussion.Where(d => d.ApplicationUserId == userId).ToListAsync(); // Only fetch discussions owned by the current user
        return View(discussions);
    }

    // GET: Discussions/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var discussion = await _context.Discussion
            .Include(d => d.Comments) // Include related comments
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
    }

    // GET: Discussions/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Discussions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFile,CreateDate")] Discussion discussion)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User); // Get the current logged-in user's ID
            discussion.ApplicationUserId = userId; // Save the user's ID with the discussion

            // Rename the uploaded file to a unique filename (GUID)
            discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile?.FileName);

            _context.Add(discussion);
            await _context.SaveChangesAsync();

            // Save the uploaded file after saving the discussion to the database
            if (discussion.ImageFile != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(fileStream);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        return View(discussion);
    }

    // GET: Discussions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var discussion = await _context.Discussion.FindAsync(id);
        if (discussion == null || discussion.ApplicationUserId != _userManager.GetUserId(User)) // Check if the current user owns the discussion
        {
            return Forbid(); // Return 403 Forbidden if the user is not the owner
        }

        return View(discussion);
    }

    // POST: Discussions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename,CreateDate")] Discussion discussion)
    {
        if (id != discussion.DiscussionId)
        {
            return NotFound();
        }

        if (discussion.ApplicationUserId != _userManager.GetUserId(User)) // Ensure the current user owns the discussion
        {
            return Forbid(); // Return 403 Forbidden if the user is not the owner
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(discussion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscussionExists(discussion.DiscussionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(discussion);
    }

    // GET: Discussions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var discussion = await _context.Discussion
            .FirstOrDefaultAsync(m => m.DiscussionId == id);
        if (discussion == null || discussion.ApplicationUserId != _userManager.GetUserId(User)) // Ensure the current user owns the discussion
        {
            return Forbid(); // Return 403 Forbidden if the user is not the owner
        }

        return View(discussion);
    }

    // POST: Discussions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var discussion = await _context.Discussion.FindAsync(id);
        if (discussion != null)
        {
            _context.Discussion.Remove(discussion);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool DiscussionExists(int id)
    {
        return _context.Discussion.Any(e => e.DiscussionId == id);
    }
}
