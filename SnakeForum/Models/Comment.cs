using Humanizer;
using Microsoft.EntityFrameworkCore.Migrations;
using SnakeForum.Data;

namespace SnakeForm.Models
{
    public class Comment
    {
        //primary key
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; }

        // Foreign Key
        public int DiscussionId { get; set; }

        // Navigation Property
        public Discussion? Discussion { get; set; } //make nullable

        // Foreign key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; } // nullable!!!


    }

}
