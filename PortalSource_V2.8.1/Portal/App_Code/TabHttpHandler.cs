using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.Handlers;
using System.Web.SessionState;
using System.Text;
using System.Globalization;

namespace Portal
{
	/// <summary>
	/// Summary description for TabHttpHandler.
	/// </summary>
	public class TabHttpHandler : IHttpHandler, IRequiresSessionState
	{
		public void ProcessRequest(HttpContext context) 
		{
			string path = context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture);
			string tabRef = path.Substring(path.LastIndexOf("/") + 1); // get "TabRef.tab"
			tabRef = tabRef.Substring(0, tabRef.LastIndexOf(".tab.aspx")); // get "TabRef"

			Hashtable r = new Hashtable();
			foreach(string key in context.Request.QueryString.Keys)
			{
				r[key] = context.Request[key];
			}

			r["TabRef"] = tabRef;

            StringBuilder url = new StringBuilder(Portal.API.Config.MainPage);
			bool firstParam = true;
			foreach(DictionaryEntry e in r)
			{
				if(firstParam)
				{
                    url.Append("?");
					firstParam = false;
				}
				else
				{
					url.Append("&");
				}
                url.Append(e.Key.ToString());
                url.Append("=");
                url.Append(e.Value.ToString());
			}
			context.Server.Transfer(url.ToString());
		}
		
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
