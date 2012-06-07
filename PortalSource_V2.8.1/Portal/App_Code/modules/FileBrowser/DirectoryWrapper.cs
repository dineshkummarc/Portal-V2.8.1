using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;

namespace Portal.Modules.FileBrowser
{
  # region Globale Enums
  public enum SortProperty
  {
    Name,
    Description,
    ModDate
  }
  #endregion

  /// <summary>
  /// Repräsentiert ein Verzeichnis.
  /// </summary>
  public class DirectoryWrapper
  {
    #region Member Variabeln

    /// <summary>
    /// Configurationsdaten (gesamtes XML) dieses Verzeichnisses.
    /// </summary>
    private DirectoryData xmlDirData;

    /// <summary>
    /// Datei in welcher die Benutzerdaten abglegt sind.
    /// </summary>
    private const string customDataFile = "_directory.config";

    /// <summary>
    /// Das Parent Verzeichnis.
    /// </summary>
    protected DirectoryWrapper parent;

    // Relativen virtueller Pfad zu diesem Verzeichnis.
    protected String virtualPath;

    /// <summary>
    /// Die Verzeichnisdaten dieses Verzeichnisses.
    /// </summary>
    protected DirectoryInfo directory;

    /// <summary>
    /// Die Unterverzeichnisse.
    /// </summary>
    protected List<DirectoryWrapper> subDirectories;

    /// <summary>
    /// Die enthaltenen Dateien.
    /// </summary>
    protected List<FileWrapper> files;

    /// <summary>
    /// Durch den Benutzer konfigurierte Daten.
    /// </summary>
    private DirectoryData.DirectoryConfigRow customData;

    /// <summary>
    /// Sortier-Eigenschaft.
    /// </summary>
    private SortProperty sortProperty = SortProperty.Name;

    /// <summary>
    /// Sortierrichtung.
    /// </summary>
    private bool sortDirectionAsc = true;


    #endregion

    #region Konstruktoren

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="path">Physikalischer Pfad zu diesem Verzeichnis</param>
    /// <param name="parent">Das Parent-Verzeichnis</param>
    public DirectoryWrapper(string path, DirectoryWrapper parent)
    {
      this.directory = new System.IO.DirectoryInfo(path);
      this.parent = parent;

      if (this.parent == null)
        this.virtualPath = "";
    }

    #endregion

    #region Öffentliche Properties

    /// <summary>
    /// Der Physikalische Name des Verzeichnis.
    /// </summary>
    public string Name
    {
      get { return directory.Name; }
      set
      {
        if (directory.Name != value)
        {
          this.directory.MoveTo(directory.Parent.FullName + "\\" + value);
          this.ResetPath();  // Der virtuelle Pfad muss aktualisiert werden.
        }
      }
    }


    /// <summary>
    /// Parent-Verzeichnis.
    /// </summary>
    public DirectoryWrapper Parent
    {
      get { return parent; }
    }


    /// <summary>
    /// Gibt den relativen virtuellen Pfad zu diesem Verzeichnis zurück.
    /// </summary>
    public String VirtualPath
    {
      get
      {
        if (this.virtualPath == null)
        {
          // Ermitteln des Pfads.
          if (parent != null)
          {
            this.virtualPath = parent.VirtualPath + this.Name + "/";
          }
          else
            throw (new Exception("Für das Root Verzeichnis wurde kein virtueller Pfad gesetzt."));
        }
        return this.virtualPath;
      }

    }


    /// <summary>
    /// Directory-Informationen.
    /// </summary>
    public DirectoryInfo DirectoryData
    {
      get { return directory; }
    }

    /// <summary>
    /// Alle enthaltenen Unterverzeichnisse.
    /// </summary>
    public DirectoryWrapper[] SubDirectories
    {
      get 
      { 
        // Einlesen der Subverzeichnisse, fals dies noch nicht erfolgt ist.
        if (null == subDirectories)
        {
          DirectoryInfo[] subDir = directory.GetDirectories();
          int nofDir = subDir.Length;
          subDirectories = new List<DirectoryWrapper>(nofDir);
          foreach (DirectoryInfo dirInfo in subDir)
          {
            // Erzeugen der Wrapper mit der selben Sortierung.
            DirectoryWrapper dirWrapper = new DirectoryWrapper(dirInfo.FullName, this);
            dirWrapper.Sort(this.sortProperty, this.sortDirectionAsc);
            subDirectories.Add(dirWrapper);
          }
        }

        return subDirectories.ToArray(); 
      }
    }

    /// <summary>
    /// Alle enthaltenen  Dateien.
    /// </summary>
    public FileWrapper[] Files
    {
      get 
      {
        // Einlesen der Subverzeichnisse, falls dies nocht nicht erfolgt ist.
        if (null == files)
        {
          FileInfo[] contFiles = directory.GetFiles();
          int nofFiles = contFiles.Length;
          files = new List<FileWrapper>(nofFiles);
          foreach (FileInfo fileData in contFiles)
          {
            if (fileData.Name[0] != '_')
            {
              // Versuche den Config-Node der Datei zu laden.
              DirectoryData.FileConfigRow fileDataRow = XmlDirectoryData.FileConfig.FindByName(fileData.Name);
              bool newData = false;
              if (null == fileDataRow)
              {
                fileDataRow = XmlDirectoryData.FileConfig.NewFileConfigRow();
                newData = true;
              }

              // Datei erzeugen.
              files.Add(new FileWrapper(fileData.FullName, fileDataRow, this));

              if (newData)
                XmlDirectoryData.FileConfig.AddFileConfigRow(fileDataRow);
            }
            // Sortieren der Liste mit den Dateien.
            SortFiles();
          }
        }

        return files.ToArray(); 
      }
    }

    /// <summary>
    /// Die Anzahl der Dateien in diesem Verzeichnis.
    /// </summary>
    public int FileCount
    {
      get 
      {
        if (files != null)
          return files.Count;
        else
          return directory.GetFiles().Length; 
      }
    }

    /// <summary>
    /// Die Anzahl der Unterverzeichnisse in diesem Verzeichnis.
    /// </summary>
    public int SubDirCount
    {
      get 
      {
        if (subDirectories != null)
          return subDirectories.Count;
        else
          return directory.GetDirectories().Length; 
      }
    }

    /// <summary>
    /// Der Name des Verzeichnis welcher angzeigt werden soll.
    /// </summary>
    public string PresentationName
    {
      get { return CustomData.PresentationName; }
      set 
      {
        CustomData.PresentationName = value;

        // Falls es sich bei diesem Verzeichnis nicht um das Root Verzeichnis handelt, wird auch der physikalische
        // Name geändert.
        if(Parent != null)
          Name = value;
      }
    }

    /// <summary>
    /// Der Darstellungspfad der gesamten Verzeichnisstruktur bis zum Root Verzeichnis (mit den Darstellungsnamen.
    /// </summary>
    public string PresentationPath
    {
      get
      {
        string path = "";
        if (parent != null)
          path = parent.PresentationPath + "\\";
        path += PresentationName;
        return path;
      }
    }

    /// <summary>
    /// Die Beschreibung des Verzeichnis.
    /// </summary>
    public string Description
    {
      get { return CustomData.Description; }
      set
      {
        CustomData.Description = value;
      }
    }

    /// <summary>
    /// Der Url zum Icon welches für dieses Verzeichnis verwendet werden soll.
    /// </summary>
    public string IconUrl
    {
      get { return "IconUrl"; }
    }
    #endregion

    #region Methoden (Öffentlich)

    /// <summary>
    /// Gibt das Unterverzeichnis mit dem angegebenen Namen zurück, bzw. null wenn es nicht gefunden wurde.
    /// </summary>
    /// <param name="dirName">Der Verzeichnisname bzw. der relative Pfad zum Verzeichnis</param>
    /// <returns>Verzeichniswrapper, bzw. null wenn es nicht gefunden wurde</returns>
    public DirectoryWrapper GetSubDirectory(string dirPath)
    {
      int slashPos = dirPath.IndexOf('/');
      // Slash an erster Position werden übersprungen.
      if (slashPos == 0)
      {
        dirPath = dirPath.Substring(1);
        slashPos = dirPath.IndexOf('/');
      }

      String subDirName;
      if(slashPos != -1)
      {
        subDirName = dirPath.Substring(0, slashPos);
        dirPath = dirPath.Substring(slashPos + 1);
        if(dirPath == "/")
          dirPath = String.Empty;
      }
      else
        subDirName = dirPath;

      // Suche nach dem Unterverzeichnis.
      DirectoryWrapper subDir = Array.Find(SubDirectories, delegate(DirectoryWrapper dir)
                                                                    { return dir.Name == subDirName; });
      if (subDir != null)
      {
        if (String.IsNullOrEmpty(dirPath))
          return subDir;
        else
          return subDir.GetSubDirectory(dirPath);
      }
      else
        return null;  // Verzeichnis konnte nicht gefunden werden.
    }


    /// <summary>
    ///  Sorgt dafür dass der Pfad zu diesem Verzeichnis aktualisiert wird.
    /// </summary>
    public void ResetPath()
    {
      virtualPath = null;

      // Alle Unterverzeichnisse und Dateien zurücksetzen, damit diese von neuem eingelesen werden.
      this.subDirectories = null;
      this.files = null;
    }


    /// <summary>
    /// Gibt die enthaltene Datei mit dem angegebenen Namen zurück, bzw. null wenn sie nicht gefunden wurde.
    /// Es können auch vorgängige Verzeichnisse durch Slash getrennt angegeben werden. z.B. "Directory/File.txt"
    /// </summary>
    /// <param name="filePath">Name der Datei</param>
    /// <returns>Dateiwrapper, bzw. null wenn sie nicht gefunden wurde</returns>
    public FileWrapper GetFile(string filePath)
    {
      int slashPos = filePath.IndexOf('/');
      // Slash an erster Position werden übersprungen.
      if (slashPos == 0)
      {
        filePath = filePath.Substring(1);
        slashPos = filePath.IndexOf('/');
      }
      if((-1 == slashPos))
      {
        // Kein Unterverzeichnis mehr enthalten. Es wird nach der Datei gesucht.
        return Array.Find(Files, delegate(FileWrapper file) { return file.FileName == filePath; });
      }
      else
      {
        // Suche nach dem Unterverzeichnis.
        string dirName = filePath.Substring(0, slashPos);
        DirectoryWrapper subDir = Array.Find(SubDirectories, delegate(DirectoryWrapper dir) 
                                                                      { return dir.Name == dirName; });
        if(subDir != null)
          return subDir.GetFile(filePath.Substring(slashPos + 1));
        else
          return null;  // Verzeichnis konnte nicht gefunden werden.
      }
    }

    /// <summary>
    /// Überprüft ob ein Dateiname erlaubt ist.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool IsFileNameAllowed(string fileName)
    {
      // Alle Dateinamen die nicht mit "_" beginnen und syntaktisch korrekt sind, sind gültig.
      return !fileName.StartsWith("_") && Portal.API.Helper.CheckValidFileName(fileName);
    }

    /// <summary>
    /// Fügt eine neue Datei der Dateiliste hinzu.
    /// </summary>
    /// <param name="fileName">Dateiname</param>
    /// <param name="description">Beschreibung der Datei.</param>
    public void AddFile(string fileName, string description, DateTime modDate)
    {
      // Falls noch kein XML-Node für diese Datei besteht, wird er erzeugt.
      DirectoryData.FileConfigRow fileDataRow = XmlDirectoryData.FileConfig.FindByName(fileName);
      bool newData = false;
      if (null == fileDataRow)
      {
        fileDataRow = XmlDirectoryData.FileConfig.NewFileConfigRow();
        fileDataRow.ModificationDate = modDate;
        newData = true;
      }

      // Dateiwrapper erzeugen.
      FileWrapper fw = new FileWrapper(this.DirectoryData.FullName + "\\" + fileName, fileDataRow, this);
      fw.Description = description;
      fw.ModificationDate = modDate;
      files.Add(fw);
      // Sortiere neu.
      SortFiles();

      // Daten speichern.
      if (newData)
        XmlDirectoryData.FileConfig.AddFileConfigRow(fileDataRow);
      SaveData();
    }

    /// <summary>
    /// Erzeugt ein neues Unterverzeichnis.
    /// </summary>
    /// <param name="dirName"></param>
    /// <param name="dirDescription"></param>
    public void CreateSubDir(string dirName, string dirDescription)
    {
      DirectoryInfo newDirInfo = DirectoryData.CreateSubdirectory(dirName);

      // Erzeugen der Wrapper mit der selben Sortierung.
      DirectoryWrapper dirWrapper = new DirectoryWrapper(newDirInfo.FullName, this);
      dirWrapper.Description = dirDescription;
      dirWrapper.SaveData();

      subDirectories = null;  // Neu einlesen der Unterverzeichnisse.
    }

    /// <summary>
    /// Schreibt die XML-Konfiguration
    /// </summary>
    public void SaveData()
    {
      // Es müssen Daten geändert worden sein, sonst wird das Schreiben unterlassen.
      if (xmlDirData != null)
      {
        string cnfgPath = this.directory.FullName + @"\" + customDataFile;
        xmlDirData.WriteXml(cnfgPath);
      }
    }


    /// <summary>
    /// Sortiert die Dateien mit den aktuellen Sortiereinstellungen.
    /// </summary>
    protected void SortFiles()
    {
      if (files != null)
      {
        int negFactor = 1;
        if (!this.sortDirectionAsc)
          negFactor = -1;

        switch (this.sortProperty)
        {
          case SortProperty.Name:
            {
              files.Sort(delegate(FileWrapper file1, FileWrapper file2)
                { return negFactor * file1.FileName.CompareTo(file2.FileName); });
              break;
            }
          case SortProperty.ModDate:
            {
              files.Sort(delegate(FileWrapper file1, FileWrapper file2)
                { return negFactor * file1.ModificationDate.CompareTo(file2.ModificationDate); });
              break;
            }
          case SortProperty.Description:
            {
              files.Sort(delegate(FileWrapper file1, FileWrapper file2)
                { return negFactor * file1.Description.CompareTo(file2.Description); });
              break;
            }
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }


    /// <summary>
    /// Übernimmt die Sortiereinstellungen und sortiert die Daten.
    /// </summary>
    /// <param name="sortProp"></param>
    /// <param name="sortDirectionAsc"></param>
    public void Sort(SortProperty sortProp, bool sortDirectionAsc)
    {
      this.sortProperty = sortProp;
      this.sortDirectionAsc = sortDirectionAsc;
      // Das Sortieren der Verzeichnisse ist noch nicht implementiert.
      SortFiles();

      // Sortierkriterien an die Unterverzeichnisse weitergeben.
      if (this.subDirectories != null)
      {
        foreach (DirectoryWrapper dir in this.subDirectories)
          dir.Sort(sortProp, sortDirectionAsc);
      }
    }


    /// <summary>
    /// Löscht das Verzeichnis.
    /// </summary>
    public void Delete()
    {
      if (Parent == null)
        throw new InvalidOperationException();

      DirectoryData.Delete(true);
      parent.Detach(this);
    }


    /// <summary>
    /// Entfernt eine Datei aus der Liste, ohne diese physikalisch zu löschen.
    /// </summary>
    /// <param name="file">Datei die entfernt werden soll.</param>
    public void Detach(FileWrapper file)
    {
      if (files != null)
      {
        files.Remove(file);
      }
    }


    /// <summary>
    /// Entfernt ein Verzeichnis aus der Liste, ohne diese physikalisch zu löschen.
    /// </summary>
    /// <param name="file">Datei die entfernt werden soll.</param>
    public void Detach(DirectoryWrapper dir)
    {
      if (subDirectories != null)
      {
        subDirectories.Remove(dir);
      }
    }


    #endregion

    #region Properties (intern)

    /// <summary>
    /// Ermittelt die konfigurierten Xml Daten.
    /// </summary>
    protected DirectoryData XmlDirectoryData
    {
      get
      {
        if (null == xmlDirData)
        {
          xmlDirData = new DirectoryData();
          string cnfgPath = this.directory.FullName + @"\" + customDataFile;
          if (System.IO.File.Exists(cnfgPath))
            xmlDirData.ReadXml(cnfgPath);
        }
        return xmlDirData;
      }
    }

    /// <summary>
    /// Ermittelt die konfigurierten Daten zu diesem Verzeichnis.
    /// </summary>
    protected DirectoryData.DirectoryConfigRow CustomData
    {
      get
      {
        // Wenn die Daten noch nicht eingelesen wurden, wird dies nun ausgeführt.
        if (null == customData)
        {
          // Wenn der Datensatz für dieses Verzeichnis nicht existiert, wird er erzeugt.
          if (XmlDirectoryData.DirectoryConfig.Rows.Count < 1)
          {
            XmlDirectoryData.DirectoryConfig.AddDirectoryConfigRow(Name, "");
          }
          // Da in diesem File nur die Informationen für dieses Verzeichnis enthalten sind, wird der erste Datensatz
          // zurückgegeben.
          this.customData = XmlDirectoryData.DirectoryConfig[0];
        }
        return this.customData;
      }
    }
    #endregion
  }
}