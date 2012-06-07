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
using System.Globalization;

namespace Portal.Modules.FileBrowser
{
  public partial class EditFile : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // Aktuellen Verzeichnispfad anzeigen.
      ConfigAgent cfgAgent = ((IStateProcessor)Parent).ConfigAgent;
      DirectoryWrapper currDir = cfgAgent.CurrentDirectory;
      dirPath.Text = currDir.PresentationPath;

      FileWrapper editFile = cfgAgent.EditFile;
      fileName.Text = editFile.FileName;
      fileDesc.Text = editFile.Description;

      string dateFormat = Portal.API.Config.DateFormat;
      modDate.DateTimeValue = editFile.ModificationDate;

      // Wird eine Datei ausgewählt, soll das Datum aktualisiert werden.
      fileSelect.Attributes.Add("onChange", modDate.GetChangeHandlerCode(DateTime.Now));
    }

    /// <summary>
    /// Ereignishandler für den OK Button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OkBtn_Click(object sender, EventArgs e)
    {
      bool success = true;

      FileWrapper editFile = ((IStateProcessor)Parent).ConfigAgent.EditFile;

      // Falls ein Name angegeben ist, wird Datei umbenannt.
      try
      {
        if (!string.IsNullOrEmpty(fileName.Text.Trim()))
          editFile.FileName = fileName.Text;
      }
      catch (IOException exc)
      {
        fileAllreadyExist.IsValid = false;
        success = false;
      }

      // Setzen der Beschreibung.
      editFile.Description = fileDesc.Text;
  
      // Setzen des Datums.
      try
      {
        editFile.ModificationDate = modDate.DateTimeValue;
      }
      catch (FormatException) { }

      // Wenn eine Upload-Datei angegeben wurde, wird die bebstehende ersetzt.
      if (fileSelect.HasFile)
      {
        try
        {
          fileSelect.PostedFile.SaveAs(editFile.PhysicalPath);
        }
        catch (Exception ex)
        {
          uploadException.IsValid = false;
          uploadException.Text = Portal.API.Language.GetText(this, "UploadException") + ": " +  ex.Message;
          success = false;
        }
      }

      if (true == success)
      {
        ProcessEvent((int)StateEvent.ok);
      }
    }


    protected void CancelBtn_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.cancel);
    }

    protected void DeleteBtn_Click(object sender, EventArgs e)
    {
      FileWrapper editFile = ((IStateProcessor)Parent).ConfigAgent.EditFile;
      editFile.DeleteFile();
      ProcessEvent((int)StateEvent.ok);
    }
}
}