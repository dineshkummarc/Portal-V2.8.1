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
  /// Dieses Usercontrol repräsentiert den Status zum Anzeigen des Inhalts eines Zeitpunkts.
  /// </summary>
  public partial class PreviewState : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
      Guid id = new Guid(TransitionArgument.ToString());
      ContentSchedulerHandler contentHandler = new ContentSchedulerHandler(this, id);
      _pageContent.Text = contentHandler.GetContent();

      _currentInfo.Text = contentHandler.CurrentContentEvent.Hint + " (" +
                     contentHandler.CurrentContentEvent.ActivationDate.ToString(Portal.API.Config.DateTimeFormat) + ")";

      _nextEvent.Visible = (contentHandler.NextContentEvent != null);
      _previousEvent.Visible = (contentHandler.PreviousContentEvent != null);

      base.OnPreRender(e);
    }


    protected void OnBack(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Back, TransitionArgument);
    }



    protected void OnNextEvent_Click(object sender, EventArgs e)
    {
      Guid id = new Guid(TransitionArgument.ToString());
      ContentSchedulerHandler contentHandler = new ContentSchedulerHandler(this, id);
      ContentSchedulerData.ContentEventRow nextRow = contentHandler.NextContentEvent;
      if (nextRow != null)
        ProcessEvent((int)StateEvent.ShowPreview, nextRow.Id);
    }


    protected void OnPreviousEvent_Click(object sender, EventArgs e)
    {
      Guid id = new Guid(TransitionArgument.ToString());
      ContentSchedulerHandler contentHandler = new ContentSchedulerHandler(this, id);
      ContentSchedulerData.ContentEventRow previousRow = contentHandler.PreviousContentEvent;
      if (previousRow != null)
        ProcessEvent((int)StateEvent.ShowPreview, previousRow.Id);
    }

    protected void OnEditPage_Click(object sender, EventArgs e)
    {
      Guid id = new Guid(TransitionArgument.ToString());
      ProcessEvent((int)StateEvent.EditPage, id);
    }
}
}