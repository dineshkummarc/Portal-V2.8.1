using System;
using System.Web.UI;
using System.ComponentModel;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageLinkButton.
	/// </summary>
	[DefaultProperty("LanguageRef")] 
	[ToolboxData(@"<{0}:TemplateColumn runat=""server""></{0}:TemplateColumn>")]
	public class TemplateColumn : System.Web.UI.WebControls.TemplateColumn
	{
        private ColumnLanguageReferences langRef = new ColumnLanguageReferences();

        public ColumnLanguageReferences LanguageRef
        {
            get { return langRef; }
            set { langRef = value; }
        }

		public override void Initialize()
		{
			base.Initialize ();

			if(LanguageRef.HeaderText != null)
			{
				base.HeaderText = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this.Owner), LanguageRef.HeaderText);
			}

			if(LanguageRef.FooterText != null)
			{
				base.FooterText = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this.Owner), LanguageRef.FooterText);
			}
		}
	}
}
