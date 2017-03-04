using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeBlog.Models
{
    public class Draft
    {
        [Key]
        public int DraftId { get; set; }

        [MaxLength(60)]
        public string DraftTitle { get; set; }

        public string DraftContents { get; set; }

        public DateTime DraftDate { get; set; }

        public PublishedPost PostRef { get; set; }

    }
}