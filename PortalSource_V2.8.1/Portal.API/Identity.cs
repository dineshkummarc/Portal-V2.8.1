using System;

namespace Portal.API
{
	/// <summary>
	/// Summary description for Identity.
	/// </summary>
	public class Identity : System.Security.Principal.IIdentity
	{
		private string authenticationType = "";

		private string id = "";
		private string login = "";
		private string firstName = "";
		private string surName = "";
		private string email = "";
		private string[] roles = new string[] { };

		public Identity(string id, string login, string authenticationType)
		{
			this.id = id;
			this.login = login;
			this.authenticationType = authenticationType;
		}

		#region Properties
		public string ID
		{
			get
			{
				return id;
			}
		}
		public string Login
		{
			get
			{
				return login;
			}
			set
			{
				login = value;
			}
		}
		public string FirstName
		{
			get
			{
				return firstName;
			}
			set
			{
				firstName = value;
			}
		}
		public string SurName
		{
			get
			{
				return surName;
			}
			set
			{
				surName = value;
			}
		}
		public string FullName
		{
			get
			{
				return FirstName + " " + SurName;
			}
		}
		public string EMail
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
			}
		}
		public string[] Roles
		{
			get
			{
				return roles;
			}
			set
			{
				roles = value;
			}
		}
		#endregion

		#region IIdentity Members

		public bool IsAuthenticated
		{
			get
			{
				return true;
			}
		}

		public string Name
		{
			get
			{
				return login;
			}
		}

		public string AuthenticationType
		{
			get
			{
				return authenticationType;
			}
		}

		#endregion
	}
}
