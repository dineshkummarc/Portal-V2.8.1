using System;
using System.Web.UI;
using System.ComponentModel;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageLinkButton.
	/// </summary>
	[DefaultProperty("LanguageRef")] 
	[ToolboxData(@"<{0}:CustomValidator runat=""server""></{0}:CustomValidator>")]
	public class CustomValidator : System.Web.UI.WebControls.CustomValidator
	{
		private string languageRef = "";
		private string languageRefText = "";

        [DefaultValue("")]
        public string LanguageRef
        {
            get { return languageRef; }
            set { languageRef = value; }
        }

        [DefaultValue("")]
        public string LanguageRefText
        {
            get { return languageRefText; }
            set { languageRefText = value; }
        }

		protected override void OnPreRender(EventArgs e)
		{
			if(!string.IsNullOrEmpty(LanguageRefText))
			{
				base.Text = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), LanguageRefText);
			}
			if(!string.IsNullOrEmpty(LanguageRef))
			{
				base.ErrorMessage = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), LanguageRef);
			}
			base.OnPreRender (e);
		}

	}
}
