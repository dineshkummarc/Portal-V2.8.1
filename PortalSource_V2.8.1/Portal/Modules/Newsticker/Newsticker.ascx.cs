using System;
using System.Collections;
using System.Data;
using System.Web.Mobile;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.Newsticker
{
  /// <summary>
  ///		Zusammenfassung der Klasse Newsticker.
  ///		Zeigt RSS- und RDF-Feeds an.
  /// </summary>
  public partial class Newsticker : Portal.API.Module
  {
    protected void Page_Load(object sender, System.EventArgs e)
    {
      // Schema existiert - Kann nicht null sein.
      DataSet ds = ReadConfig();

      // Liste, die die Daten für den Repeater speichert.
      ArrayList feedList = new ArrayList();

      // Mobile Client unterstützen in den meisten Fällen kein Ajax. Darum ist das asynchrone Laden nur
      // für alle anderen Clients aktiviert.
      MobileCapabilities caps = (MobileCapabilities)Request.Browser;
      bool useAjax = !caps.IsMobileDevice;

      // Daten für die einzelnen darzustellenden Feeds werden abgefüllt..
      for (int nzRow = 0; nzRow < ds.Tables["news"].Rows.Count; nzRow++)
      {
        string szName = (string)ds.Tables["news"].Rows[nzRow]["Name"];
        string szUrl = (string)ds.Tables["news"].Rows[nzRow]["Url"];
        int nMaxCount = (int)ds.Tables["news"].Rows[nzRow]["MaxCount"];

        feedList.Add(new RssFeedItem(szName, szUrl, nMaxCount, useAjax));
      }

      // Binden des Datenobjektes an den Repeater.
      m_Repeater.DataSource = feedList;
      m_Repeater.DataBind();
    }

    private void LoadClientData(RssFeedItem FeedItem, System.Web.UI.Control TargetCtrl)
    {
      // Ermitteln den Pfad des Zieldokuments.
      string szDataDir = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
      if (!szDataDir.EndsWith("/"))
        szDataDir += "/";
      szDataDir += "modules/Newsticker";

      // Script-Include einfügen.
      if (!Page.IsClientScriptBlockRegistered("ClientLoadInclude"))
      {
        string szScriptInc = "<script language=\"javascript\" src=\"" + szDataDir + "/ClientDataLoad.js\"> </script>";
        Page.RegisterClientScriptBlock("ClientLoadInclude", szScriptInc);
      }

      string szScript = "<script language=\"JavaScript\">\n"
        + String.Format("RequestData('{0}/DataSource.aspx?Tab={1}&Ctrl={2}&Name={3}&Src={4}&Nof={5}', '{6}');\n",
            szDataDir, TabRef, ModuleRef, FeedItem.Title, Server.UrlEncode(FeedItem.Url), FeedItem.MaxNofItems,
            TargetCtrl.ClientID)
        + "</script>";

      Page.RegisterStartupScript(TargetCtrl.ClientID + "Loader", szScript);
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
    ///		Erforderliche Methode für die Designerunterstützung
    ///		Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    /// <summary>
    /// Wird für jede Zeile aufgerufen, welche beim Repeater gebunden wird.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      // Für alle Einträge ausführen.
      if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
      {
        // Datenobjekt ermitteln.
        RssFeedItem FeedItem = (RssFeedItem)e.Item.DataItem;

        // Parent Element ermitteln.
        Control ParentCtrl = e.Item.FindControl("NewsData");

        // Falls ein Feed vorhanden ist, fügen wir diesen ein. 
        if (FeedItem.DataExist)
        {
          FeedItem.AddFeedRepresentation(ParentCtrl.Controls);
        }
        else
        {
          // Es ist kein Feed vorhanden, darum fügen wir ein Script ein, welches diesen nachträglich anfordert.
          // Andernfalls fügen wir ein Script ein, welches den Feed nachträglich anfordert.
          LoadClientData(FeedItem, ParentCtrl);
        }
      }
    }
  }
}
