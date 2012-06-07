using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ValidAlias : BaseValidator, INamingContainer
	{
		private string	_allowedAlias;

		public ValidAlias()
		{
		}

		protected override bool EvaluateIsValid()
		{
			string alias = GetControlValidationValue(ControlToValidate);

			if (alias.Length < 1 || alias.Length > 100)
			{
				if (alias.Length == 0)
					ErrorMessage = "Please enter an alias.";
				else
					ErrorMessage = "Please enter an alias between 1 and 100 characters long.";
				return false;
			}

			if (alias == _allowedAlias)
				return true;

			string folder;
			int webID = WebSolutionDB.GetWebIDAndFolder(Page.Request.Url.Host, out folder);

			if (UserDB.AliasExists(alias, webID))
			{
				ErrorMessage = "Alias already in-use.";
				return false;
			}

			return true;
		}

		public string Allow
		{
			get
			{
				return _allowedAlias;
			}
			set
			{
				_allowedAlias = value;
			}
		}
	}
}
