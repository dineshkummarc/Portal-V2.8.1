using System;
using System.Collections;
using System.Globalization;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Security;
using System.Security.Principal;

using Portal.API;
using Portal.API.Statistics.Service;

namespace Portal.API
{
	/// <summary>
	/// Helper Class for the Portals User Management.
	/// </summary>
	public sealed class UserManagement
	{
        /// <summary>
        /// UserManagement-Class must not be instantiated, so set the constructor to private.
        /// </summary>
        private UserManagement() { }

		/// <summary>
		/// Returns the Users Dataset
		/// </summary>
		/// <returns>Users Dataset</returns>
		public static Users Users
		{
            get
            {
                // Lookup in Cache
                Users u = (Users)System.Web.HttpContext.Current.Cache["Users"];
                if (u != null) return u;

                u = new Users();
                u.ReadXml(Config.UserListPhysicalPath);

                // Add to Cache
                System.Web.HttpContext.Current.Cache.Insert("Users", u,
                    new System.Web.Caching.CacheDependency(Config.UserListPhysicalPath));

                return u;
            }
		}

		/// <summary>
		/// Returns an Identityobject for the given account
		/// </summary>
		/// <param name="account">User Account/Login</param>
		/// <returns>Identity</returns>
		public static Portal.API.Principal GetUser(string account)
		{
            if (null == account)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "account");
            
            Users u = Users;

			Users.UserRow user = u.User.FindBylogin(account.ToLower(CultureInfo.CurrentCulture));
			if(user == null) return null;

			IIdentity UsrIdentity = new GenericIdentity(user.login, HttpContext.Current.User.Identity.AuthenticationType);
      Portal.API.Principal UsrPrincipal = new Portal.API.Principal(UsrIdentity, GetRoles(account));
      UsrPrincipal.Id = user.id;
      UsrPrincipal.FirstName = user.IsfirstNameNull() ? "" : user.firstName;
      UsrPrincipal.SurName = user.IssurNameNull() ? "" : user.surName;
      UsrPrincipal.EMail = user.IsemailNull() ? "" : user.email;

      return UsrPrincipal;
		}

		/// <summary>
		/// Saves the Users Dataset
		/// </summary>
		/// <param name="u">Users Dataset</param>
		public static void SetUsers(Users u)
		{
            if (null == u)
                throw new ArgumentNullException("u");

			u.WriteXml(Config.UserListPhysicalPath);
			System.Web.HttpContext.Current.Cache.Remove("Users");
		}

		/// <summary>
		/// Performs the Login.
		/// </summary>
		/// <param name="account">Users account</param>
		/// <param name="password">Users password</param>
		/// <returns>true if the credentials are valid</returns>
		public static bool Login(string account, string password)
		{
            if (null == account)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "account");
            
            Users u = Users;

			Users.UserRow user = u.User.FindBylogin(account.ToLower(CultureInfo.CurrentCulture));
			if(user == null) return false;
			if(user.password != password) return false;

            // Add login to the statistics.
            LoginStatisticService service = (LoginStatisticService)Portal.API.Statistics.Statistic.GetService(typeof(LoginStatisticService));
            if (null != service)
                service.AddLogin(HttpContext.Current, user.id);

			FormsAuthentication.SetAuthCookie(account, false);
			return true;
		}

		/// <summary>
		/// Returns the current Users Roles.
		/// </summary>
		/// <param name="account">Users account</param>
		/// <returns>string array of the users roles. Returns a empty array if the user is not found</returns>
		public static string[] GetRoles(string account)
		{
            if (null == account)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "account");
            
            Users u = Users;

			Users.UserRow user = u.User.FindBylogin(account.ToLower(CultureInfo.CurrentCulture));
			if(user == null) return new string[0];

			Users.UserRoleRow[] roles = user.GetUserRoleRows();
			string[] result = new string[roles.Length];
			for(int i=0;i<roles.Length;i++)
			{
				result[i] = roles[i].RoleRow.name;
			}

			return result;
		}

		/// <summary>
		/// Saves a single User. Do not use this Method in combination with GetUsers/SetUsers!
		/// </summary>
		/// <param name="account">Users Account. If it does not exists a new User is created</param>
		/// <param name="password">Users password</param>
		/// <param name="firstName">Users First Name</param>
		/// <param name="surName">Users Sur Name</param>
		/// <param name="roles">ArrayList of Roles</param>
		/// <param name="userId">Users Id</param>
		public static void SaveUser(string account, string password, string firstName, string surName, string email, ArrayList roles, Guid userId)
		{
            if (null == account)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "account");
            if (null == roles)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "roles");

			Users u = Users;

			Users.UserRow user = u.User.FindBylogin(account.ToLower(CultureInfo.CurrentCulture));
			if(user == null)
			{
				user = u.User.AddUserRow(account, password, firstName, surName, email, userId);
			}
			else
			{
				if(!string.IsNullOrEmpty(password))
				{
					user.password = password;
				}
				user.firstName = firstName;
				user.surName = surName;
				user.email = email;
				user.id = userId;
			}

			// Delete old Roles
			foreach(Users.UserRoleRow r in user.GetUserRoleRows())
			{
				r.Delete();
			}

			// Add new Roles
			foreach(string newRole in roles)
			{
				u.UserRole.AddUserRoleRow(u.Role.FindByname(newRole), user);
			}
			

			SetUsers(u);
		}

		/// <summary>
		/// Deletes a single user. Do not use this Method in combination with GetUsers/SetUsers!
		/// </summary>
		/// <param name="account"></param>
		public static void DeleteUser(string account)
		{
            if (null == account)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "account");

			Users u = Users;
			Users.UserRow user = u.User.FindBylogin(account.ToLower(CultureInfo.CurrentCulture));
			if(user == null)
			{
				throw new PortalException(Language.GetText("exception_UserNotFound"));
			}
			if(string.Compare(account, API.Config.AdminRole, true, CultureInfo.CurrentCulture) == 0)
			{
				throw new PortalException(Language.GetText("exception_DeletingOfAdminNotAllowed"));
			}
			user.Delete();
			SetUsers(u);
		}

		/// <summary>
		/// Checks if a user has View Rights on a Tab or Module
		/// </summary>
		/// <param name="user">User Principal Object</param>
		/// <param name="roles">ArrayList with the users Roles</param>
		/// <returns>true if the user has View Rights</returns>
		public static bool HasViewRights(IPrincipal user, ArrayList roles)
		{
            if (null == user)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "user");
            if (null == roles)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "roles");

            if (user.IsInRole(Config.AdminRole)) return true;

			foreach(Role r in roles)
			{
				if(r.name == Config.EveryoneRole) return true;
				if(r.name == Config.AnonymousRole && !user.Identity.IsAuthenticated) return true;
				if(r.name == Config.UserRole && user.Identity.IsAuthenticated) return true;
				if(user.IsInRole(r.name))
				{
					return true;
				}
			}
			
			return false;
		}
		/// <summary>
		/// Checks if a user has Edit Rights on a Tab or Module
		/// </summary>
		/// <param name="user">User Principal Object</param>
		/// <param name="roles">ArrayList with the users Roles</param>
		/// <returns>true if the user has Edit Rights</returns>
		public static bool HasEditRights(IPrincipal user, ArrayList roles)
		{
            if (null == user)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "user");
            if (null == roles)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "roles");

            if (user.IsInRole(Config.AdminRole)) return true;

			foreach(Role r in roles)
			{
				EditRole er = r as EditRole;
				if(er != null)
				{
					if(er.name == Config.EveryoneRole) return true;
					if(er.name == Config.AnonymousRole && !user.Identity.IsAuthenticated) return true;
					if(r.name == Config.UserRole && user.Identity.IsAuthenticated) return true;
					if(user.IsInRole(er.name))
					{
						return true;
					}
				}
			}
			
			return false;
		}
	}
}
