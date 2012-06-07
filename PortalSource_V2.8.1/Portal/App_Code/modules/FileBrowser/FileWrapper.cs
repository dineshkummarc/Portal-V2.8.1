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
using System.Globalization;

namespace Portal.Modules.FileBrowser
{
  /// <summary>
  /// Repräsentiert eine Datei.
  /// </summary>
  public class FileWrapper
  {
    #region Member Variabeln.

    /// <summary>
    /// Das Parent Verzeichnis, welches diese Datei enthält.
    /// </summary>
    protected DirectoryWrapper parent;

    /// <summary>
    /// Dateiinformation.
    /// </summary>
    protected FileInfo fileInfo;

    /// <summary>
    /// Zusätzliche Dateiinformation.
    /// </summary>
    protected DirectoryData.FileConfigRow fileInfoCfg;

    #endregion

    #region Konstruktoren.
    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="path">Physikalischer Pfad zu dieser Datei.</param>
    /// <param name="cnfgRow">DataRow welche die Daten dieses Files enthält.</param>
    /// <param name="parent">Das Verzeichnis welche diese Datei enthält.</param>
    public FileWrapper(string path, DirectoryData.FileConfigRow cnfgRow, DirectoryWrapper parent)
    {
      if (null == parent)
        throw new ArgumentNullException("Verzeichnis darf nicht null sein.");
      this.parent = parent;
      this.fileInfo = new FileInfo(path);

      this.fileInfoCfg = cnfgRow;
      this.fileInfoCfg.Name = this.fileInfo.Name;
    }

    #endregion

    #region Properties (Physikalischer Zusammenhang)

    /// <summary>
    /// Der Name der Datei.
    /// </summary>
    /// <exception cref="IOException">Wenn ein Dateiname bereits existiert.</exception>
    public string FileName
    {
      get { return fileInfo.Name; }
      set
      {
        if (value != fileInfo.Name)
        {
          // Sicherstellen dass die Datei noch nicht existiert.
          if (null == parent.GetFile(value))
          {
            this.fileInfo.MoveTo(fileInfo.DirectoryName + "\\" + value);
            this.fileInfoCfg.Name = value;
            SaveData();
          }
          else
            throw new IOException("Dateiname bereits vorhanden");
        }
      }
    }

    // Ermittelt den vollständigen physikalischen Pfad dieser Datei.
    public string PhysicalPath
    {
      get
      {
        return this.fileInfo.FullName;
      }
    }

    /// <summary>
    /// Ermittelt die virtuelle (vom Root her gesehen) Adresse dieser Datei.
    /// </summary>
    public string VirtualPath
    {
      get
      {
        return parent.VirtualPath + this.FileName;
      }
    }

    /// <summary>
    /// Ermittelt die Dateigrösse in bytes.
    /// </summary>
    public long FileSize
    {
      get { return this.fileInfo.Length; }
    }

    public String MimeType
    {
      get
      {
        // This section shoud be extracted in a configuration file...
        String mimeType = null;
        if (0 == String.Compare(Extension, "pdf", true))
          mimeType = "application/pdf";
        else if (0 == String.Compare(Extension, "txt", true))
          mimeType = "text/plain";
        else if ((0 == String.Compare(Extension, "doc", true)) || (0 == String.Compare(Extension, "dot", true)))
          mimeType = "application/msword";
        else if (0 == String.Compare(Extension, "xls", true))
          mimeType = "application/vnd.ms-excel";
        else if ((0 == String.Compare(Extension, "ppt", true)) || (0 == String.Compare(Extension, "pps", true)))
          mimeType = "application/mspowerpoint";
        else if (0 == String.Compare(Extension, "gif", true))
          mimeType = "image/gif";
        else if ((0 == String.Compare(Extension, "jpg", true)) || (0 == String.Compare(Extension, "jpeg", true)))
          mimeType = "image/jpeg";
        else if ((0 == String.Compare(Extension, "mpg", true)) || (0 == String.Compare(Extension, "mpeg", true)))
          mimeType = "video/mpeg";
        else if (0 == String.Compare(Extension, "mov", true))
          mimeType = "video/quicktime";
        else if (0 == String.Compare(Extension, "rtf", true))
          mimeType = "application/rtf";
        else if (0 == String.Compare(Extension, "zip", true))
          mimeType = "application/zip";
        else if (0 == String.Compare(Extension, "mp3", true))
          mimeType = "audio/mpeg";
        else
          mimeType = "application/octet-stream";  // Unspezifizierter Mime Type.

        return mimeType;
      }
    }

    /// <summary>
    /// Ermittelt die Dateierweiterung.
    /// </summary>
    public String Extension
    {
      get
      {
        string extension = fileInfo.Extension;
        if(!string.IsNullOrEmpty(extension))
          extension = extension.Substring(1);

        return extension;
      }
    }

    #endregion

    #region Properties (Darstellung)

    /// <summary>
    /// Beschreibung dieser Datei.
    /// </summary>
    public string Description
    {
      get { return fileInfoCfg.Description; }
      set 
      {
        if (value != fileInfoCfg.Description)
        {
          fileInfoCfg.Description = value;
          SaveData();
        }
      }
    }

    /// <summary>
    /// Das Änderungsdatum der Datei.
    /// </summary>
    public DateTime ModificationDate
    {
      get 
      {
        if (fileInfoCfg.IsModificationDateNull())
        {
          // Der Zeitpunkt wird von der physikalischen Datei übernommen.
          fileInfoCfg.ModificationDate = fileInfo.LastWriteTime;
        }
        return fileInfoCfg.ModificationDate;
      }
      set
      {
        if (value != fileInfoCfg.ModificationDate)
        {
          fileInfoCfg.ModificationDate = value;
          SaveData();
        }
      }
    }

  #endregion

    # region Öffentliche Methoden
    
    /// <summary>
    /// Löscht die Datei.
    /// </summary>
    public void DeleteFile()
    {
      // Zuerst wird die physikalische Datei gelöscht.
      fileInfo.Delete();
      
      // Löschen der zugehörigen Daten.
      fileInfoCfg.Delete();
      parent.SaveData();

      parent.Detach(this);
    }

    #endregion

    # region Interne Methoden
    // Speichert die XML Daten.
    private void SaveData()
    {
      parent.SaveData();
    }

    #endregion
  }
}