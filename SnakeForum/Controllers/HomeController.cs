using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SnakeForum.Models;

namespace SnakeForum.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SnakeForum.Data;
    using SnakeForum.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly SnakeForumContext _context;

        public HomeController(SnakeForumContext context)
        {
            _context = context;
        }

        // ? Home Page - Show All Discussions
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
                .Include(d => d.Comments)  // Include comments to count them
                .OrderByDescending(d => d.CreateDate) // Sort by newest first
                .ToListAsync();

            return View(discussions);
        }

        // ? GetDiscussion Page - Show Discussion Details
        public async Task<IActionResult> GetDiscussion(int id)
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
    }
}
