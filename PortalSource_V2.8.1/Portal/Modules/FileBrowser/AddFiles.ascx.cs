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
  public partial class AddFiles : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // Aktuellen Verzeichnispfad anzeigen.
      DirectoryWrapper currDir = ((IStateProcessor)Parent).ConfigAgent.CurrentDirectory;
      dirPath.Text = currDir.PresentationPath;

      if (DateTime.MinValue == modDate.DateTimeValue)
        modDate.DateTimeValue = DateTime.Now;
    }

    /// <summary>
    /// Ereignishandler für den OK Button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OkBtn_Click(object sender, EventArgs e)
    {
      bool success = false;

      // Falls kein Name angegeben wurde, wird der Dateiname der Upload Datei verwendet.
      if (string.IsNullOrEmpty(fileName.Text.Trim()))
        fileName.Text = fileSelect.FileName;
      else
      {
        // Sicherstellen dass der Dateiname die selbe Endung aufweist.
        fileName.Text = fileName.Text.Trim();
        int lastPoint = fileSelect.FileName.LastIndexOf('.');
        if(-1 != lastPoint)
        {
          // Ermitteln der Endung der Upload Datei.
          string extension = fileSelect.FileName.Substring(lastPoint);
          
          // Stimmt die Endung nicht überein, wird sie einfach angehängt. 
          if (!fileName.Text.EndsWith(extension, true, null))
          {
            // Zuerst muss aber sichergestellt werden dass die maximale Länge nicht überschritten wird.
            int availLength = 255 - extension.Length;
            if (availLength < fileName.Text.Length)
              fileName.Text.Remove(availLength);

            fileName.Text += extension;
          }
        }
      }

      try
      {
        // Überprüfen ob die Datei bereits existiert und ob der Name gültig ist.
        DirectoryWrapper currDir = ((IStateProcessor)Parent).ConfigAgent.CurrentDirectory;
        if (null != currDir.GetFile(fileName.Text) || !currDir.IsFileNameAllowed(fileName.Text))
          fileAllreadyExist.IsValid = false;
        else
        {
          // Datei speichern.
          fileSelect.PostedFile.SaveAs(currDir.DirectoryData.FullName + "\\" + fileName.Text);

          // Dateiwrapper erzeugen und dem Cache hinzufügen.
          currDir.AddFile(fileName.Text, fileDesc.Text, modDate.DateTimeValue); 

          success = true;
        }
      }
      catch (Exception ex)
      {
        uploadException.Text = Portal.API.Language.GetText(this, "UploadException") + ": " + ex.Message;
        uploadException.IsValid = false;
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
  }
}