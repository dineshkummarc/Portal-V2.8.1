using System;
using System.Web;


namespace Portal.Modules.FileBrowser
{
  /// <summary>
  /// Verwaltet die aktuelle Konfiguration.
  /// </summary>
  public class ConfigAgent
  {
    #region Member Variabeln
    /// <summary>
    /// Das Root Verzeichnis.
    /// </summary>
    private DirectoryWrapper rootDirectory;

    /// <summary>
    /// Das aktuelle Verzeichnis.
    /// </summary>
    private DirectoryWrapper currDirectory;

    /// <summary>
    /// Das Verzeichnis welches bearbeitet werden soll.
    /// </summary>
    private DirectoryWrapper editDirectory;

    /// <summary>
    /// Die Datei die bearbeitet werden soll.
    /// </summary>
    private FileWrapper editFile;

    // Die Konfiguration dieses Filebrowsers.
    private ModuleConfig config;

    #endregion

    #region Konstruktoren
    /// <summary>
    /// Konstruktor.
    /// </summary>
    public ConfigAgent(Portal.Modules.FileBrowser.ModuleConfig config)
    {
      this.config = config;
      ResetData();
    }
    #endregion

    #region Methoden (öffentlich)

    public void ResetData()
    {
      CurrentDirectory = null;
    }

    #endregion

    #region Properties (öffentlich)

    /// <summary>
    /// Ermittelt das Root-Verzeichnis.
    /// </summary>
    public DirectoryWrapper RootDirectory
    {
      get
      {
        if (null == rootDirectory)
        {
           // Erzeugen des Root-Verzeichniswrappers.
          if (System.IO.Directory.Exists(PhysicalRoot))
          {
            rootDirectory = new DirectoryWrapper(PhysicalRoot, null);
            rootDirectory.Sort(config.SortProperty, config.SortDirectionAsc);
          }
        }
        return rootDirectory;
      }
    }

    /// <summary>
    /// Das aktuelle Verzeichnis.
    /// </summary>
    public DirectoryWrapper CurrentDirectory
    {
      get
      {
        if (null == currDirectory)
        {
          // Es ist noch kein aktuelles Verzeichnis vorgewählt. Darum wird das Root Verzeichnis verwendet.
          currDirectory = RootDirectory;
        }
        return currDirectory;
      }
      set
      {
        currDirectory = value;
      }
    }

    /// <summary>
    /// Verzeichnis das bearbeitet werden soll.
    /// </summary>
    public DirectoryWrapper EditDirectory
    {
      get { return editDirectory; }
      set { editDirectory = value;}
    }

    /// <summary>
    /// Datei die bearbeitet werden soll.
    /// </summary>
    public FileWrapper EditFile
    {
      get { return editFile; }
      set { editFile = value; }
    }

    /// <summary>
    /// Ermittelt den physikalischen Pfad zum Root-Verzeichnis.
    /// </summary>
    public string PhysicalRoot
    {
      get
      {
        try
        {
          return HttpContext.Current.Server.MapPath(VirtualRoot);
        }
        catch
        {
          return null;
        }
      }
    }

    public string VirtualRoot
    {
      get
      {
        return config.VirtualRoot;
      }
    }

    #endregion
  }
}