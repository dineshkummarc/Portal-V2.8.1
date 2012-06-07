namespace Portal
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for TabPath.
	/// </summary>
	public partial  class TabPath : System.Web.UI.UserControl
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
			/// <summary>
			/// True if the menu item represents the current Tab
			/// </summary>
			public bool IsCurrentTab
			{
				get { return m_CurrentTab; }
			}

			internal string m_Text = "";
			internal string m_URL = "";
			internal bool m_CurrentTab = false;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Load Protal Definition and the current Tab
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab currentTab = pd.GetTab(Request["TabRef"]);
			if(currentTab == null) return;

			// Great, we are a top level Tab
			if(currentTab.parent == null) return;

			ArrayList tabList = new ArrayList();
			while(currentTab != null)
			{
				DisplayTabItem dt = new DisplayTabItem();
				tabList.Insert(0, dt);

				dt.m_Text = currentTab.title;
				dt.m_URL = Portal.API.Config.GetTabUrl(currentTab.reference);

				// one up...
				currentTab = currentTab.parent;
			}
			
			// Bind Repeater
			tabpath.DataSource = tabList;
			tabpath.DataBind();		
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
