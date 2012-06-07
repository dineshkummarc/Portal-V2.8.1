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

namespace Portal.Modules.FileBrowser
{
  public partial class ModuleEditFileBrowser : Portal.API.EditModule
  {
      protected void Page_Load(object sender, EventArgs e)
      {
        if (!IsPostBack)
        {
          ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
          if (cfg == null)
            cfg = new ModuleConfig();
          VirtualRootEdit.Text = cfg.VirtualRoot;

          SortPropertyCombo.Items.Add(new ListItem(Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this),
                                                              "Name"), SortProperty.Name.ToString()));
          SortPropertyCombo.Items.Add(new ListItem(Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this),
                                                              "Description"), SortProperty.Description.ToString()));
          SortPropertyCombo.Items.Add(new ListItem(Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this),
                                                              "Date"), SortProperty.ModDate.ToString()));
          SortPropertyCombo.SelectedValue = cfg.SortProperty.ToString();

          SortDirCheck.Checked = cfg.SortDirectionAsc;
        }
      }

      protected void SaveLB_Click(object sender, System.EventArgs e)
      {
        FileBrowser.ModuleConfig cfg = (FileBrowser.ModuleConfig)ReadConfig(typeof(FileBrowser.ModuleConfig));
        if (cfg == null)
        {
          cfg = new ModuleConfig();
        }

        cfg.VirtualRoot = VirtualRootEdit.Text;
        cfg.SortProperty = (SortProperty)Enum.Parse(typeof(SortProperty), SortPropertyCombo.SelectedValue);
        cfg.SortDirectionAsc = SortDirCheck.Checked;
        WriteConfig(cfg);
        RedirectBack();
      }

      private void CancelLB_Click(object sender, System.EventArgs e)
      {
        RedirectBack();
      }
  }
}
