using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.FormBuilder
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für IValidator
  /// </summary>
  public interface IValidator
  {
    /// <summary>
    /// Validiert den angegebenen String. 
    /// </summary>
    /// <param name="value">Der String welcher überprüft werden soll.</param>
    /// <returns>true, wenn die Validierung erfolgreich war.</returns>
    bool Validate(string value);

    /// <summary>
    /// Fügt dem ParentCtrl die Controls zum Definieren der Einstellungen hinzu.
    /// </summary>
    /// <param name="parentCtrl"></param>
    /// <param name="validator"></param>
    void AddValidatorSettingsEdit(Control parentCtrl);

    /// <summary>
    /// Fügt dem ParentCtrl die Controls zum Anzeigen der Einstellungen hinzu.
    /// </summary>
    /// <param name="parentCtrl"></param>
    /// <param name="validator"></param>
    void AddValidatorSettingsView(Control parentCtrl);

    /// <summary>
    /// Speichert die Einstellungen des Validators.
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="parentCtrl"></param>
    void SaveValidatorSettings(Control parentCtrl);
  }
}