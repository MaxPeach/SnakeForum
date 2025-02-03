using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnakeForm.Models
{
    public class Discussion
    {
        //primary key

        public int DiscussionId { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageFilename { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? ImageFile { get; set; } // nullable!


        public DateTime CreateDate { get; set; }

        // Navigation Property
        public List<Comment>? Comments { get; set; } //make nullable
  
}
}
