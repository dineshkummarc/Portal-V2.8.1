using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RiversideInternet.WebSolution
{
	public class ForumDB
	{
		public static ForumPost GetPost(int postID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetPost", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = postID;

			conn.Open();
			
			SqlDataReader dr = cmd.ExecuteReader();

			// Should throw an exception here if dr.Read returns false
			dr.Read();
			ForumPost forumPost = PopulateForumPost(dr);
			forumPost.PostID = postID;

			dr.Close();
			conn.Close();

			return forumPost;
		}

		private static ForumPost PopulateForumPost(SqlDataReader dr)
		{
			ForumPost forumPost = new ForumPost();

			// Post specifics
			forumPost.Body			= Convert.ToString(dr["Body"]);
			forumPost.FlatSortOrder	= Convert.ToInt32(dr["FlatSortOrder"]);
			forumPost.Notify		= Convert.ToBoolean(dr["Notify"]);
			forumPost.ParentPostID	= Convert.ToInt32(dr["ParentPostID"]);
			forumPost.PostDate		= Convert.ToDateTime(dr["PostDate"]);
			forumPost.PostLevel		= Convert.ToInt32(dr["PostLevel"]);
			forumPost.RemoteAddr	= Convert.ToString(dr["RemoteAddr"]);
			forumPost.Subject		= Convert.ToString(dr["Subject"]);
			forumPost.ThreadID		= Convert.ToInt32(dr["ThreadID"]);
			forumPost.TreeSortOrder	= Convert.ToInt32(dr["TreeSortOrder"]);
			forumPost.FlatSortOrder	= Convert.ToInt32(dr["FlatSortOrder"]);

			forumPost.User = new User();

			// User specifics
			forumPost.User.Alias		= Convert.ToString(dr["Alias"]);
			forumPost.User.Avatar		= Convert.IsDBNull(dr["Avatar"]) ? string.Empty : Convert.ToString(dr["Avatar"]);
			forumPost.User.Email		= Convert.ToString(dr["Email"]);
			forumPost.User.PostCount	= Convert.ToInt32(dr["PostCount"]);
			forumPost.User.Password		= Convert.ToString(dr["Password"]);
			forumPost.User.UserID		= Convert.ToInt32(dr["UserID"]);
			forumPost.User.WebID		= Convert.ToInt32(dr["WebID"]);

			return forumPost;
		}

		public static int AddPostPinned(ForumPost forumPost, int forumID, DateTime pinnedDate)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_AddPost", conn);

			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@ParentPostID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@ForumID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@UserID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@RemoteAddr", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@Notify", SqlDbType.Bit, 1);
			cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 255);
			cmd.Parameters.Add("@Body", SqlDbType.Text);
			cmd.Parameters.Add("@PinnedDate", SqlDbType.DateTime);
			cmd.Parameters.Add("@PostDate", SqlDbType.DateTime);

			cmd.Parameters[0].Direction = ParameterDirection.Output;
			cmd.Parameters[1].Value = forumPost.ParentPostID;
			cmd.Parameters[2].Value = forumID;
			cmd.Parameters[3].Value = forumPost.User.UserID;
			cmd.Parameters[4].Value = forumPost.RemoteAddr;
			cmd.Parameters[5].Value = forumPost.Notify;
			cmd.Parameters[6].Value = forumPost.Subject;
			cmd.Parameters[7].Value = forumPost.Body;
			cmd.Parameters[8].Value = pinnedDate;
			cmd.Parameters[9].Value = DateTime.Now;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			forumPost.PostID = (int)cmd.Parameters[0].Value;

			return forumPost.PostID;
		}

		public static int AddPost(ForumPost forumPost, int forumID)
		{
			return AddPostPinned(forumPost, forumID, DateTime.Now);
		}

		public static void UpdatePost(ForumPost forumPost)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_UpdatePost", conn);

			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 255);
			cmd.Parameters.Add("@Body", SqlDbType.Text);
			cmd.Parameters.Add("@Notify", SqlDbType.Bit, 1);

			cmd.Parameters[0].Value = forumPost.PostID;
			cmd.Parameters[1].Value = forumPost.Subject;
			cmd.Parameters[2].Value = forumPost.Body;
			cmd.Parameters[3].Value = forumPost.Notify;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		public static void UpdatePostPinned(ForumPost forumPost, DateTime pinnedDate)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_UpdatePostPinned", conn);

			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 255);
			cmd.Parameters.Add("@Body", SqlDbType.Text);
			cmd.Parameters.Add("@Notify", SqlDbType.Bit, 1);
			cmd.Parameters.Add("@PinnedDate", SqlDbType.DateTime);

			cmd.Parameters[0].Value = forumPost.PostID;
			cmd.Parameters[1].Value = forumPost.Subject;
			cmd.Parameters[2].Value = forumPost.Body;
			cmd.Parameters[3].Value = forumPost.Notify;
			cmd.Parameters[4].Value = pinnedDate;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		private static ForumSearchInfo PopulateForumSearchInfo(SqlDataReader dr)
		{
			ForumSearchInfo forumSearchInfo = new ForumSearchInfo();

			forumSearchInfo.Alias		= Convert.ToString(dr["Alias"]);
			forumSearchInfo.PostDate	= Convert.ToDateTime(dr["PostDate"]);
			forumSearchInfo.PostID		= Convert.ToInt32(dr["PostID"]);
			forumSearchInfo.RecordCount	= Convert.ToInt32(dr["RecordCount"]);
			forumSearchInfo.Subject		= Convert.ToString(dr["Subject"]);

			return forumSearchInfo;
		}

		public static ForumSearchInfoCollection GetForumSearchResults(string searchTerms, int forumID, int pageSize, int pageIndex)
		{
			// Construct WHERE clause for search
			string whereClause = " AND (";
			string[] termsAnd = null;
			termsAnd = searchTerms.Split(' ');
			whereClause += "Body LIKE '%" + string.Join("%' AND Body LIKE '%", termsAnd) + "%'";
			whereClause += ") ";

			// Limit to just one forum if required
			whereClause += " AND WS_Threads.ForumID = " + forumID.ToString() + " ";

			// Execute stored procedure
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetForumSearchResults", conn);

			// Populate parameters
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@WhereClause", SqlDbType.NVarChar, 500);
			cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4);
			cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = whereClause;
			cmd.Parameters[1].Value = pageSize;
			cmd.Parameters[2].Value = pageIndex;

			conn.Open();
			
			ForumSearchInfoCollection forumSearchInfoCollection = new ForumSearchInfoCollection();

			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				ForumSearchInfo forumSearchInfo = PopulateForumSearchInfo(dr);
				forumSearchInfoCollection.Add(forumSearchInfo);
			}

			dr.Close();
			conn.Close();

			return forumSearchInfoCollection;
		}

		private static ForumThreadInfo PopulateForumThreadInfo(SqlDataReader dr)
		{
			ForumThreadInfo forumThreadInfo = new ForumThreadInfo();

			forumThreadInfo.DateLastPost	= Convert.ToDateTime(dr["DateLastPost"]);
			forumThreadInfo.PinnedDate		= Convert.ToDateTime(dr["PinnedDate"]);
			forumThreadInfo.LastPostAlias	= Convert.ToString(dr["LastPostAlias"]);
			forumThreadInfo.Replies			= Convert.ToInt32(dr["Replies"]);
			forumThreadInfo.StartedByAlias	= Convert.ToString(dr["StartedByAlias"]);
			forumThreadInfo.Subject			= Convert.ToString(dr["Subject"]);
			forumThreadInfo.ThreadID		= Convert.ToInt32(dr["ThreadID"]);
			forumThreadInfo.Views			= Convert.ToInt32(dr["Views"]);
			forumThreadInfo.LastPostID		= Convert.ToInt32(dr["LastPostID"]);

			return forumThreadInfo;
		}

		public static ForumThreadInfoCollection GetThreads(int forumID, int pageSize, int pageIndex)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetThreads", conn);

			// Populate parameters
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ForumID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@PageSize", SqlDbType.Int, 4);
			cmd.Parameters.Add("@PageIndex", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = forumID;
			cmd.Parameters[1].Value = pageSize;
			cmd.Parameters[2].Value = pageIndex;

			conn.Open();

			ForumThreadInfoCollection forumThreadInfoCollection = new ForumThreadInfoCollection();

			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				ForumThreadInfo forumThreadInfo = PopulateForumThreadInfo(dr);
				forumThreadInfoCollection.Add(forumThreadInfo);
			}

			dr.Close();
			conn.Close();

			return forumThreadInfoCollection;
		}

		public static int GetThreadCount(int forumID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetThreadCount", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ForumID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Count", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = forumID;
			cmd.Parameters[1].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (int)cmd.Parameters[1].Value;
		}

		public static int GetThreadFromPost(int postID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetThreadFromPost", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = postID;
			cmd.Parameters[1].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (int)cmd.Parameters[1].Value;
		}

		public static int GetSortOrderFromPost(int postID, ForumUtils.ForumView forumView)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetSortOrderFromPost", conn);

			bool flatView = (forumView == ForumUtils.ForumView.FlatView);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@FlatView", SqlDbType.Bit, 1);
			cmd.Parameters.Add("@SortOrder", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = postID;
			cmd.Parameters[1].Value = flatView;
			cmd.Parameters[2].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (int)cmd.Parameters[2].Value;
		}

		public static int GetPostFromThreadAndPage(int threadID, int threadPage, int postsPerPage, ForumUtils.ForumView forumView)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetPostFromThreadAndPage", conn);

			bool flatView = (forumView == ForumUtils.ForumView.FlatView);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@ThreadPage", SqlDbType.Int, 4);
			cmd.Parameters.Add("@PostsPerPage", SqlDbType.Int, 4);
			cmd.Parameters.Add("@FlatView", SqlDbType.Bit, 1);
			cmd.Parameters.Add("@PostID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = threadID;
			cmd.Parameters[1].Value = threadPage;
			cmd.Parameters[2].Value = postsPerPage;
			cmd.Parameters[3].Value = flatView;
			cmd.Parameters[4].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (int)cmd.Parameters[4].Value;
		}

		public static void IncrementThreadViews(int threadID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_IncrementThreadViews", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = threadID;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		public static ForumPostCollection GetThread(int threadID, int threadPage, int postsPerPage, ForumUtils.ForumView forumView)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetThread", conn);

			bool flatView = (forumView == ForumUtils.ForumView.FlatView);

			// Populate parameters
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@ThreadPage", SqlDbType.Int, 4);
			cmd.Parameters.Add("@PostsPerPage", SqlDbType.Int, 4);
			cmd.Parameters.Add("@FlatView", SqlDbType.Bit, 1);
			cmd.Parameters[0].Value = threadID;
			cmd.Parameters[1].Value = threadPage;
			cmd.Parameters[2].Value = postsPerPage;
			cmd.Parameters[3].Value = flatView;

			conn.Open();

			ForumPostCollection forumPostCollection = new ForumPostCollection();

			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				ForumPost forumPost = PopulateForumPost(dr);
				forumPost.PostID = Convert.ToInt32(dr["PostID"]);
				forumPostCollection.Add(forumPost);
			}

			dr.Close();
			conn.Close();

			return forumPostCollection;
		}

		public static int GetThreadRepliesCount(int threadID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetRepliesFromThread", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ThreadID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Replies", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = threadID;
			cmd.Parameters[1].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (int)cmd.Parameters[1].Value;
		}
	}
}
