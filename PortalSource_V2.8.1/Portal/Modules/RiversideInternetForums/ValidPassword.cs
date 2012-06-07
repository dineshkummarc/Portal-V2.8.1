using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ValidPassword : BaseValidator, INamingContainer
	{
		public ValidPassword()
		{
		}

		protected override bool EvaluateIsValid()
		{
			string password = GetControlValidationValue(ControlToValidate);

			if (password.Length < 3 || password.Length > 50)
			{
				if (password.Length == 0)
					ErrorMessage = "You must enter a password.";
				else
					ErrorMessage = "Your password must be between 3 and 50 characters long.";
				return false;
			}

			return true;
		}
	}
}
