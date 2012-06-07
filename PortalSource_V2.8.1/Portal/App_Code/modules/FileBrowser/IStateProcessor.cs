namespace Portal.Modules.FileBrowser
{
  /// <summary>
  /// Die möglichen Stati.
  /// </summary>
  public enum State : int
  {
    viewFiles = 0,
    editDirectory,
    addFiles,
    editFile,
  };

  /// <summary>
  /// Die Events welche den Status beeinflussen können.
  /// </summary>
  public enum StateEvent
  {
    ok = 0,
    cancel,
    editDirectory,
    addFiles,
    editFile,
  }


  /// <summary>
  /// Interface für zentrale Klasse, welche den Status verwaltet.
  /// </summary>
  public interface IStateProcessor
  {
    /// <summary>
    /// Ermittelt das zentrale Konfigurationsobjekt.
    /// </summary>
    /// <returns></returns>
    ConfigAgent ConfigAgent { get; }
  }
}
