using System;

namespace Portal.Modules.Forum
{
  /// <summary>
  /// Die Events welche den Status beeinflussen können.
  /// </summary>
  public enum StateEvents
  { 
    CancelToThreadView = 0,
    CancelToForumView,
    ShowThread,
    NewArticle,
    SearchText
  }

  /// <summary>
  /// IStateProcessor ist ein Interface für die Klassen, welche den Events welche sich auf den aktuellen Status 
  /// auswirken verarbeiten können.
  /// </summary>
  public interface IStateProcessor
  {
    /// <summary>
    /// Verarbeitet ein aufgetretenes Event.
    /// </summary>
    /// <returns>Ist eine Statusänderung aufgetreten wird true zurückgegeben.</returns>
    bool SetEvent(StateEvents newEvent);

    /// <summary>
    /// Ermittelt das zentrale Konfigurationsobjekt.
    /// </summary>
    /// <returns></returns>
    ConfigAgent ConfigAgent { get; }
  }
}
