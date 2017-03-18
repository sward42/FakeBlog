using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeBlog.DAL;
using Moq;
using FakeBlog.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace FakeBlog.Tests.DAL
{
    [TestClass]
    public class FakeBlogRepoTests
    {
        public Mock<FakeBlogContext> fakeContext { get; set; }
        public FakeBlogRepository repo { get; set; }
        public Mock<DbSet<PublishedPost>> mockPostSet { get; set; }
        public Mock<DbSet<Draft>> mockDraftSet { get; set; }
        public IQueryable<PublishedPost> query_posts { get; set; }
        public IQueryable<Draft> query_drafts { get; set; }
        public List<PublishedPost> fake_post_table { get; set; }
        public List<Draft> fake_draft_table { get; set; }
        public ApplicationUser john { get; set; }
        public ApplicationUser sammy { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fake_post_table = new List<PublishedPost>();
            fakeContext = new Mock<FakeBlogContext>();
            mockPostSet = new Mock<DbSet<PublishedPost>>();
            repo = new FakeBlogRepository(fakeContext.Object);

            john = new ApplicationUser { Id = "john-id-1" };
            sammy = new ApplicationUser { Id = "sammy-id-1" };
        }

        public void CreateFakePostDatabase()
        {
            query_posts = fake_post_table.AsQueryable(); 

            mockPostSet.As<IQueryable<PublishedPost>>().Setup(b => b.Provider).Returns(query_posts.Provider);
            mockPostSet.As<IQueryable<PublishedPost>>().Setup(b => b.Expression).Returns(query_posts.Expression);
            mockPostSet.As<IQueryable<PublishedPost>>().Setup(b => b.ElementType).Returns(query_posts.ElementType);
            mockPostSet.As<IQueryable<PublishedPost>>().Setup(b => b.GetEnumerator()).Returns(() => query_posts.GetEnumerator());

            mockPostSet.Setup(b => b.Add(It.IsAny<PublishedPost>())).Callback((PublishedPost post) => fake_post_table.Add(post));

            mockPostSet.Setup(b => b.Remove(It.IsAny<PublishedPost>())).Callback((PublishedPost post) => fake_post_table.Remove(post));

            fakeContext.Setup(c => c.PublishedPosts).Returns(mockPostSet.Object);
        }

        public void CreateFakeDraftDatabase()
        {
            query_drafts = fake_draft_table.AsQueryable();

            mockDraftSet.As<IQueryable<Draft>>().Setup(b => b.Provider).Returns(query_drafts.Provider);
            mockDraftSet.As<IQueryable<Draft>>().Setup(b => b.Expression).Returns(query_drafts.Expression);
            mockDraftSet.As<IQueryable<Draft>>().Setup(b => b.ElementType).Returns(query_drafts.ElementType);
            mockDraftSet.As<IQueryable<Draft>>().Setup(b => b.GetEnumerator()).Returns(() => query_drafts.GetEnumerator());

            mockDraftSet.Setup(b => b.Add(It.IsAny<Draft>())).Callback((Draft draft) => fake_draft_table.Add(draft));

            mockDraftSet.Setup(b => b.Remove(It.IsAny<Draft>())).Callback((Draft draft) => fake_draft_table.Remove(draft));

            fakeContext.Setup(c => c.Drafts).Returns(mockDraftSet.Object);
        }

        [TestMethod]
        public void EnsureICanCreateInstanceofRepo()
        {
            FakeBlogRepository repo = new FakeBlogRepository();

            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void EnsureIHaveNotNullContext()
        {
            FakeBlogRepository repo = new FakeBlogRepository();

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanInjectContextInstance()
        {
            FakeBlogContext context = new FakeBlogContext();
            FakeBlogRepository repo = new FakeBlogRepository(context);

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanAddPublishedPost()
        {
            CreateFakePostDatabase();

            ApplicationUser a_user = new ApplicationUser
            {
                Id = "my-user-id",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            repo.AddPost("First Post", "Questions I have", a_user);

            Assert.AreEqual(1, repo.Context.PublishedPosts.Count());
        }

        [TestMethod]
        public void EnsureICanCreateDraft()
        {
            CreateFakeDraftDatabase();

            ApplicationUser a_user = new ApplicationUser
            {
                Id = "my-user-id",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            repo.CreateDraft("The First", "Questions and Answers", a_user);

            Assert.AreEqual(1, repo.Context.Drafts.Count());
        }

        [TestMethod]
        public void EnsureICanDeleteDraft()
        {
            fake_draft_table.Add(new Draft { DraftId = 1, DraftTitle = "One", DraftContents = "blah", DraftAuthor = sammy });
            fake_draft_table.Add(new Draft { DraftId = 2, DraftTitle = "Two", DraftContents = "yuck", DraftAuthor = sammy });
            CreateFakeDraftDatabase();

            int expectedResult = 1;
            repo.DeleteDraft(2);
            int actualResult = repo.Context.Drafts.Count();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void EnsureICanDeletePost()
        {
            fake_post_table.Add(new PublishedPost { PublishedPostId = 1, Title = "One", PostAuthor = sammy });
            fake_post_table.Add(new PublishedPost { PublishedPostId = 2, Title = "Two", PostAuthor = sammy });
            CreateFakePostDatabase();

            int expectedResult = 1;
            repo.DeletePost(2);
            int actualResult = repo.Context.PublishedPosts.Count();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void EnsureICanEditDraft()
        {

        }

        [TestMethod]
        public void EnsureICanEditPost()
        {
            fake_post_table.Add(new PublishedPost { PublishedPostId = 1, Title = "One", Contents = "blah", PostAuthor = sammy });
            CreateFakePostDatabase();

            repo.EditPost(1, "blah, blah, blah");

            string expectedResult = "blah, blah, blah";
            var updatedPost = repo.ReturnSinglePost(1);
            string actualResult = updatedPost.Contents;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
