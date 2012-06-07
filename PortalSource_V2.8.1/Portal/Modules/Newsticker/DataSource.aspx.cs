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
using System.IO;
using Portal;
using Portal.API;


namespace Portal.Modules.Newsticker
{
  /// <summary>
  /// Zusammenfassung für DataSource.
  /// </summary>
  public partial class DataSource : System.Web.UI.Page
  {
    private void Page_Load(object sender, System.EventArgs e)
    {
      bool bAccessAllowed = false;
      PortalDefinition.Module SelModule = GetModule(Request["Tab"], Request["Ctrl"]);
      if((SelModule != null) && (SelModule.type == "Newsticker"))     
      {
        string szUrl = Server.UrlDecode(Request["Src"]);
        if(UrlExist(Request["Tab"], SelModule, szUrl))
        {
          bAccessAllowed = true;

          // Ermitteln der Anzahl Einträge.
          int nNofItems = 0;
          try
          {
            nNofItems = Convert.ToInt32(Request["Nof"]);
          }
          catch(Exception)
          {
            nNofItems = 5;
          }

          // Wir Laden den gesuchten Feed.
          RssFeedItem Feed = new RssFeedItem(Request["Name"], szUrl, nNofItems, false);

          // Wir ermitteln die Repräsentation dieses Objekts.
          Control FeedParent = new Control();
          Feed.AddFeedRepresentation(FeedParent.Controls);

          // Wir geben die Repräsentation in HTML Code zurück.
          StringWriter StrWriter = new StringWriter();
          HtmlTextWriter TxtWriter = new HtmlTextWriter(StrWriter);
          FeedParent.RenderControl(TxtWriter);
          Response.Write(StrWriter.ToString());
        }
      }

      if(!bAccessAllowed)
      {
        Response.Write("No Access");
      }       

      Response.End(); 
    }

    /// <summary>
    /// Ermittelt die Definition des Modules des angegebenene Tabs.
    /// </summary>
    /// <param name="szTab"></param>
    /// <param name="szCtrl"></param>
    /// <returns></returns>
    private PortalDefinition.Module GetModule(string szTab, string szCtrl)
    {
      PortalDefinition.Module SelectedModule = null;

      if((szTab != null) && (szCtrl != null))
      {
        PortalDefinition PortDef = PortalDefinition.Load();
        PortalDefinition.Tab NewsTab = PortDef.GetTab(szTab);
        if(UserManagement.HasViewRights(Page.User, NewsTab.roles))
        {
          PortalDefinition.Module Md = NewsTab.GetModule(szCtrl);
          if(UserManagement.HasViewRights(Page.User, Md.roles))
            SelectedModule = Md;

        }
      }

      return SelectedModule;
    }

    /// <summary>
    /// Überprüft ob diese Url im angegebenen Modul konfiguriert ist.
    /// </summary>
    /// <param name="szTab"></param>
    /// <param name="HostingModule"></param>
    /// <param name="szUrl"></param>
    /// <returns></returns>
    private bool UrlExist(string szTab, PortalDefinition.Module HostingModule, string szUrl)
    {
      bool bUrlFound = false;

      // Wir laden das Modul in den Speicher, damit wir einfach auf die Konfiguration zugrefen können.
      Portal.API.Module Md = (Portal.API.Module)LoadControl(Config.GetModuleVirtualPath(HostingModule.type) + 
                              HostingModule.type + ".ascx");

      // Modul initialisieren.
      Md.InitModule(szTab, HostingModule.reference, HostingModule.type, 
                    Config.GetModuleDataVirtualPath(HostingModule.type), false);

      // Konfiguration laden.
      DataSet ModConfig = Md.ReadConfig();
      if(ModConfig != null)
      {
        // Nun iterieren wir über alle Url's in diesem Module.
        foreach(DataRow Entry in ModConfig.Tables["news"].Rows)
        {
          if(szUrl == (string) Entry["Url"])
            return true;
        }
      }

      return bUrlFound;
    }



    #region Vom Web Form-Designer generierter Code
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN: Dieser Aufruf ist für den ASP.NET Web Form-Designer erforderlich.
      //
      InitializeComponent();
      base.OnInit(e);
    }
		
    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {    
      this.Load += new System.EventHandler(this.Page_Load);
    }
    #endregion
  }
}




