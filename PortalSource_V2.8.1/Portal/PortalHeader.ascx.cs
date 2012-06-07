using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Portal.API;
using Portal;
using System.IO;

namespace Portal
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für PortalHeader
  /// </summary>
  public partial class PortalHeader : System.Web.UI.UserControl
  {
    void Page_Load(object sender, EventArgs args)
    {
      // If a user did use the auto-login feature, we can use the login cooky and do a login if the session was timeouted.
      if (!Page.User.Identity.IsAuthenticated)
      {
        HttpCookie cookie = (HttpCookie)Request.Cookies["PortalUser"];
        if (cookie != null)
        {
          try
          {
            if (UserManagement.Login(Crypto.Decrypt(cookie.Values["AC"]), Crypto.Decrypt(cookie.Values["PW"])))
              Response.Redirect(Request.RawUrl);
          }
          catch (Exception) { }
        }
      }

      if (Page.User.Identity.IsAuthenticated)
        CommonUtilities.AddLifeguard(this.Page);

      LoadCustomHeader();
    }

    private void LoadCustomHeader()
    {
      const string placeHolder = "{PlaceHolderLogout}";

      // Load File.            
      string headerHtml = string.Empty;
      string fileName = Config.GetModuleDataPhysicalPath() + "PortalHeader.htm";
      if (File.Exists(fileName))
      {
        FileStream fs = File.OpenRead(fileName);
        StreamReader sr = new StreamReader(fs);
        headerHtml = sr.ReadToEnd();
        fs.Close();

        // Find Logout-PlaceHolder Position.
        int placehPos = headerHtml.IndexOf(placeHolder);
        if (-1 == placehPos)
          placehPos = headerHtml.Length;  // No Placeholder found.

        // Add Text up to placeholder position.
        portalHeaderContent.Controls.Add(new LiteralControl(headerHtml.Substring(0, placehPos)));

        // Add Logout Text.
        if (Page.User.Identity.IsAuthenticated)
          portalHeaderContent.Controls.Add(GetUserInfo());

        // Add Rest of Text.
        placehPos += placeHolder.Length;
        if(placehPos < headerHtml.Length)
          portalHeaderContent.Controls.Add(new LiteralControl(headerHtml.Substring(placehPos)));
      }
      else if (Page.User.Identity.IsAuthenticated)
        portalHeaderContent.Controls.Add(GetUserInfo());     
    }

    void OnSignOut(object sender, EventArgs args)
    {
      HttpCookie cookie = (HttpCookie)Request.Cookies["PortalUser"];
      if (cookie != null)
      {
        cookie.Values["AC"] = "";
        cookie.Values["PW"] = "";
        DateTime dt = DateTime.Now;
        dt.AddDays(-1);
        cookie.Expires = dt;
        Response.Cookies.Add(cookie);
      }
      FormsAuthentication.SignOut();
      Response.Redirect(Request.RawUrl);
    }

    Control GetUserInfo()
    {
      HtmlGenericControl userInfoDiv = new HtmlGenericControl("div");
      userInfoDiv.Attributes.Add("class", "PortalHeaderUserInfo");

      System.Web.UI.WebControls.Label userName = new System.Web.UI.WebControls.Label();
      userName.Text = Portal.API.Language.GetText("Welcome") + " " + ((Portal.API.Principal)Page.User).FullName;
      userInfoDiv.Controls.Add(userName);

      userInfoDiv.Controls.Add(new LiteralControl("&nbsp;|&nbsp;"));

      LinkButton lnk = new LinkButton();
      lnk.CssClass = "PortalHeaderLogoff";
      lnk.Click += OnSignOut;
      lnk.Text = Portal.API.Language.GetText("Logoff");
      lnk.CausesValidation = false;
      userInfoDiv.Controls.Add(lnk);

      return userInfoDiv;
    }
  }
}