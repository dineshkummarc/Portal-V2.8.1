using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ForumThread : ForumObject
	{
		private ForumUtils.ForumView	_forumView = ForumUtils.ForumView.FlatView;
		private bool					_viewDropDownSelected;
		private ForumPostCollection		_forumPostCollection;
		private	DropDownList			_viewDropDownList;
		private int						_postID;
		private	int						_threadID;
		private	int						_threadPage;
		private TextBox					_searchTextBox;

		public ForumThread(Forum forum, int postID) : base(forum)
		{
			_postID = postID;
		}

		private void ViewDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
		{
			// Either tree view or flat view has been selected from the view drop down list.
			// We need to update the user's preference in the Forum_View cookie.  Maybe this
			// setting would be better stored in a user profile (in the database).
			_forumView = ForumUtils.GetForumViewFromString(_viewDropDownList.SelectedItem.Value);

			ForumUtils.SetForumView(Page, _forumView);

			_viewDropDownSelected = true;
		}

		private void SearchTextBox_TextChanged(object sender, EventArgs e)
		{
			CheckStartSearch(_searchTextBox.Text);
		}

		public override void CreateChildControls()
		{
			// Create view drop down list for switching between flat view and tree view
			_viewDropDownList = new DropDownList();
			_viewDropDownList.AutoPostBack = true;
			_viewDropDownList.Items.Add(new ListItem("Dynamic", ForumUtils.ForumView.TreeViewDynamic.ToString()));
			_viewDropDownList.Items.Add(new ListItem("Flat", ForumUtils.ForumView.FlatView.ToString()));
			_viewDropDownList.Items.Add(new ListItem("Tree", ForumUtils.ForumView.TreeView.ToString()));
			_viewDropDownList.Width = Unit.Pixel(150);
			_viewDropDownList.CssClass = "WebSolutionFormControl";
			_viewDropDownList.SelectedIndexChanged += new System.EventHandler(ViewDropDownList_SelectedIndexChanged);

			// Create search text box
			_searchTextBox = new TextBox();
			_searchTextBox.CssClass = "WebSolutionFormControl";
			_searchTextBox.Width = Unit.Pixel(150);
			_searchTextBox.MaxLength = 500;
			_searchTextBox.TextChanged += new System.EventHandler(SearchTextBox_TextChanged);

			// Add controls to page
			Controls.Add(_viewDropDownList);
			Controls.Add(_searchTextBox);
		}

		private void RegisterJavascript()
		{
			StringBuilder sb = new StringBuilder();

			// SwitchMessage function (copied from CodeProject forums)
			sb.Append("<SCRIPT language=\"JavaScript\">");
			sb.Append("var Selected = \"");
			sb.Append(_postID.ToString());
			sb.Append("\";");
			sb.Append("function SwitchMessage(elm)");
			sb.Append(" {");
			sb.Append(" if (Selected == elm.name)");
			sb.Append(" {");
			sb.Append(" window.location = \"#\" + Selected;");
			sb.Append(" return false;");
			sb.Append(" }");
			sb.Append(" var elmrefmessagerow;");
			sb.Append(" var elmrefavatarrow;");
			sb.Append(" var elmrefactionsrow;");
			sb.Append(" var elmrefheadercell;");
			sb.Append(" var elmrefmessagetable;");
			sb.Append(" if (Selected != \"\")");
			sb.Append(" {");
			sb.Append(" elmrefmessagerow = eval(\"document.getElementById('\" + Selected + \"_messagerow')\");");
			sb.Append(" elmrefavatarrow = eval(\"document.getElementById('\" + Selected + \"_avatarrow')\");");
			sb.Append(" elmrefactionsrow = eval(\"document.getElementById('\" + Selected + \"_actionsrow')\");");
			sb.Append(" elmrefheadercell = eval(\"document.getElementById('\" + Selected + \"_headercell')\");");
			sb.Append(" elmrefmessagetable = eval(\"document.getElementById('\" + Selected + \"_messagetable')\");");
			sb.Append(" if (elmrefmessagerow) elmrefmessagerow.style.display = 'none';");
			sb.Append(" if (elmrefavatarrow) elmrefavatarrow.style.display = 'none';");
			sb.Append(" if (elmrefactionsrow) elmrefactionsrow.style.display = 'none';");
			sb.Append(" if (elmrefheadercell) elmrefheadercell.style.height = '100%';");
			sb.Append(" if (elmrefmessagetable) elmrefmessagetable.style.height = '100%';");
			sb.Append(" }");
			sb.Append(" if (Selected != elm.name) ");
			sb.Append(" {");
			sb.Append(" Selected = elm.name;");
			sb.Append(" elmrefmessagerow = eval(\"document.getElementById('\" + Selected + \"_messagerow')\");");
			sb.Append(" elmrefavatarrow = eval(\"document.getElementById('\" + Selected + \"_avatarrow')\");");
			sb.Append(" elmrefactionsrow = eval(\"document.getElementById('\" + Selected + \"_actionsrow')\");");
			sb.Append(" elmrefheadercell = eval(\"document.getElementById('\" + Selected + \"_headercell')\");");
			sb.Append(" elmrefmessagetable = eval(\"document.getElementById('\" + Selected + \"_messagetable')\");");
			sb.Append(" if (elmrefmessagerow) ");
			sb.Append(" {");
			sb.Append(" if (elmrefmessagerow.style.display=='none') elmrefmessagerow.style.display='';");
			sb.Append(" else elmrefmessagerow.style.display = 'none';");
			sb.Append(" }");
			sb.Append(" if (elmrefavatarrow) ");
			sb.Append(" {");
			sb.Append(" if (elmrefavatarrow.style.display=='none') elmrefavatarrow.style.display='';");
			sb.Append(" else elmrefavatarrow.style.display = 'none';");
			sb.Append(" }");
			sb.Append(" if (elmrefactionsrow) ");
			sb.Append(" {");
			sb.Append(" if (elmrefactionsrow.style.display=='none') elmrefactionsrow.style.display='';");
			sb.Append(" else elmrefactionsrow.style.display = 'none';");
			sb.Append(" }");
			sb.Append(" if (elmrefheadercell) ");
			sb.Append(" {");
			sb.Append(" if (elmrefheadercell.style.height != '0px') elmrefheadercell.style.height='0px';");
			sb.Append(" else elmrefheadercell.style.height = '100%';");
			sb.Append(" }");
			sb.Append(" if (elmrefmessagetable) ");
			sb.Append(" {");
			sb.Append(" if (elmrefmessagetable.style.height != '0px') elmrefmessagetable.style.height='0px';");
			sb.Append(" else elmrefmessagetable.style.height = '100%';");
			sb.Append(" }");
			sb.Append(" }");
			sb.Append(" else");
			sb.Append(" Selected=\"\";");
			sb.Append(" window.location = \"#\" + Selected;");
			sb.Append(" return false;");
			sb.Append(" }");
			sb.Append("</SCRIPT>");

			// When clicking subject link, call SwitchMessage (copied from CodeProject forums)
			sb.Append("<SCRIPT language=\"javascript\" event=\"onclick\" for=\"DynMessLink\">");
			sb.Append("return SwitchMessage(this);");
			sb.Append("</SCRIPT>");

			// Register client side script block
			Page.RegisterClientScriptBlock("DynamicTreeView", sb.ToString());
		}

		public override void OnPreRender()
		{
			// Determine whether we are in flat view or tree view mode.  This setting is
			// stored in a cookie, but should probably be moved in to the user's profile
			// which is stored in the database.
			if (!_viewDropDownSelected)
				_forumView = ForumUtils.GetForumView(Page);
			ListItem listItem = _viewDropDownList.Items.FindByValue(_forumView.ToString());
			if (listItem != null)
				listItem.Selected = true;

			// _threadID identifies the thread we are currently viewing.  _postID identifies
			// the post within the thread that is currently being shown. (i.e. in tree view,
			// only one post can be viewed at a time).  _threadID can be obtained from the value
			// of _postID.  We will also determine what page we are looking at.
			_threadID = ForumDB.GetThreadFromPost(_postID);
			_threadPage = ForumDB.GetSortOrderFromPost(_postID, _forumView)/ForumUtils.GetPostsPerPage();

			// If looking at the first post in a thread, then increment the thread view count
			if (_threadID == _postID)
				ForumDB.IncrementThreadViews(_threadID);

			// Get page of posts that will be rendered
			_forumPostCollection = ForumDB.GetThread(_threadID, _threadPage, ForumUtils.GetPostsPerPage(), _forumView);

			// Register javascript for dynamically showing threads in tree view
			if (_forumView == ForumUtils.ForumView.TreeViewDynamic)
				RegisterJavascript();
		}

		private void RenderHeader(HtmlTextWriter writer)
		{
			// Start a new row, 25 pixels high that spans two columns
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// We will put the header within a table
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Left hand side links: All Threads | New Thread
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// All Threads link
			writer.Write("&nbsp;");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=showall", "threadspage=&searchpage=&postid="));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("All Threads");
			writer.RenderEndTag();	// A

			// Divider
			writer.Write("&nbsp;|&nbsp;");

			// New Thread
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=new", "searchpage=&threadspage=&postid="));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("New Thread");
			writer.RenderEndTag();	// A
			writer.RenderEndTag();	// Td

			// Right hand side: View drop down list and search text box
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// View drop down list
			writer.Write("View:");
			_viewDropDownList.RenderControl(writer);

			// Gap
			writer.Write("&nbsp;&nbsp;");

			// Search list box
			writer.Write("Search:&nbsp;");
			_searchTextBox.RenderControl(writer);
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td

			// End table and row
			writer.RenderEndTag();	// Tr
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		private void RenderPosts(HtmlTextWriter writer)
		{
			// Header row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			// Author
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100");
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Author&nbsp;");
			writer.RenderEndTag();	// Td

			// Thread
			ForumPost firstForumPost = (ForumPost) _forumPostCollection[0];
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			ForumText subjectForumText = new ForumText(firstForumPost.Subject);
			writer.Write(string.Format("&nbsp;Thread: {0}", subjectForumText.ProcessSingleLine(GetImages())));
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Work out when to display new images
			string cookieName = "Forum" + ForumID + "_LastVisited";
			DateTime lastVisited = Convert.ToDateTime(Page.Session[cookieName]);

			// Loop round rows in selected thread
			string document = GetDocument();
			string images = GetImages();
			foreach (ForumPost forumPost in _forumPostCollection)
			{
				bool selected = (_postID == forumPost.PostID);
				string avatar = forumPost.User.Avatar;
				if (avatar != string.Empty)
					avatar = GetAvatar(avatar);
				forumPost.Render(writer, true, _forumView, selected, lastVisited, Page, LoggedOnUserID, avatar, images, document);
			}
		}

		private void RenderThreadPaging(HtmlTextWriter writer, int pageCount)
		{
			// Start the new column
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// First, previous, next, last thread hyperlinks
			bool backwards = _threadPage != 0;
			bool forwards = _threadPage != pageCount - 1;

			// << First and < Previous links
			if (backwards)
			{
				int postID = 0;

				// << First
				postID = _threadID;
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("postid={0}", postID), "forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("&lt;&lt;");
				writer.RenderEndTag();	// A
				writer.Write("&nbsp;");

				// < Previous
				postID = ForumDB.GetPostFromThreadAndPage(_threadID, _threadPage - 1, ForumUtils.GetPostsPerPage(), _forumView);
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("postid={0}", postID), "forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("&lt;&nbsp;Previous");
				writer.RenderEndTag();	// A
			}

			// Divider
			if (backwards && forwards)
				writer.Write("&nbsp;|&nbsp;");

			// Next > and Last >> links
			if (forwards)
			{
				int postID = 0;

				// Next >
				postID = ForumDB.GetPostFromThreadAndPage(_threadID, _threadPage + 1, ForumUtils.GetPostsPerPage(), _forumView);
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("postid={0}", postID), "forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("Next&nbsp;&gt;");
				writer.RenderEndTag();
				writer.Write("&nbsp;");

				// Last >>
				postID = ForumDB.GetPostFromThreadAndPage(_threadID, pageCount - 1, ForumUtils.GetPostsPerPage(), _forumView);
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("postid={0}", postID), "forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("&gt;&gt;");
				writer.RenderEndTag();
			}

			// Close column
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td
		}

		private void RenderFooter(HtmlTextWriter writer)
		{
			// Start the footer row
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Into which we will put a table that will contain
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// On the left hand side: Page x of y
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Page x of y text
			int pageCount = ((ForumDB.GetThreadRepliesCount(_threadID))/ForumUtils.GetPostsPerPage()) + 1;
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			writer.Write(string.Format("&nbsp;Page {0} of {1}", _threadPage + 1, pageCount));
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td

			// And on the right hand side, render thread navigation (<< < Previous | Next > >>)
			RenderThreadPaging(writer, pageCount);

			// Close out this table and row
			writer.RenderEndTag();	// Tr
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		public override void Render(HtmlTextWriter writer)
		{
			RenderTableBegin(writer, 1, 0);
			RenderHeader(writer);
			RenderPosts(writer);
			RenderFooter(writer);
			RenderTableEnd(writer);
		}
	}
}
