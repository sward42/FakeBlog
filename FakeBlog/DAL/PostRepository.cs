using FakeBlog.Controllers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeBlog.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FakeBlog.DAL
{
    public class PostRepository : IPostRepository
    {

        IDbConnection _trelloConnection;

        public PostRepository(IDbConnection trelloConnection)
        {
            _trelloConnection = trelloConnection
        }

        public void AddPost(string title, string contents, ApplicationUser author)
        {
            _trelloConnection.Open();

            try
            {
                var AddPostCommand = _trelloConnection.CreateCommand();
                AddPostCommand.CommandText = "Insert into PublishedPosts(Title,Contents,PostAuthor) values (@title, @contents, @authorId)";
                var titleParameter = new SqlParameter("title", SqlDbType.VarChar);
                titleParameter.Value = title;
                AddPostCommand.Parameters.Add(titleParameter);
                var contentsParameter = new SqlParameter("contents", SqlDbType.VarChar);
                contentsParameter.Value = contents;
                AddPostCommand.Parameters.Add(contentsParameter);
                var authorParameter = new SqlParameter("author", SqlDbType.VarChar);
                contentsParameter.Value = author.Id;
                AddPostCommand.Parameters.Add(contentsParameter);

                AddPostCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }
        }

        public bool DeletePost(int postId)
        {
            _trelloConnection.Open();

            try
            {
                var deletePostCommand = _trelloConnection.CreateCommand();
                deletePostCommand.CommandText = @"
                Delete
                From PublishPosts
                Where PublishedPostId = @postId";

                var postIdParameter = new SqlParameter("postId", SqlDbType.Int);
                postIdParameter.Value = postId;
                deletePostCommand.Parameters.Add(postIdParameter);

                deletePostCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }

            return false;
        }

        public bool EditPost(int postId, string newContents)
        {
            _trelloConnection.Open();

            try
            {
                var editPostCommand = _trelloConnection.CreateCommand();
                editPostCommand.CommandText = @"
                Update PublishedPosts
                Set Contents = @contents
                Where PublishedPostId = @postId";

                var postIdParameter = new SqlParameter("postId", SqlDbType.Int);
                postIdParameter.Value = postId;
                editPostCommand.Parameters.Add(postIdParameter);
                var contentsParameter = new SqlParameter("contents", SqlDbType.VarChar);
                contentsParameter.Value = newContents;
                editPostCommand.Parameters.Add(contentsParameter);

                editPostCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _trelloConnection.Close();
            }
            return false;
        }

        public List<PublishedPost> ReturnPosts(string authorId)
        {
            _trelloConnection.Open();

            try
            {
                var returnPostsCommand = _trelloConnection.CreateCommand();
                returnPostsCommand.CommandText = @"
                    Select PublishedPostId, Title, Contents, PublishedDate, PostAuthor
                    From PublishedPosts
                    Where PostAuthor_Id = @authorId";
                var authorIdParameter = new SqlParameter("authorId", SqlDbType.VarChar);
                authorIdParameter.Value = authorId;
                returnPostsCommand.Parameters.Add(authorIdParameter);

                var reader = returnPostsCommand.ExecuteReader();

                var posts = new List<PublishedPost>();
                while (reader.Read())
                {
                    var post = new PublishedPost
                    {
                        PublishedPostId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Contents = reader.GetString(2),
                        PublishedDate = reader.GetDateTime(3),
                        PostAuthor = new ApplicationUser { Id = reader.GetString(4) }
                    };
                    posts.Add(post);
                }
                return posts;
            }
            catch (Exception ex) { }
            finally
            {
                _trelloConnection.Close();
            }

            return new List<PublishedPost>();
        }

        public PublishedPost ReturnSinglePost(int postId)
        {
            _trelloConnection.Open();

            try
            {
                var returnSinglePostCommand = _trelloConnection.CreateCommand();
                returnSinglePostCommand.CommandText = @"
                    SELECT PublishedPostId, Title, Contents, PublishedDate, PostAuthor 
                    FROM PublishedPosts
                    WHERE PublishedPostId = @postId";
                var PostIdParam = new SqlParameter("postId", SqlDbType.Int);
                PostIdParam.Value = postId;
                returnSinglePostCommand.Parameters.Add(PostIdParam);

                var reader = returnSinglePostCommand.ExecuteReader();

                if (reader.Read())
                {
                    var post = new PublishedPost
                    {
                        PublishedPostId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Contents = reader.GetString(2),
                        PublishedDate = reader.GetDateTime(3),
                        PostAuthor = new ApplicationUser { Id = reader.GetString(4) }
                    };
                    return post;
                }
            }
            catch (Exception ex) { }
            finally
            {
                _trelloConnection.Close();
            }

            return null;
        }
    }
}