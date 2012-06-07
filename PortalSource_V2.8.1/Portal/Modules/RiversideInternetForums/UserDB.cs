using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RiversideInternet.WebSolution
{
	public class UserDB
	{
		public static User GetUser(int userID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetUser", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UserID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = userID;

			conn.Open();
			
			SqlDataReader dr = cmd.ExecuteReader();

			// Should throw an exception here if dr.Read returns false
			dr.Read();
			User user = PopulateUser(dr);
			user.UserID = userID;

			dr.Close();
			conn.Close();

			return user;
		}

		private static User PopulateUser(SqlDataReader dr)
		{
			User user = new User();

			user.Alias		= Convert.ToString(dr["Alias"]);
			user.Email		= Convert.ToString(dr["Email"]);
			user.PostCount	= Convert.ToInt32(dr["PostCount"]);
			user.Password	= Convert.ToString(dr["Password"]);
			user.WebID		= Convert.ToInt32(dr["WebID"]);
			user.Roles		= Convert.IsDBNull(dr["Roles"]) ? string.Empty : Convert.ToString(dr["Roles"]);
			user.Avatar		= Convert.IsDBNull(dr["Avatar"]) ? string.Empty : Convert.ToString(dr["Avatar"]);

			return user;
		}

		public static int GetUserIDFromEmail(string email, int webID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetUserIDFromEmail", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@WebID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@UserID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = email;
			cmd.Parameters[1].Value = webID;
			cmd.Parameters[2].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			if (Convert.IsDBNull(cmd.Parameters[2].Value))
				return 0;

			return (int)cmd.Parameters[2].Value;
		}
		// CHANGED by Arthur Zaczek
		public static int GetUserIDFromAlias(string alias, int webID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetUserIDFromAlias", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Alias", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@WebID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@UserID", SqlDbType.Int, 4);
			cmd.Parameters[0].Value = alias;
			cmd.Parameters[1].Value = webID;
			cmd.Parameters[2].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			if (Convert.IsDBNull(cmd.Parameters[2].Value))
				return 0;

			return (int)cmd.Parameters[2].Value;
		}

		public static void UpdateUser(User user)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_UpdateUser", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UserID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Alias", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@Avatar", SqlDbType.NVarChar, 50);
			cmd.Parameters[0].Value = user.UserID;
			cmd.Parameters[1].Value = user.Alias;
			cmd.Parameters[2].Value = user.Email;
			cmd.Parameters[3].Value = user.Password;
			if (user.Avatar == string.Empty)
				cmd.Parameters[4].Value = System.DBNull.Value;
			else
				cmd.Parameters[4].Value = user.Avatar;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		public static int AddUser(User user)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_AddUser", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UserID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Alias", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@WebID", SqlDbType.Int, 4);
			cmd.Parameters[0].Direction = ParameterDirection.Output;
			cmd.Parameters[1].Value = user.Alias;
			cmd.Parameters[2].Value = user.Email;
			cmd.Parameters[3].Value = user.Password;
			cmd.Parameters[4].Value = user.WebID;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			user.UserID = (int)cmd.Parameters[0].Value;

			return user.UserID;
		}

		// CHANGED by Arthur Zaczek
		public static int GetLoggedOnUser(string identityName, int webID)
		{
			// The user variable corresponds to a string which is the number of
			// the currently logged on user.  Generally, this function should be
			// called with user set to "Page.User.Identity.Name".
			int userID = 0;

			try
			{
				if(identityName == "") 
				{
					// Return Guest
					userID = GetUserIDFromAlias("guest", webID);
					if (userID > 0)
					{
						User user = GetUser(userID);
						if (user.WebID != webID)
							userID = 0;
					}
					else
					{
						// User does not exist -> create
						User user = new User();
						user.Alias = "guest";
						user.WebID = webID;
						user.Email = "guest@localhost";
						user.Password = "nopassword";
						userID = AddUser(user);
					}
				}
				else
				{
					userID = GetUserIDFromAlias(identityName, webID);
					if (userID > 0)
					{
						User user = GetUser(userID);
						if (user.WebID != webID)
							userID = 0;
					}
					else
					{
						// User does not exist -> create
						User user = new User();
						user.Alias = identityName;
						user.WebID = webID;
						user.Email = identityName + "@localhost";
						user.Password = "nopassword";
						userID = AddUser(user);
					}
				}
			}
			catch(Exception e)
			{
				System.Web.HttpContext.Current.Trace.Write(e.Message);
				userID = 0;
			}

			return userID;
		}

		public static bool AliasExists(string alias, int webID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_AliasExists", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Alias", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@WebID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Exists", SqlDbType.Bit, 1);
			cmd.Parameters[0].Value = alias;
			cmd.Parameters[1].Value = webID;
			cmd.Parameters[2].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (bool)cmd.Parameters[2].Value;
		}

		public static bool EmailExists(string email, int webID)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_EmailExists", conn);

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@WebID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Exists", SqlDbType.Bit, 1);
			cmd.Parameters[0].Value = email;
			cmd.Parameters[1].Value = webID;
			cmd.Parameters[2].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			return (bool)cmd.Parameters[2].Value;
		}
	}
}
