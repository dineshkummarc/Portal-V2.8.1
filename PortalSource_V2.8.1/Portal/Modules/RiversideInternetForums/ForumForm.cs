using System;
using System.Text;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ForumForm : ForumObject
	{
		private Button					_submitButton;
		private CheckBox				_notifyCheckBox;
		private DropDownList			_pinnedDropDownList;
		private ForumPost				_forumPost;
		private int						_postID;
		private int						_userID;
		private Label					_nameLabel;
		private RequiredFieldValidator	_bodyValidator;
		private RequiredFieldValidator	_subjectValidator;
		private string					_action;
		private TextBox					_bodyTextBox;
		private TextBox					_subjectTextBox;

		public ForumForm(Forum forum, int postID, string action) : base(forum)
		{
			_postID = postID;
			_action = action;

			CheckRedirect();
		}

		private void CheckRedirect()
		{
			if (_action != null && _action != string.Empty && _action != "showall")
			{
				_userID = LoggedOnUserID;

				if (_userID == 0)
				{
					RedirectToLoginPage();
				}
			}
		}

		private void EmailReplyNotification(ForumPost parentForumPost, ForumPost forumPost)
		{
			User user = UserDB.GetUser(forumPost.User.UserID);
			User userParent = UserDB.GetUser(parentForumPost.User.UserID);

			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("A reply from {0} has been posted to your message entitled \"{1}\"\r\n\r\n", user.Alias, parentForumPost.Subject);
			sb.AppendFormat("Subject: \"{0}\"\r\n\r\n", forumPost.Subject);
			sb.AppendFormat("Reply: \r\n\r\n{0}\r\n\r\n", forumPost.Body);
			string url = "http://" + Page.Request.ServerVariables["SERVER_NAME"] + "/" + WebSolutionUtils.GetURL(GetDocument(), Page, "postid=" + forumPost.PostID + "#" + forumPost.PostID, "forumaction=&threadspage=&searchpage=");
			sb.Append("To view this reply at the forum, click on the following link:\r\n\r\n");
			sb.Append(url);

			MailMessage mailMessage = new MailMessage();
			mailMessage.To = userParent.Email;
			mailMessage.Subject = "Message board reply";
			mailMessage.Body = sb.ToString();
			mailMessage.BodyFormat = MailFormat.Text;
			string host = Page.Request.Url.Host;
			if (host.Length > 4 && host.Substring(0, 4) == "www.")
				host = host.Substring(4, host.Length - 4);
			mailMessage.From = "forums@" + host;

			SmtpMail.SmtpServer = Page.Request.ServerVariables["SERVER_NAME"];

			try
			{
				SmtpMail.Send(mailMessage);
			}
			catch (Exception)
			{
			}
		}

		private void GetPinnedInfo(out DateTime pinnedDate)
		{
			int days = 0;
			pinnedDate = DateTime.Now;

			try
			{
				days = Convert.ToInt32(_pinnedDropDownList.SelectedItem.Value);
			}
			catch (Exception)
			{
			}

			pinnedDate = pinnedDate.AddDays(days);
		}

		private void EditPost(ForumPost forumPost)
		{
			if (_pinnedDropDownList == null)
			{
				ForumDB.UpdatePost(forumPost);
				return;
			}

			DateTime pinnedDate;
			GetPinnedInfo(out pinnedDate);

			ForumDB.UpdatePostPinned(forumPost, pinnedDate);
		}

		private int AddNew(ForumPost forumPost)
		{
			if (_pinnedDropDownList == null)
				return ForumDB.AddPost(forumPost, ForumID);

			DateTime pinnedDate;
			GetPinnedInfo(out pinnedDate);

			return ForumDB.AddPostPinned(forumPost, ForumID, pinnedDate);
		}

		private int AddReply(ForumPost forumPost)
		{
			// Add reply
			int postID = ForumDB.AddPost(forumPost, ForumID);

			// Check to see whether author of parent post should be notified
			ForumPost parentForumPost = ForumDB.GetPost(forumPost.ParentPostID);

			// Send e-mail
			if (parentForumPost.Notify && parentForumPost.User.UserID != forumPost.User.UserID)
				EmailReplyNotification(parentForumPost, forumPost);

			// Return identifier of newly created post
			return postID;
		}

		private void SubmitButton_Click(object sender, EventArgs e)
		{
			// Only allow an action to be completed if the subject and body validators
			// are valid - i.e. we will not allow empty strings in either text box.
			if (_bodyValidator.IsValid && _subjectValidator.IsValid)
			{
				// Create forum post object
				ForumPost forumPost = new ForumPost();

				// Post specific fields
				forumPost.Notify		= _notifyCheckBox.Checked;
				forumPost.Body			= _bodyTextBox.Text;
				forumPost.Subject		= _subjectTextBox.Text;
				forumPost.RemoteAddr	= Page.Request.ServerVariables["REMOTE_ADDR"];

				// User specific fields
				forumPost.User = new User();
				forumPost.User.UserID = _userID;

				// Perform action depending on button command name
				int postID = 0;
				string action = _submitButton.CommandName;

				// Perform new post, reply, quote or edit action
				switch (action)
				{
					case "new":
						postID = AddNew(forumPost);
						break;

					case "reply":
					case "quote":
						forumPost.ParentPostID = _postID;
						postID = AddReply(forumPost);
						break;

					case "edit":
						forumPost.PostID = _postID;
						postID = _postID;
						EditPost(forumPost);
						break;
				}

				// Redirect user to page displaying new information
				string redirectURL = null;
				if (postID > 0)
					redirectURL = WebSolutionUtils.GetURL(GetDocument(), Page, "postid=" + postID, "forumaction=&threadspage=&searchpage=");
				else
					redirectURL = WebSolutionUtils.GetURL(GetDocument(), Page, "", "postid=&forumaction=&threadspage=&searchpage=");
				if (DocumentID > 0)
					redirectURL = "../" + redirectURL;
				Page.Response.Redirect(redirectURL);
			}
		}

		private bool PostCanBePinned()
		{
			if (!Page.User.IsInRole("ForumAdmin"))
				return false;

			if (_action == "new")
				return true;

			if (_action == "edit" && _postID == ForumDB.GetThreadFromPost(_postID))
				return true;

			return false;
		}

		public override void CreateChildControls()
		{
			// Name label
			_nameLabel = new Label();
			_nameLabel.Width = Unit.Percentage(100);

			// Subject text box
			_subjectTextBox = new TextBox();
			_subjectTextBox.Width = Unit.Percentage(100);
			_subjectTextBox.CssClass = "WebSolutionFormControl";
			_subjectTextBox.MaxLength = 255;
			_subjectTextBox.ID = "_subjectTextBox";

			// Body message text box
			_bodyTextBox = new TextBox();
			_bodyTextBox.Width = Unit.Percentage(100);
			_bodyTextBox.CssClass = "WebSolutionFormControl";
			_bodyTextBox.Rows = 10;
			_bodyTextBox.TextMode = TextBoxMode.MultiLine;
			_bodyTextBox.ID = "_bodyTextBox";

			// Notify me by e-mail check box
			_notifyCheckBox = new CheckBox();
			_notifyCheckBox.Text = "Notify by e-mail when reply posted";

			// Submit button
			_submitButton = new Button();
			_submitButton.CssClass = "WebSolutionFormControl";
			switch (_action)
			{
				case "new":
					_submitButton.Text = "Start New Thread";
					break;

				case "edit":
					_submitButton.Text = "Edit Message";
					break;

				case "reply":
				case "quote":
					_submitButton.Text = "Reply to Message";
					break;
			}
			_submitButton.CommandName = _action;
			_submitButton.Click += new System.EventHandler(SubmitButton_Click);

			// Subject validator
			_subjectValidator = new RequiredFieldValidator();
			_subjectValidator.ErrorMessage = "You must enter a subject.";
			_subjectValidator.ControlToValidate = "_subjectTextBox";
			_subjectValidator.Display = ValidatorDisplay.Dynamic;

			// Body validator
			_bodyValidator = new RequiredFieldValidator();
			_bodyValidator.ErrorMessage = "You must enter a message.";
			_bodyValidator.ControlToValidate = "_bodyTextBox";
			_bodyValidator.Display = ValidatorDisplay.Dynamic;

			// Add child controls to control
			Controls.Add(_subjectTextBox);
			Controls.Add(_bodyTextBox);
			Controls.Add(_notifyCheckBox);
			Controls.Add(_submitButton);
			Controls.Add(_subjectValidator);
			Controls.Add(_bodyValidator);

			// Allow post to be pinned?
			if (PostCanBePinned())
			{
				_pinnedDropDownList = new DropDownList();
				_pinnedDropDownList.ID = "_pinnedDropDownList";
				_pinnedDropDownList.CssClass = "WebSolutionFormControl";
			
				Controls.Add(_pinnedDropDownList);
			}
		}

		private void RegisterJavascript()
		{
			// Find control which is of type HtmlForm
			Control control = _bodyTextBox;
			while (control != null && !(control is System.Web.UI.HtmlControls.HtmlForm))
				control = control.Parent;

			// Use string builder to build <SCRIPT> block
			StringBuilder sb = new StringBuilder();

			// Javascript for inserting emoticons into message body
			if (control != null)
			{
				// Copied from CodeProject forums
				sb.Append("<SCRIPT language=\"javascript\">");
				sb.Append("function InsertText(text) {");
				string id = string.Format("document.{0}.{1}", control.ClientID, _bodyTextBox.ClientID);
				sb.Append(string.Format(" var TextArea = {0}; ", id));
				sb.Append(" if (TextArea) {");
				sb.Append(" TextArea.value += text + \" \"; ");
				sb.Append(" TextArea.focus(); } ");
				sb.Append(" return false; } ");
				sb.Append("</SCRIPT>");
			}

			// Javascript for updating status bar when hovering over emoticon (copied from CodeProject forums)
			sb.Append("<SCRIPT language=\"javascript\" event=\"onmouseover\" for=\"emoticon\">");
			sb.Append("window.self.status=\"Insert the code for this emoticon in your message\";");
			sb.Append("return true;");
			sb.Append("</SCRIPT>");

			// Javascript for clearing out status bar when mouse leaves emoticon (copied from CodeProject forums)
			sb.Append("<SCRIPT language=\"javascript\" event=\"onmouseout\" for=\"emoticon\">");
			sb.Append("window.self.status=\"\";");
			sb.Append("return true;");
			sb.Append("</SCRIPT>");

			// Register client side script block
			Page.RegisterClientScriptBlock("InsertEmoticon", sb.ToString());
		}

		private void RedirectUserHasNoAuthority()
		{
			Page.Response.Redirect(WebSolutionUtils.GetURL(GetDocument(), Page, "", "forumaction="));
		}

		private void PopulateUnits()
		{
			if (_action == "new" || _action == "reply" || _action == "quote")
			{
				ForumText aliasForumText = new ForumText(UserDB.GetUser(_userID).Alias);
				_nameLabel.Text = aliasForumText.ProcessSingleLine(GetImages());
			}

			if (_action == "edit" || _action == "reply" || _action == "quote")
			{
				_forumPost = ForumDB.GetPost(_postID);

				if (_action == "edit")
				{
					if (LoggedOnUserID != _forumPost.User.UserID && !Page.User.IsInRole("ForumAdmin"))
						RedirectUserHasNoAuthority();

					ForumText postAliasForumText = new ForumText(_forumPost.User.Alias);
					_nameLabel.Text = postAliasForumText.ProcessSingleLine(GetImages());
					_subjectTextBox.Text = _forumPost.Subject;
					_bodyTextBox.Text = _forumPost.Body;
					_notifyCheckBox.Checked = _forumPost.Notify;
				}
				else
				{
					// If action is quote or reply, make sure subject begins "Re:"
					string subject = _forumPost.Subject;
					string replySubject = subject;
					if (replySubject.Length >= 3)
					{
						if (replySubject.Substring(0, 3) != "Re:")
							replySubject = "Re: " + replySubject;
					}
					else
					{
						replySubject = "Re: " + replySubject;
					}
					_subjectTextBox.Text = replySubject;

					// If action is quote, add message being replied to within QUOTE tags
					if (_action == "quote")
					{
						ForumText forumText = new ForumText(_forumPost.Body);
						_bodyTextBox.Text = forumText.ProcessQuoteBody(_forumPost.User.Alias);
					}
				}
			}

			if (_pinnedDropDownList != null && !Page.IsPostBack)
			{
				_pinnedDropDownList.Items.Add(new ListItem("Not Sticky", "0"));
				_pinnedDropDownList.Items.Add(new ListItem("1 Day", "1"));
				_pinnedDropDownList.Items.Add(new ListItem("3 Days", "3"));
				_pinnedDropDownList.Items.Add(new ListItem("1 Week", "7"));
				_pinnedDropDownList.Items.Add(new ListItem("2 Weeks", "14"));
				_pinnedDropDownList.Items.Add(new ListItem("1 Month", "30"));
				_pinnedDropDownList.Items.Add(new ListItem("3 Months", "90"));
				_pinnedDropDownList.Items.Add(new ListItem("6 Months", "180"));
				_pinnedDropDownList.Items.Add(new ListItem("1 Year", "365"));
				_pinnedDropDownList.Items.Add(new ListItem("3 Years", "1095"));
			}
		}

		public override void OnPreRender()
		{
			// Create javascript for emoticon insertion and status bar text
			RegisterJavascript();

			// Various form unit population
			PopulateUnits();
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

			// Left hand side link: All Threads | Back to Referring Post
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// All Threads link always displayed
			writer.Write("&nbsp;");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "forumaction=showall", "postid=&threadspage=&searchpage="));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("All Threads");
			writer.RenderEndTag();	// A

			// Back to Referring Post not displayed when creating a new thread
			if (_action == "edit" || _action == "reply" || _action == "quote")
			{
				writer.Write("&nbsp;|&nbsp;");
				writer.AddAttribute(HtmlTextWriterAttribute.Href, WebSolutionUtils.GetURL(GetDocument(), Page, "postid=" + _postID + "#" + _postID, "forumaction=&threadspage=&searchpage="));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write("Back to Referring Post");
				writer.RenderEndTag();	// A
			}
			writer.RenderEndTag();	// Td

			// End table and row
			writer.RenderEndTag();	// Tr
			writer.RenderEndTag();	// Table
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		private void InsertSmiley(HtmlTextWriter writer, string smiley, string image, string tooltip)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Href, "./");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, "emoticon");
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("return InsertText('{0}')", smiley));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, tooltip);
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, GetImages() + image);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();	// Img
			writer.RenderEndTag();	// A
			writer.Write(" ");
		}

		private void RenderFormControls(HtmlTextWriter writer)
		{
			// Put form controls in their own table
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Name row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("Name:");
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_nameLabel.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Subject row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("Subject:");
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_subjectTextBox.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Body row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "Top");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("Message:");
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_bodyTextBox.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Pinned drop down list?
			if (_pinnedDropDownList != null)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("Sticky:");
				writer.RenderEndTag();	// Td
				writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				_pinnedDropDownList.RenderControl(writer);
				writer.RenderEndTag();	// Td
				writer.RenderEndTag();	// Tr
			}

			// Notify row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_notifyCheckBox.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Blank row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Smiley row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "Center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("Smiley:");
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			InsertSmiley(writer, ":)",			"smiley_smile.gif",		"Insert a smile :)");
			InsertSmiley(writer, ";)",			"smiley_wink.gif",		"Insert a wink ;)");
			InsertSmiley(writer, ";P",			"smiley_tongue.gif",	"Stick out your tongue ;P");
			InsertSmiley(writer, ":-D",			"smiley_biggrin.gif",	"Insert a big grin :-D");
			InsertSmiley(writer, ":(",			"smiley_frown.gif",		"Insert a frown :(");
			InsertSmiley(writer, ":((",			"smiley_cry.gif",		"Insert tears :((");
			InsertSmiley(writer, ":-O",			"smiley_redface.gif",	"Insert blush :-O");
			InsertSmiley(writer, ":rolleyes:",	"smiley_rolleyes.gif",	"Insert roll eyes :rolleyes:");
			InsertSmiley(writer, ":laugh:",		"smiley_laugh.gif",		"Insert laugh :laugh:");
			InsertSmiley(writer, ":mad:",		"smiley_mad.gif",		"Insert mad :mad:");
			InsertSmiley(writer, ":confused:",	"smiley_confused.gif",	"Insert confused :confused:");
			InsertSmiley(writer, ":|",			"smiley_line.gif",		"Insert Unimpressed :|");
			InsertSmiley(writer, " X| ",		"smiley_dead.gif",		"Insert unwell X| ");
			InsertSmiley(writer, ":suss:",		"smiley_suss.gif",		"Insert a suspicious :suss:");
			InsertSmiley(writer, ":cool:",		"smiley_cool.gif",		"Insert cool :cool:");
			InsertSmiley(writer, ":eek:",		"smiley_eek.gif",		"Insert an eek! :eek:");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Blank row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Submit button row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_submitButton.RenderControl(writer);
			writer.Write("&nbsp;&nbsp;");
			_subjectValidator.RenderControl(writer);
			writer.Write("&nbsp;&nbsp;");
			_bodyValidator.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Finish form controls
			writer.RenderEndTag();	// Table
		}

		private void RenderForm(HtmlTextWriter writer)
		{
			// When replying, show message replying to
			if (_action == "reply" || _action == "edit" || _action == "quote")
			{
				string cookieName = "Forum" + ForumID + "_LastVisited";
				DateTime lastVisited = Convert.ToDateTime(Page.Session[cookieName]);
				
				string avatar = string.Empty;
				if (_forumPost.User.Avatar != string.Empty)
					avatar = GetAvatar(_forumPost.User.Avatar);

				_forumPost.Render(writer, false, ForumUtils.ForumView.FlatView, true, lastVisited, Page, LoggedOnUserID, avatar, GetImages(), GetDocument());
			}

			// Start a new row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionRow");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.Br);
			RenderFormControls(writer);

			// Finish form
			writer.RenderEndTag();	// Br
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
		}

		public override void Render(HtmlTextWriter writer)
		{
			RenderTableBegin(writer, 1, 0);
			RenderHeader(writer);
			RenderForm(writer);
			RenderTableEnd(writer);
		}
	}
}
