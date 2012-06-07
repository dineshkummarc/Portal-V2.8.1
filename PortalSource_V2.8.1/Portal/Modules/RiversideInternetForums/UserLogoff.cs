using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class UserLogoff : UserObject
	{
		private bool	_loggedOff;
		private Button	_logoffButton;

		public UserLogoff(WebSolutionControl webSolutionControl) : base(webSolutionControl)
		{
		}

		public void LogoffButton_Click(object sender, System.EventArgs e)
		{
			// Sign user out and redirect to return URL if one specified
			FormsAuthentication.SignOut();
			RedirectReturnURL();
			_loggedOff = true;
		}

		public override void CreateChildControls()
		{
			// Logoff button
			_logoffButton = new Button();
			_logoffButton.Text = "Logoff";
			_logoffButton.CssClass = "WebSolutionFormControl";
			_logoffButton.Click += new System.EventHandler(LogoffButton_Click);

			// Add control
			Controls.Add(_logoffButton);
		}

		public override void Render(HtmlTextWriter writer)
		{
			RenderTableBegin(writer, 1, 0);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Check to see whether a user is currently logged on.  If not, no need
			// to display logoff info message and logoff button.
			if (_loggedOff || LoggedOnUserID == 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.P);
				writer.Write("Nobody is logged on.");
				writer.RenderEndTag();
			}
			else
			{
				// Display logged on user
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.P);
				ForumText aliasForumText = new ForumText(UserDB.GetUser(LoggedOnUserID).Alias);
				writer.Write(aliasForumText.ProcessSingleLine(GetImages()) + " is logged on.");
				writer.RenderEndTag();	// P

				// Informative message
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.P);
				writer.Write("Click the button below to logoff.");
				writer.RenderEndTag();

				// Logoff button
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.P);
				_logoffButton.RenderControl(writer);
				writer.RenderEndTag();
			}

			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
			RenderTableEnd(writer);		// Table
		}
	}
}
