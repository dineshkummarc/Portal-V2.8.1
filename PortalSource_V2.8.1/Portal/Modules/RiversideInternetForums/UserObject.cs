using System;
using System.Web.UI;

namespace RiversideInternet.WebSolution
{
	public class UserObject : WebSolutionObject
	{
		private string _returnURL;

		public UserObject(WebSolutionControl webSolutionControl) : base(webSolutionControl)
		{
		}

		protected void RedirectReturnURL()
		{
			string returnURL = Page.Request.QueryString["returnurl"];

			if (returnURL == null)
				returnURL = string.Empty;

			returnURL = returnURL.Replace(":amp:", "&");
			returnURL = returnURL.Replace(":?:", "?");

			if (returnURL.Length > 0)
				Page.Response.Redirect(returnURL);

			if (ReturnURL != null)
				Page.Response.Redirect(ReturnURL);
		}

		public string ReturnURL
		{
			get
			{
				return _returnURL;
			}
			set
			{
				_returnURL = value;
			}
		}
	}
}
