using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using Portal.API;

namespace Portal
{
	/// <summary>
	/// UserControl for the EditLink. Derived form HtmlAnchor. 
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:EditLink runat=server></{0}:EditLink>")]
	public class EditLink : HtmlAnchor
	{
		/// <summary>
		/// The Modules reference
		/// </summary>
		public PortalDefinition.Module Module = null;

		protected override void OnLoad(EventArgs args)
		{
			base.OnLoad(args);
			base.InnerText = Language.GetText("EditLink_Text");
      HRef = Helper.GetEditLink(Module);
		}
	}
}
