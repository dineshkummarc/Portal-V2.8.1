using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace RiversideInternet.WebSolution
{
	public class LoginControl : WebSolutionControl, INamingContainer
	{
		protected override void CreateObject()
		{
			WebSolutionObject = new LoginControlObject(this);
		}
	}
}
