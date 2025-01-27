using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnakeForm.Models;

namespace SnakeForum.Data
{
    public class SnakeForumContext : DbContext
    {
        public SnakeForumContext (DbContextOptions<SnakeForumContext> options)
            : base(options)
        {
        }

        public DbSet<SnakeForm.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<SnakeForm.Models.Comment> Comment { get; set; } = default!;
    }
}
