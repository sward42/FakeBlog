using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBlog.Controllers.Contracts
{
    interface IPostRepository
    {
        List<PublishedPost> ReturnPosts(string authorId);
        PublishedPost ReturnSinglePost(int postId);

        void AddPost(string title, string contents, ApplicationUser author);

        bool DeletePost(int postId);

        bool EditPost(int postId, string newContents);
    }
}
