//===========================================================================
// Diese Datei wurde als Teil einer ASP.NET 2.0-Webprojektkonvertierung geändert.
// Der Klassenname wurde geändert, und die Klasse wurde so geändert, dass sie von der abstrakten 
//-Basisklasse in Datei "App_Code\Migrated\modules\forum\Stub_searchresults_ascx_cs.cs" erbt.
// Andere Klassen in der Webanwendung können während der Laufzeit die Code-Behind-Seite
//" mithilfe der abstrakten Basisklasse binden und darauf zugreifen.
// Die zugehörige Inhaltsseite "modules\forum\searchresults.ascx" wurde ebenfalls geändert und verweist auf den neuen Klassennamen.
// Weitere Informationen zu diesem Codemuster erhalten Sie unter http://go.microsoft.com/fwlink/?LinkId=46995. 
//===========================================================================
namespace Portal.Modules.Forum
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// Zusammenfassung für SearchResults.
	/// </summary>
	public partial class SearchResultsControl : SearchResults
	{
    private ConfigAgent configAgent;
    private DataTable m_Data;
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
      configAgent = ((IStateProcessor)Parent).ConfigAgent;

      ArticleManager mgr = new ArticleManager(configAgent);
      m_Data = mgr.SearchResults;
      m_SearchResultsTable.DataSource = m_Data;
      m_SearchResultsTable.DataBind();

      string szHits = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), "Hits");
      m_StateLbl.Text = m_Data.Rows.Count + " " + szHits;
    }

    /// <summary>
    /// Liefert zurück, ob der Artikel neu ist.
    /// </summary>
    /// <param name="objDate"></param>
    /// <returns></returns>
    protected bool IsThreadNew(object objDate)
    {
      if (System.DBNull.Value != objDate)
      {
        DateTime dt = (DateTime)objDate;
        TimeSpan ts = DateTime.Now.Subtract(dt);
        return ts.Days < 7;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Liefert den Benutzernamen des Authors.
    /// </summary>
    /// <param name="objAuthor"></param>
    /// <param name="objUserId"></param>
    /// <returns></returns>
    protected string GetUserName(object objAuthor, object objUserId)
    {
      string szUserName = "";

      if (System.DBNull.Value != objUserId)
      {
        Guid userId = new Guid(HttpUtility.HtmlDecode((string)objUserId));
        szUserName = configAgent.GetUserWithId(userId);
      }
      else if (System.DBNull.Value != objAuthor)
      {
        szUserName = (string)objAuthor;
      }

      return szUserName;
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
      this.m_SearchResultsTable.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.m_SearchResultsTable_ItemCommand);

    }
		#endregion

    protected void m_CancelBtn_Click(object sender, System.EventArgs e)
    {
      ArticleManager mgr = new ArticleManager(configAgent);
      mgr.ResetSearch();
      ((IStateProcessor)Parent).SetEvent(StateEvents.CancelToForumView);
    }

    private void m_SearchResultsTable_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
      if (e.CommandName == "ShowThread")
      {
        ArticleManager mgr = new ArticleManager(configAgent);
        mgr.ResetSearch();

        string szCmdArg     = (string)e.CommandArgument;
        string threadFile = szCmdArg.Substring(0, szCmdArg.IndexOf(":"));
        int nId             = int.Parse(szCmdArg.Substring(szCmdArg.IndexOf(":")+1));

        configAgent.ThreadFile    = threadFile;
        configAgent.ArticleToShow = nId;
        ((IStateProcessor)Parent).SetEvent(StateEvents.ShowThread);
      }
    }
	}
}
