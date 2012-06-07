using System;

namespace RiversideInternet.WebSolution
{
	public class User
	{
		private int		_postCount;
		private int		_userID;
		private int		_webID;
		private string	_alias;
		private string	_avatar;
		private string	_email;
		private string	_password;
		private string	_roles;

		public User()
		{
		}

		public int PostCount
		{
			get
			{
				return _postCount;
			}
			set
			{
				_postCount = value;
			}
		}

		public int UserID
		{
			get
			{
				return _userID;
			}
			set
			{
				_userID = value;
			}
		}

		public int WebID
		{
			get
			{
				return _webID;
			}
			set
			{
				_webID = value;
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

		public string Avatar
		{
			get
			{
				return _avatar;
			}
			set
			{
				_avatar = value;
			}
		}

		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public string Roles
		{
			get
			{
				return _roles;
			}
			set
			{
				_roles = value;
			}
		}
	}
}
