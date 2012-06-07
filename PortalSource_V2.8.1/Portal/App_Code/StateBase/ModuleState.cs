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
  /// Zusammenfassungsbeschreibung für ModuleState
  /// </summary>
  public class ModuleState : Portal.API.Module
  {
    /// <summary>
    /// Delegate zum verarbeiten eines Statusübergangs.
    /// </summary>
    /// <param name="eventKey">Der Identifier des Events, welcher den Statusübergang ausgelöst hat.</param>
    /// <param name="transitionArgument">Ein Argument welches dem neuen Status übermittelt wird.</param>
    /// <returns>Konnte der Statusübergang erfolgreich verarbeitet werden?</returns>
    public delegate bool StateTransition(int eventKey, object transitionArgument);

    /// <summary>
    /// Event für die Registrierung des Statusübergangs.
    /// </summary>
    public event StateTransition EventOccured;

    /// <summary>
    /// Handelt es sich um ein Postback (aus Sicht des Controls).
    /// </summary>
    private bool controlPostback = false;

    // Der aktuelle Status.
    private int currentState = 0;

    // Eine Zusatzinformation zum letzten Statusübergang.
    private object transitionArgument;

    /// <summary>
    /// Wurde dieses Control als Postback geladen? Der normale IsPostback() status gilt für die Page. Aus diesem Grund
    /// kann es vorkommen dass die Page im Postback status ist, das Control jedoch zum ersten mal geladen wird. Dieser
    /// Status definiert spezifische Informationen zum Control.
    /// </summary>
    public bool ControlPostback
    {
      get { return this.controlPostback; }
      set { this.controlPostback = value; }
    }

    /// <summary>
    /// Der aktuelle Status, in welchem sich dieses Modul befindet.
    /// </summary>
    public int CurrentState
    {
      get { return this.currentState; }
      set { this.currentState = value; }
    }


    public object TransitionArgument
    {
      get { return this.transitionArgument; }
      set { this.transitionArgument = value; }
    }

    /// <summary>
    /// Verarbeitet ein aufgetretenes Event.
    /// </summary>
    /// <param name="occuredEvent">Key des aufgetretenen Ereignis</param>
    /// <returns>Ist eine Statusänderung erfolgt, wird true zurückgegeben.</returns>
    public bool ProcessEvent(int occuredEvent)
    {
      return ProcessEvent(occuredEvent, null);
    }

    /// <summary>
    /// Verarbeitet ein aufgetretenes Event.
    /// </summary>
    /// <param name="occuredEvent">Key des aufgetretenen Ereignis</param>
    /// <param name="transitionArgument">Argument welches an den neuen Status übermittelt wird.</param>
    /// <returns>Ist eine Statusänderung erfolgt, wird true zurückgegeben.</returns>
    public bool ProcessEvent(int occuredEvent, object transitionArgument)
    {
      if (this.EventOccured != null)
        return EventOccured(occuredEvent, transitionArgument);
      else
        return false;
    }
  }
}
