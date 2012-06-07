using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class WebSolutionObject
	{
		private WebSolutionControl _webSolutionControl;

		public WebSolutionObject(WebSolutionControl webSolutionControl)
		{
			_webSolutionControl = webSolutionControl;
		}

		public virtual void CreateChildControls()
		{
		}

		public virtual void OnPreRender()
		{
		}

		public virtual void Render(HtmlTextWriter writer)
		{
		}

		public string GetDocument()
		{
			string url = string.Empty;

			// Following code removed as it is to do with WebSolution features that are
			// currently not implemented in the CodeProject article.
#if false			
			if (DocumentID > 0)
			{
				string documentURL = DocumentsDB.GetDocumentURL(DocumentID);
				if (documentURL != string.Empty)
				{
					url = string.Format("{0}.aspx", documentURL);
				}
			}
#endif
			if (url == string.Empty)
				url = Page.Request.ServerVariables["URL"];
			return url;
		}

		public string GetImages()
		{
			string configValue = System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsImagesURL"];
			if (configValue != null)
			{
				if (configValue.IndexOf("{0}") >= 0)
					configValue = string.Format(configValue, WebFolder);
				return configValue;
			}
			return string.Empty;
		}

		protected string GetAvatar(string filename)
		{
			string configValue = System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsAvatarsURL"];
			if (configValue != null)
			{
				if (configValue.IndexOf("{0}") >= 0)
					configValue = string.Format(configValue, WebFolder);
				return configValue + filename;
			}

			return filename;
		}

		protected string GetAvatarPath(string filename)
		{
			string avatarPath = System.Configuration.ConfigurationManager.AppSettings["RiversideInternetForumsAvatarsURL"];
			if (avatarPath != null)
			{
				if (avatarPath.IndexOf("{0}") >= 0)
					avatarPath = string.Format(avatarPath, WebFolder);
				avatarPath = System.Web.HttpContext.Current.Server.MapPath(avatarPath);
				return avatarPath + filename;
			}

			return filename;
		}

/*		protected string GetUserManagement(string action)
		{
			string configValue = System.Configuration.ConfigurationManager.AppSettings["WebSolutionUserManagementURL"];
			if (configValue != null)
			{
				string rawURL = Page.Request.RawUrl;
				rawURL = rawURL.Replace("&", ":amp:");
				configValue = string.Format("{0}?useraction={1}&returnurl={2}", configValue, action, rawURL);
			}

			return configValue;
		}

		protected void RedirectToLoginPage()
		{
			string url = GetUserManagement("login");
			if (url != string.Empty && url != null)
			{
				if (DocumentID > 0)
					url = "../" + url;
				Page.Response.Redirect(url);
			}
		}*/
		protected string GetUserManagement(string action)
		{
			throw new Exception("Cant be...");
		}
		protected void RedirectToLoginPage()
		{
			throw new Exception("Cant be...");
		}

		protected void RenderTableBegin(HtmlTextWriter writer, int cellSpacing, int cellPadding)
		{
			// Begin table in which we will render object
			if (_webSolutionControl.BorderStyle != BorderStyle.None)
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionBorder");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, cellSpacing.ToString());
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, cellPadding.ToString());
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
		}

		protected void RenderTableEnd(HtmlTextWriter writer)
		{
			// End table in which object was rendered
			writer.RenderEndTag();
		}

		protected int WebID
		{
			get
			{
				return _webSolutionControl.WebID;
			}
		}

		protected int DocumentID
		{
			get
			{
				return _webSolutionControl.DocumentID;
			}
		}

		protected string WebFolder
		{
			get
			{
				return _webSolutionControl.WebFolder;
			}
		}

		protected Page Page
		{
			get
			{
				return _webSolutionControl.Page;
			}
		}

		protected WebSolutionControl WebSolutionControl
		{
			get
			{
				return _webSolutionControl;
			}
		}

		protected ControlCollection Controls
		{
			get
			{
				return _webSolutionControl.Controls;
			}
		}

		protected int LoggedOnUserID
		{
			get
			{
				return UserDB.GetLoggedOnUser(Page.User.Identity.Name, WebID);
			}
		}
	}
}
