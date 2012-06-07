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
  /// Basisklasse für die Validator, ohne spezifische Properties.
  /// </summary>
  public class Validator : IValidator
  {
    private ModuleConfig.ValidationData _data;

    public Validator(ModuleConfig.ValidationData data)
    {
      _data = data;
    }

    public virtual bool Validate(string value)
    {
      // Die Pflichtfeldüberprüfung.
      return !Data.IsMandatory || ((value != null) && !string.IsNullOrEmpty(value.Trim()));
    }


    public ModuleConfig.ValidationData Data
    {
      get { return _data; }
    }

    #region IValidator Member

    public virtual void AddValidatorSettingsEdit(Control parentCtrl)
    {
    }


    public virtual void AddValidatorSettingsView(Control parentCtrl)
    {
    }


    public virtual void SaveValidatorSettings(Control parentCtrl)
    {
    }
    #endregion

    #region Hilfsfunktionen


    /// <summary>
    /// Versucht einen int Wert aus der spezifischen Konfiguration zu ermitteln.
    /// </summary>
    /// <param name="key">Schlüssel, unter welchem der Wert abgelegt ist.</param>
    /// <param name="value">Der gefundene Wert</param>
    /// <returns>Konnte der Wert erfolgreich ermittelt werden?</returns>
    protected bool TryGetProperty<TYPE>(string key, ref TYPE value)
    {
      bool found = false;
      object objValue;
      if (this.Data.Properties.TryGetValue(key, out objValue))
      {
        try
        {
          value = (TYPE)objValue;
          found = true;
        }
        catch (InvalidCastException)
        {
        }
      }

      return found;
    }



    #endregion
  }

}