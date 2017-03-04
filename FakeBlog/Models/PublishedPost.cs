using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeBlog.Models
{
    public class PublishedPost
    {
        [Key]
        public int PublishedPostId { get; set; }

        [MinLength(3)]
        [Required]
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime PublishedDate { get; set; } // notNull by default
        public DateTime CreatedDate { get; set; }
        public bool IsDraft { get; set; }
        public string URL { get; set; }
    }
}