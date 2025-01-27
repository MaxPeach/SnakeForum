namespace SnakeForm.Models
{
    public class Comment
    {
        //primary key
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CommentDate { get; set; }

        // Foreign Key
        public int DiscussionId { get; set; }

        // Navigation Property
        public Discussion? Discussion { get; set; } //make nullable
    }

}
