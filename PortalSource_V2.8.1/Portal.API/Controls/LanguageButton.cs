using System;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageButton.
	/// </summary>
	[DefaultProperty("LanguageRef")] 
	[ToolboxData(@"<{0}:Button runat=""server""></{0}:Button>")]
	public class Button : System.Web.UI.WebControls.Button
	{
		private string _languageRef = "";
    private string _tooltipLangRef = "";
    private string _confirmLangRef = "";

    [DefaultValue("")]
    public string LanguageRef
    {
      get { return _languageRef; }
      set { _languageRef = value; }
    }

    [DefaultValue("")]
    [Description("The language reference for a tooltip")]
    public string TooltipLanguageRef
    {
      get { return _tooltipLangRef; }
      set { _tooltipLangRef = value; }
    }

    [DefaultValue("")]
    [Description("The language reference for a confirmation of the action")]
    public string ConfirmationLanguageRef
    {
      get { return _confirmLangRef; }
      set { _confirmLangRef = value; }
    }

		protected override void OnPreRender(EventArgs e)
		{
      base.Text = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), _languageRef);
      
      if (!string.IsNullOrEmpty(TooltipLanguageRef))
        base.ToolTip = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), TooltipLanguageRef);

      if (!string.IsNullOrEmpty(ConfirmationLanguageRef))
      {
        string confirmText = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), ConfirmationLanguageRef);
        base.Attributes.Add("OnClick", string.Format(CultureInfo.InvariantCulture, "return confirm('{0}');", confirmText));
      }

			base.OnPreRender (e);
		}

	}
}
