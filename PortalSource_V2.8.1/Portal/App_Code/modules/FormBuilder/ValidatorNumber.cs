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
  public class ValidatorNumber : Validator
  {
    public ValidatorNumber(ModuleConfig.ValidationData data)
      : base(data)
    {
    }

    #region IValidator Member

    public override void AddValidatorSettingsEdit(Control parentCtrl)
    {
      // Minimum
      Portal.API.Controls.Label lbl = new Portal.API.Controls.Label();
      lbl.LanguageRef = "Minimum";
      parentCtrl.Controls.Add(lbl);

      TextBox minimumBox = new TextBox();
      minimumBox.ID = "_minimum";
      int minimum = 0;
      if (TryGetProperty("MinimumValue", ref minimum))
        minimumBox.Text = minimum.ToString();
      parentCtrl.Controls.Add(minimumBox);

      parentCtrl.Controls.Add(new LiteralControl("</br>"));

      // Maximum.
      lbl = new Portal.API.Controls.Label();
      lbl.LanguageRef = "Maximum";
      parentCtrl.Controls.Add(lbl);

      TextBox maximumBox = new TextBox();
      maximumBox.ID = "_maximum";
      int maximum = 0;
      if (TryGetProperty("MaximumValue", ref maximum))
        maximumBox.Text = maximum.ToString();
      parentCtrl.Controls.Add(maximumBox);
    }


    public override void AddValidatorSettingsView(Control parentCtrl)
    {
      int minimum = 0;
      bool minExist = TryGetProperty("MinimumValue", ref minimum);

      int maximum = 0;
      bool maxExist = TryGetProperty("Maximum", ref maximum);

      if (minExist)
      {
        Portal.API.Controls.Label desc = new Portal.API.Controls.Label();
        desc.LanguageRef = "Minimum";
        parentCtrl.Controls.Add(desc);

        Label val = new Label();
        val.Text = minimum.ToString();
        parentCtrl.Controls.Add(val);

        if (maxExist)
          parentCtrl.Controls.Add(new LiteralControl("</br>"));
      }

      if (maxExist)
      {
        Portal.API.Controls.Label desc = new Portal.API.Controls.Label();
        desc.LanguageRef = "Maximum";
        parentCtrl.Controls.Add(desc);

        Label val = new Label();
        val.Text = maximum.ToString();
        parentCtrl.Controls.Add(val);
      }
    }


    public override void SaveValidatorSettings(Control parentCtrl)
    {
      this.Data.Properties.Clear();

      TextBox minBox = (TextBox)parentCtrl.FindControl("_minimum");
      if (minBox != null)
      {
        int minimum;
        if (int.TryParse(minBox.Text, out minimum))
        {
          this.Data.Properties["MinimumValue"] = minimum;
        }
      }

      TextBox maxBox = (TextBox)parentCtrl.FindControl("_maximum");
      if (maxBox != null)
      {
        int maximum;
        if (int.TryParse(maxBox.Text, out maximum))
        {
          this.Data.Properties["MaximumValue"] = maximum;
        }
      }
    }

    public override bool Validate(string value)
    {
      int valueNumber = 0;
      bool valid = base.Validate(value);
      value = value.Trim();

      if (valid && !string.IsNullOrEmpty(value))
      {
        valid = int.TryParse(value, out valueNumber);
          
        int limit = 0;

        // Minumum.
        if (TryGetProperty("MinimumValue", ref limit))
        {
          if (valueNumber < limit)
            valid = false;
        }

        // Maximum.
        if (TryGetProperty("MaximumValue", ref limit))
        {
          if (valueNumber > limit)
            valid = false;
        }
      }
      
      return valid;
    }

    #endregion
  }

}