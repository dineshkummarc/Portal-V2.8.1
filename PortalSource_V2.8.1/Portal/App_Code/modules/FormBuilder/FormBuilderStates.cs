namespace Portal.Modules.FormBuilder
{
  /// <summary>
  /// Die möglichen Status.
  /// </summary>
  public enum State : int
  {
    EditMain = 0,
		EditForm,
    EditValidation,
		EditEmail,
		EditResultSuccess,
    EditResultError
  };

  /// <summary>
  /// Die Events welche den Status beeinflussen können.
  /// </summary>
  public enum StateEvent
  {
    Save = 0,
    Cancel,
		EditForm,
    EditValidation,
		EditEmail,
    EditResultSuccess,
    EditResultError
  }
}
