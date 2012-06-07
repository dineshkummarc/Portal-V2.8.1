using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ValidEmail : BaseValidator
	{
		private string	_allowedEmail;

		public ValidEmail()
		{
		}

		protected override bool EvaluateIsValid()
		{
			string email = GetControlValidationValue(ControlToValidate);

			if (email.Length < 1 || email.Length > 100)
			{
				if (email.Length == 0)
					ErrorMessage = "You must enter an e-mail address.";
				else
					ErrorMessage = "Your e-mail address must be between 1 and 100 characters long.";
				return false;
			}

			if (email == _allowedEmail)
				return true;

			string folder;
			int webID = WebSolutionDB.GetWebIDAndFolder(Page.Request.Url.Host, out folder);

			if (UserDB.EmailExists(email, webID))
			{
				ErrorMessage = "This e-mail is already in-use.";
				return false;
			}

			return true;
		}

		public string Allow
		{
			get
			{
				return _allowedEmail;
			}
			set
			{
				_allowedEmail = value;
			}
		}
	}
}
