using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for LanguageLinkButton.
	/// </summary>
	[ToolboxData(@"<{0}:DropDownList runat=""server""></{0}:DropDownList>")]
	public class DropDownList : System.Web.UI.WebControls.DropDownList
	{
		protected override void OnPreRender(EventArgs e)
		{
			foreach(ListItem i in this.Items)
			{
				if(!string.IsNullOrEmpty(i.Text))
				{					
					i.Text = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), i.Attributes["LanguageRef"]);
				}
			}
			base.OnPreRender (e);
		}

	}
}
