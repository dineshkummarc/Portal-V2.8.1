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

namespace Portal.Modules.FormBuilder
{
	public partial class EditFormBuilder : Portal.StateBase.StateContainer<State, StateEvent>
	{		
    #region Initialisierung des Regelwerks.

    static EditFormBuilder()
    {
      // Status hinzufügen.
      AddState(State.EditMain, "EditMainState.ascx");
      AddState(State.EditForm, "EditFormState.ascx");
      AddState(State.EditValidation, "EditValidationState.ascx");
      AddState(State.EditResultSuccess, "EditSuccessState.ascx");
      AddState(State.EditResultError, "EditErrorState.ascx");
      AddState(State.EditEmail, "EditEmailState.ascx");
      

      // Statusübergänge hinzufügen.
      AddTransition(State.EditMain, StateEvent.EditForm, State.EditForm);
      AddTransition(State.EditMain, StateEvent.EditResultError, State.EditResultError);
      AddTransition(State.EditMain, StateEvent.EditResultSuccess, State.EditResultSuccess);
      AddTransition(State.EditMain, StateEvent.EditEmail, State.EditEmail);
      AddTransition(State.EditMain, StateEvent.EditValidation, State.EditValidation);

      AddTransition(State.EditForm, StateEvent.Save, State.EditMain);
			AddTransition(State.EditForm, StateEvent.Cancel, State.EditMain);
      
      AddTransition(State.EditResultSuccess, StateEvent.Save, State.EditMain);
      AddTransition(State.EditResultSuccess, StateEvent.Cancel, State.EditMain);
      
      AddTransition(State.EditResultError, StateEvent.Save, State.EditMain);
      AddTransition(State.EditResultError, StateEvent.Cancel, State.EditMain);

      AddTransition(State.EditEmail, StateEvent.Save, State.EditMain);
      AddTransition(State.EditEmail, StateEvent.Cancel, State.EditMain);

      AddTransition(State.EditValidation, StateEvent.Save, State.EditMain);
      AddTransition(State.EditValidation, StateEvent.Cancel, State.EditMain);
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      // Laden des entsprechenden Ccontrols zum aktuellen Status.
      LoadCurrentCtrl(IsPostBack);
    }
	}
}
