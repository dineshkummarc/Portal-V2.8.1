using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RiversideInternet.WebSolution
{
	public class WebSolutionDB
	{
		public static int GetWebIDAndFolder(string host, out string folder)
		{
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsConnectionString"]);
			SqlCommand cmd = new SqlCommand("WS_GetWebIDAndFolder", conn);

			host = host.Replace("www.", "");

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@WebDomain", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@WebID", SqlDbType.Int, 4);
			cmd.Parameters.Add("@Folder", SqlDbType.NVarChar, 50);
			cmd.Parameters[0].Value = host;
			cmd.Parameters[1].Direction = ParameterDirection.Output;
			cmd.Parameters[2].Direction = ParameterDirection.Output;

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();

			int webID = (int)cmd.Parameters[1].Value;

			if (Convert.IsDBNull(cmd.Parameters[2].Value))
				folder = string.Empty;
			else
				folder = (string)cmd.Parameters[2].Value;

			return webID;		
		}
	}
}
