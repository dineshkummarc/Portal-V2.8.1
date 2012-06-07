using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;

namespace Portal.Modules.TabList
{
	/// <summary>
	///		Summary description for TabList.
	/// </summary>
	public partial  class TabList : API.Module
	{

		/// <summary>
		/// Wrapper Class for the Tab Object.
		/// </summary>
		public class DisplayTabItem
		{
			/// <summary>
			/// Menus Text
			/// </summary>
			public string Text
			{
				get { return m_Text; }
			}
			/// <summary>
			/// Tabs URL.
			/// </summary>
			public string URL
			{
				get { return m_URL; }
			}

			internal string m_Text = "";
			internal string m_URL = "";
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			PortalDefinition.Tab currentTab = PortalDefinition.CurrentTab;
			if(currentTab == null || currentTab.tabs == null) return;

			ArrayList tabList = new ArrayList();
			foreach(PortalDefinition.Tab t in currentTab.tabs)
			{
				if(UserManagement.HasViewRights(Page.User, t.roles))
				{
					DisplayTabItem dt = new DisplayTabItem();
					tabList.Add(dt);

					dt.m_Text = t.title;
					dt.m_URL = "../../" + Portal.API.Config.GetTabUrl(t.reference);
				}
			}
			Tabs.DataSource = tabList;
			Tabs.DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
