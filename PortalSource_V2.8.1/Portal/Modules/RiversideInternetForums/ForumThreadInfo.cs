using System;
using System.Web.UI;

namespace RiversideInternet.WebSolution
{
	public class ForumThreadInfo
	{
		private DateTime	_dateLastPost;
		private DateTime	_pinnedDate;
		private int			_lastPostID;
		private int			_replies;
		private int			_threadID;
		private int			_views;
		private string		_lastPostAlias;
		private string		_startedByAlias;
		private string		_subject;

		public ForumThreadInfo()
		{
		}

		public void Render(HtmlTextWriter writer, int threadsPerPage, DateTime lastVisited, Page page, string images, string document, int postsPerPage, ForumUtils.ForumView forumView)
		{
			// Start row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			// Render thread image.  If the total number of posts in this thread is great than the
			// number of posts that can be displayed on one page, then we display a special image that
			// indicates this thread is "on fire".
			int totalPosts = (int)Replies + 1;
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "25");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			if (totalPosts > threadsPerPage)
				writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "board_thread_fire.gif");
			else
				writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "board_thread.gif");
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();	// Img
			writer.RenderEndTag();	// Td

			// Thread subject with link (and indicate whether or not this thread is pinned)
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "25");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			if (PinnedDate > DateTime.Now)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.B);
				writer.Write("Sticky: ");
				writer.RenderEndTag();	// B
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", ThreadID), "forumaction=&searchpage=&threadspage=") + string.Format("#{0}", ThreadID));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			ForumText subjectForumText = new ForumText(Subject);
			writer.Write(subjectForumText.ProcessSingleLine(images));
			writer.RenderEndTag();	// A

			// If thread spans several pages, then we need to indicate this in the thread list
			// by displaying text like (Page 1, 2, 3, ..., 5)
			if (totalPosts > threadsPerPage)
			{
				writer.Write(" (Page: ");
				int pageCount = ((totalPosts - 1)/threadsPerPage) + 1;
				int pageCountCapped = Math.Min(pageCount, 4);
				bool showFinalPage = (pageCountCapped < pageCount);
				for (int threadPage = 0; threadPage < pageCountCapped; threadPage++)
				{
					int postID = ForumDB.GetPostFromThreadAndPage(ThreadID, threadPage, postsPerPage, forumView);
					writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", postID), "forumaction=&searchpage=&threadspage="));
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(string.Format("{0}", threadPage + 1));
					writer.RenderEndTag();	// A
					if ((threadPage < pageCountCapped - 1) || showFinalPage)
						writer.Write(", ");
				}
				if (showFinalPage)
				{
					if (pageCount > 5)
						writer.Write("..., ");
					int postID = ForumDB.GetPostFromThreadAndPage(ThreadID, pageCount - 1, postsPerPage, forumView);
					writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", postID), "forumaction=&searchpage=&threadspage="));
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(pageCount.ToString());
					writer.RenderEndTag();	// A
				}
				writer.Write(")");
			}

			// Display new image if this thread is new since last time user visited
			if (lastVisited < _dateLastPost)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "new.gif");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// Started by
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			ForumText startedByAliasForumText = new ForumText(StartedByAlias);
			writer.Write(startedByAliasForumText.ProcessSingleLine(images));
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// Replies
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.Write(Replies);
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// Views
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "50");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.Write(Views);
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// Last Post
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRowHighlight");
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "140");
			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionSmallerFont");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.Write(DateLastPost.ToString("dd MMM yy"));
			writer.Write("&nbsp;");
			writer.Write(DateLastPost.ToString("t"));
			writer.Write("<BR>");
			ForumText lastPostAliasForumText = new ForumText(LastPostAlias);
			writer.Write(lastPostAliasForumText.ProcessSingleLine(images));
			writer.Write("&nbsp;");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(document, page, string.Format("postid={0}", LastPostID), "forumaction=&searchpage=&threadspage=") + string.Format("#{0}", LastPostID));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.AddAttribute(HtmlTextWriterAttribute.Src, images + "last_post.gif");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();	// Img
			writer.RenderEndTag();	// A
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Span
			writer.RenderEndTag();	// Td

			// End row
			writer.RenderEndTag();	// Tr
		}

		public DateTime DateLastPost
		{
			get
			{
				return _dateLastPost;
			}
			set
			{
				_dateLastPost = value;
			}
		}

		public DateTime PinnedDate
		{
			get
			{
				return _pinnedDate;
			}
			set
			{
				_pinnedDate = value;
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

		public int LastPostID
		{
			get
			{
				return _lastPostID;
			}
			set
			{
				_lastPostID = value;
			}
		}

		public int Replies
		{
			get
			{
				return _replies;
			}
			set
			{
				_replies = value;
			}
		}

		public int Views
		{
			get
			{
				return _views;
			}
			set
			{
				_views = value;
			}
		}

		public string LastPostAlias
		{
			get
			{
				return _lastPostAlias;
			}
			set
			{
				_lastPostAlias = value;
			}
		}

		public string StartedByAlias
		{
			get
			{
				return _startedByAlias;
			}
			set
			{
				_startedByAlias = value;
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
