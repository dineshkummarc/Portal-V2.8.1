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

namespace Portal.Modules.ContentScheduler
{
  /// <summary>
  /// Dieses UserControl repräsentiert den Status in welchem die Übersicht der Events angezeigt wird.
  /// </summary>
  public partial class EventOverviewState : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!ControlPostback)
      {
        ContentSchedulerHandler ContentSchedulerHandler = new ContentSchedulerHandler(this);
        // Is there another way than referencing the column by index?...
        ((BoundField)_contentEventList.Columns[2]).DataFormatString = "{0:" + Portal.API.Config.DateTimeFormat + "}";
        _contentEventList.DataSource = ContentSchedulerHandler.ConfigurationData.ContentEvent.DefaultView;
        _contentEventList.DataBind();

        _cleanUpBtn.Visible = ContentSchedulerHandler.ConfigurationData.ContentEvent.Count > 0;
      }
    }


    protected void OnAddEvent(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.NewEvent);
    }

    /// <summary>
    /// Ereignishandler zum Bearbeiten des ContentEvents.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnEditEvent(object sender, CommandEventArgs e)
    {
      ProcessEvent((int)StateEvent.EditEvent, e.CommandArgument);
    }

    /// <summary>
    /// Ereignishandler zum Bearbeiten des ContentEvents.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnEditPage(object sender, CommandEventArgs e)
    {
      ProcessEvent((int)StateEvent.EditPage, e.CommandArgument);
    }

    /// <summary>
    /// Ereignishandler zum Anzeigen der Vorschau.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnPreviewEvent(object sender, CommandEventArgs e)
    {
      ProcessEvent((int)StateEvent.ShowPreview, e.CommandArgument);
    }

    /// <summary>
    /// Ereignishandler zum entfernen der alten Daten.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnCleanUp(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.CleanUp);
    }

    protected void OnBack(object sender, EventArgs e)
    {
      Response.Redirect(Helper.GetCancelEditLink());

    }
}
}
