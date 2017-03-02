using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeBlog.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        [MaxLength(60)]
        public string FullName { get; set; }

        [MaxLength(60)]
        public string Email { get; set; }

        public ApplicationUser BaseUser { get; set; }

        public List<Draft> Drafts { get; set; }

        public List<PublishedPost> PublishedPosts { get; set; }
    }
}