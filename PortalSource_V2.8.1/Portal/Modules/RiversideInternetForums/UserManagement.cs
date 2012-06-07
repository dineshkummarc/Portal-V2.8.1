using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class UserManagement : WebSolutionControl, INamingContainer
	{
		public UserManagement()
		{
		}

		// CHANGED by Arthur Zaczek
		protected override void CreateObject()
		{
			// Create the correct user object given query string parrameters in order
			// to perform correct CreateChildControls, OnPreRender and Render operations.
/*			string userAction = Page.Request.QueryString["useraction"];

			switch (userAction)
			{
				case "login":
					WebSolutionObject = new UserLogin(this);
					break;

				case "logoff":
					WebSolutionObject = new UserLogoff(this);
					break;

				case "settings":
				case "join":
					WebSolutionObject = new UserSettings(this, userAction);
					break;
			}*/

			WebSolutionObject = new UserSettings(this, "settings");
		}
	}
}
