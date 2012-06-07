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

namespace Portal.Modules.FormBuilder
{
  public partial class EditSuccessState : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // FCK Editor konfigurieren.
      string applicationDir = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
      FCKeditor1.BasePath = Path.Combine(applicationDir, "FCKeditor/");
      FCKeditor1.EditorAreaCSS = Portal.Helper.CssEditPath;
      if (!ControlPostback)
      {
        // Daten abfüllen.
        ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if (cfg == null)
          cfg = new ModuleConfig();

        FCKeditor1.Value = cfg.SuccessContent;
        _showSuccess.Checked = cfg.ShowSuccessPage;
      }
    }

    protected void Save()
    {
      lock (ModuleConfig.ConfigLock)
      {
        ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if (cfg == null)
          cfg = new ModuleConfig();

        cfg.SuccessContent = FCKeditor1.Value;
        cfg.ShowSuccessPage = _showSuccess.Checked;

        WriteConfig(cfg);
      }
    }

    protected void OnSave(object sender, EventArgs e)
    {
      Save();
     
    }

    protected void OnCancel(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Cancel);
    }

    protected void OnSaveBack(object sender, EventArgs e)
    {
      Save();
      ProcessEvent((int)StateEvent.Save);
    }
  }
}
