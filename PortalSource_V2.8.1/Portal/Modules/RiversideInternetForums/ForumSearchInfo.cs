using System;
using System.Web.UI;

namespace RiversideInternet.WebSolution
{
	public class ForumSearchInfo
	{
		private DateTime	_postDate;
		private int			_postID;
		private int			_recordCount;	// Should not be moved from this class
		private string		_alias;
		private string		_subject;

		public ForumSearchInfo()
		{
		}

		public void Render(HtmlTextWriter writer, DateTime lastVisited, Page page, string images, string document)
		{
			// Start row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			// Render post image
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "25");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "board_thread.gif");
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();	// Img
			writer.RenderEndTag();	// Td

			// Post subject with hyperlink
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", PostID), "forumaction=&threadspage=&searchpage=&searchterms=") + string.Format("#{0}", PostID));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			ForumText subjectForumText = new ForumText(Subject);
			writer.Write(subjectForumText.ProcessSingleLine(images));
			writer.RenderEndTag();	// A

			// Display new image if this post is new since last time user visited
			if (lastVisited < PostDate)
			{
				writer.Write(" ");
				writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "new.gif");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// Posted by
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			ForumText aliasForumText = new ForumText(Alias);
			writer.Write(aliasForumText.ProcessSingleLine(images));
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// Date
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.Write(string.Format("{0}", PostDate.ToString("dd MMM yy")));
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// End row
			writer.RenderEndTag();	// Tr
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

		public int RecordCount
		{
			get
			{
				return _recordCount;
			}
			set
			{
				_recordCount = value;
			}
		}

		public string Alias
		{
			get
			{
				return _alias;
			}
			set
			{
				_alias = value;
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
