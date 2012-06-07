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
  public class ValidatorEmail : Validator
  {
    public ValidatorEmail(ModuleConfig.ValidationData data)
      : base(data)
    {
    }

    public override bool Validate(string value)
    {
      bool valid = base.Validate(value);
      if(valid && !string.IsNullOrEmpty(value))
      {
        Regex regex = new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1" +
                                ",3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$",
                                  RegexOptions.IgnoreCase
                                  | RegexOptions.CultureInvariant
                                  | RegexOptions.IgnorePatternWhitespace
                                  | RegexOptions.Compiled
                                );
        valid = regex.IsMatch(value);
      }
      return valid;
    }
  }
}