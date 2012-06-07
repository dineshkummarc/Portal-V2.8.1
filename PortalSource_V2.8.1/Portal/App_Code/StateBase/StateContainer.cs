using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.Serialization;

namespace Portal.StateBase
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für StateContainer
  /// </summary>
  public class StateContainer<STATEKEY, TRANSITIONKEY> : Portal.API.Module
  {
    #region Membervariabeln
    
    /// <summary>
    /// Die Statusmaschine, welche den aktuellen Status verwaltet.
    /// </summary>
    private StateMachine stateMachine;

    /// <summary>
    /// Das Regelwerk, welches die Konfiguration der möglichen Stati und die Statusübergänge beinhaltet.
    /// </summary>
    private static RuleSet stateRuleSet = new RuleSet();

    #endregion

    #region Ereignishandler

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
    } 

    #endregion

    #region Statische Methoden zum Konfigurieren der Statusmaschine

        /// <summary>
    /// Fügt einen neuen Status hinzu oder aktualisiert diesen.
    /// </summary>
    /// <param name="stateKey">Der Key, unter welchem dieser Status abgelegt werden soll. Intern wird dieser Key in 
    /// einen Integer konvertiert, um den Status zu referenzieren. Existiert dieser Key bereits, wird der entsprechende
    /// Status aktualisiert.</param>
    /// <param name="ctrlFile">Pfad zur ascx-Datei, welches diesen Status repräsentiert.</param>
    static protected void AddState(STATEKEY stateKey, string ctrlFile)
    {
      stateRuleSet.AddState(Convert.ToInt32(stateKey), ctrlFile);
    }


    /// <summary>
    /// Fügt einen Übergang zwischen zwei Stati der Statusmaschine ein, oder aktualisiert diesen.
    /// </summary>
    /// <param name="stateSrc">Der Quellstatus der Transition.</param>
    /// <param name="transitionKey">Der Key, welcher zur Referenzierung der Transition verwendet wird. Intern wird 
    /// dieser Key in einen Integer konvertiert, um den Status zu referenzieren. Existiert dieser Key bereits, wird der 
    /// entsprechende Status aktualisiert.</param>
    /// <param name="stateTarget">Der Zielstatus der Transition.</param>
    public static void AddTransition(STATEKEY stateSrc, TRANSITIONKEY transitionKey, STATEKEY stateTarget)
    {
      stateRuleSet.AddTransition(Convert.ToInt32(stateSrc), Convert.ToInt32(transitionKey), 
                                 Convert.ToInt32(stateTarget));
    }
    
    #endregion

    #region Methoden zum Laden des Controls.

    /// <summary>
    /// Lädt das momentan aktuelle Control, in den Container.
    /// </summary>
    protected virtual void LoadCurrentCtrl(bool isPostBack)
    {
      Controls.Add(CreateCurrentCtrl(isPostBack));
    }


    /// <summary>
    /// Erzeugt das aktuelle Control.
    /// </summary>
    /// <param name="isPostBack">Soll der ControlPostback Status gesetzt sein?</param>
    /// <returns>Das erzeugte Control</returns>
    protected virtual Portal.StateBase.ModuleState CreateCurrentCtrl(bool isPostBack)
    {
      return CreateCtrl(ModuleStateMachine.CurrentCtrlPath, isPostBack);
    }


    /// <summary>
    /// Erzeugt das angegebene Control.
    /// </summary>
    /// <param name="CtrlPath">Der Pfad zur ascx-Datei.</param>
    /// <param name="isPostBack">Soll der ControlPostback Status gesetzt sein?</param>
    /// <returns>Das erzeugte Control</returns>
    protected virtual Portal.StateBase.ModuleState CreateCtrl(string ctrlPath, bool isPostBack)
    {
      ModuleState newCtrl = (ModuleState) LoadControl(ctrlPath);
      newCtrl.InitModule(TabRef, ModuleRef, ModuleType, ModuleDataVirtualPath, ModuleHasEditRights);
      newCtrl.CurrentState = ModuleStateMachine.CurrentStateKey;
      newCtrl.ControlPostback = isPostBack;
      newCtrl.TransitionArgument = ViewState[UniqueID + "TransitionArg"];
      newCtrl.EventOccured += new ModuleState.StateTransition(OnProcessEvent);
      return newCtrl;
    }

    #endregion

    # region Verarbeiten des Statusübergangs
    /// <summary>
    /// Verarbeitet ein Event, das einen Statusübergang zur Folge hat.
    /// </summary>
    /// <param name="newEvent"></param>
    /// <returns></returns>
    public bool OnProcessEvent(int newEvent, object transitionArgument)
    {
      // Das Transition-Argument wird in jedem Fall abgelegt.
      ViewState[UniqueID + "TransitionArg"] = transitionArgument;

      bool changed = ModuleStateMachine.SetEvent(newEvent);
      if (changed)
      {
        ViewState[UniqueID + "CurrState"] = ModuleStateMachine.CurrentStateKey;
        Controls.Clear();
        LoadCurrentCtrl(false);
      }

      return changed;
    }

    #endregion

    #region Properties (intern)
    /// <summary>
    /// Ermittelt die State-Machine, welche den Ablauf steuert.
    /// </summary>
    private StateMachine ModuleStateMachine
    {
      get
      {
        // Falls die Statusmaschine noch nicht existiert, wird sie erzeugt.
        if (this.stateMachine == null)
        {
          object vsState = ViewState[UniqueID + "CurrState"];
          if(null == vsState)
            this.stateMachine = new StateMachine(stateRuleSet);                 // Initialstatus.
          else
            this.stateMachine = new StateMachine(stateRuleSet, (int) vsState);  // Neuer Status.
        }

        return this.stateMachine;
      }
    }
    #endregion
  }
}