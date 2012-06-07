using System;

namespace Portal.Modules.Forum
{
  /// <summary>
  /// Zusammenfassung für PhotoGalleryStateMachine.
  /// </summary>
  public class StateMachine
  {
    #region Membervariablen
        
    /// <summary>
    /// Der Session Key welcher eingesetzt wird.
    /// </summary>
    private string sessionKey;

    public struct StateInfos
    {
      public string ControlPath;
    }

    private enum ForumStates
    {
      ForumView = 0,
      ThreadView,
      WriteArticleView,
      SearchResults
    };

    // Aktueller Status der Statusmaschine.
    ForumStates currentState = ForumStates.ForumView;
 
    #endregion
        
    #region Methoden
    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="szSessKey">Key welcher für die Identifikation des States in der Session verwendet wird.</param>
    public StateMachine(string szSessKey)
    {
      sessionKey = szSessKey + "State";

      // Wir versuchen den aktuellen Status aus der Session zu laden.
      if (null != System.Web.HttpContext.Current.Session[sessionKey])
      {
        currentState = (ForumStates)System.Web.HttpContext.Current.Session[sessionKey];      
        System.Web.HttpContext.Current.Session[sessionKey] = currentState;
      }
    }


    /// <summary>
    /// Signalisiert ein Event, welches den Status der Statusmaschine beeinflussen kann. Der Status wird wenn nötig 
    /// angepasst.
    /// </summary>
    /// <returns>true, wenn eine Änderung erfolgt ist.</returns>    
    /// <param name="NewEvent">Das aufgetretene Ereignis.</param>
    public virtual bool SetEvent(StateEvents newEvent)
    {
      bool changed = false;

      switch(currentState)
      {
        case ForumStates.ForumView:                       // Aktueller Status MainView.
        {
          switch(newEvent)
          {
            case StateEvents.ShowThread:
              currentState = ForumStates.ThreadView;
              changed = true;
              break;
            case StateEvents.NewArticle:
              currentState = ForumStates.WriteArticleView;
              changed = true;
              break;
          }
          break;
        }

        case ForumStates.ThreadView:
        {
          switch (newEvent)
          {
            case StateEvents.CancelToForumView:
              currentState = ForumStates.ForumView;
              changed = true;
              break;
            case StateEvents.NewArticle:
              currentState = ForumStates.WriteArticleView;
              changed = true;
              break;
          }
          break;
        }

        case ForumStates.WriteArticleView:
        {
          if((newEvent == StateEvents.CancelToThreadView) ||
             (newEvent == StateEvents.ShowThread))
          {
            currentState = ForumStates.ThreadView;
            changed = true;
          }
          else if (newEvent == StateEvents.CancelToForumView)
          {
            currentState = ForumStates.ForumView;
            changed = true;
          }
          break;
        }

        case ForumStates.SearchResults:
        {
          if (newEvent == StateEvents.CancelToForumView)
          {
            currentState = ForumStates.ForumView;
            changed = true;
          }
          else if (newEvent == StateEvents.ShowThread)
          {
            currentState = ForumStates.ThreadView;
            changed = true;
          }
          break;
        }
      }

      if (newEvent == StateEvents.SearchText)
      {
        currentState = ForumStates.SearchResults;
        changed = true;
      }

      // Abspeichern des aktuellen Status.
      if(changed)
        System.Web.HttpContext.Current.Session[sessionKey] = currentState;
              
      return changed;
    }

    /// <summary>
    /// Ermittelt die Informationen zum aktuellen Status.
    /// </summary>
    /// <returns></returns>
    public StateInfos StateInfo
    {
        get
        {
            StateInfos info = new StateInfos();

            switch (currentState)
            {
                case ForumStates.ForumView:
                    info.ControlPath = "ForumView.ascx";
                    break;
                case ForumStates.ThreadView:
                    info.ControlPath = "ThreadView.ascx";
                    break;
                case ForumStates.WriteArticleView:
                    info.ControlPath = "WriteArticleView.ascx";
                    break;
                case ForumStates.SearchResults:
                    info.ControlPath = "SearchResults.ascx";
                    break;
            }

            return info;
        }
    }

    /// <summary>
    /// Setzt den Zustand der Statusmaschine zurück.
    /// </summary>
    public void ResetState()
    {
      currentState = ForumStates.ForumView;
      System.Web.HttpContext.Current.Session[sessionKey] = currentState;
    }

    #endregion
  }
}
