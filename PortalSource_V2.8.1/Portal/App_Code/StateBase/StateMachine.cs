using System;

namespace Portal.StateBase
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für StateMachine
  /// </summary>
  public class StateMachine
  {
    #region Membervariabeln.

    /// <summary>
    /// Regelwerk mit den übergängen zwischen den verfügbaren Stati.
    /// </summary>
    private RuleSet ruleSet;

    /// <summary>
    /// Aktuelle Status.
    /// </summary>
    private RuleSet.State currentState;

    #endregion

    # region Konstruktoren.

    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="ruleSet">Regelwerk für die Übergänge zwischen den einzelnen Stati</param>
    public StateMachine(RuleSet ruleSet)
      : this(ruleSet, ruleSet.InitialState)
    {
    }


    /// <summary>
    /// Konstruktor, welcher den Key des aktuellen Status übernimmt.
    /// </summary>
    /// <param name="ruleSet">Regelwerk für die Übergänge zwischen den einzelnen Stati</param>
    /// <param name="currentState">Key des aktuellen Status</param>
    public StateMachine(RuleSet ruleSet, int currentState)
      : this(ruleSet, ruleSet.GetState(currentState))
    {
    }


    /// <summary>
    /// Konstruktor, welcher den aktuellen Status übernimmt.
    /// </summary>
    /// <param name="ruleSet">Regelwerk für die Übergänge zwischen den einzelnen Stati</param>
    /// <param name="currentState">Aktuellen Status</param>
    public StateMachine(RuleSet ruleSet, RuleSet.State currentState)
    {
      this.ruleSet = ruleSet;
      this.currentState = currentState;
    }

    #endregion

    # region Öffentliche Methoden

    /// <summary>
    /// Setzt den Zustand der Statusmaschine zurück.
    /// </summary>
    public void ResetState()
    {
      currentState = this.ruleSet.InitialState;
    }

    /// <summary>
    /// Signalisiert ein Event, welches den Status der Statusmaschine beeinflussen kann. Der Status wird wenn nötig 
    /// angepasst. Diese Methode enthält die Logik über die möglichen Statusübergänge.
    /// </summary>
    /// <returns>true, wenn eine Änderung erfolgt ist.</returns>    
    /// <param name="NewEvent">Das aufgetretene Ereignis.</param>
    public virtual bool SetEvent(int transition)
    {
      bool changed = false;

      RuleSet.State newState = currentState.GetTransitionTarget(Convert.ToInt32(transition));
      if(null != newState)
      {
        currentState = newState;
        changed = true;
      }

      return changed;
    }

    # endregion

    #region Öffentliche Properties
    
    /// <summary>
    /// Ermittelt die Informationen zum aktuellen Status.
    /// </summary>
    /// <returns></returns>
    public string CurrentCtrlPath
    {
      get { return currentState.ControlFile; }
    }

    /// <summary>
    /// Ermittelt den Key des aktuellen Status.
    /// </summary>
    public int CurrentStateKey
    {
      get { return currentState.StateKey; }
    }
    
    #endregion
  }
}
