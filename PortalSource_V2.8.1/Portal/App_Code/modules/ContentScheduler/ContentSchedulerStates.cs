namespace Portal.Modules.ContentScheduler
{
  /// <summary>
  /// Die möglichen Stati.
  /// </summary>
  public enum State : int
  {
    EventOverview = 0,
    EditEvent,
    EditPage,
    Preview,
    CleanUp,
  };

  /// <summary>
  /// Die Events welche den Status beeinflussen können.
  /// </summary>
  public enum StateEvent
  {
    Save = 0,
    Cancel,
    Back,
    NewEvent,
    EditEvent,
    EditPage,
    ShowPreview,
    CleanUp
  }
}
