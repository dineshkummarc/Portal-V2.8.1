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
using Portal.API;

namespace Portal.Modules.ContentScheduler
{
  public partial class CleanUpState : Portal.StateBase.ModuleState
  {
    ContentSchedulerHandler _contentSchedulerHandler;

    protected void Page_Load(object sender, EventArgs e)
    {
      _contentSchedulerHandler = new ContentSchedulerHandler(this);

      if(!ControlPostback)
      {
        // Abfüllen der bestehenden Einträge für die Auswahl.
        int numInPast = 0;
        DateTime defaultThresholdTime = DateTime.Now;
        foreach (DataRowView row in _contentSchedulerHandler.ConfigurationData.ContentEvent.DefaultView)
        {
          ContentSchedulerData.ContentEventRow eventData = (ContentSchedulerData.ContentEventRow) row.Row;
          string textData = eventData.Hint + " (" 
                            + eventData.ActivationDate.ToString(Portal.API.Config.DateTimeFormat) + ")";
          _thresholdEvent.Items.Add(new ListItem(textData, eventData.Id.ToString()));

          // Der zweite Eintrag der vor dem aktuellen Zeitpunkt liegt, wird markiert. (Dieser ist sicher nicht mehr
          // aktiv).
          if (eventData.ActivationDate < defaultThresholdTime)
          {
            if(++numInPast == 2)
              _thresholdEvent.SelectedIndex = _thresholdEvent.Items.Count - 1;
          }
        }
        // Wenn noch kein Datensatz markiert wurde, wird der letzte markiert.
        if (numInPast < 2 )
          _thresholdEvent.SelectedIndex = _thresholdEvent.Items.Count - 1;
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
      int numOfEventsToDel = _thresholdEvent.Items.Count  - _thresholdEvent.SelectedIndex;
      _removeInfoLbl.Text = string.Format(Language.GetText(this, "RemoveInfo"), numOfEventsToDel);
    }


    protected void OnCleanUp(object sender, EventArgs e)
    {
      // Die Einträge entfernen.
      _contentSchedulerHandler.CleanUp(new Guid(_thresholdEvent.SelectedValue));

      ProcessEvent((int)StateEvent.Back);
    }
    protected void OnBack(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Back);
    }
}
}
