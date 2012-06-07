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
using System.Web.Caching;

namespace Portal.Modules.ContentScheduler
{
  /// <summary>
  /// Hilfsklasse zur Verwaltung der Daten.
  /// </summary>
  public class ContentSchedulerHandler
  {
    # region Delegates
    # endregion

    # region Member Variables

    // Das aktuelle Portalmodul.
    private Portal.API.Module _portalModule;

    // Die Konfigurationsdaten, nachdem sie eingelesen wurden.
    private ContentSchedulerData _contentSchedulerData;

    // Referenz auf den aktuellen Datensatz, falls explizit ein Datensatz bearbeitet wird.
    private ContentSchedulerData.ContentEventRow _currentItem;

    // Das Object zum Schützen des Zugriffs auf die Physikalische XML Datei.
    private static object _lockObject = new object();

    # endregion

    # region Construction / Destruction

    /// <summary>
    /// Konstruktor der nicht explizit einen Datensatz bearbeitet.
    /// </summary>
    /// <param name="portalModule">Das Portal Modul.</param>
    public ContentSchedulerHandler(Portal.API.Module portalModule)
    {
      if (portalModule == null)
        throw new ArgumentNullException("portalModule");

      _portalModule = portalModule;
    }

    /// <summary>
    /// Verwaltet den angegebenen ContentEvent.
    /// </summary>
    /// <param name="portalModule">Das Portal Modul welches diesen Manager einsetzt.</param>
    /// <param name="contentEventId">Id des ContentEvents.</param>
    public ContentSchedulerHandler(Portal.API.Module portalModule, Guid contentEventId)
      : this(portalModule)
    {
      _currentItem = ConfigurationData.ContentEvent.FindById(contentEventId);
      if (_currentItem == null)
        throw new ArgumentException("Invalid Id", "contentEventId");
    }

    # endregion

    # region Properties

    /// <summary>
    /// Der aktuelle ContentEvent.
    /// </summary>
    public ContentSchedulerData.ContentEventRow CurrentContentEvent
    {
      get { return _currentItem; }
    }

    /// <summary>
    /// Die Id des aktuellen ContentEvents. Die 0-Guid, falls kein aktueller ContentEvent definiert ist.
    /// </summary>
    public Guid CurrentContentEventId
    {
      get
      {
        if (_currentItem != null)
          return _currentItem.Id;
        else
          return Guid.Empty;
      }
      set
      {
        _currentItem = ConfigurationData.ContentEvent.FindById(value);
      }
    }


    /// <summary>
    /// Ermittelt das gesamte DataSet mit allen ContentEvent-Daten.
    /// </summary>
    public ContentSchedulerData ConfigurationData
    {
      get
      {
        lock (_lockObject)
        {
          if (_contentSchedulerData == null)
          {
            // Versuche die Konfiguration aus der Applikation zu lesen.
            string applicationKey = "ContentScheduler" + _portalModule.ModuleRef + "Data";
            _contentSchedulerData = (ContentSchedulerData)_portalModule.Application[applicationKey];
            if (_contentSchedulerData == null)
            {
              // Laden der Konfiguration von Filesystem, falls eine entsprechende Datei dort existiert.
              _contentSchedulerData = new ContentSchedulerData();
              if (System.IO.File.Exists(_portalModule.ModuleConfigFile))
              {
                _contentSchedulerData.ReadXml(_portalModule.ModuleConfigFile);
                _contentSchedulerData.ContentEvent.DefaultView.Sort = "ActivationDate DESC, Hint, Id";
              }

              // Daten in der Application ablegen.
              _portalModule.Application[applicationKey] = _contentSchedulerData;
            }
          }
        }
        return _contentSchedulerData;
      }
    }



    /// <summary>
    /// Sucht nach dem nächsten Event.
    /// </summary>
    public ContentSchedulerData.ContentEventRow NextContentEvent
    {
      get
      {
        if (_currentItem == null)
          throw new InvalidOperationException("Current data is not defined");

        int nzIndex = 0;
        foreach (DataRowView eventRow in ConfigurationData.ContentEvent.DefaultView)
        {
          if (eventRow.Row == _currentItem)
          {
            if (--nzIndex >= 0)
              return (ContentSchedulerData.ContentEventRow)ConfigurationData.ContentEvent.DefaultView[nzIndex].Row;
            else
              return null;
          }
          nzIndex++;
        }

        return null;
      }
    }


    /// <summary>
    /// Sucht nach dem vorhergehenden Event.
    /// </summary>
    public ContentSchedulerData.ContentEventRow PreviousContentEvent
    {
      get
      {
        if (_currentItem == null)
          throw new InvalidOperationException("Current data is not defined");

        int nzIndex = 0;
        foreach (DataRowView eventRow in ConfigurationData.ContentEvent.DefaultView)
        {
          if (eventRow.Row == _currentItem)
          {
            if (++nzIndex < ConfigurationData.ContentEvent.Count)
              return (ContentSchedulerData.ContentEventRow)ConfigurationData.ContentEvent.DefaultView[nzIndex].Row;
            else
              return null;
          }
          nzIndex++;
        }

        return null;
      }
    }

    /// <summary>
    /// Ermittelt den Key für den Cache.
    /// </summary>
    private string CacheKey
    {
      get { return "ContentScheduler" + _portalModule.ModuleRef + "Content"; }
    }


    # endregion

    # region Methods

    /// <summary>
    /// Ermittelt den Inhalt der Seite, welche für den aktuellen Zeitpunkt gültig ist (Dieser wird gecached, um den
    /// Zugriff zu optimieren.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentContent()
    {
      // Versuche die Daten aus dem Cache zu ermitteln.
      object cacheContent = _portalModule.Cache[CacheKey];
      if (null == cacheContent)
      {
        // Die Daten direkt ermitteln.
        DateTime targetTime = DateTime.Now;
        DateTime nextTime = DateTime.Now.AddDays(1);
        string content = "";
        foreach (DataRowView row in ConfigurationData.ContentEvent.DefaultView)
        {
          ContentSchedulerData.ContentEventRow eventData = (ContentSchedulerData.ContentEventRow)row.Row;
          if (eventData.ActivationDate < targetTime)
          {
            content = GetContent(eventData);

            // Die Daten im Cache ablegen. Als Ablaufdatum wird das aktivierungsdatum des nächsten Eintrags angegeben.
            _portalModule.Cache.Insert(CacheKey, content, null, nextTime, Cache.NoSlidingExpiration);
            return content;

          }
          else
            nextTime = eventData.ActivationDate;
        }
        return content;
      }
      else
        return (string)cacheContent;
    }


    /// <summary>
    /// Setzt die Daten des aktuellen Eintrags. Falls kein aktueller Eintrag definiert ist, wird er erzeugt.
    /// </summary>
    /// <param name="hint">Der Hinweis zum ContentEvent</param>
    /// <param name="activationDate">Das Aktivierungsdatum des ContentEvents.</param>
    public void SetData(string hint, DateTime activationDate)
    {
      if (_currentItem == null)
      {
        // Neu.
        _currentItem = ConfigurationData.ContentEvent.NewContentEventRow();
        _currentItem.Id = Guid.NewGuid();
        _currentItem.ActivationDate = activationDate;
        _currentItem.Hint = hint;

        ConfigurationData.ContentEvent.AddContentEventRow(_currentItem);
      }
      else
      {
        // Edit.
        _currentItem.ActivationDate = activationDate;
        _currentItem.Hint = hint;
      }
      SaveAllData();
    }
    
    /// <summary>
    /// Speichert das Dataset mit den ContentEvents persistent in die XML Datei.
    /// </summary>
    public void SaveAllData()
    {
      if (_contentSchedulerData != null)
      {
        lock (_lockObject)
        {
          // Make sure the directory exists.
          System.IO.FileInfo fileInfo = new FileInfo(_portalModule.ModuleConfigFile);
          if (!fileInfo.Directory.Exists)
            System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName);
          _contentSchedulerData.WriteXml(_portalModule.ModuleConfigFile);
        }
      }
      
      // Cache zurücksetzen, damit der Inhalt von neuem ermittelt wird.
      _portalModule.Cache.Remove(CacheKey);
    }


    /// <summary>
    /// Ermittelt den Inhalt des aktuellen ContentEvents.
    /// </summary>
    /// <returns>Den Inhalt, bzw. ein leerer String.</returns>
    public string GetContent()
    {
      if (_currentItem == null)
        throw new InvalidOperationException("Current data is not defined");

      return GetContent(_currentItem);
    }


    /// <summary>
    /// Ermittelt den Inhalt des aktuellen ContentEvents.
    /// </summary>
    /// <param name="eventData">Id des Eintrags zu welchem der Inhalt ermittelt werden soll.</param>
    /// <returns>Den Inhalt, bzw. ein leerer String.</returns>
    public string GetContent(ContentSchedulerData.ContentEventRow eventData)
    {
      if (eventData == null)
        throw new ArgumentNullException("eventData");

      string contentData = "";

      // Datei öffnen und einlesen.        
      string file = GetContentPath(eventData, false);
      if ((file != null) && File.Exists(file))
      {
        using (FileStream fs = File.OpenRead(file))
        {
          StreamReader sr = new StreamReader(fs);
          contentData = sr.ReadToEnd();
          fs.Close();
        }
      }

      return contentData;
    }


    /// <summary>
    /// Speichert den Inhalt des aktuellen ContentEvents. 
    /// Es wird davon ausgegangen dass nur berechtigte Personen diesen Inhalt verändern können. Aus diesem Grund wird
    /// keine Überprüfung auf Script Tags durchgeführt.
    /// </summary>
    /// <param name="newContent"></param>
    public void SaveContent(string newContent)
    {
      if (_currentItem == null)
        throw new InvalidOperationException("Current data is not defined");

      using (FileStream fs = new FileStream(GetContentPath(_currentItem, true), FileMode.OpenOrCreate, 
                                            FileAccess.ReadWrite, FileShare.None))
      {
        fs.SetLength(0); // Truncate
        using (StreamWriter sw = new StreamWriter(fs))
        {
          sw.Write(newContent);
          _portalModule.Cache.Remove(CacheKey);
        }
      }
    }



    /// <summary>
    /// Gibt den Gesamten Pfad der Inhaltsdatei des angegebenen Datensatz zurück.  
    /// </summary>
    /// <param name="item">Datensatz zu welchem der Pfad ermittelt werden soll.</param>
    /// <param name="createPath">Erzeugt den Pfad-String, falls er nicht ermittelt werden kann (Dies hat auch ein 
    /// Speichern des Datensatzes zur Folge).</param>
    protected string GetContentPath(ContentSchedulerData.ContentEventRow itemData, bool createPath)
    {
      if (itemData == null)
        throw new ArgumentNullException("itemData");

      string directoryPath = Path.Combine(_portalModule.ModuleDataPhysicalPath, _portalModule.ModuleRef);
      string pageFile;
      if (!itemData.IsHtmlPageNull())
        pageFile = itemData.HtmlPage;
      else
        pageFile = null;

      if ((pageFile == null) && createPath)
      {
        if (!Directory.Exists(directoryPath))
          Directory.CreateDirectory(directoryPath);

        string hint = itemData.IsHintNull() ? "" : itemData.Hint;
        pageFile = Portal.API.Helper.CreateValidFileName(hint, "");

        // Falls dieser Dateiname bereits existiert, wird ein zähler hinzugefügt.
        lock (new object())
        {
          string pageFileBase = pageFile;
          pageFile += ".htm";
          int counter = 0;
          while (File.Exists(Path.Combine(directoryPath, pageFile)))
          {
            pageFile = pageFileBase + (++counter).ToString() + ".htm";
          }
          itemData.HtmlPage = pageFile;
          SaveAllData();
        }
      }

      if (pageFile != null)
        pageFile = Path.Combine(directoryPath, pageFile);

      return pageFile;
    }


    /// <summary>
    /// Kopiert den Inhalt von einem anderen ContentEvent in den aktuellen Wert.
    /// </summary>
    /// <param name="copySourceId"></param>
    public void CopyContent(Guid copySourceId)
    {
      if(_currentItem == null)
        throw new InvalidOperationException("Current data is not defined");

      // Suche den Quelldatensatz.
      ContentSchedulerData.ContentEventRow sourceData = ConfigurationData.ContentEvent.FindById(copySourceId);
      if(sourceData == null)
        throw new ArgumentException("copySourceId");

      // Falls die Source Datei existiert wird sie kopiert.
      string sourceFile = GetContentPath(sourceData, false);
      if(!string.IsNullOrEmpty(sourceFile) && File.Exists(sourceFile))
      {
        string destFile = GetContentPath(_currentItem, true);
        File.Copy(sourceFile, destFile, true);
      }
    }


    /// <summary>
    /// Löscht den aktuellen Eintrag. Falls kein Eintrag aktuell ist, wird nichts unternommen.
    /// </summary>
    public void Delete()
    {
      if(_currentItem != null)
      {
        string path = GetContentPath(_currentItem, false);
        if (!string.IsNullOrEmpty(path))
          File.Delete(path);
        ConfigurationData.ContentEvent.RemoveContentEventRow(_currentItem);
        _currentItem = null;
        SaveAllData();
      }
    }


    /// <summary>
    /// Löscht alle Events vor (in der Vergangenheit) dem angegebenen Eintrag (inklusive).
    /// </summary>
    /// <param name="Id">Die Id des ersten Eintrags der gelöscht werden soll.</param>
    public void CleanUp(Guid Id)
    {
      // Über die Daten iterieren und alle Einträge vor dem gesuchten in einer Liste ablegen. (Diese dürfen nicht direkt
      // gelöscht werden, damit es beim Iterieren nicht zu einem Problem kommt.
      List<ContentSchedulerData.ContentEventRow> rowsToDelete = new List<ContentSchedulerData.ContentEventRow>();

      bool deleteRange = false;
      foreach (DataRowView row in ConfigurationData.ContentEvent.DefaultView)
      {
        ContentSchedulerData.ContentEventRow eventData = (ContentSchedulerData.ContentEventRow)row.Row;
        if (deleteRange || eventData.Id == Id)
        {
          deleteRange = true;

          rowsToDelete.Add(eventData);

          // Falls das Ereignis dem aktuellen entspricht, muss die Referenz des aktuellen zurückgesetzt werden.
          if (_currentItem == eventData)
            _currentItem = null;
        }
      }

      // Alle Rows in der Liste entfernen.
      foreach (ContentSchedulerData.ContentEventRow row in rowsToDelete)
      {
        // Entfernen.
        string path = GetContentPath(row, false);
        if (!string.IsNullOrEmpty(path))
          File.Delete(path);
        ConfigurationData.ContentEvent.RemoveContentEventRow(row);
      }

      SaveAllData();
    }


    # endregion
  }
}