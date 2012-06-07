//===========================================================================
// Diese Datei wurde als Teil einer ASP.NET 2.0-Webprojektkonvertierung geändert.
// Der Klassenname wurde geändert, und die Klasse wurde so geändert, dass sie von der abstrakten 
//-Basisklasse in Datei "App_Code\Migrated\modules\adminportal\Stub_adminportal_ascx_cs.cs" erbt.
// Andere Klassen in der Webanwendung können während der Laufzeit die Code-Behind-Seite
//" mithilfe der abstrakten Basisklasse binden und darauf zugreifen.
// Die zugehörige Inhaltsseite "modules\adminportal\adminportal.ascx" wurde ebenfalls geändert und verweist auf den neuen Klassennamen.
// Weitere Informationen zu diesem Codemuster erhalten Sie unter http://go.microsoft.com/fwlink/?LinkId=46995. 
//===========================================================================
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using iiuga.Web.UI;
using Portal.API;


namespace Portal.Modules.AdminPortal
{
	/// <summary>
	///		Summary description for AdminPortal.
	/// </summary>
	public partial  class AdminPortalControl : AdminPortal
	{
		private string CurrentReference = "";
		private string CurrentParentReference = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				TabCtrl.Visible = false;
				BuildTree();
				SelectTab("");
			}
		}
		protected void OnSave(object sender, PortalDefinition.Tab t)
		{
			BuildTree();
		}
		protected void OnCancel(object sender, PortalDefinition.Tab t)
		{
		}
		protected void OnDelete(object sender, PortalDefinition.Tab t)
		{
			BuildTree();
			SelectTab(CurrentParentReference);
		}

		protected override void LoadViewState(object bag)
		{
			base.LoadViewState(bag);
			CurrentReference = (string)ViewState["CurrentReference"];
			CurrentParentReference = (string)ViewState["CurrentParentReference"];
		}
		protected override object SaveViewState()
		{
			ViewState["CurrentReference"] = CurrentReference;
			ViewState["CurrentParentReference"] = CurrentParentReference;
			return base.SaveViewState();
		}

//		internal void BuildTree()
		override public   void BuildTree()
		{
			PortalDefinition pd = PortalDefinition.Load();
			tree.Elements[0].Elements.Clear();

			InternalBuildTree(pd.tabs, tree.Elements[0]);
			
			tree.Elements[0].Expand();
			tree.Elements[0].Text = "<a class=\"LinkButton\" href=" + 
				Page.GetPostBackClientHyperlink(this, "") + 
				">Portal</a>";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabs">IList - can be a Array or ArrayList</param>
		/// <param name="parent"></param>
		private void InternalBuildTree(ArrayList tabs, TreeElement parent)
		{
			foreach(PortalDefinition.Tab t in tabs)
			{
				int n = parent.Elements.Add(t.title);
				parent.Elements[n].Key = t.reference;
				parent.Elements[n].Text = "<a class=\"LinkButton\" href=" + 
					Page.GetPostBackClientHyperlink(this, t.reference) + 
					">" + t.title + "</a>";

				if(t.tabs != null && t.tabs.Count != 0)
				{
					InternalBuildTree(t.tabs, parent.Elements[n]);
					parent.Elements[n].Expand();
				}
				else
				{
					parent.Elements[n].ImageIndex = 0;
				}
			}
		}

//		public void SelectTab(string reference)
		override public void SelectTab(string reference)
		{
			PortalDefinition pd = PortalDefinition.Load();
			CurrentReference = reference;
			if(reference == "") // Root Node
			{
				CurrentParentReference = "";
				TabCtrl.Visible = false;
				TabListCtrl.LoadData(pd);
			}
			else
			{
				PortalDefinition.Tab t = pd.GetTab(reference);
				CurrentParentReference = t.parent != null ? t.parent.reference : "";
				TabListCtrl.LoadData(t);
				TabCtrl.Visible = true;
				TabCtrl.LoadData(t);
			}
		}

//		public void AddTab()
		override public void AddTab()
		{
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = PortalDefinition.Tab.Create();

			if(CurrentReference == "") // Root Node
			{
				pd.tabs.Add(t);
			}
			else
			{
				PortalDefinition.Tab pt = pd.GetTab(CurrentReference);
				pt.tabs.Add(t);
			}

			pd.Save();

			BuildTree();
			SelectTab(t.reference);
		}

//		internal void MoveTabUp(int index)
		override public   void MoveTabUp(int index)
		{
			if(index <= 0) return;

			PortalDefinition pd = PortalDefinition.Load();
			ArrayList a = null;
			if(CurrentReference == "")
			{
				// Root
				a = pd.tabs;
			}
			else
			{
				PortalDefinition.Tab pt = pd.GetTab(CurrentReference);
				a = pt.tabs;
			}

			PortalDefinition.Tab t = (PortalDefinition.Tab)a[index];
			a.RemoveAt(index);
			a.Insert(index - 1, t);

			pd.Save();

			// Rebind
			BuildTree();
			SelectTab(CurrentReference);
		}

//		internal void MoveTabDown(int index)
		override public   void MoveTabDown(int index)
		{
			PortalDefinition pd = PortalDefinition.Load();
			ArrayList a = null;
			if(CurrentReference == "")
			{
				// Root
				a = pd.tabs;
			}
			else
			{
				PortalDefinition.Tab pt = pd.GetTab(CurrentReference);
				a = pt.tabs;
			}

			if(index >= a.Count-1) return;

			PortalDefinition.Tab t = (PortalDefinition.Tab)a[index];
			a.RemoveAt(index);
			a.Insert(index + 1, t);

			pd.Save();

			// Rebind
			BuildTree();
			SelectTab(CurrentReference);
		}

//		public void RaisePostBackEvent(string eventArgument)
		override public void RaisePostBackEvent(string eventArgument)
		{
			SelectTab(eventArgument);
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
