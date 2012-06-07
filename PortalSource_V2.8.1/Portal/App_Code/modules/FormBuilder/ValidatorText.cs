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
  /// Validiert normalen Text.
  /// </summary>
  public class ValidatorText : Validator
  {

    public ValidatorText(ModuleConfig.ValidationData data)
      : base(data)
    {
    }

    #region IValidator Member

    public override void AddValidatorSettingsEdit(Control parentCtrl)
    {
      Portal.API.Controls.Label lbl = new Portal.API.Controls.Label();
      lbl.LanguageRef = "MinimumLength";
      parentCtrl.Controls.Add(lbl);

      // Minimale Länge.
      TextBox lengthBox = new TextBox();
      lengthBox.ID = "_minLength";
      int length = 0;
      if(TryGetProperty("MinimumLength", ref length))
        lengthBox.Text = length.ToString();

      parentCtrl.Controls.Add(lengthBox);

      parentCtrl.Controls.Add(new LiteralControl("</br>"));

      // Maximale Länge.
      lbl = new Portal.API.Controls.Label();
      lbl.LanguageRef = "MaximumLength";
      parentCtrl.Controls.Add(lbl);

      lengthBox = new TextBox();
      lengthBox.ID = "_maxLength";
      if (TryGetProperty("MaximumLength", ref length))
        lengthBox.Text = length.ToString();
        
      parentCtrl.Controls.Add(lengthBox);
    }


    public override void AddValidatorSettingsView(Control parentCtrl)
    {
      int minLength = 0;
      bool minExist = TryGetProperty("MinimumLength", ref minLength);

      int maxLength = 0;
      bool maxExist = TryGetProperty("MaximumLength", ref maxLength);

      if (minExist)
      {
        Portal.API.Controls.Label lbl = new Portal.API.Controls.Label();
        lbl.LanguageRef = "MinimumLength";
        parentCtrl.Controls.Add(lbl);

        Label val = new Label();
        val.Text = minLength.ToString();
        parentCtrl.Controls.Add(val);

        if (maxExist)
          parentCtrl.Controls.Add(new LiteralControl("</br>"));
      }

      if (maxExist)
      {
        Portal.API.Controls.Label lbl = new Portal.API.Controls.Label();
        lbl.LanguageRef = "MaximumLength";
        parentCtrl.Controls.Add(lbl);

        Label val = new Label();
        val.Text = maxLength.ToString();
        parentCtrl.Controls.Add(val);
      }
    }


    public override void SaveValidatorSettings(Control parentCtrl)
    {
      this.Data.Properties.Clear();

      // Minimale Länge.
      TextBox lengthBox = (TextBox) parentCtrl.FindControl("_minLength");
      if (lengthBox != null)
      {
        int length;
        if (int.TryParse(lengthBox.Text, out length))
        {
          if(length > 0)
            this.Data.Properties["MinimumLength"] = length;
        }
      }

      // Maximale Länge.
      lengthBox = (TextBox)parentCtrl.FindControl("_maxLength");
      if (lengthBox != null)
      {
        int length;
        if (int.TryParse(lengthBox.Text, out length))
        {
          if (length > 0)
            this.Data.Properties["MaximumLength"] = length;
        }
      }
    }

    public override bool Validate(string value)
    {
      bool valid = base.Validate(value);
      if(valid && !string.IsNullOrEmpty(value))
      {
        int length = 0;
        if (TryGetProperty("MinimumLength", ref length))
        {
          if (value.Length < length)
            valid = false;
        }

        if (TryGetProperty("MaximumLength", ref length))
        {
          if (value.Length > length)
            valid = false;
        }
      }
      return valid;
    }

    #endregion
  }

}