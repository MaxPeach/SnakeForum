namespace SnakeForm.Models
{
    public class Discussion
    {
        //primary key

        public int DiscussionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageFilename { get; set; }
        public DateTime CreateDate { get; set; }

        // Navigation Property
        public List<Comment>? Comments { get; set; } //make nullable
  
}
}
