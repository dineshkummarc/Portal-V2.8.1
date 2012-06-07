using System;
using System.Data;
using System.Web.UI;
using Portal.API;

namespace Portal.Modules.Forum
{
  /// <summary>
  /// Zusammenfassung für ConfigAgent.
  /// </summary>
  public class ConfigAgent
  {
    /// <summary>
    /// Speichert die Modul-Konfiguration.
    /// </summary>
    private ModuleConfig moduleConfig;

    /// <summary>
    /// Speichert die ID der Parent-Message, falls ein neuer Artikel erfasst werden soll.
    /// </summary>
    int parentMessage;

    /// <summary>
    /// Speichert den Namen des Thread-Files, in welchem der neue Artikel gespeichert werden soll.
    /// </summary>
    string threadFile;

    /// <summary>
    /// Speichert den Suchtext.
    /// </summary>
    string searchText;

    /// <summary>
    /// Speichert den Suchtext des letzten Suche.
    /// </summary>
    string lastSearchText;

    /// <summary>
    /// Speichert die Ergebnisse der letzten Suche.
    /// </summary>
    DataTable searchResults;

    /// <summary>
    /// ID des Artikels, zu welchem bei einer Suche gesprungen werden soll.
    /// </summary>
    int articleIdToShow = -1;

    /// <summary>
    /// Speichert das aktuelle Modul.
    /// </summary>
    Portal.API.Module module;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public ConfigAgent()
    {
    }

    /// <summary>
    /// Gibt zurück, ob der aktuelle User das Recht zur Erstellung eines neuen Threads hat.
    /// </summary>
    /// <returns>True, falls der aktuelle User einen neuen Thread erzeugen darf, ansonsten false.</returns>
    public bool IsThreadCreationAllowed()
    {
      if (module.Page.User.IsInRole(Portal.API.Config.AdminRole))
        return true;

      int nThreadCreationRight = moduleConfig.ThreadCreationRight;

      if ((nThreadCreationRight == 2 /*ModuleAdmin*/) &&
          (module.ModuleHasEditRights))
      {
        return true;
      }

      if ((nThreadCreationRight == 1 /*User*/) &&
          (module.Page.User.IsInRole(Portal.API.Config.UserRole)))
      {
        return true;
      }

      if (nThreadCreationRight == 0 /*Everyone*/)
        return true;

      return false;
    }

    /// <summary>
    /// Gibt zurück, um welche Art von Artikel es sich handelt.
    /// </summary>
    /// <returns>True, wenn es sich um einen Thread-Artikel, false, wenn es sich um einen Top-Level Artikel handelt.</returns>
    public bool IsThreadMessage()
    {
      return parentMessage != -1;
    }

    /// <summary>
    /// Legt die internen Variablen so fest, dass die Daten für einen neuen Thread bekannt sind.
    /// </summary>
    public void SetNewThread()
    {
      parentMessage = -1;
      threadFile = Guid.NewGuid().ToString() + ".xml";
    }

    /// <summary>
    /// Liefert den vollen Namen des Users anhand der ID.
    /// </summary>
    /// <param name="szUserId"></param>
    /// <returns></returns>
    public string GetUserWithId(Guid userId)
    {
        Portal.API.Users.UserRow row = UserManagement.Users.FindById(userId);
        if (null != row)
            return row.firstName + " " + row.surName;
        else
            return "Mr. Unknown";
    }

    #region Eigenschaften
    public ModuleConfig ModuleConfig
    {
      get
      {
        return moduleConfig;
      }
      set
      {
        if (null != value)
          moduleConfig = (ModuleConfig)value;
        else
          moduleConfig = new ModuleConfig();
      }
    }

    public int ParentMessage
    {
      get
      {
        return parentMessage;
      }
      set
      {
        parentMessage = value;
      }
    }

    public string ThreadFile
    {
      get
      {
        return threadFile;
      }
      set
      {
        threadFile = value;
      }
    }

    public Portal.API.Module Module
    {
      get
      {
        return module;
      }
      set
      {
        module = value;
      }
    }

    public string CurrentUserName
    {
      get
      {
        Portal.API.Principal principal = (Portal.API.Principal) Module.Page.User;
        return principal.FullName;
      }
    }

    public string CurrentUserEmail
    {
      get
      {
        Portal.API.Principal principal = (Portal.API.Principal)Module.Page.User;
        return principal.EMail;
      }
    }

    public Guid CurrentUserId
    {
      get
      {
        Portal.API.Principal principal = (Portal.API.Principal)Module.Page.User;
        return principal.Id;
      }
    }

    public string SearchText
    {
      get
      {
        return searchText;
      }
      set
      {
        searchText = value;
      }
    }

    public string LastSearchText
    {
      get
      {
        return lastSearchText;
      }
      set
      {
        lastSearchText = value;
      }
    }

    public DataTable SearchResults
    {
      get
      {
        return searchResults;
      }
      set
      {
        searchResults = value;
      }
    }

    public int ArticleToShow
    {
      get
      {
        return articleIdToShow;
      }
      set
      {
        articleIdToShow = value;
      }
    }
    #endregion
  }
}
