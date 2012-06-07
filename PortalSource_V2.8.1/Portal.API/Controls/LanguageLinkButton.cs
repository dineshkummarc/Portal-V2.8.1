using System;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageLinkButton.
	/// </summary>
	[DefaultProperty("LanguageRef")]
    [ToolboxData(@"<{0}:LinkButton runat=""server""></{0}:LinkButton>")]
	public class LinkButton : System.Web.UI.WebControls.LinkButton
	{
    private string _languageRef = "";
    private string _tooltipLangRef = "";
    private string _confirmLangRef = "";

    private const string _addClickScript = "addClickFunction('{0}');";

    private const string _addClickFunctionScript =
        @"  function addClickFunction(id) {{
            var b = document.getElementById(id);
            if (b && typeof(b.click) == 'undefined') b.click = function() {{
                var result = true; if (b.onclick) result = b.onclick();
                if (typeof(result) == 'undefined' || result) {{ eval(b.href); }}
            }}}};";

    protected override void OnLoad(System.EventArgs e)
    {
      // Add a client Script to handle the DefaultButton Event of the panel Control. (Otherwies it will not work on some
      // browsers i.e. Firefox.)
      if (!Page.IsClientScriptBlockRegistered("addClickFunctionScript"))
      {
        Page.ClientScript.RegisterStartupScript(GetType(), "addClickFunctionScript",
          _addClickFunctionScript, true);

        string script = String.Format(_addClickScript, ClientID);
        Page.ClientScript.RegisterStartupScript(GetType(), "click_" + ClientID,
            script, true);
      }

      base.OnLoad(e);
    }

    [DefaultValue("")]
    [Description("The language reference id")]
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
      if (!string.IsNullOrEmpty(LanguageRef))
			  base.Text = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), LanguageRef);

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
