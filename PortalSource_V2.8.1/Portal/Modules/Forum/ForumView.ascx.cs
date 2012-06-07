namespace Portal.Modules.Forum
{
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using Portal.API;

  /// <summary>
  /// Zusammenfassung für ForumView.
  /// </summary>
  public partial class ForumView : System.Web.UI.UserControl
  {
    private ConfigAgent configAgent;

    protected void Page_Load(object sender, System.EventArgs e)
    {
      configAgent = ((IStateProcessor)Parent).ConfigAgent;
      m_NewThreadLnk.Visible = configAgent.IsThreadCreationAllowed();

      ArticleManager mgr = new ArticleManager(configAgent);
      ForumData data = mgr.ThreadRootData;
      data.Forum.DefaultView.Sort = "Id DESC";
      m_ForumTable.DataSource = data.Forum.DefaultView;
      m_ForumTable.DataBind();
    }

    /// <summary>
    /// Liefert den Benutzernamen des Authors.
    /// </summary>
    /// <param name="objAuthor"></param>
    /// <param name="objUserId"></param>
    /// <returns></returns>
    protected string GetUserName(object objAuthor, object objUserId)
    {
      string userName = "";

      if (System.DBNull.Value != objUserId)
      {
        Guid userId = (Guid)objUserId;
        userName = configAgent.GetUserWithId(userId);
      }
      else if (System.DBNull.Value != objAuthor)
      {
        userName = (string)objAuthor;
      }

      return userName;
    }

    /// <summary>
    /// Liefert den Benutzernamen des letzten Posters.
    /// </summary>
    /// <param name="objAuthor"></param>
    /// <param name="objUserId"></param>
    /// <returns></returns>
    protected string GetLastPosterName(object objAuthor, object objUserId)
    {
      string userName = "";

      if (System.DBNull.Value != objUserId)
      {
        Guid userId = (Guid)objUserId;
        userName = configAgent.GetUserWithId(userId);
      }
      else if (System.DBNull.Value != objAuthor)
      {
        userName = (string)objAuthor;
      }

      return userName;
    }

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
    ///        Erforderliche Methode für die Designerunterstützung
    ///        Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {

      this.m_ForumTable.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.m_ForumTable_ItemCommand);
    }
    #endregion

    protected void NewThread_Click(object sender, System.EventArgs e)
    {
      configAgent.SetNewThread();
      ((IStateProcessor)Parent).SetEvent(StateEvents.NewArticle);
    }

    private void m_ForumTable_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
      if (e.CommandName == "ShowThread")
      {
        configAgent.ThreadFile = (string)e.CommandArgument;
        ((IStateProcessor)Parent).SetEvent(StateEvents.ShowThread);
      }
    }

    protected void m_SearchTB_TextChanged(object sender, System.EventArgs e)
    {
      if (m_SearchTB.Text.Trim().Length == 0)
        return;

      ArticleManager mgr = new ArticleManager(configAgent);
      mgr.ResetSearch();
      configAgent.SearchText = HttpUtility.HtmlEncode(m_SearchTB.Text);
      ((IStateProcessor)Parent).SetEvent(StateEvents.SearchText);
    
    }
  }
}
