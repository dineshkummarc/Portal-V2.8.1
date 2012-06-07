using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.AdminPortal
{
	/// <summary>
	///		Summary description for TabList.
	/// </summary>
	public partial  class TabList : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected void OnEditTab(object sender, CommandEventArgs args)
		{
			((AdminPortal)Parent).SelectTab((string)args.CommandArgument);
		}
		protected void OnAddTab(object sender, EventArgs args)
		{
			((AdminPortal)Parent).AddTab();
		}
		protected void OnTabUp(object sender, CommandEventArgs args)
		{
			int idx = Int32.Parse((string)args.CommandArgument);
			((AdminPortal)Parent).MoveTabUp(idx);			
		}
		protected void OnTabDown(object sender, CommandEventArgs args)
		{
			int idx = Int32.Parse((string)args.CommandArgument);
			((AdminPortal)Parent).MoveTabDown(idx);			
		}

		public void LoadData(PortalDefinition pd)
		{
			LoadData(pd.tabs);
		}
		public void LoadData(PortalDefinition.Tab tab)
		{
			LoadData(tab.tabs);
		}

		private void LoadData(ArrayList subTabList)
		{
			ArrayList tabList = new ArrayList();
			foreach(PortalDefinition.Tab t in subTabList)
			{
				DisplayTabItem dt = new DisplayTabItem(t, false);
				tabList.Add(dt);
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
