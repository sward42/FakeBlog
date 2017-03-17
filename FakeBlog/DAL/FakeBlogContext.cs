using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FakeBlog.DAL
{
    public class FakeBlogContext : ApplicationDbContext
    {
        public virtual DbSet<Draft> Drafts { get; set; }
        public virtual DbSet<PublishedPost> PublishedPosts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
    }
}