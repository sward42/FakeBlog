using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeBlog.Models;

namespace FakeBlog.DAL
{
    public class FakeBlogRepository : IRepository
    {
        public FakeBlogContext Context {get; set;}

        public FakeBlogRepository()
        {
            Context = new FakeBlogContext();
        }

        public FakeBlogRepository(FakeBlogContext context)
        {
            Context = context;
        }

        public void AddPost(string title, string contents, ApplicationUser author)
        {
            PublishedPost post = new PublishedPost { Title = title, Contents = contents, PostAuthor = author, PublishedDate = new DateTime()};
            Context.PublishedPosts.Add(post);
            Context.SaveChanges();
        }

        public void CreateDraft(string title, string contents, ApplicationUser author)
        {
            Draft draft = new Draft { DraftTitle = title, DraftContents = contents, DraftAuthor = author, DraftDate = new DateTime() };
            Context.Drafts.Add(draft);
            Context.SaveChanges();
        }

        public bool DeleteDraft(int draftId)
        {
            Draft found_draft = ReturnSingleDraft(draftId);
            if (found_draft != null)
            {
                Context.Drafts.Remove(found_draft);
                Context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeletePost(int postId)
        {
            PublishedPost found_post = ReturnSinglePost(postId);
            if (found_post != null)
            {
                Context.PublishedPosts.Remove(found_post);
                Context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool EditDraft(int draftId, string newContents)
        {
            Draft found_draft = ReturnSingleDraft(draftId);
            if (found_draft != null)
            {
                CreateDraft(found_draft.DraftTitle, newContents, found_draft.DraftAuthor);
                return true;
            }
            return false;
        }

        public bool EditPost(int postId, string newContents)
        {
            PublishedPost found_post = ReturnSinglePost(postId);
            if (found_post != null)
            {
                AddPost(found_post.Title, newContents, found_post.PostAuthor);
                return true;
            }
            return false;
        }

        public void PublishDraft(int draftId)
        {
            Draft found_draft = ReturnSingleDraft(draftId);
            if (found_draft != null)
            {
                AddPost(found_draft.DraftTitle, found_draft.DraftContents, found_draft.DraftAuthor); 
            }
            
        }

        public List<Draft> ReturnDrafts(string authorId)
        {
            List<Draft> userDrafts = Context.Drafts.Where(a => a.DraftAuthor.Id == authorId).ToList();
            return userDrafts;
        }

        public List<PublishedPost> ReturnPosts(string authorId)
        {
            List<PublishedPost> userPosts = Context.PublishedPosts.Where(a => a.PostAuthor.Id == authorId).ToList();
            return userPosts;
        }

        public Draft ReturnSingleDraft( int draftId)
        {
            Draft selectedDraft = Context.Drafts.FirstOrDefault(d => d.DraftId == draftId);
            return selectedDraft;
        }

        public PublishedPost ReturnSinglePost(int postId)
        {
            PublishedPost selectedPost = Context.PublishedPosts.FirstOrDefault(d => d.PublishedPostId == postId);
            return selectedPost;
        }
    }
}