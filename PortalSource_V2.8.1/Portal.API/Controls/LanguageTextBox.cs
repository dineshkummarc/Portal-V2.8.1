using System;
using System.Web.UI;
using System.ComponentModel;

namespace Portal.API.Controls
{
  /// <summary>
  /// Summary description for LanguageLinkButton.
  /// </summary>
  [DefaultProperty("LanguageRef")]
  [ToolboxData(@"<{0}:TextBox runat=""server""></{0}:TextBox>")]
  public class TextBox : System.Web.UI.WebControls.TextBox
  {
    private string _tooltipLangRef = "";

    [DefaultValue("")]
    [Description("The language reference for a tooltip")]
    public string TooltipLanguageRef
    {
      get { return _tooltipLangRef; }
      set { _tooltipLangRef = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
      if (string.IsNullOrEmpty(TooltipLanguageRef))
        base.ToolTip = "";
      else
        base.ToolTip = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), TooltipLanguageRef);
      base.OnPreRender(e);
    }

  }
}
