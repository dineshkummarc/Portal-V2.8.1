using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class UserLogin : UserObject
	{
		private int						_userID;
		private Button					_loginButton;
		private CheckBox				_rememberMeCheckBox;
		private TextBox					_emailTextBox;
		private TextBox					_passwordTextBox;
		private RequiredFieldValidator	_emailValidator;
		private RequiredFieldValidator	_passwordValidator;

		public UserLogin(WebSolutionControl webSolutionControl) : base(webSolutionControl)
		{
		}

		// Note: For an explanation of the forums authentication code used
		// in this function, please refer to Heath Stewart's article on Code Project:
		// Role-based Security with Forms Authentication
		// http://www.codeproject.com/aspnet/formsroleauth.asp
		private void LoginButton_Click(object sender, System.EventArgs e)
		{
			// If email and password are entered, try to log user on
			if (_emailValidator.IsValid && _passwordValidator.IsValid)
			{
				int userID = UserDB.GetUserIDFromEmail(_emailTextBox.Text, WebID);

				if (userID > 0)
				{
					// Get information for user with identifier userID
					User user = UserDB.GetUser(userID);

					if (user.Password == _passwordTextBox.Text)
					{
						// Record the user that is going to be logged on
						_userID = userID;

						// Initialise forms authentication
						FormsAuthentication.Initialize();

						// Create a new ticket used for authentication
						DateTime expire = DateTime.Now;
						if (_rememberMeCheckBox.Checked)
							expire = expire.AddYears(10);
						else
							expire = expire.AddMinutes(30);
						FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
							1,										// Ticket version
							userID.ToString(),						// Username associated with ticket
							DateTime.Now,							// Date/time issued
							expire,									// Date/time to expire
							_rememberMeCheckBox.Checked,			// "true" for a persistent user cookie
							user.Roles,								// User-data, in this case the roles
							FormsAuthentication.FormsCookiePath);	// Path cookie valid for

						// Encrypt the cookie using the machine key for secure transport
						string hash = FormsAuthentication.Encrypt(ticket);
						HttpCookie cookie = new HttpCookie(
							FormsAuthentication.FormsCookieName, // Name of auth cookie
							hash); // Hashed ticket

						// Set the cookie's expiration time to the tickets expiration time
						if (ticket.IsPersistent)
							cookie.Expires = ticket.Expiration;

						// Add the cookie to the list for outgoing response
						Page.Response.Cookies.Add(cookie);

						RedirectReturnURL();
					}
				}
			}
		}

		public override void CreateChildControls()
		{
			// E-mail entry text box
			_emailTextBox = new TextBox();
			_emailTextBox.ID = "_emailTextBox";
			_emailTextBox.Width = Unit.Pixel(150);
			_emailTextBox.MaxLength = 100;
			_emailTextBox.CssClass = "WebSolutionFormControl";

			// E-mail validator
			_emailValidator = new RequiredFieldValidator();
			_emailValidator.ControlToValidate = "_emailTextBox";
			_emailValidator.ErrorMessage = "You must enter a valid e-mail address.";

			// Password text box
			_passwordTextBox = new TextBox();
			_passwordTextBox.ID = "_passwordTextBox";
			_passwordTextBox.Width = Unit.Pixel(150);
			_passwordTextBox.MaxLength = 50;
			_passwordTextBox.TextMode = TextBoxMode.Password;
			_passwordTextBox.CssClass = "WebSolutionFormControl";
			_passwordTextBox.Controls.Add(_passwordTextBox);

			// Password validator
			_passwordValidator = new RequiredFieldValidator();
			_passwordValidator.ControlToValidate = "_passwordTextBox";
			_passwordValidator.ErrorMessage = "You must enter a password.";

			// Remember me check box
			_rememberMeCheckBox = new CheckBox();
			_rememberMeCheckBox.Text = "Remember me";
			_rememberMeCheckBox.CssClass = "WebSolutionFormControl";

			// Login button
			_loginButton = new Button();
			_loginButton.Text = "Login";
			_loginButton.Click += new System.EventHandler(LoginButton_Click);
			_loginButton.CssClass = "WebSolutionFormControl";

			// Add child controls to control
			Controls.Add(_emailTextBox);
			Controls.Add(_emailValidator);
			Controls.Add(_passwordTextBox);
			Controls.Add(_passwordValidator);
			Controls.Add(_rememberMeCheckBox);
			Controls.Add(_loginButton);
		}

		public override void Render(HtmlTextWriter writer)
		{
			RenderTableBegin(writer, 1, 0);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Display logged on user
			if (LoggedOnUserID > 0 || _userID > 0)
			{
				int userID = _userID;
				if (userID == 0)
					userID = LoggedOnUserID;

				User user = UserDB.GetUser(userID);

				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.P);
				writer.Write(user.Alias + " is logged on.");
				writer.RenderEndTag();	// P
			}

			// On post back, display whether user is or is not logged on
			if (Page.IsPostBack)
			{
				if (_userID == 0)
				{
					// Login attempt must have been invalid.  So display this fact.
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
					writer.RenderBeginTag(HtmlTextWriterTag.P);
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionErrorText");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					writer.Write("Invalid login attempt.  Please try again.");
					writer.RenderEndTag();	// Span
					writer.RenderEndTag();	// P
				}
			}

			// Start table into which we will render login controls
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// E-mail row
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			writer.Write("E-mail:");
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_emailTextBox.RenderControl(writer);			
			writer.RenderEndTag();	// Td
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_emailValidator.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Password row
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			writer.Write("Password:");
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_passwordTextBox.RenderControl(writer);			
			writer.RenderEndTag();	// Td
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_passwordValidator.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Remember me check box
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_rememberMeCheckBox.RenderControl(writer);			
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Gap row
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Login button
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_loginButton.RenderControl(writer);			
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Close table
			writer.RenderEndTag();	// Table

			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
			RenderTableEnd(writer);		// Table
		}
	}
}
