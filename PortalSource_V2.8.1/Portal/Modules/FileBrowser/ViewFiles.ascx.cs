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
  public partial class ViewFiles : Portal.StateBase.ModuleState
  {
    #region Ereignishandler

    /// <summary>
    /// Handler der beim Laden der Seite aufgerufen wird.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
      DirectoryWrapper currDir = FbConfigAgent.CurrentDirectory;
      if (!ControlPostback)
      {
        // Daten hinzufügen.
        if (currDir != null)
        {
          if (currDir.SubDirectories.Length > 0)
          {
            directoryList.DataSource = currDir.SubDirectories;
            directoryList.DataBind();
            directoryArea.Visible = true;
          }
          else
            directoryArea.Visible = false;

          fileList.DataSource = currDir.Files;

          // Is there another way than referncing the column by index?...
          ((BoundField)fileList.Columns[4]).DataFormatString = "{0:" + Portal.API.Config.DateFormat +"}";
          fileList.DataBind();

        }
      }

      // Die Controls im Header abhängig der aktuellen Einstellungen konfigurieren.
      ConfigureControls();
    }


    /// <summary>
    /// Ereignishandler der das aktuelle Verzeichnis wechselt.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ChangeDir(object sender, CommandEventArgs e)
    {
      ConfigAgent cfgAgent = FbConfigAgent;
      // Wechle zu dem entsprechenden Verzeichnis, welches als CommandArgument angegeben wurde.
      DirectoryWrapper newCurrDir = cfgAgent.RootDirectory.GetSubDirectory(e.CommandArgument.ToString());
      if (null == newCurrDir)
      {
        // Inzwischen scheint dieses Verzeichnis nicht mehr vorhanden zu sein. Aus diesem Grund wird wieder zum
        // Root Verzeichnis gewechselt.
        newCurrDir = cfgAgent.RootDirectory;
      }
      ChangeCurrentDir(newCurrDir);
    }

    /// <summary>
    /// Ereignishandler der eine Verzeichnisebene nach oben wechselt.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ChangeDirUp(object sender, CommandEventArgs e)
    {
      int nofLayer = Convert.ToInt32(e.CommandArgument);
      DirectoryWrapper newCurrDir = FbConfigAgent.CurrentDirectory;
      while ((nofLayer > 0) && (newCurrDir.Parent != null))
      {
        newCurrDir = newCurrDir.Parent;
        nofLayer--;
      }
      ChangeCurrentDir(newCurrDir);
    }

    /// <summary>
    /// Ereignishändler zum bearbeiten des aktuellen Verzeichnis.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void editDirectoryBtn_Click(object sender, EventArgs e)
    {
      // Das aktuelle Verzeichnis zum Bearbeiten vorwählen.
      ConfigAgent cfgAgent = FbConfigAgent;
      cfgAgent.EditDirectory = cfgAgent.CurrentDirectory;

      // Statuswechsel in den EditModus.
      ProcessEvent((int)StateEvent.editDirectory);
    }

    /// <summary>
    /// Ereignishandler zum Bearbeiten des Unterverzeichnis.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void editSubDirectoryBtn_Click(object sender, CommandEventArgs e)
    {
      // Das Subverzeichnis zum Bearbeiten vorwählen.
      ConfigAgent cfgAgent = FbConfigAgent;
      cfgAgent.EditDirectory = cfgAgent.CurrentDirectory.GetSubDirectory(e.CommandArgument.ToString());

      // Statuswechsel in den EditModus.
      ProcessEvent((int)StateEvent.editDirectory);
    }

    /// <summary>
    /// Ereignishandler zum Erzeugen eines neuen Unterverzeichnis.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void newDirectoryBtn_Click(object sender, EventArgs e)
    {
      FbConfigAgent.EditDirectory = null; // Kein Verzeichnis zum Bearbeiten bedeutet neues Unterverzeichnis.
      
      // Statuswechsel in den EditModus.
      ProcessEvent((int)StateEvent.editDirectory);
    }

    /// <summary>
    /// Ereignishandler der eine neue Datei zum Verzeichnis hinzufügt.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnAddFiles(object sender, EventArgs e)
    {
      // Statuswechsel.
      ProcessEvent((int)StateEvent.addFiles);
    }

    /// <summary>
    /// Ereignishandler zum Bearbeiten der enthaltenen Datei.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void editFileBtn_Click(object sender, CommandEventArgs e)
    {
      // Die Datei zum Bearbeiten vorwählen.
      ConfigAgent cfgAgent = FbConfigAgent;
      cfgAgent.EditFile = cfgAgent.CurrentDirectory.GetFile(e.CommandArgument.ToString());
      cfgAgent.EditDirectory = cfgAgent.CurrentDirectory;

      // Statuswechsel in den EditModus.
      ProcessEvent((int)StateEvent.editFile);
    }

    /// <summary>
    /// Erzeugt das physikalische Root-Verzeichnis.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void createRootDir_Click(object sender, EventArgs e)
    {
      ConfigAgent cfgAgent = FbConfigAgent;
      if ((cfgAgent.CurrentDirectory == null) && (cfgAgent.PhysicalRoot != null))
      {
        try
        {
          System.IO.Directory.CreateDirectory(cfgAgent.PhysicalRoot);
          pathNotFound.Visible = false;
        }
        catch (Exception exc)
        {
          pathNotFound.Visible = true;
          Label errorLbl = new Label();
          errorLbl.Text = exc.Message;
          pathNotFound.Controls.Add(errorLbl);
        }
      }
      ConfigureControls();
    }

    #endregion

    #region Methoden (intern)


    /// <summary>
    /// Konfiguriert die Controls im Header abhängig der aktuellen Einstellungen.
    /// </summary>
    private void ConfigureControls()
    {
      DirectoryWrapper currDir = FbConfigAgent.CurrentDirectory;

      // Existiert das Verzeichnis bereits?
      if (currDir != null)
      { 
        // Edit-Funktion einblenden, wenn die Benutzerrechte entsprechend gesetzt sind.
        EditTools.Visible = ModuleHasEditRights;
        directoryList.Columns[1].Visible = ModuleHasEditRights;
        fileList.Columns[1].Visible = ModuleHasEditRights;

        // Der Pfad zum akuellen Verzeichhnis nur einblenden, wenn überhaupt Verzeichnisse vorhanden sind.
        if (((currDir.SubDirectories.Length > 0) || (currDir.Parent != null)))
        {
          currPathDiv.Visible = true;
          CreateDirPath(currDir, currPath);
        }
        else
          currPathDiv.Visible = false;

        pathNotFound.Visible = false;
      }
      else
      {
        // Das Verzeichnis existiert nicht. Der Benutzer hat die Möglichkeit, das Verzeichnis zu erstellen.
        pathNotFound.Visible = ModuleHasEditRights;
        string message = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this),
                                                              "DirectoryNotFoundCreateQuestion");
        directoryCreateQuestion.Text = string.Format(message, FbConfigAgent.VirtualRoot);
      }
    }


    /// <summary>
    /// Wechselt das aktuelle Verzeichnis und lädt die Daten neu.
    /// </summary>
    /// <param name="newCurrDir">Neues Verzeichnis, oder null wenn das RootVerzeichnis gelten soll.</param>
    protected void ChangeCurrentDir(DirectoryWrapper newCurrDir)
    {
      // Zuweisen des neuen Verzeichnis.
      FbConfigAgent.CurrentDirectory = newCurrDir;
      if (null == newCurrDir)
        newCurrDir = FbConfigAgent.CurrentDirectory;

      directoryList.DataSource = newCurrDir.SubDirectories;
      directoryList.DataBind();
      fileList.DataSource = newCurrDir.Files;
      fileList.DataBind();

      // Pfad zu diesem Verzeichnis aktualisieren.
      currPath.Controls.Clear();
      CreateDirPath(newCurrDir, currPath);

      // Wenn das aktuelle Verzeichnis das Root Verzeichnis ist, wird der Up-Button ausgeblendet.
      dirUpBtn.Visible = (FbConfigAgent.CurrentDirectory.Parent != null);
    }

    /// <summary>
    /// Erzeugt die Anzeige des aktuellen Pfads mit den Links.
    /// </summary>
    /// <param name="targetCtrl"></param>
    protected void CreateDirPath(DirectoryWrapper directory, Control targetCtrl)
    {
      if (null != directory)
      {
        // Aktuelles Verzeichnis hinzufügen.
        Literal currLayer = new Literal();
        currLayer.Text = directory.PresentationName;
        targetCtrl.Controls.Add(currLayer);

        // Alle übergeordneten Verzeichnisse hinzufügen.
        int step = 0;
        DirectoryWrapper dir = directory.Parent;
        while (dir != null)
        {
          targetCtrl.Controls.AddAt(0, new LiteralControl(" >> "));

          LinkButton dirLayer = new LinkButton();
          dirLayer.ID = "DirUp" + step;
          dirLayer.Text = dir.PresentationName;
          dirLayer.CommandArgument = (++step).ToString();
          dirLayer.Command += new CommandEventHandler(this.ChangeDirUp);
          targetCtrl.Controls.AddAt(0, dirLayer);

          dir = dir.Parent;
        }
      }
      else
      {
        string dirNotFound = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), "DirectoryNotFound");
        targetCtrl.Controls.AddAt(0, new LiteralControl("<i>" + dirNotFound + "</i>"));
      }
    }

    /// <summary>
    /// Ermittelt die Download URL für eine Datei.
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    protected String GetDownloadUrl(String relativePath)
    {
      return String.Format("~/Download.aspx?Type=FileBrowser&Module={0}&File={1}&Att=1", Server.UrlEncode(this.ModuleRef),
                              Server.UrlEncode(relativePath));
    }

    protected String GetImageUrl(string extension)
    {
      if(string.IsNullOrEmpty(extension))
        extension = "unknown";
      return "~/PortalImages/Extensions/" + extension + ".gif";
    }

    #endregion

    #region Properties (intern)
    protected ConfigAgent FbConfigAgent
    {
      get
      {
        return ((IStateProcessor)Parent).ConfigAgent;
      }
    }
    #endregion


}
}
