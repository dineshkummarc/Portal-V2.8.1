using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;


namespace Portal.Modules.FormBuilder
{
  /// <summary>
  /// Validiert eine Emailadresse.
  /// </summary>
  public class ValidatorRegex : Validator
  {
    public ValidatorRegex(ModuleConfig.ValidationData data)
      : base(data)
    {
    }

    public override void AddValidatorSettingsEdit(Control parentCtrl)
    {
      Portal.API.Controls.Label lbl = new Portal.API.Controls.Label();
      lbl.LanguageRef = "Expression";
      parentCtrl.Controls.Add(lbl);

      // Ausdruck.
      TextBox expressionBox = new TextBox();
      expressionBox.ID = "_expression";
      string expression = "";
      if (TryGetProperty("Regex", ref expression))
        expressionBox.Text = expression;
      parentCtrl.Controls.Add(expressionBox);

      HyperLink info = new HyperLink();
      info.ImageUrl = "~/PortalImages/help.gif";
      info.NavigateUrl = "http://msdn2.microsoft.com/en-us/library/hs600312.aspx";
      info.Target = "_blank";
      parentCtrl.Controls.Add(info);
    }



    public override void AddValidatorSettingsView(Control parentCtrl)
    {    
      Portal.API.Controls.Label lbl = new Portal.API.Controls.Label();
      lbl.LanguageRef = "Expression";
      parentCtrl.Controls.Add(lbl);

      string expression = "";
      if (TryGetProperty("Regex", ref expression))
      {
        Label val = new Label();
        val.Text = expression;
        parentCtrl.Controls.Add(val);
      }
    }


    public override void SaveValidatorSettings(Control parentCtrl)
    {
      this.Data.Properties.Clear();

      TextBox expressionBox = (TextBox)parentCtrl.FindControl("_expression");
      if (expressionBox != null)
      {
        this.Data.Properties["Regex"] = HttpContext.Current.Server.HtmlEncode(expressionBox.Text);
      }
    }


    public override bool Validate(string value)
    {
      bool valid = base.Validate(value);
      if(valid && !string.IsNullOrEmpty(value))
      {
        string expression = "";
        if (TryGetProperty("Regex", ref expression))
        {
          Regex regex = new Regex(expression,
                                    RegexOptions.IgnoreCase
                                    | RegexOptions.CultureInvariant
                                    | RegexOptions.IgnorePatternWhitespace
                                    | RegexOptions.Compiled
                                  );
          valid = regex.IsMatch(value);
        }
      }
      return valid;
    }
  }
}