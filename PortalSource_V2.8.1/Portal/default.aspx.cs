using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.Modules.AdminPortal;
using System.IO;
using Portal.API.Statistics.Service;

namespace Portal
{
  /// <summary>
  /// Startup Page.
  /// </summary>
  public partial class StartPage : System.Web.UI.Page
  {
    protected void Page_Load(object sender, System.EventArgs e)
    {
      // Set the CSS-Path.
      cssLink.Href = Portal.Helper.CssPath;

      Control TabContent = null;
      // Depending on current mode, load the Content Control.
      if (Request["Edit"] == "Tab")
      {
        // Try to load the Tab in Edit Mode.
        Portal.PortalDefinition.Tab EditTab = Helper.GetEditTab(Request["TabRef"]);
        if (EditTab != null)
        {
          Tab TabCtrl = (Tab)LoadControl("Modules/AdminPortal/Tab.ascx");
          TabCtrl.LoadData(EditTab);
          TabContent = TabCtrl;
        }
        else
        {
          // No Access.
          TabContent = null;
        }
      }

      if (TabContent == null)
      {
        // Load Tab in View Mode.
        TabContent = LoadControl("PortalTab.ascx");

        // Add request to the statistics.
        if (!IsPostBack)
        {
          RequestStatisticService service = (RequestStatisticService)Portal.API.Statistics.Statistic.GetService(typeof(RequestStatisticService));
          if (null != service)
            service.AddRequest(Context);
        }
      }
      TabContent.ID = "Content";
      ContentPlace.Controls.Add(TabContent);

      // Set the name of the website if available
      try
      {
        string title = System.Configuration.ConfigurationManager.AppSettings["Title"];
        if (!string.IsNullOrEmpty(title))
          this.Title = HttpUtility.HtmlEncode(title);
        else
          this.Title = "Personal .NET Portal";
      }
      catch (System.Configuration.ConfigurationErrorsException) { }
      catch (ArgumentNullException) { }
      catch (FormatException) { }

      // Set the portal footer visible depending on the web config settings.
      try
      {
        PortalFooter.Visible = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["UsePortalFooter"]);
      }
      catch (System.Configuration.ConfigurationErrorsException) { }
      catch (ArgumentNullException) { }
      catch (FormatException) { }

      // Set the favicon visible if the file exists.
      try
      {
        string faviconPath = System.Configuration.ConfigurationManager.AppSettings["FavIconPath"];
        favicon.Href = faviconPath;
        favicon.Visible = !string.IsNullOrEmpty(faviconPath);
      }
      catch (System.Configuration.ConfigurationErrorsException) { }

      try
      {
        string description = System.Configuration.ConfigurationManager.AppSettings["Description"];
        metaDescription.Content = description;
        metaDescription.Visible = !string.IsNullOrEmpty(description);
      }
      catch (System.Configuration.ConfigurationErrorsException) { }

      try
      {
        string author = System.Configuration.ConfigurationManager.AppSettings["Author"];
        metaAuthor.Content = author;
        metaAuthor.Visible = !string.IsNullOrEmpty(author);
      }
      catch (System.Configuration.ConfigurationErrorsException) { }

      try
      {
        string keywords = System.Configuration.ConfigurationManager.AppSettings["Keywords"];
        metaKeywords.Content = keywords;
        metaKeywords.Visible = !string.IsNullOrEmpty(keywords);
      }
      catch (System.Configuration.ConfigurationErrorsException) { }

      try
      {
        string robots = System.Configuration.ConfigurationManager.AppSettings["Robots"];
        metaRobots.Content = robots;
        metaRobots.Visible = !string.IsNullOrEmpty(robots);
      }
      catch (System.Configuration.ConfigurationErrorsException) { }

      // Disable browser cache if the user is authenticated.
      if (User.Identity.IsAuthenticated)
      {
        bool preventBrowserCache = false;
        bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["PreventBrowserCache"], out preventBrowserCache);
        if (preventBrowserCache)
          Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
      }
    }

    override protected void OnInit(EventArgs e)
    {
      // CODEGEN: This call is required by the ASP.NET Web Form Designer.
      InitializeComponent();
      base.OnInit(e);

    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
    }
  }
}