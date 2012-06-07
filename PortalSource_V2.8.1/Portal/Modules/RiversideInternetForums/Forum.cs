using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class Forum : WebSolutionControl, INamingContainer
	{
		private int	_forumID;

		public Forum()
		{
		}

		protected override void CreateObject()
		{
			// Create the correct forum object given query string parrameters in order
			// to perform correct CreateChildControls, OnPreRender and Render operations.
			int postID = Convert.ToInt32(Page.Request.QueryString["postid"]);
			string action = Page.Request.QueryString["forumaction"];

			if (postID == 0)
			{
				if (action == "new")
				{
					WebSolutionObject = new ForumForm(this, postID, action);
				}
				else if (action == "search")
				{
					int searchPage = Convert.ToInt32(Page.Request.QueryString["searchpage"]);
					if (searchPage > 0)
						searchPage = searchPage - 1;

					string searchTerms = Page.Request.QueryString["searchterms"];
					if (searchTerms != null && searchTerms != string.Empty)
					{
						searchTerms = searchTerms.Replace(":amp:", "&");
						searchTerms = searchTerms.Replace("'", "''");
					}

					WebSolutionObject = new ForumSearch(this, searchPage, searchTerms);
				}
				else
				{
					int threadsPage = Convert.ToInt32(Page.Request.QueryString["threadspage"]);
					if (threadsPage > 0)
						threadsPage = threadsPage - 1;

					WebSolutionObject = new ForumThreads(this, threadsPage);
				}
			}
			else
			{
				if (action == "edit" || action == "reply" || action == "quote")
					WebSolutionObject = new ForumForm(this, postID, action);
				else
					WebSolutionObject = new ForumThread(this, postID);
			}
		}

		private void SetupCookies()
		{
			// Date last visited stuff
			string cookieName = "Forum" + ForumID + "_LastVisited";

			// Update Session[key] from cookies if not already set
			if (Page.Session[cookieName] == null)
			{
				Page.Session[cookieName] = new DateTime(1971, 11, 13);

				HttpCookie myCookie = Page.Request.Cookies[cookieName];

				if (myCookie != null)
				{
					string boardsLastVisited = myCookie.Value;
					DateTime dt = DateTime.Parse(boardsLastVisited);
					Page.Session[cookieName] = dt;
				}
			}

			// Put in cookie last time board visited
			string cookieValue = DateTime.Now.ToString();
			HttpCookie httpCookie = new HttpCookie(cookieName, cookieValue);
			DateTime expires = DateTime.Now;
			expires = expires.AddDays(355);
			httpCookie.Expires = expires;
			Page.Response.Cookies.Add(httpCookie);
		}

		protected override void Initialise()
		{
			base.Initialise();

			// Following code removed as it is to do with WebSolution features that are
			// currently not implemented in the CodeProject article.
#if false
			// If document ID specified, get forum ID from document information
			if (DocumentID > 0)
				ForumID = DocumentsDB.GetBoardFromDocument(DocumentID);
#endif
		}

		protected override void CreateChildControls()
		{
			SetupCookies();
			base.CreateChildControls();
		}

		public int ForumID
		{
			get
			{
				return _forumID;
			}
			set
			{
				_forumID = value;
			}
		}
	}
}
