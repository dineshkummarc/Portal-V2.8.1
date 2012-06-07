//===========================================================================
// Diese Datei wurde als Teil einer ASP.NET 2.0-Webprojektkonvertierung geändert.
// Der Klassenname wurde geändert, und die Klasse wurde so geändert, dass sie von der abstrakten 
//-Basisklasse in Datei "App_Code\Migrated\modules\adminportal\Stub_tab_ascx_cs.cs" erbt.
// Andere Klassen in der Webanwendung können während der Laufzeit die Code-Behind-Seite
//" mithilfe der abstrakten Basisklasse binden und darauf zugreifen.
// Die zugehörige Inhaltsseite "modules\adminportal\tab.ascx" wurde ebenfalls geändert und verweist auf den neuen Klassennamen.
// Weitere Informationen zu diesem Codemuster erhalten Sie unter http://go.microsoft.com/fwlink/?LinkId=46995. 
//===========================================================================
namespace Portal.Modules.AdminPortal
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Tab.
	/// </summary>
	public partial  class TabControl : Tab
	{

    private PortalDefinition.Tab CurrentTab = null;

		public delegate void TabEventHandler(object sender, PortalDefinition.Tab tab);

		public event TabEventHandler Save = null;
		public event TabEventHandler Cancel = null;
		public event TabEventHandler Delete = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected override void LoadViewState(object bag)
		{
			base.LoadViewState(bag);
			CurrentTab = Helper.GetEditTab((string)ViewState["CurrentTab"]);
		}
		protected override object SaveViewState()
		{
      string TabReference = "";
      if (CurrentTab != null)
        TabReference = CurrentTab.reference;
      ViewState["CurrentTab"] = TabReference;
			return base.SaveViewState();
		}

		private void ShowEditModules()
		{
			ModuleEditCtrl.Visible = true;
			ModuleListCtrl_Left.Visible = false;
			ModuleListCtrl_Middle.Visible = false;
			ModuleListCtrl_Right.Visible = false;
		}
		private void ShowModulesList()
		{
			ModuleEditCtrl.Visible = false;
			ModuleListCtrl_Left.Visible = true;
			ModuleListCtrl_Middle.Visible = true;
			ModuleListCtrl_Right.Visible = true;
		}

		override public void LoadData(PortalDefinition.Tab t)
		{
			CurrentTab = t;

			txtTitle.Text = HttpUtility.HtmlDecode(t.title);
			txtReference.Text = CurrentTab.reference;
      txtImagePathI.Text = HttpUtility.HtmlDecode(t.imgPathInactive);
      txtImagePathA.Text = HttpUtility.HtmlDecode(t.imgPathActive);

			RolesCtrl.LoadData(t.roles);
			ModuleListCtrl_Left.LoadData(t.left);
			ModuleListCtrl_Middle.LoadData(t.middle);
			ModuleListCtrl_Right.LoadData(t.right);
		}

		override public   void EditModule(string reference)
		{
			ShowEditModules();
			ModuleEditCtrl.LoadData(CurrentTab.reference, reference);
		}

		override public   void AddModule(ModuleList list)
		{
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = CurrentTab;

			PortalDefinition.Module m = PortalDefinition.Module.Create();

			if(list == ModuleListCtrl_Left)
			{
				t.left.Add(m);
			}
			else if(list == ModuleListCtrl_Middle)
			{
				t.middle.Add(m);
			} 
			else if(list == ModuleListCtrl_Right)
			{
				t.right.Add(m);
			}

			pd.Save();

			// Rebind
			LoadData(CurrentTab);

			EditModule(m.reference);
		}

		protected void OnCancelEditModule(object sender, EventArgs args)
		{
			ShowModulesList();
		}

		protected void OnSaveModule(object sender, EventArgs args)
		{
			// Rebind
			LoadData(CurrentTab);
			ShowModulesList();
		}

		protected void OnDeleteModule(object sender, EventArgs args)
		{
			// Rebind
			LoadData(CurrentTab);
			ShowModulesList();
		}

//		internal void MoveModuleUp(int idx, ModuleList list)
		override public   void MoveModuleUp(int idx, ModuleList list)
		{
			if(idx <= 0) return;

			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = CurrentTab;

			ArrayList a = null;

			if(list == ModuleListCtrl_Left)
			{
				a = t.left;
			}
			else if(list == ModuleListCtrl_Middle)
			{
				a = t.middle;
			} 
			else if(list == ModuleListCtrl_Right)
			{
				a = t.right;
			}

			PortalDefinition.Module m = (PortalDefinition.Module)a[idx];
			a.RemoveAt(idx);
			a.Insert(idx - 1, m);

			pd.Save();

			// Rebind
			LoadData(CurrentTab);
		}

//		internal void MoveModuleDown(int idx, ModuleList list)
		override public   void MoveModuleDown(int idx, ModuleList list)
		{
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = CurrentTab;

			ArrayList a = null;

			if(list == ModuleListCtrl_Left)
			{
				a = t.left;
			}
			else if(list == ModuleListCtrl_Middle)
			{
				a = t.middle;
			} 
			else if(list == ModuleListCtrl_Right)
			{
				a = t.right;
			}

			if(idx >= a.Count-1) return;

			PortalDefinition.Module m = (PortalDefinition.Module)a[idx];
			a.RemoveAt(idx);
			a.Insert(idx + 1, m);

			pd.Save();

			// Rebind
			LoadData(CurrentTab);
		}

		protected void OnCancel(object sender, EventArgs args)
		{
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = CurrentTab;

			if(Cancel != null)
			{
				Cancel(this, t);
			}

			LoadData(CurrentTab);
			ShowModulesList();
		}


		protected void OnSave(object sender, EventArgs args)
		{
			try
			{
				if(!Page.IsValid) return;

				PortalDefinition pd = PortalDefinition.Load();
				PortalDefinition.Tab t = CurrentTab;

				t.title = HttpUtility.HtmlEncode(txtTitle.Text);
				t.reference = txtReference.Text;
        t.imgPathInactive = HttpUtility.HtmlEncode(txtImagePathI.Text);
        t.imgPathActive = HttpUtility.HtmlEncode(txtImagePathA.Text);
				t.roles = RolesCtrl.GetData();

				pd.Save();

				CurrentTab = t;

				if(Save != null)
				{
					Save(this, t);
				}

				ShowModulesList();
			}
			catch(Exception e)
			{
				lbError.Text = e.Message;
			}
		}

		protected void OnDelete(object sender, EventArgs args)
		{
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = CurrentTab;

			PortalDefinition.DeleteTab(CurrentTab.reference);

			if(Delete != null)
			{
				Delete(this, t);
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
