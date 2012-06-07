using System;
using System.Web.UI;
using System.ComponentModel;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageLinkButton.
	/// </summary>
	[DefaultProperty("LanguageRef")] 
	[ToolboxData(@"<{0}:Label runat=""server""></{0}:Label>")]
	public class Label : System.Web.UI.WebControls.Label
	{
		private string languageRef = "";

        [DefaultValue("")]
        public string LanguageRef
        {
            get { return languageRef; }
            set { languageRef = value; }
        }

		protected override void OnPreRender(EventArgs e)
		{
			base.Text = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), LanguageRef);
			base.OnPreRender (e);
		}

	}
}
