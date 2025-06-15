using System.ComponentModel.DataAnnotations;

namespace Blogify.AdminApi.Models.ViewModels
{
    public class ArticleViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<Category> CategoryList { get; set; }

        public int? Id { get; set; }

        public string Excerpt { get; set; }

        public string Author { get; set; }

        [Required]
        public int Category { get; set; }

        public string Tags { get; set; }

        [Range(1, int.MaxValue)]
        public int ReadTime { get; set; } = 5;

        [Url]
        public string Image { get; set; }

        public string Status { get; set; } = "draft";

        public bool Featured { get; set; }
    }
}
