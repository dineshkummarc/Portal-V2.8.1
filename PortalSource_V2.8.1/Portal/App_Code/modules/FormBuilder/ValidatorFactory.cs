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
  /// Einfache Factory für die Validatoren.
  /// </summary>
  public class ValidatorFactory
  {
    public ValidatorFactory()
    {
      
    }

    /// <summary>
    /// Erzeugt einen Validator vom entsprechenden Typ.
    /// </summary>
    /// <param name="validatorType"></param>
    /// <returns></returns>
    public static IValidator CreateValidator(ModuleConfig.ValidationData validator)
    {
      switch (validator.FieldType)
      {
      	case ModuleConfig.ValidationData.ValidationType.Text:
          return new ValidatorText(validator);
          break;
      	case ModuleConfig.ValidationData.ValidationType.Email:
          return new ValidatorEmail(validator);
      		break;
        case ModuleConfig.ValidationData.ValidationType.Number:
          return new ValidatorNumber(validator);
          break;
        case ModuleConfig.ValidationData.ValidationType.RegularExpression:
          return new ValidatorRegex(validator);
          break;
      	default:
          return null;
      		break;
      }
    }
  }
}