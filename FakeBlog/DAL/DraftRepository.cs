using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeBlog.Models;
using FakeBlog.Controllers.Contracts;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FakeBlog.DAL
{
    public class DraftRepository : IDraftRepository
    {
        IDbConnection __blogConnection;

        public DraftRepository(IDbConnection blogConnection)
        {
            __blogConnection = blogConnection;
        }

        public void CreateDraft(string title, string contents, ApplicationUser author)
        {
            __blogConnection.Open();

            try
            {
                var CreateDraftCommand = __blogConnection.CreateCommand();
                CreateDraftCommand.CommandText = "Insert into Drafts(DraftTitle,DraftContents,DraftAuthor) values (@title, @contents, @authorId)";
                var titleParameter = new SqlParameter("title", SqlDbType.VarChar);
                titleParameter.Value = title;
                CreateDraftCommand.Parameters.Add(titleParameter);
                var contentsParameter = new SqlParameter("contents", SqlDbType.VarChar);
                contentsParameter.Value = contents;
                CreateDraftCommand.Parameters.Add(contentsParameter);
                var authorParameter = new SqlParameter("author", SqlDbType.VarChar);
                contentsParameter.Value = author.Id;
                CreateDraftCommand.Parameters.Add(contentsParameter);

                CreateDraftCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                __blogConnection.Close();
            }
        }

        public bool DeleteDraft(int draftId)
        {
            __blogConnection.Open();

            try
            {
                var deleteDraftCommand = __blogConnection.CreateCommand();
                deleteDraftCommand.CommandText = @"
                Delete
                From Drafts
                Where DraftId = @draftId";

                var draftIdParameter = new SqlParameter("draftId", SqlDbType.Int);
                draftIdParameter.Value = draftId;
                deleteDraftCommand.Parameters.Add(draftIdParameter);

                deleteDraftCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                __blogConnection.Close();
            }

            return false;
        }

        public bool EditDraft(int draftId, string newContents)
        {
            __blogConnection.Open();

            try
            {
                var editDraftCommand = __blogConnection.CreateCommand();
                editDraftCommand.CommandText = @"
                Update Drafts
                Set DraftContents = @contents
                Where DraftId = @draftId";

                var draftIdParameter = new SqlParameter("draftId", SqlDbType.Int);
                draftIdParameter.Value = draftId;
                editDraftCommand.Parameters.Add(draftIdParameter);
                var contentsParameter = new SqlParameter("contents", SqlDbType.VarChar);
                contentsParameter.Value = newContents;
                editDraftCommand.Parameters.Add(contentsParameter);

                editDraftCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                __blogConnection.Close();
            }
            return false;
        }

        

        public void PublishDraft(int draftId)
        {
            



        }

        public List<Draft> ReturnDrafts(string authorId)
        {
            __blogConnection.Open();

            try
            {
                var returnDraftsCommand = __blogConnection.CreateCommand();
                returnDraftsCommand.CommandText = @"
                    Select DraftId, DraftTitle, DraftContents, DraftDate, DraftAuthor
                    From Drafts
                    Where DraftAuthor_Id = @authorId";
                var authorIdParameter = new SqlParameter("authorId", SqlDbType.VarChar);
                authorIdParameter.Value = authorId;
                returnDraftsCommand.Parameters.Add(authorIdParameter);

                var reader = returnDraftsCommand.ExecuteReader();

                var drafts = new List<Draft>();
                while (reader.Read())
                {
                    var draft = new Draft
                    {
                        DraftId = reader.GetInt32(0),
                        DraftTitle = reader.GetString(1),
                        DraftContents = reader.GetString(2),
                        DraftDate = reader.GetDateTime(3),
                        DraftAuthor = new ApplicationUser { Id = reader.GetString(4) }
                    };
                    drafts.Add(draft);
                }
                return drafts;
            }
            catch (Exception ex) { }
            finally
            {
                __blogConnection.Close();
            }

            return new List<Draft>();
        }

        

        public Draft ReturnSingleDraft( int draftId)
        {
            __blogConnection.Open();

            try
            {
                var returnSingleDraftCommand = __blogConnection.CreateCommand();
                returnSingleDraftCommand.CommandText = @"
                    SELECT DraftId, DraftTitle, DraftContents, DraftDate, DraftAuthor 
                    FROM Drafts
                    WHERE draftId = @draftId";
                var DraftIdParam = new SqlParameter("draftId", SqlDbType.Int);
                DraftIdParam.Value = draftId;
                returnSingleDraftCommand.Parameters.Add(DraftIdParam);

                var reader = returnSingleDraftCommand.ExecuteReader();

                if (reader.Read())
                {
                    var draft = new Draft
                    {
                        DraftId = reader.GetInt32(0),
                        DraftTitle = reader.GetString(1),
                        DraftContents = reader.GetString(2),
                        DraftDate = reader.GetDateTime(3),
                        DraftAuthor = new ApplicationUser { Id = reader.GetString(4) }
                    };
                    return draft;
                }
            }
            catch (Exception ex) { }
            finally
            {
                __blogConnection.Close();
            }

            return null;
        }

        
    }
}