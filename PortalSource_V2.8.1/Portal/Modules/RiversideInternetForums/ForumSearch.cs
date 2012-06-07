using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ForumSearch : ForumObject
	{
		private ForumSearchInfoCollection	_forumSearchInfoCollection;
		private int							_postsCount;
		private int							_searchPage;
		private string						_searchTerms;
		private TextBox						_searchTextBox;

		public ForumSearch(Forum forum, int searchPage, string searchTerms) : base(forum)
		{
			_searchPage = searchPage;
			_searchTerms = searchTerms;
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

			// Add controls to the page
			Controls.Add(_searchTextBox);
		}

		public override void OnPreRender()
		{
			// Get search results that will be rendered and the total number of search results
			_forumSearchInfoCollection = ForumDB.GetForumSearchResults(_searchTerms, ForumID, ForumUtils.GetPostsPerPage(), _searchPage);
			if (_forumSearchInfoCollection.Count > 0)
				_postsCount = ((ForumSearchInfo)_forumSearchInfoCollection[0]).RecordCount;
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

			// Left hand side link: All Threads | New Thread
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// All Threads hyper link
			writer.Write("&nbsp;");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=showall", "searchterms=&threadspage=&searchpage=&postid="));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("All Threads");
			writer.RenderEndTag();	// A
			writer.Write("&nbsp;|&nbsp;");

			// New Thread link
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=new", "postid=&threadspage=&searchpage="));
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

		private void RenderPosts(HtmlTextWriter writer)
		{
			// Render header row
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Search Results&nbsp;");
			writer.RenderEndTag();	// Td

			// Posted by column
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Posted By&nbsp;");
			writer.RenderEndTag();	// Td

			// Date column
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionHeader");
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;Date&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Work out when to display new images
			string cookieName = "Forum" + ForumID + "_LastVisited";
			DateTime lastVisited = Convert.ToDateTime(Page.Session[cookieName]);

			// Loop round posts found by search, rendering information each one
			string document = GetDocument();
			string images = GetImages();
			foreach (ForumSearchInfo forumSearchInfo in _forumSearchInfoCollection)
				forumSearchInfo.Render(writer, lastVisited, Page, images, document);
		}

		private void RenderSearchPaging(HtmlTextWriter writer)
		{
			// Start the new column
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// First, previous, next, last thread hyperlinks
			int pageCount = ((_postsCount - 1)/ForumUtils.GetPostsPerPage()) + 1;
			bool backwards = _searchPage != 0;
			bool forwards = _searchPage != pageCount - 1;

			// << First and < Previous links
			if (backwards)
			{
				// << First
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "", "postid=&threadspage=&searchpage="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("<<");
				writer.RenderEndTag();	// A
				writer.Write("&nbsp;");

				// < Previous
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("searchpage={0}", _searchPage), "postid="));
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
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("searchpage={0}", _searchPage + 2), "postid="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("Next >");
				writer.RenderEndTag();	// A
				writer.Write("&nbsp;");

				// Last >>
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, string.Format("searchpage={0}", pageCount), "postid="));
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
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Into which we will put a table that will contain
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Start row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// On the left hand side: Page x of y
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			int pageCount = ((_postsCount - 1)/ForumUtils.GetPostsPerPage()) + 1;
			if (_postsCount == 0)
				writer.Write("&nbsp;No Search Results");
			else
				writer.Write(string.Format("&nbsp;Page {0} of {1}", _searchPage + 1, pageCount));
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td

			// And on the right hand side, render search navigation (<< < Previous | Next > >>)
			RenderSearchPaging(writer);

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
			RenderPosts(writer);
			RenderFooter(writer);
			RenderTableEnd(writer);
		}
	}
}
