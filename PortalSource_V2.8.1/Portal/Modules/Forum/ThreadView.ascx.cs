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
  /// Zusammenfassung für ThreadView.
  /// </summary>
  public partial class ThreadView : System.Web.UI.UserControl
  {
    protected ConfigAgent configAgent;
    protected System.Web.UI.WebControls.Image Image1;
    private DataTable m_Data;

    protected void Page_Load(object sender, System.EventArgs e)
    {
      // Holen des Konfigurationsobjektes.
      configAgent = ((IStateProcessor)Parent).ConfigAgent;

      Bind();

      if (configAgent.ArticleToShow != -1)
      {
        if(!Page.IsStartupScriptRegistered("jump"))
        {
          string szScript = "<script language=\"javascript\">" +  
                            "  function jump() { " + 
                            "    window.location='#" + configAgent.ArticleToShow + "'; } " +
                            "  window.onLoad() = jump(); " +
                            "</script>";
          Page.RegisterStartupScript("jump", szScript);
        }
      }
    }

    private void Bind()
    {
      // Einlesen der Threaddaten.
      ArticleManager mgr = new ArticleManager(configAgent);
      ThreadData data = mgr.GetThreadData(configAgent.ThreadFile);
      data.Thread.DefaultView.Sort = "DateTime DESC";

      // Ermitteln der Row, welche den Root-Node enthält.
      ThreadData.ThreadRow root = null;
      foreach (ThreadData.ThreadRow r in data.Thread.Rows)
      {
          if (r.Id == 0)
          {
              root = r;
              break;
          }
      }
      //ThreadData.ThreadRow root = data.Thread.FindById(0);

      // Initialisieren des Datensets.
      m_Data = new DataTable();
      m_Data.Columns.Add("Title", typeof(string));
      m_Data.Columns.Add("Text", typeof(string));
      m_Data.Columns.Add("Author", typeof(string));
      m_Data.Columns.Add("Email", typeof(string));
      m_Data.Columns.Add("UserId", typeof(string));
      m_Data.Columns.Add("Depth", typeof(int));
      m_Data.Columns.Add("Id", typeof(int));
      m_Data.Columns.Add("Parent", typeof(int));
      m_Data.Columns.Add("DateTime", typeof(DateTime));

      // Zusammenstellen eines neuen Datensets.
      if (null != root)
      {
        AddThreadMessage(root, 0);
        m_ThreadTable.DataSource = m_Data;
        m_ThreadTable.DataBind();
      }
    }

    /// <summary>
    /// Diese Methode stellt eine Threadmessage dar und ruft sich rekursiv für alle
    /// Child-Nodes der entsprechenden Threadmessage auf.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="nDepth"></param>
    private void AddThreadMessage(ThreadData.ThreadRow row, int nDepth)
    {
      DataRow datarow = m_Data.NewRow();
      datarow["Title"]    = row["Title"];
      datarow["Text"]     = row["Text"];
      datarow["Author"]   = row["Author"];
      datarow["Email"]    = row["Email"];
      datarow["UserId"]   = row["UserId"];
      datarow["Id"]       = row["Id"];
      datarow["Parent"]   = row["Parent"];
      datarow["DateTime"] = row["DateTime"];
      datarow["Depth"]    = nDepth;
      m_Data.Rows.Add(datarow);

      // Rekursiver Aufruf aller Child-Nodes.
      foreach (ThreadData.ThreadRow r in row.GetChildRows("ThreadThread"))
      {
        AddThreadMessage(r, nDepth+1);
      }
    }

    /// <summary>
    /// Berechnet die Spaltenbreite für das Symbol und damit den Einzug der Message.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string GetColumnWidth(object obj)
    {
      int nDepth = int.Parse(obj.ToString());
      return (nDepth*25+15).ToString();
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

    protected string GetArticleText(object objText)
    {
      if (System.DBNull.Value != objText)
      {
        string szText = (string)objText;
        szText = szText.Replace(":)", "<img src=\"Modules/Forum/images/emoticons/smiley_smile.gif\"/>");
        szText = szText.Replace(";)", "<img src=\"Modules/Forum/images/emoticons/smiley_wink.gif\"/>");
        szText = szText.Replace(";P", "<img src=\"Modules/Forum/images/emoticons/smiley_tongue.gif\"/>");
        szText = szText.Replace(":-D", "<img src=\"Modules/Forum/images/emoticons/smiley_biggrin.gif\"/>");
        szText = szText.Replace(":((", "<img src=\"Modules/Forum/images/emoticons/smiley_cry.gif\"/>");
        szText = szText.Replace(":(", "<img src=\"Modules/Forum/images/emoticons/smiley_frown.gif\"/>");
        szText = szText.Replace(":-O", "<img src=\"Modules/Forum/images/emoticons/smiley_redface.gif\"/>");
        szText = szText.Replace(":rolleyes:", "<img src=\"Modules/Forum/images/emoticons/smiley_rolleyes.gif\"/>");
        szText = szText.Replace(":laugh:", "<img src=\"Modules/Forum/images/emoticons/smiley_laugh.gif\"/>");
        szText = szText.Replace(":mad:", "<img src=\"Modules/Forum/images/emoticons/smiley_mad.gif\"/>");
        szText = szText.Replace(":confused:", "<img src=\"Modules/Forum/images/emoticons/smiley_confused.gif\"/>");
        szText = szText.Replace(":|", "<img src=\"Modules/Forum/images/emoticons/smiley_line.gif\"/>");
        szText = szText.Replace(" X| ", "<img src=\"Modules/Forum/images/emoticons/smiley_dead.gif\"/>");
        szText = szText.Replace(":suss:", "<img src=\"Modules/Forum/images/emoticons/smiley_suss.gif\"/>");
        szText = szText.Replace(":cool:", "<img src=\"Modules/Forum/images/emoticons/smiley_cool.gif\"/>");
        szText = szText.Replace(":eek:", "<img src=\"Modules/Forum/images/emoticons/smiley_eek.gif\"/>");

        szText = szText.Replace("\n", "<br/>");
        szText = Portal.API.Helper.ActivateWebSiteUrl(szText);
        return szText;
      }

      return "";
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
    /// Erforderliche Methode für die Designerunterstützung
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.m_ThreadTable.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.m_ThreadTable_ItemCommand);

    }
    #endregion


    protected void BackLB_Click(object sender, System.EventArgs e)
    {
      // Ziel des Rücksprungs ermitteln.
      StateEvents eEvent = StateEvents.CancelToForumView;

      // War ein Artikel als Sprungziel angegeben, war die letzte Seite die Suchseite.
      if (configAgent.ArticleToShow != -1)
      {
        eEvent = StateEvents.SearchText;
      }

      // Zurücksetzen des Konfigurationsobjektes.
      configAgent.ThreadFile = null;
      configAgent.ArticleToShow = -1;
      ((IStateProcessor)Parent).SetEvent(eEvent);
    }

    private void m_ThreadTable_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
      if (e.CommandName == "Answer")
      {
        configAgent.ParentMessage = int.Parse(e.CommandArgument.ToString());
        ((IStateProcessor)Parent).SetEvent(StateEvents.NewArticle);
      }
      else if (e.CommandName == "Delete")
      {
        ArticleManager mgr = new ArticleManager(configAgent);
        int nArticleId = int.Parse(e.CommandArgument.ToString());
        mgr.DeleteArticle(nArticleId);
        if (nArticleId == 0)
        {
          ((IStateProcessor)Parent).SetEvent(StateEvents.CancelToForumView);
        }
        else
        {
          Bind();
        }
      }
    }
  }
}
