namespace Portal.Modules.Forum
{
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using Portal.API;
  using Portal.Modules.Forum;

  /// <summary>
  ///	Zusammenfassung für EditForum.
  /// </summary>
  public partial class EditForum : Portal.API.EditModule
  {

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if(!IsPostBack)
      {
        ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if(cfg != null)
        {
          m_ThreadCreationRightCombo.SelectedIndex = cfg.ThreadCreationRight;
          m_TopLevelCheck.Checked = cfg.UseHTMLEditorOnTopLevel;
          m_LowerLevelCheck.Checked = cfg.UseHTMLEditorOnLowerLevel;
        }
      }

      m_TopLevelCheck.Enabled = false;
      m_LowerLevelCheck.Enabled = false;
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

    protected void SaveLB_Click(object sender, System.EventArgs e)
    {
      ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
      if(cfg == null)
      {
        cfg = new ModuleConfig();
      }

      cfg.ThreadCreationRight = int.Parse(m_ThreadCreationRightCombo.SelectedValue);
      cfg.UseHTMLEditorOnTopLevel = m_TopLevelCheck.Checked;
      cfg.UseHTMLEditorOnLowerLevel = m_LowerLevelCheck.Checked;
      WriteConfig(cfg);
      RedirectBack();
    }

    private void CancelLB_Click(object sender, System.EventArgs e)
    {
      RedirectBack();
    }
  }
}
