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
using System.IO;

namespace Portal.Modules.ContentScheduler
{
  public partial class EditPageState : Portal.StateBase.ModuleState
  {
    ContentSchedulerHandler _contentSchedulerHandler;

    protected void Page_Load(object sender, EventArgs e)
    {
      // Handler für diesen Datensatz einrichten.
      _contentSchedulerHandler = new ContentSchedulerHandler(this, new Guid(TransitionArgument.ToString()));

      // FCK Editor konfigurieren.
      string applicationDir = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
      FCKeditor1.BasePath = Path.Combine(applicationDir, "FCKeditor/");
      FCKeditor1.EditorAreaCSS = Portal.Helper.CssEditPath;
      if (!ControlPostback)
      {
        // Daten abfüllen.
        FCKeditor1.Value = _contentSchedulerHandler.GetContent();
      }
    }

    protected void OnSave(object sender, EventArgs e)
    {
      _contentSchedulerHandler.SaveContent(FCKeditor1.Value);
    }

    protected void OnCancel(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Cancel);
    }

    protected void OnSaveBack(object sender, EventArgs e)
    {
      _contentSchedulerHandler.SaveContent(FCKeditor1.Value);
      ProcessEvent((int)StateEvent.Save);
    }
  }
}