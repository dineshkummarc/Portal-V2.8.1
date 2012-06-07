using System;
using System.Collections;
using System.Text;
using System.Web.UI;

namespace RiversideInternet.WebSolution
{
	public class ForumPost
	{
		private bool		_notify;
		private DateTime	_postDate;
		private int			_flatSortOrder;
		private int			_parentPostID;
		private int			_postID;
		private int			_postLevel;
		private int			_threadID;
		private int			_treeSortOrder;
		private string		_body;
		private string		_remoteAddr;
		private string		_subject;
		private User		_user;

		public ForumPost()
		{
		}

		private void RenderLevelIndentCell(HtmlTextWriter writer)
		{
			int width = PostLevel*16;
			if (width > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Width, width.ToString());
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag();
			}
		}

		public void Render(HtmlTextWriter writer, bool displayActions, ForumUtils.ForumView forumView, bool selected, DateTime lastVisited, Page page, int loggedOnUserID, string avatar, string images, string document)
		{
			// New row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			// Left hand side contains user information (user alias, avatar, number of posts etc)
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "160");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// We will put this user information in its own table in the first column
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "3");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "160");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// User alias and number of posts
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			ForumText userAliasForumText = new ForumText(User.Alias);
			writer.Write(userAliasForumText.ProcessSingleLine(images));
			writer.RenderEndTag();	// B
			writer.RenderBeginTag(HtmlTextWriterTag.Br);
			writer.Write(string.Format("Posts: {0}", User.PostCount));
			writer.RenderEndTag();	// Br
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Avatar
			if (forumView == ForumUtils.ForumView.TreeViewDynamic)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, PostID.ToString() + "_avatarrow");
				if (!selected)
					writer.AddStyleAttribute("display", "none");
			}
			if ((selected && forumView == ForumUtils.ForumView.TreeView) || forumView != ForumUtils.ForumView.TreeView)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				if (avatar != string.Empty)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
					writer.AddAttribute(HtmlTextWriterAttribute.Src, avatar);
					writer.RenderBeginTag(HtmlTextWriterTag.Img);
					writer.RenderEndTag();	// Img
				}
				writer.RenderEndTag();	// Td
				writer.RenderEndTag();	// Tr
			}

			// End user information table
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td

			// Start row which will display subject, body and actions (reply, edit, etc)
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			if (forumView != ForumUtils.ForumView.FlatView && !selected)
				writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			if (forumView == ForumUtils.ForumView.TreeViewDynamic)
				writer.AddAttribute(HtmlTextWriterAttribute.Id, PostID.ToString() + "_headercell");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Start a new table for this information
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "3");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if (forumView != ForumUtils.ForumView.FlatView && !selected)
				writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			if (forumView == ForumUtils.ForumView.TreeViewDynamic)
				writer.AddAttribute(HtmlTextWriterAttribute.Id, PostID.ToString() + "_messagetable");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Highlighted row (subject and when posted information)
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			if (forumView != ForumUtils.ForumView.FlatView)
				RenderLevelIndentCell(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			writer.AddAttribute(HtmlTextWriterAttribute.Name, PostID.ToString());
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.RenderEndTag();

			// Provide link to select a different post
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", PostID), "forumaction=&searchpage=&threadspage=") + "#" + PostID);
			if (forumView == ForumUtils.ForumView.TreeViewDynamic)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, "DynMessLink");
				writer.AddAttribute(HtmlTextWriterAttribute.Name, PostID.ToString());
			}
			writer.RenderBeginTag(HtmlTextWriterTag.A);

			ForumText subjectForumText = new ForumText(Subject);
			writer.Write(subjectForumText.ProcessSingleLine(images));
			writer.RenderEndTag();	// A
			writer.RenderEndTag();	// B

			// Display new image if this post is new since last time user visited
			if (lastVisited < PostDate)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "new.gif");
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Br);
			writer.Write(string.Format("Posted: {0} {1}", PostDate.ToString("dd MMM yy"), PostDate.ToString("t")));
			writer.RenderEndTag();	// Br
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Message row
			if ((selected && forumView == ForumUtils.ForumView.TreeView) || forumView != ForumUtils.ForumView.TreeView)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				if (forumView == ForumUtils.ForumView.TreeViewDynamic)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Id, PostID.ToString() + "_messagerow");
					if (!selected)
						writer.AddStyleAttribute("display", "none");
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				if (forumView != ForumUtils.ForumView.FlatView)
					RenderLevelIndentCell(writer);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);

				ForumText bodyForumText = new ForumText(Body);
				writer.Write(bodyForumText.Process(images));

				writer.RenderEndTag();	// Td
				writer.RenderEndTag();	// Tr
			}

			// Reply, Quote, Edit, Get Link row
			if (displayActions && ((selected && forumView == ForumUtils.ForumView.TreeView) || forumView != ForumUtils.ForumView.TreeView))
			{
				if (forumView == ForumUtils.ForumView.TreeViewDynamic)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Id, PostID.ToString() + "_actionsrow");
					if (!selected)
						writer.AddStyleAttribute("display", "none");
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				if (forumView != ForumUtils.ForumView.FlatView)
					RenderLevelIndentCell(writer);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);

				writer.AddAttribute(HtmlTextWriterAttribute.Align, "Left");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);

				// Reply link
				writer.Write("[");
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}&forumaction=reply", PostID), "searchpage=&threadspage="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("Reply");
				writer.RenderEndTag();	// A
				writer.Write("]");

				// Quote link
				writer.Write("[");
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}&forumaction=quote", PostID), "searchpage=&threadspage="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("Quote");
				writer.RenderEndTag();	// A
				writer.Write("]");

				if (forumView == ForumUtils.ForumView.TreeViewDynamic)
				{
					// Get Link
					writer.Write("[");
					writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", PostID), "forumaction=&searchpage=&threadspage=") + "#" + PostID);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write("Get Link");
					writer.RenderEndTag();	 // A
					writer.Write("]");
				}

				writer.RenderEndTag();	// Td

				writer.AddAttribute(HtmlTextWriterAttribute.Align, "Right");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);

				// Edit link (only allowed if this post by currently logged on user or an administrator)
				if (User.UserID == loggedOnUserID || page.User.IsInRole("ForumAdmin"))
				{
					writer.Write("[");
					writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}&forumaction=edit", PostID), "searchpage=&threadspage="));
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write("Edit");
					writer.RenderEndTag();	// A
					writer.Write("]");
				}

				writer.RenderEndTag();	// Td
				writer.RenderEndTag();	// Tr
				writer.RenderEndTag();	// Table

				writer.RenderEndTag();	// Td
				writer.RenderEndTag();	// Tr
			}

			// Close out table and this row
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		public bool Notify
		{
			get
			{
				return _notify;
			}
			set
			{
				_notify = value;
			}
		}

		public DateTime PostDate
		{
			get
			{
				return _postDate;
			}
			set
			{
				_postDate = value;
			}
		}

		public int FlatSortOrder
		{
			get
			{
				return _flatSortOrder;
			}
			set
			{
				_flatSortOrder = value;
			}
		}

		public int ParentPostID
		{
			get
			{
				return _parentPostID;
			}
			set
			{
				_parentPostID = value;
			}
		}

		public int PostID
		{
			get
			{
				return _postID;
			}
			set
			{
				_postID = value;
			}
		}

		public int PostLevel
		{
			get
			{
				return _postLevel;
			}
			set
			{
				_postLevel = value;
			}
		}

		public int ThreadID
		{
			get
			{
				return _threadID;
			}
			set
			{
				_threadID = value;
			}
		}

		public int TreeSortOrder
		{
			get
			{
				return _treeSortOrder;
			}
			set
			{
				_treeSortOrder = value;
			}
		}

		public User User
		{
			get
			{
				return _user;
			}
			set
			{
				_user = value;
			}
		}

		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				_body = value;
			}
		}

		public string RemoteAddr
		{
			get
			{
				return _remoteAddr;
			}
			set
			{
				_remoteAddr = value;
			}
		}

		public string Subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}
	}
}
