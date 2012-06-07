using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Portal.API;

namespace Portal
{
  /// <summary>
  /// Entry Point for Download of Files, i.e. Files from a protected directory.
  /// </summary>
  public partial class Download : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      ApplyAutoLogin();

      string moduleType = Server.UrlDecode(Request.QueryString["Type"]);
      string moduleRef = Server.UrlDecode(Request.QueryString["Module"]);

      // Check the required attributes and redirect, if not specified.
      if(String.IsNullOrEmpty(moduleType) || String.IsNullOrEmpty(moduleRef))
        Response.Redirect(Portal.API.Config.MainPage);

      // Check if the user has access to the Module.
      PortalDefinition portDef = PortalDefinition.Load();
      if(CheckAccess(portDef.tabs, moduleType, moduleRef))
      {
        IDownloadProcessor downloadObj = null;

        // Hardcoded at the moment. Possible replacement with dynamic creation i.e. the activator class.
        if (0 == "FileBrowser".CompareTo(Request.QueryString["Type"]))
        {
          downloadObj = new Portal.Modules.FileBrowser.Download();  
        }

        // Process the Download.
        if (downloadObj != null)
        {
          Response.Clear();
          try
          {
            downloadObj.Process(Request, Response, moduleType, moduleRef);
          }
          catch(Exception exc)
          {
            Response.Clear();
            errorMessage.Text = exc.Message;
            return;
          }
          Response.End();
          return;
        }
      }
      errorMessage.Text = "File not found or access denied";
    }

    private void ApplyAutoLogin()
    {
      // If a user did use the auto-login feature, we can use the login cooky and do a login.
      if (!Page.User.Identity.IsAuthenticated)
      {
        HttpCookie cookie = (HttpCookie)Request.Cookies["PortalUser"];
        if (cookie != null)
        {
          try
          {
            if (UserManagement.Login(Crypto.Decrypt(cookie.Values["AC"]), Crypto.Decrypt(cookie.Values["PW"])))
              Response.Redirect(Request.RawUrl, true);
          }
          catch (Exception) { }
        }
      }
    }

    /// <summary>
    /// Check if the user has access to the modules in the list (recursive).
    /// </summary>
    /// <param name="tabList"></param>
    /// <param name="modType"></param>
    /// <returns></returns>
    private bool CheckAccess(ArrayList tabList, string modType, string modRef)
    {
      bool allowed = false;
      foreach (PortalDefinition.Tab portalTab in tabList)
      {
        // Check subtabs recursive.
        if(!allowed)
          allowed = CheckAccess(portalTab.tabs, modType, modRef);
        
        if(!allowed)
          allowed = CheckAccessModules(portalTab.left, modType, modRef);
        if (!allowed)
          allowed = CheckAccessModules(portalTab.middle, modType, modRef);
        if (!allowed)
          allowed = CheckAccessModules(portalTab.right, modType, modRef);

        if (allowed)
          return true;  // Shortcut.
      }
      return allowed;
    }

    private bool CheckAccessModules(ArrayList modules, string modType, string modRef)
    {
      foreach (PortalDefinition.Module md in modules)
      {
        md.LoadModuleSettings();
        if ((md.type == modType) && (md.reference == modRef))
        {
          // Check if the user has View Rights. It verifys only the module itself, not the parent tabs. This enables
          // to download a file from a list, without having access to the list itself. (For instance, the user can
          // download files linked in a page. The files are managed in a list, but has no possibility to see the
          // list itself, because he has no access to the tab.
          if (UserManagement.HasViewRights(Page.User, md.roles))
            return true;  // A module with View Access found.
        }
      }
      return false;       // No Module with View Access found.
    }
  }
}