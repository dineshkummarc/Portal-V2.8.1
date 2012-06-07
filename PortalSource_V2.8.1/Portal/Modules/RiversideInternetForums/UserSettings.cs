using System;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class UserSettings : UserObject
	{
		// CHANGED by Arthur Zaczek
		private ValidAlias				_aliasValidator;
		private ValidAvatar				_avatarValidator;
		private Button					_submitButton;
		private CheckBox				_rememberMeCheckBox;
		private CompareValidator		_confirmValidator;
//		private RequiredFieldValidator	_confirmRequiredValidator;
		private ValidEmail				_emailValidator;
		private HtmlInputFile			_inputFile;
		private int						_userID;
//		private ValidPassword			_passwordValidator;
		private string					_userAction;
		private TextBox					_aliasTextBox;
//		private TextBox					_confirmTextBox;
		private TextBox					_emailTextBox;
//		private TextBox					_passwordTextBox;

		public UserSettings(WebSolutionControl webSolutionControl, string userAction) : base(webSolutionControl)
		{
			_userAction = userAction;
		}

		private string UploadAvatar()
		{
			// This function tries to save any uploaded avatar to the web server.
			// The core code is put in a try/catch block in case permissions on the
			// web server do not allow the avatar to be saved.  In order to guarantee
			// the avatar filename is unique, it will be prepended with the user ID
			// and an underscore.
			string filename = string.Empty;

			if (_inputFile.PostedFile != null)
			{
				try
				{
					filename = Path.GetFileName(_inputFile.PostedFile.FileName);

					if (filename != string.Empty)
					{
						filename = _userID.ToString() + "_" + filename;

						_inputFile.PostedFile.SaveAs(GetAvatarPath(filename));
					}
				}
				catch (Exception e)
				{
					string message = e.Message;
					filename = string.Empty;
				}
			}

			return filename;
		}

		private void DeletePreviousAvatar(string previousFilename)
		{
			User user = UserDB.GetUser(_userID);

			if (user.Avatar != string.Empty && user.Avatar != previousFilename)
			{
				string avatarPath = GetAvatarPath(user.Avatar);

				try
				{
					File.Delete(avatarPath);
				}
				catch (Exception)
				{
				}
			}
		}

		private void SettingsClicked()
		{
			// Only update a logged on user if all validators are valid
			if (Page.IsValid)
			{
				_userID = LoggedOnUserID;

				if (_userID > 0)
				{
					// Update user details
					string avatar = UploadAvatar();
					if (avatar != string.Empty)
						DeletePreviousAvatar(avatar);

					// Populate user object with information that will be changed
					User user = new User();
					user.UserID		= _userID;
					user.Alias		= _aliasTextBox.Text;
					user.Email		= _emailTextBox.Text;
					// CHANGED by Arthur Zaczek
					user.Password	= "thereisnopassword"; //_passwordTextBox.Text;
					user.Avatar		= avatar;

					// Update the user
					UserDB.UpdateUser(user);

					// Redirect if return URL specified
					RedirectReturnURL();
				}
			}
		}

		private void JoinClicked()
		{
			// Only add new user if all validators are valid
			if (Page.IsValid)
			{
				// Create new user
				User user = new User();

				user.Alias		= _aliasTextBox.Text;
				user.Email		= _emailTextBox.Text;
//				user.Password	= _passwordTextBox.Text;
				user.WebID		= WebID;

				_userID = UserDB.AddUser(user);

				if (_userID > 0)
				{
					// Avatar considerations
					string avatar = UploadAvatar();
					if (avatar != string.Empty)
					{
						user.Avatar = avatar;
						UserDB.UpdateUser(user);
					}

					// Log user on using forums authentication and redirect if return URL specified
					FormsAuthentication.SetAuthCookie(_userID.ToString(), _rememberMeCheckBox.Checked);
					RedirectReturnURL();
				}
			}
		}

		private void SubmitButton_Click(object sender, System.EventArgs e)
		{
			// Action will depend on whether user is creating an account or changing their profile
			if (_userAction == "settings")
				SettingsClicked();
			else
				JoinClicked();
		}

		private void PopulateUserSettings()
		{
			// Initially populate units with user's profile settings
			int userID = LoggedOnUserID;

			User user = UserDB.GetUser(userID);

			_aliasValidator.Allow = user.Alias;
			_emailValidator.Allow = user.Email;

			_emailTextBox.Text = user.Email;
			_aliasTextBox.Text = user.Alias;
			// CHANGED by Arthur Zaczek
//			_passwordTextBox.Text = user.Password;
//			_confirmTextBox.Text = user.Password;

			_rememberMeCheckBox.Visible = false;
		}

		public override void CreateChildControls()
		{
			// If user trying to visit settings screen, they must be logged on
			if (_userAction == "settings" && LoggedOnUserID == 0)
				RedirectToLoginPage();

			// Alias text box
			_aliasTextBox = new TextBox();
			_aliasTextBox.ID = "_aliasTextBox";
			_aliasTextBox.Width = Unit.Pixel(150);
			_aliasTextBox.MaxLength = 100;
			_aliasTextBox.CssClass = "WebSolutionFormControl";

			// Alias validator
			_aliasValidator = new ValidAlias();
			_aliasValidator.ControlToValidate = "_aliasTextBox";
			_aliasValidator.CssClass = "WebSolutionFormControl";

			// Email entry row
			_emailTextBox = new TextBox();
			_emailTextBox.ID = "_emailTextBox";
			_emailTextBox.Width = Unit.Pixel(150);
			_emailTextBox.MaxLength = 100;
			_emailTextBox.CssClass = "WebSolutionFormControl";

			// Email validator
			_emailValidator = new ValidEmail();
			_emailValidator.ControlToValidate = "_emailTextBox";
			_emailValidator.CssClass = "WebSolutionFormControl";

			// CHANGED by Arthur Zaczek
/**
			// Password entry row
			_passwordTextBox = new TextBox();
			_passwordTextBox.ID = "_passwordTextBox";
			_passwordTextBox.Width = Unit.Pixel(150);
			_passwordTextBox.MaxLength = 50;
			_passwordTextBox.TextMode = TextBoxMode.Password;
			_passwordTextBox.CssClass = "WebSolutionFormControl";

			// Password validator
			_passwordValidator = new ValidPassword();
			_passwordValidator.ControlToValidate = "_passwordTextBox";
			_passwordValidator.CssClass = "WebSolutionFormControl";

			// Password confirm entry row
			_confirmTextBox = new TextBox();
			_confirmTextBox.ID = "_confirmTextBox";
			_confirmTextBox.Width = Unit.Pixel(150);
			_confirmTextBox.MaxLength = 50;
			_confirmTextBox.TextMode = TextBoxMode.Password;
			_confirmTextBox.CssClass = "WebSolutionFormControl";*/

			// Confirm validator
			_confirmValidator = new CompareValidator();
			_confirmValidator.CssClass = "WebSolutionFormControl";
			_confirmValidator.ControlToValidate = "_confirmTextBox";
			_confirmValidator.ControlToCompare = "_passwordTextBox";
			_confirmValidator.ErrorMessage = "Passwords must be identical.";
			_confirmValidator.Display = ValidatorDisplay.Dynamic;

/*			// Confirm required field validator
			_confirmRequiredValidator = new RequiredFieldValidator();
			_confirmRequiredValidator.CssClass = "WebSolutionFormControl";
			_confirmRequiredValidator.ControlToValidate = "_confirmTextBox";
			_confirmRequiredValidator.ErrorMessage = "Please enter a password.";
			_confirmRequiredValidator.Display = ValidatorDisplay.Dynamic;*/

			// Remember me check box
			_rememberMeCheckBox = new CheckBox();
			_rememberMeCheckBox.Text = "Remember me";
			_rememberMeCheckBox.CssClass = "WebSolutionFormControl";

			// Submit button
			_submitButton = new Button();
			_submitButton.Text = _userAction == "settings" ? "Save" : "Create Account";
			_submitButton.Click += new System.EventHandler(SubmitButton_Click);
			_submitButton.CssClass = "WebSolutionFormControl";

			// Avatar
			_inputFile = new HtmlInputFile();
			_inputFile.ID = "_inputFile";

			// Avatar validator
			_avatarValidator = new ValidAvatar();
			_avatarValidator.ControlToValidate = "_inputFile";
			_avatarValidator.InputFile = _inputFile;
			_avatarValidator.CssClass = "WebSolutionFormControl";

			// Add child controls
			Controls.Add(_aliasTextBox);
			Controls.Add(_aliasValidator);
			Controls.Add(_emailTextBox);
			Controls.Add(_emailValidator);
//			Controls.Add(_passwordTextBox);
//			Controls.Add(_passwordValidator);
//			Controls.Add(_confirmTextBox);
//			Controls.Add(_confirmValidator);
//			Controls.Add(_confirmRequiredValidator);
			Controls.Add(_rememberMeCheckBox);
			Controls.Add(_submitButton);
			Controls.Add(_inputFile);
			Controls.Add(_avatarValidator);

			// Populate controls with user settings?
			if (_userAction == "settings")
				PopulateUserSettings();
		}

		private void RenderRow(HtmlTextWriter writer, string label, TextBox textBox, BaseValidator validator1, BaseValidator validator2)
		{
			// Row
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			// Label
			writer.AddAttribute(HtmlTextWriterAttribute.Wrap, "false");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			writer.Write(string.Format("{0}&nbsp;&nbsp;", label));
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td

			// Text box
			writer.AddAttribute(HtmlTextWriterAttribute.Wrap, "false");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			textBox.RenderControl(writer);
			writer.RenderEndTag();	// Td

			// Gap
			writer.AddAttribute(HtmlTextWriterAttribute.Wrap, "false");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;&nbsp;");
			writer.RenderEndTag();	// Td

			// Validator
			writer.AddAttribute(HtmlTextWriterAttribute.Wrap, "false");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			validator1.RenderControl(writer);
			if (validator2 != null)
				validator2.RenderControl(writer);
			writer.RenderEndTag();	// Td

			// Close out row
			writer.RenderEndTag();	// Tr
		}

		private void RenderForm(HtmlTextWriter writer)
		{
			// Start table into which various form elements will be rendered
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Standard rows
			RenderRow(writer, "Alias:", _aliasTextBox, _aliasValidator, null);
			RenderRow(writer, "E-mail:", _emailTextBox, _emailValidator, null);
//			RenderRow(writer, "Password:", _passwordTextBox, _passwordValidator, null);
//			RenderRow(writer, "Confirm&nbsp;Password:", _confirmTextBox, _confirmValidator, _confirmRequiredValidator);

			// Remember me check box
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_rememberMeCheckBox.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Single row gap
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Avatar
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderBeginTag(HtmlTextWriterTag.B);
			writer.Write("Avatar&nbsp;&nbsp;");
			writer.RenderEndTag();	// B
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_inputFile.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Avatar validator
			if (!_avatarValidator.IsValid)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag();	// Td
				writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
				writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				_avatarValidator.RenderControl(writer);
				writer.RenderEndTag();	// Td
				writer.RenderEndTag();	// Tr
			}

			// Single row gap
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write("&nbsp;");
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			// Submit button
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.RenderEndTag();	// Td
			writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			_submitButton.RenderControl(writer);
			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr

			writer.RenderEndTag(); // Table
		}

		public override void Render(HtmlTextWriter writer)
		{
			RenderTableBegin(writer, 1, 0);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// CHANGED by Arthur Zaczek
			// On post back, display whether user action was successful
/*			if (Page.IsPostBack)
			{
				// Start paragraph
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
				writer.RenderBeginTag(HtmlTextWriterTag.P);

				if (_userID > 0)
				{
					// Valid attempt
					if (_userAction == "settings")
						writer.Write("User settings changed.");
					else
						writer.Write("User account created.");
				}
				else
				{
					// Something went wrong
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSolutionErrorText");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					writer.Write("A problem was encountered.  Please try again.");
					writer.RenderEndTag();	// Span
				}

				// End paragraph
				writer.RenderEndTag();	// P
			}*/

			// Render main form area
			RenderForm(writer);

			writer.RenderEndTag();	// Td
			writer.RenderEndTag();	// Tr
			RenderTableEnd(writer);		// Table
		}
	}
}
