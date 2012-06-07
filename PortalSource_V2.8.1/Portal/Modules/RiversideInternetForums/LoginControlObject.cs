using System;
using System.Web.UI;

namespace RiversideInternet.WebSolution
{
	public class LoginControlObject : WebSolutionObject
	{
		public LoginControlObject(LoginControl loginControl) : base(loginControl)
		{
		}

		private void RenderOptionsUserLoggedOn(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Href, GetUserManagement("settings"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "header");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("My Settings");
			writer.RenderEndTag();	// A

			writer.Write("&nbsp;|&nbsp;");

			writer.AddAttribute(HtmlTextWriterAttribute.Href, GetUserManagement("logoff"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "header");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("Logoff");
			writer.RenderEndTag();	// A
		}

		private void RenderOptionsUserLoggedOff(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Href, GetUserManagement("join"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "header");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("Join");
			writer.RenderEndTag();	// A

			writer.Write("&nbsp;|&nbsp;");

			writer.AddAttribute(HtmlTextWriterAttribute.Href, GetUserManagement("login"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "header");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("Logon");
			writer.RenderEndTag();	// A
		}

		public override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);

			int userID = LoggedOnUserID;

			if (userID > 0)
				RenderOptionsUserLoggedOn(writer);
			else
				RenderOptionsUserLoggedOff(writer);

			writer.RenderEndTag();	// Span
		}
	}
}
