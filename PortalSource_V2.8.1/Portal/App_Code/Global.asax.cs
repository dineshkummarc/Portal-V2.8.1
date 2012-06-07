using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Security;
using System.Security.Principal;
using Portal.API.Statistics.Service;
using Portal.API.Statistics;

namespace Portal 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
      //Portal.API.Helper.InstallFile("Portal.config");
      //Portal.API.Helper.InstallFile("Portal.css");
      //Portal.API.Helper.InstallFile("PortalFooter.ascx");
      //Portal.API.Helper.InstallFile("PortalHeader.ascx");
      //Portal.API.Helper.InstallFile("Users.config");
      //if(Portal.API.Helper.InstallFile("web.config"))
      //{
      //  throw new Exception("web.config file installed, reload the application");
      //}
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
      // If its not the LifeguardData.aspx file, then register the Session Start as a Visit.
      if (0 != string.Compare(Request.AppRelativeCurrentExecutionFilePath, "~/LifeguardData.aspx", true))
      {
        RequestStatisticService service = (RequestStatisticService)Statistic.GetService(typeof(RequestStatisticService));
        service.AddVisit(Context);
      }
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			if(Request.IsAuthenticated)
			{
				HttpContext.Current.User = Portal.API.UserManagement.GetUser(HttpContext.Current.User.Identity.Name);
			}
		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}

