namespace Portal.Modules.Forum
{
  using System;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using Portal.API;
  

  /// <summary>
  /// Zusammenfassung für WriteArticleView.
  /// </summary>
  public partial class WriteArticleView : System.Web.UI.UserControl
  {
    protected System.Web.UI.WebControls.RequiredFieldValidator m_TitleRFV;
    private ConfigAgent configAgent;

    protected void Page_Load(object sender, System.EventArgs e)
    {    
      configAgent = ((IStateProcessor)Parent).ConfigAgent;
      
      if (configAgent.Module.Page.User.Identity.IsAuthenticated)
      {
        m_NameTB.Text = HttpUtility.HtmlDecode(configAgent.CurrentUserName);
        m_NameTB.ReadOnly = true;
        m_EmailTB.Text = configAgent.CurrentUserEmail;
        m_EmailTB.ReadOnly = true;
        m_UserIdTB.Text = configAgent.CurrentUserId.ToString();
      }

      RegisterJavascript();
    }

    private void RegisterJavascript()
    {
      // Find control which is of type HtmlForm
      Control control = m_TextTB;
      while (control != null && !(control is System.Web.UI.HtmlControls.HtmlForm))
        control = control.Parent;

      // Use string builder to build <SCRIPT> block
      StringBuilder sb = new StringBuilder();

      // Javascript for inserting emoticons into message body
      if (control != null)
      {
        // Copied from CodeProject forums
        sb.Append("<SCRIPT language=\"javascript\">");
        sb.Append("function InsertText(text) {");
        string id = string.Format("document.{0}.{1}", control.ClientID, m_TextTB.ClientID);
        sb.Append(string.Format(" var TextArea = {0}; ", id));
        sb.Append(" if (TextArea) {");
        sb.Append(" TextArea.value += text + \" \"; ");
        sb.Append(" TextArea.focus(); } ");
        sb.Append(" return false; } ");
        sb.Append("</SCRIPT>");
      }

      // Javascript for updating status bar when hovering over emoticon (copied from CodeProject forums)
      sb.Append("<SCRIPT language=\"javascript\" event=\"onmouseover\" for=\"emoticon\">");
      sb.Append("window.self.status=\"Einfügen des Platzhalters für dieses Emoticon in den Artikel.\";");
      sb.Append("return true;");
      sb.Append("</SCRIPT>");

      // Javascript for clearing out status bar when mouse leaves emoticon (copied from CodeProject forums)
      sb.Append("<SCRIPT language=\"javascript\" event=\"onmouseout\" for=\"emoticon\">");
      sb.Append("window.self.status=\"\";");
      sb.Append("return true;");
      sb.Append("</SCRIPT>");

      // Register client side script block
      Page.RegisterClientScriptBlock("InsertEmoticon", sb.ToString());
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

    }
    #endregion

    protected void m_CancelBtn_Click(object sender, System.EventArgs e)
    {
      if (configAgent.IsThreadMessage())
      {
        ((IStateProcessor)Parent).SetEvent(StateEvents.CancelToThreadView);
      }
      else
      {
        configAgent.ThreadFile = null;
        ((IStateProcessor)Parent).SetEvent(StateEvents.CancelToForumView);
      }
    }

    protected void m_SubmitBtn_Click(object sender, System.EventArgs e)
    {
      // Wurde kein Titel eingegeben, darf der Beitrag nicht gespeichert werden.
      if (m_TitleTB.Text.Length == 0)
        return;

      // Das Erfassen von Artikeln wird durch den ArticleManager erledigt.
      ArticleManager mgr = new ArticleManager(configAgent);

      if (configAgent.IsThreadMessage())
      {
        if (configAgent.Module.Page.User.Identity.IsAuthenticated)
        {
          mgr.AddThreadArticle(configAgent.ThreadFile, 
                               configAgent.ParentMessage, 
                               m_UserIdTB.Text,
                               m_TitleTB.Text,
                               m_TextTB.Text);
        }
        else
        {
          mgr.AddThreadArticle(configAgent.ThreadFile, 
                               configAgent.ParentMessage, 
                               m_NameTB.Text,
                               m_EmailTB.Text,
                               m_TitleTB.Text,
                               m_TextTB.Text);
        }
      }
      else
      {
        if (configAgent.Module.Page.User.Identity.IsAuthenticated)
        {
          mgr.AddForumArticle(configAgent.ThreadFile, 
                              m_UserIdTB.Text,
                              m_TitleTB.Text,
                              m_TextTB.Text);
        }
        else
        {
          mgr.AddForumArticle(configAgent.ThreadFile, 
                              m_NameTB.Text,
                              m_EmailTB.Text,
                              m_TitleTB.Text,
                              m_TextTB.Text);
        }
      }

      ((IStateProcessor)Parent).SetEvent(StateEvents.ShowThread);
    }
  }
}
