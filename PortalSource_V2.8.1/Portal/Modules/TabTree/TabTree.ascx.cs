using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;

namespace Portal.Modules.TabList
{
	/// <summary>
	///		Summary description for TabList.
	/// </summary>
	public partial  class TabTree : API.Module, IPostBackEventHandler
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabs">IList - can be a Array or ArrayList</param>
		/// <param name="parent"></param>
		private void InternalBuildTree(ArrayList tabs, iiuga.Web.UI.ElementsCollection parent)
		{
			foreach(PortalDefinition.Tab t in tabs)
			{
				int n = parent.Add(t.title);
				parent[n].Key = t.reference;
				parent[n].Text = "<a class=\"LinkButton\" href=" + 
					Page.GetPostBackClientHyperlink(this, t.reference) + 
					">" + t.title + "</a>";

				if(t.tabs != null && t.tabs.Count != 0)
				{
					InternalBuildTree(t.tabs, parent[n].Elements);
					parent[n].Expand();
				}
				else
				{
					parent[n].ImageIndex = 0;
				}
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			tree.Elements.Clear();

			PortalDefinition pd = PortalDefinition.Load();
			InternalBuildTree(PortalDefinition.CurrentTab.tabs, tree.Elements);
		}

		public void RaisePostBackEvent(string args)
		{
			Response.Redirect(Helper.GetTabLink(args));
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
