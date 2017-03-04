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

        [MaxLength(60)]
        public string Title { get; set; }

        public string Contents { get; set; }

        public DateTime PublishedDate { get; set; }

        public Draft DraftRef { get; set; }

        public Author PostAuthor { get; set; }

    }
}