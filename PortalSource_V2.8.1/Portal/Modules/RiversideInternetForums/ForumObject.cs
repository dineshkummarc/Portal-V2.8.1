using System;

namespace RiversideInternet.WebSolution
{
	public class ForumObject : WebSolutionObject
	{
		public ForumObject(Forum forum) : base(forum)
		{
		}

		protected void CheckStartSearch(string searchTerms)
		{
			// Redirect user to search page
			if (searchTerms.Length > 0)
			{
				string redirectURL = null;
				searchTerms = searchTerms.Replace("&", ":amp:");
				redirectURL = WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=search&searchterms=" + searchTerms, "postid=&threadspage=&searchpage=");

				if (DocumentID > 0)
					redirectURL = "../" + redirectURL;
				Page.Response.Redirect(redirectURL);
			}
		}

		protected int ForumID
		{
			get
			{
				return Forum.ForumID;
			}
		}

		protected Forum Forum
		{
			get
			{
				return (Forum)WebSolutionControl;
			}
		}
	}
}
