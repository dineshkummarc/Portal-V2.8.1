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
using System.Web.Caching;
using Portal.API;
using Portal.StateBase;

namespace Portal.Modules.FileBrowser
{
  public partial class ModuleFileBrowser : Portal.StateBase.StateContainer<State, StateEvent>, FileBrowser.IStateProcessor
  {
    #region Membervariabeln

    private ConfigAgent configAgent;
    
    #endregion

    #region Initialisierung des Regelwerks.

    static ModuleFileBrowser()
    {
      // Stati hinzufügen.
      AddState(State.viewFiles, "ViewFiles.ascx");
      AddState(State.editDirectory, "EditDirectory.ascx");
      AddState(State.addFiles, "AddFiles.ascx");
      AddState(State.editFile, "EditFile.ascx");

      // Statusübergänge hinzufügen.
      AddTransition(State.viewFiles, StateEvent.editDirectory, State.editDirectory);
      AddTransition(State.viewFiles, StateEvent.addFiles, State.addFiles);
      AddTransition(State.viewFiles, StateEvent.editFile, State.editFile);

      AddTransition(State.editDirectory, StateEvent.ok, State.viewFiles);
      AddTransition(State.editDirectory, StateEvent.cancel, State.viewFiles);

      AddTransition(State.editFile, StateEvent.ok, State.viewFiles);
      AddTransition(State.editFile, StateEvent.cancel, State.viewFiles);

      AddTransition(State.addFiles, StateEvent.ok, State.viewFiles);
      AddTransition(State.addFiles, StateEvent.cancel, State.viewFiles);
    }

    #endregion

    #region IStateProcessor Member

    /// <summary>
    /// Ermittelt das Verwaltungsobjekt der Konfiguration.
    /// </summary>
    public ConfigAgent ConfigAgent
    {
      get
      {
        if (this.configAgent== null)
        {
          string sessionKey = UniqueID + "Cfg";
          if (Session[sessionKey] != null)
          {
            this.configAgent = (ConfigAgent)Session[sessionKey];
          }
          else
          {
            ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
            if (cfg == null)
              cfg = new ModuleConfig();

            this.configAgent = new ConfigAgent(cfg);
            Session[sessionKey] = this.configAgent;
          }
        }

        return this.configAgent;
      }
      private set
      {
        this.configAgent = value;
        string sessionKey = UniqueID + "Cfg";
        Session[sessionKey] = value;
      }
    }

    #endregion

    #region Ereignishandler
    private void Page_Load(object sender, System.EventArgs e)
    {
      if (!IsPostBack)
        ConfigAgent = null; // Zurücksetzen des Config Agents, damit änderungen an der Konfiguration übernommen werden.

      // Laden des entsprechenden Ccontrols zum aktuellen Status.
      LoadCurrentCtrl(IsPostBack);
    }
    #endregion

  }
}
