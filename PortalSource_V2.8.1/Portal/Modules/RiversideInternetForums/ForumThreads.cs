using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ForumThreads : ForumObject
	{
		private ForumThreadInfoCollection	_forumThreadInfoCollection;
		private int							_threadCount;
		private int							_threadsPage;
		private const int					_threadsPerPage = 25;
		private TextBox						_searchTextBox;

		public ForumThreads(Forum forum, int threadsPage) : base(forum)
		{
			_threadsPage = threadsPage;
		}

		private void SearchTextBox_TextChanged(object sender, EventArgs e)
		{
			CheckStartSearch(_searchTextBox.Text);
		}

		public override void CreateChildControls()
		{
			// Create the text box
			_searchTextBox = new TextBox();
			_searchTextBox.CssClass = "WebSolutionFormControl";
			_searchTextBox.Width = Unit.Pixel(150);
			_searchTextBox.MaxLength = 500;
			_searchTextBox.TextChanged += new System.EventHandler(SearchTextBox_TextChanged);

			// Add control to page
			Controls.Add(_searchTextBox);
		}

		public override void OnPreRender()
		{
			// Get threads that will be rendered and the total number of forum threads
			_forumThreadInfoCollection = ForumDB.GetThreads(ForumID, _threadsPerPage, _threadsPage);
			_threadCount = ForumDB.GetThreadCount(ForumID);
		}

		private void RenderHeader(HtmlTextWriter writer)
		{
			// Start a new row, 25 pixels high that spans two columns
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "6");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// We will put the header within a table
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Left hand side link: New Thread
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// New Thread link
			writer.Write("&nbsp;");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=new", "postid=&searchpage=&threadspage="));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("New Thread");
			writer.RenderEndTag();	// A
			writer.RenderEndTag();	// Td

			// Right hand side: Search text box
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Search list box
			writer.Write("Search:&nbsp;");
			_searchTextBox.RenderControl(writer);
			writer.RenderEndTag();	// Td

			// End table and row
			writer.RenderEndTag();	// Tr
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		private void RenderThreads(HtmlTextWriter writer)
		{
			// Render header row
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Thread&nbsp;");
			writer.RenderEndTag();	// Td

			// Started by column
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Started By&nbsp;");
			writer.RenderEndTag();	// Td

			// Replies column
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Replies&nbsp;");
			writer.RenderEndTag();	// Td

			// Views column
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Views&nbsp;");
			writer.RenderEndTag();	// Td

			// Last Post column
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Last Post&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Work out when to display new images
			string cookieName = "Forum" + ForumID + "_LastVisited";
			DateTime lastVisited = Convert.ToDateTime(Page.Session[cookieName]);

			// Loop round thread items, rendering information about individual threads
			string document = GetDocument();
			string images = GetImages();
			foreach (ForumThreadInfo forumThreadInfo in _forumThreadInfoCollection)
				forumThreadInfo.Render(writer, ForumUtils.GetPostsPerPage(), lastVisited, Page, images, document, ForumUtils.GetPostsPerPage(), ForumUtils.GetForumView(Page));
		}

		private void RenderThreadsPaging(HtmlTextWriter writer)
		{
			// Start the new column
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// First, previous, next, last thread hyperlinks
			int pageCount = ((_threadCount - 1)/_threadsPerPage) + 1;
			bool backwards = _threadsPage != 0;
			bool forwards = _threadsPage != pageCount - 1;

			// << First and < Previous links
			if (backwards)
			{
				// << First
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "", "postid=&forumaction=&searchpage=&threadspage="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("<<");
				writer.RenderEndTag();	// A
				writer.Write("&nbsp;");

				// < Previous
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("threadspage={0}", _threadsPage), "postid=&forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("< Previous");
				writer.RenderEndTag();	// A
			}

			// Divider
			if (backwards && forwards)
				writer.Write("&nbsp;|&nbsp;");

			// Next > and Last >> links
			if (forwards)
			{
				// Next >
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("threadspage={0}", _threadsPage + 2), "postid=&forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("Next >");
				writer.RenderEndTag();	// A
				writer.Write("&nbsp;");

				// Last >>
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("threadspage={0}", pageCount), "postid=&forumaction="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write(">>");
				writer.RenderEndTag();	// A
			}

			// Close column
			writer.RenderEndTag();	// Td
		}

		private void RenderFooter(HtmlTextWriter writer)
		{
			// Start the footer row
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "6");
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
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			int pageCount = ((_threadCount - 1)/_threadsPerPage) + 1;
			if (_threadCount == 0)
				writer.Write("&nbsp;No Threads");
			else
				writer.Write(string.Format("&nbsp;Page {0} of {1}", _threadsPage + 1, pageCount));
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td

			// And on the right hand side, render threads navigation (<< < Previous | Next > >>)
			RenderThreadsPaging(writer);

			// Close out this table and row
			writer.RenderEndTag();	// Tr
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		public override void Render(HtmlTextWriter writer)
		{
			RenderTableBegin(writer, 1, 3);
			RenderHeader(writer);
			RenderThreads(writer);
			RenderFooter(writer);
			RenderTableEnd(writer);
		}
	}
}
