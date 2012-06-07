using System;
using System.Web;
using System.Globalization;

namespace Portal.API
{  
  /// <summary>
	/// Configuration Management Class
	/// </summary>
	public sealed class Config
	{
        /// <summary>
        /// Config-Class must not be instantiated, so set the constructor to private.
        /// </summary>
        private Config() { }

		/// <summary>
		/// Administrator Role. May edit/view everything
		/// </summary>
		public const string AdminRole = "Admin";
		/// <summary>
		/// Signed in User
		/// </summary>
		public const string UserRole = "User";
		/// <summary>
		/// Signed in User or Anonymous User
		/// </summary>
		public const string EveryoneRole = "Everyone";
		/// <summary>
		/// Not signed in User
		/// </summary>
		public const string AnonymousRole = "Anonymous";

		/// <summary>
		/// Returns the virtual Modules Path
		/// </summary>
		/// <returns>~/Modules/</returns>
		public static string GetModuleVirtualPath()
		{
			return "~/Modules/";
		}
		/// <summary>
		/// Returns the physical Module Path. Uses the "GetModuleVirtualPath()" Method
		/// </summary>
		/// <returns>[Application Base Path]/Modules</returns>
		public static string GetModulePhysicalPath()
		{
      String ModPhysPath = HttpContext.Current.Server.MapPath(GetModuleVirtualPath());
      if(!ModPhysPath.EndsWith("\\"))     // XP Home does not include a trailing backslash.
        ModPhysPath += "\\";
			return ModPhysPath;
		}
		/// <summary>
		/// Returns the virtual Path of a Module. Uses the "GetModuleVirtualPath()" method 
		/// </summary>
		/// <param name="ctrlType">Module Type</param>
		/// <returns>~/Module/[ctrlType]/</returns>
		public static string GetModuleVirtualPath(string ctrlType)
		{
			return GetModuleVirtualPath() + ctrlType + "/";
		}
		/// <summary>
		/// Returns the physical Path of a Module. Uses the "GetModuleVirtualPath()" method 
		/// </summary>
		/// <param name="ctrlType"></param>
		/// <returns></returns>
		public static string GetModulePhysicalPath(string ctrlType)
		{
			String ModPhysPath = HttpContext.Current.Server.MapPath(GetModuleVirtualPath(ctrlType));
      if (!ModPhysPath.EndsWith("\\"))     // XP Home does not include a trailing backslash.
        ModPhysPath += "\\";
      return ModPhysPath;
		}
    /// <summary>
    /// Returns the Virtual Path to the Data.
    /// </summary>
    /// <returns></returns>
    public static string GetModuleDataVirtualPath()
    {
      return System.Configuration.ConfigurationManager.AppSettings["ConfigDataPath"];
    }
    /// <summary>
    /// Returns the Physical Path to the Data. Uses the "GetModuleDataVirtualPath()" method.
    /// </summary>
    /// <returns>[Application Base Path]/Modules</returns>
    public static string GetModuleDataPhysicalPath()
    {
      String ModPhysPath = HttpContext.Current.Server.MapPath(GetModuleDataVirtualPath());
      if (!ModPhysPath.EndsWith("\\"))     // XP Home does not include a trailing backslash.
        ModPhysPath += "\\";
      return ModPhysPath;
    }
    /// <summary>
    /// Returns the Virtual Path to the Data. Uses the "GetModuleDataVirtualPath()" method.
    /// </summary>
    /// <returns></returns>
    public static string GetModuleDataVirtualPath(string ctrlType)
    {
      return GetModuleDataVirtualPath() + ctrlType + "/"; ;
    }
    /// <summary>
    /// Returns the physical Path of a Module. Uses the "GetModuleDataVirtualPath()" method 
    /// </summary>
    /// <param name="ctrlType"></param>
    /// <returns></returns>
    public static string GetModuleDataPhysicalPath(string ctrlType)
    {
      String ModPhysPath = HttpContext.Current.Server.MapPath(GetModuleDataVirtualPath(ctrlType));
      if (!ModPhysPath.EndsWith("\\"))     // XP Home does not include a trailing backslash.
        ModPhysPath += "\\";
      return ModPhysPath;
    }

		public static string GetModuleLanguagePhysicalPath(string ctrlType, string language)
		{
			if(string.IsNullOrEmpty(language))
				return GetModulePhysicalPath(ctrlType) + @"\Language.config";
			else
				return GetModulePhysicalPath(ctrlType) + @"\Language." + language + ".config";
		}

		/// <summary>
		/// Returns the physical path to the Portal Definition File
		/// </summary>
		/// <returns>[Application Base Path]/Portal.config</returns>
		public static string PortalDefinitionPhysicalPath
		{
            get
            {
                return HttpContext.Current.Server.MapPath(GetModuleDataVirtualPath() + "Portal.config");
            }
		}
		/// <summary>
		/// Returns the physical path to the User List File
		/// </summary>
		/// <returns>[Application Base Path]/Users.config</returns>
		public static string UserListPhysicalPath
		{
            get
            {
                return HttpContext.Current.Server.MapPath(GetModuleDataVirtualPath() + "Users.config");
            }
		}

		public static string GetLanguagePhysicalPath(string language)
		{
			if(string.IsNullOrEmpty(language))
				return HttpContext.Current.Server.MapPath("~/Language.config");
			else
				return HttpContext.Current.Server.MapPath("~/Language." + language + ".config");
		}

		/// <summary>
		/// Returns the Main Render Page.
		/// </summary>
		public static string MainPage
		{
      get
      {
        return "default.aspx";
      }
		}

		public static string GetTabUrl(string tabRef)
		{
      bool tabRefValid = !string.IsNullOrEmpty(tabRef);
			if(UseTabHttpModule && tabRefValid)
			{
         return tabRef + ".tab.aspx";
			}
      else if (!tabRefValid)
      {
        return MainPage;
      }
      else
      {
        return MainPage + "?TabRef=" + tabRef;
      }
		}

		/// <summary>
		/// Use the Tab HTTP Module. 
		/// The Tab ID will be passed as a "Page", not encoded in the URL
		/// </summary>
		public static bool UseTabHttpModule
		{
			get
			{
                string useTabHttpModule;
                try
                {
                    useTabHttpModule = System.Configuration.ConfigurationManager.AppSettings["UseTabHttpModule"];
                }
                catch (System.Configuration.ConfigurationErrorsException) { return false; }

                try
				{
                    return Boolean.Parse(useTabHttpModule);
				}
                catch (ArgumentNullException) { return false; }
                catch (FormatException) { return false; }
			}
		}

		/// <summary>
		/// Show SubTabs in the Tab Menu
		/// </summary>
		public static bool TabMenuShowSubTabs
		{
			get
			{
                string tabMenuShowSubTabs;
                try
                {
                    tabMenuShowSubTabs = System.Configuration.ConfigurationManager.AppSettings["TabMenuShowSubTabs"];
                }
                catch (System.Configuration.ConfigurationErrorsException) { return false; }

                try
				{
                    return Boolean.Parse(tabMenuShowSubTabs);
				}
                catch (ArgumentNullException) { return false; }
                catch (FormatException) { return false; }
			}
		}		

		/// <summary>
		/// Log the Requests URLReferrer Property a startup.
		/// </summary>
		public static bool LogUrlReferrer
		{
			get
			{
                string logUrlReferrer;
                try
                {
                    logUrlReferrer = System.Configuration.ConfigurationManager.AppSettings["LogUrlReferrer"];
                }
                catch (System.Configuration.ConfigurationErrorsException) { return false; }

				try
				{
                    return Boolean.Parse(logUrlReferrer);
				}
                catch (ArgumentNullException) { return false; }
                catch (FormatException) { return false; }
            }
		}
		/// <summary>
		/// Displays Module Exceptions
		/// </summary>
		public static bool ShowModuleExceptions
		{
			get
			{
                string showModuleExceptions;
                try
                {
                    showModuleExceptions = System.Configuration.ConfigurationManager.AppSettings["ShowModuleExceptions"];
                }
                catch (System.Configuration.ConfigurationErrorsException) { return false; }

                try
                {
                    return Boolean.Parse(showModuleExceptions);
                }
                catch (ArgumentNullException) { return false; }
                catch (FormatException) { return false; }
			}
		}


    /// <summary>
    /// The Configured Date-Time Format.
    /// </summary>
    public static string DateTimeFormat
    {
      get
      {
        // Datetimeformat override in the settings.
        string format = System.Configuration.ConfigurationManager.AppSettings["DateTimeFormat"];
        if (string.IsNullOrEmpty(format))
        {
          // Apply default Format.
          DateTimeFormatInfo dtFormatInfo = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
          format = dtFormatInfo.ShortDatePattern + " " + dtFormatInfo.ShortTimePattern;
        }
        return format;
      }
    }


    /// <summary>
    /// The Configured Date Format.
    /// </summary>
    public static string DateFormat
    {
      get
      {
        // Datetimeformat override in the settings.
        string format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
        if (string.IsNullOrEmpty(format))
        {
          // Apply default Format.
          DateTimeFormatInfo dtFormatInfo = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
          format = dtFormatInfo.ShortDatePattern;
        }
        return format;
      }
    }
	}
}
