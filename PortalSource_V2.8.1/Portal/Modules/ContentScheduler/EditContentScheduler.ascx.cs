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
using Portal.StateBase;

namespace Portal.Modules.ContentScheduler
{
  public partial class EditContentScheduler : Portal.StateBase.StateContainer<State, StateEvent>
  {

    #region Initialisierung des Regelwerks.

    static EditContentScheduler()
    {
      // Stati hinzufügen.
      AddState(State.EventOverview, "EditOverviewState.ascx");
      AddState(State.EditEvent, "EditEventState.ascx");
      AddState(State.EditPage, "EditPageState.ascx");
      AddState(State.Preview, "PreviewState.ascx");
      AddState(State.CleanUp, "CleanUpState.ascx");

      // Statusübergänge hinzufügen.
      AddTransition(State.EventOverview, StateEvent.NewEvent, State.EditEvent);
      AddTransition(State.EventOverview, StateEvent.EditEvent, State.EditEvent);
      AddTransition(State.EventOverview, StateEvent.EditPage, State.EditPage);
      AddTransition(State.EventOverview, StateEvent.ShowPreview, State.Preview);
      AddTransition(State.EventOverview, StateEvent.CleanUp, State.CleanUp);

      AddTransition(State.EditEvent, StateEvent.Save, State.EventOverview);
      AddTransition(State.EditEvent, StateEvent.Cancel, State.EventOverview);
      AddTransition(State.EditEvent, StateEvent.EditPage, State.EditPage);
      
      AddTransition(State.EditPage, StateEvent.Save, State.EventOverview);
      AddTransition(State.EditPage, StateEvent.Cancel, State.EventOverview);

      AddTransition(State.Preview, StateEvent.ShowPreview, State.Preview);
      AddTransition(State.Preview, StateEvent.Back, State.EventOverview);
      AddTransition(State.Preview, StateEvent.EditPage, State.EditPage);

      AddTransition(State.CleanUp, StateEvent.Back, State.EventOverview);
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      // Laden des entsprechenden Ccontrols zum aktuellen Status.
      LoadCurrentCtrl(IsPostBack);
    }
  }
}