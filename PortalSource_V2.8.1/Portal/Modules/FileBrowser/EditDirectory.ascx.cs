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
  public partial class EditDirectory : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!ControlPostback)
      {
        DirectoryWrapper editDir = ((IStateProcessor)Parent).ConfigAgent.EditDirectory;
        // Ist es ein Edit oder ein New.
        if (editDir != null)
        {
          dirName.Text = editDir.PresentationName;
          description.Text = editDir.Description;
          
          // Der löschen Button ist nur aktiv, wenn es nicht das Root Verzeichnis ist.
          if(Parent != null)
            DeleteBtn.Visible = true;
        }
      }
    }


    protected void OkBtn_Click(object sender, EventArgs e)
    {
      DirectoryWrapper editDir = ((IStateProcessor)Parent).ConfigAgent.EditDirectory;
      if(editDir != null)
      {
        editDir.PresentationName = dirName.Text;
        editDir.Description = description.Text;
        editDir.SaveData();
      }
      else if (!string.IsNullOrEmpty(dirName.Text))
      {
        DirectoryWrapper parentDir = ((IStateProcessor)Parent).ConfigAgent.CurrentDirectory;
        parentDir.CreateSubDir(dirName.Text, description.Text);
      }

      ProcessEvent((int)StateEvent.ok);
    }


    protected void CancelBtn_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.cancel);
    }

    protected void DeleteBtn_Click(object sender, EventArgs e)
    {
      ConfigAgent cfgAgent = ((IStateProcessor)Parent).ConfigAgent;
      DirectoryWrapper editDir = cfgAgent.EditDirectory;
      if (editDir != null)
      {
        editDir.Delete();

        // Falls das aktuelle Verzeichnis das aktive ist, wird eine Ebene nach oben navigiert.
        if (cfgAgent.EditDirectory == cfgAgent.CurrentDirectory)
          cfgAgent.CurrentDirectory = cfgAgent.CurrentDirectory.Parent;
      }
     
      ProcessEvent((int)StateEvent.ok);
    }
  }
}