using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakeBlog.DAL
{
    public interface IRepository
    {
        void CreateDraft(string title, string contents, ApplicationUser author);
        void PublishDraft(int draftId);
        void AddPost(string title, string contents, ApplicationUser author);

        bool DeletePost(int postId);
        bool DeleteDraft(int draftId);

        bool EditDraft(int draftId, string newContents);
        bool EditPost(int postId, string newContents);

        List<Draft> ReturnDrafts(string authorId);
        Draft ReturnSingleDraft(string authorId, int draftId);
        List<PublishedPost> ReturnPosts(string authorId);
        PublishedPost ReturnSinglePost(string authorId, string postTitle);
    }
}