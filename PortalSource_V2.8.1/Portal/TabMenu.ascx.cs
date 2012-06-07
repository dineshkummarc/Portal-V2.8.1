using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;

namespace Portal
{
	/// <summary>
	///	Renders the Headers Tab Menu. 
	/// </summary>
	public partial  class TabMenu : System.Web.UI.UserControl
	{
		private DisplayTabItem BuildDisplayTabItem(PortalDefinition.Tab t)
		{
			if(UserManagement.HasViewRights(Page.User, t.roles))
			{
				// User may view the tab, create a Display Item
        DisplayTabItem dt = new DisplayTabItem(t, false);

				return dt;
			}

			return null;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Load Protal Definition and the current Tab
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab currentTab = pd.GetTab(Request["TabRef"]);

			// Foreach Tab...
			ArrayList tabList = new ArrayList();
			ArrayList subTabList = new ArrayList();
			foreach(PortalDefinition.Tab t in pd.tabs)
			{
				DisplayTabItem dt = BuildDisplayTabItem(t);
				if(dt != null)
				{
					// Set current Tab Property
					if(currentTab == null)
					{
						if(tabList.Count == 1)
						{
							// First tab -> default
							dt.CurrentTab = true;
						}
					}
					else
					{
						dt.CurrentTab = currentTab.RootTab == t;
					}
					tabList.Add(dt);

					if(dt.CurrentTab && Config.TabMenuShowSubTabs)
					{
						foreach(PortalDefinition.Tab st in t.tabs)
						{
							DisplayTabItem sdt = BuildDisplayTabItem(st);
							if(sdt != null)
							{
								subTabList.Add(sdt);
							}
						}
					}
				}
			} // foreach(tab)

			// Bind Repeater
			Tabs.DataSource = tabList;
			Tabs.DataBind();

			if(subTabList.Count > 0)
			{
				TabMenu_SubTab.Visible = true;
				SubTabs.DataSource = subTabList;
				SubTabs.DataBind();
			}
			else
			{
				TabMenu_SubTab.Visible = false;
			}
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
