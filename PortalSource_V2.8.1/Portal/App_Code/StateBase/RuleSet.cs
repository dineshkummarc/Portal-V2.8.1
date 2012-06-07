using System;
using System.Collections.Generic;

namespace Portal.StateBase
{
  /// <summary>
  /// Klasse welche die Status und die Statusübergänge der Statusmaschine speichert.
  /// </summary>
  public class RuleSet
  {
    #region Status
    /// <summary>
    /// Repräsentiert einen Status.
    /// </summary>
    public class State
    {
      /// <summary>
      /// Der Pfad zum Control (ascx).
      /// </summary>
      private string ctrlFile;

      /// <summary>
      /// Die Nummer dieses Status.
      /// </summary>
      private int stateKey;
      
      /// <summary>
      /// Die möglichen Statusübergänge.
      /// </summary>
      private Dictionary<int, State> transitions;

      /// <summary>
      /// Konstruktor.
      /// </summary>
      /// <param name="ctrlFile">Der Pfad zum Control (ascx).</param>
      public State(string ctrlFile, int stateKey)
      {
        this.ctrlFile = ctrlFile;
        this.stateKey = stateKey;
        transitions = new Dictionary<int, State>();
      }

      /// <summary>
      /// Pfad zum Control.
      /// </summary>
      public string ControlFile
      {
        get { return this.ctrlFile; }
        set { this.ctrlFile = value; }
      }

      /// <summary>
      /// Key dieses Status.
      /// </summary>
      public int StateKey
      {
        get { return this.stateKey; }
      }

      /// <summary>
      /// Fügt einen neuen Statusübergang hinzu oder aktualisiert diesen.
      /// </summary>
      /// <param name="key">Der Key des Übergangs.</param>
      /// <param name="targetState">Der neue Status, in welcher das Ziel des Übergangs ist.</param>
      public void AddTransition(int key, State targetState)
      {
        this.transitions[key] = targetState;
      }

      /// <summary>
      /// Sucht nach dem Zielstatus des Übergangs mit dem übergebenen Key.
      /// </summary>
      /// <param name="key">Key des Übergangs.</param>
      /// <returns>Der neue Status, oder null wenn der Übergang nicht existiert.</returns>
      public State GetTransitionTarget(int key)
      {
        State stateResult = null;
        if (this.transitions.ContainsKey(key))
          stateResult = this.transitions[key];
        return stateResult;
      }
    }
    #endregion

    /// <summary>
    /// Der Initialisierungsstatus dieses Rulesets.
    /// </summary>
    private RuleSet.State initialState = null;

    /// <summary>
    /// Alle vorhandenen Stati. Der Key ist die Nummer des Status.
    /// </summary>
    private Dictionary<int, State> states;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public RuleSet()
    {
      this.states = new Dictionary<int, State>();
    }

    /// <summary>
    /// Fügt einen neuen Status hinzu oder aktualisiert diesen.
    /// </summary>
    /// <param name="stateKey">Der Key, unter welchem dieser Status abgelegt werden soll. Existiert dieser Key bereits, 
    /// wird der entsprechende Status aktualisiert.</param>
    /// <param name="ctrlFile">Pfad zur ascx-Datei, welches diesen Status repräsentiert.</param>
    /// <returns>Gibt den neu erzeugten Status zurück.</returns>    
    public RuleSet.State AddState(int stateKey, string ctrlFile)
    {
      State newState;
      // Überprüfe ob der Status bereits existiert.
      if (states.ContainsKey(stateKey))
      {
        // Aktualisiere die Daten.
        newState = states[stateKey];
        newState.ControlFile = ctrlFile;
      }
      else
      {
        // Erzeuge den neuen Status.
        newState = new State(ctrlFile, stateKey);
        states.Add(stateKey, newState);
      }

      // Falls noch keine Initialisierungsstatus gesetzt wurde, wird dieser Status übernommen.
      if (null == InitialState)
        InitialState = newState;

      return newState;
    }

    /// <summary>
    /// Fügt einen Übergang zwischen zwei Stati ein, oder aktualisiert diesen.
    /// </summary>
    /// <param name="stateSrc">Der Quellstatus der Transition.</param>
    /// <param name="transitionKey">Der Key, welcher zur Referenzierung der Transition verwendet wird.</param>
    /// <param name="stateTarget">Der Zielstatus der Transition.</param>
    public void AddTransition(int stateSrc, int transitionKey, int stateTarget)
    {
      // Exisitert der Quellstatus?
      if (!this.states.ContainsKey(stateSrc))
        throw new ArgumentException("Source-state does not exist");

      // Exisitert der Zielstatus?
      if (!this.states.ContainsKey(stateTarget))
        throw new ArgumentException("Target-state does not exist");

      // Statusübergang hinzufügen.
      states[stateSrc].AddTransition(Convert.ToInt32(transitionKey), states[stateTarget]);
    }


    /// <summary>
    /// Liefert den Status zum Key.
    /// </summary>
    /// <param name="stateKey"></param>
    /// <returns></returns>
    public RuleSet.State GetState(int stateKey)
    {
      return this.states[stateKey];
    }


    /// <summary>
    /// Initialisierungsstatus
    /// </summary>
    public RuleSet.State InitialState
    {
      get { return initialState; }
      set { initialState = value; }
    }
  }
}