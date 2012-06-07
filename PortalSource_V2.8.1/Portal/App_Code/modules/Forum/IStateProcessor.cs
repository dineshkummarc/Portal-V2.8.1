using System;

namespace Portal.Modules.Forum
{
  /// <summary>
  /// Die Events welche den Status beeinflussen k�nnen.
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
  /// IStateProcessor ist ein Interface f�r die Klassen, welche den Events welche sich auf den aktuellen Status 
  /// auswirken verarbeiten k�nnen.
  /// </summary>
  public interface IStateProcessor
  {
    /// <summary>
    /// Verarbeitet ein aufgetretenes Event.
    /// </summary>
    /// <returns>Ist eine Status�nderung aufgetreten wird true zur�ckgegeben.</returns>
    bool SetEvent(StateEvents newEvent);

    /// <summary>
    /// Ermittelt das zentrale Konfigurationsobjekt.
    /// </summary>
    /// <returns></returns>
    ConfigAgent ConfigAgent { get; }
  }
}
