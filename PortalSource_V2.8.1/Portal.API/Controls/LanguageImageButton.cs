using System;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageImageButton.
	/// </summary>
	[DefaultProperty("LanguageRef")]
  [ToolboxData(@"<{0}:ImageButton runat=""server""></{0}:ImageButton>")]
  public class ImageButton : System.Web.UI.WebControls.ImageButton
	{
    private string _alternatelanguageRef = "";
    private string _tooltipLangRef = "";
    private string _confirmLangRef = "";

    [DefaultValue("")]
    [Description("The language reference id for the alternate Text")]
    public string AlternateTextLanguageRef
    {
        get { return _alternatelanguageRef; }
        set { _alternatelanguageRef = value; }
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
      if (!string.IsNullOrEmpty(AlternateTextLanguageRef))
        base.AlternateText = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), AlternateTextLanguageRef);

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
