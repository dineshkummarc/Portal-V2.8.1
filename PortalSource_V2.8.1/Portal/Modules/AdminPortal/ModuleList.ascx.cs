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
	///		Summary description for ModuleList.
	/// </summary>
    public partial class ModuleListControl : ModuleList
	{
		private ArrayList moduleList = null;

        /// <summary>
        /// Wrapper Class for the Tab Object.
        /// </summary>
        public class DisplayModuleItemControl : DisplayModuleItem
        {
            /// <summary>
            /// Tabs Text
            /// </summary>
            public override string Title
            {
                get { return m_Title; }
            }
            /// <summary>
            /// Tabs Reference
            /// </summary>
            public override string Reference
            {
                get { return m_Reference; }
            }
            /// <summary>
            /// Modules Type
            /// </summary>
            public override string ModuleType
            {
                get { return m_ModuleType; }
            }

            internal string m_Title = "";
            internal string m_Reference = "";
            internal string m_ModuleType = "";
        }
        
        protected void Page_Load(object sender, System.EventArgs e)
		{
			divTitle.InnerText = Portal.API.Language.GetText(
				Portal.API.Module.GetModuleControl(this), TitleLanguageRef);
		}

		protected override void LoadViewState(object bag)
		{
			base.LoadViewState(bag);
			moduleList = (ArrayList)ViewState["moduleList"];
		}
		protected override object SaveViewState()
		{
			ViewState["moduleList"] = moduleList;
			return base.SaveViewState();
		}

		public override void LoadData(ArrayList modules)
		{
			// Init Data
			moduleList = modules;
			Bind();
		}

		private void Bind()
		{
			ArrayList bindList = new ArrayList();
			foreach(PortalDefinition.Module m in moduleList)
			{
                DisplayModuleItemControl dt = new DisplayModuleItemControl();
				bindList.Add(dt);

				dt.m_Title = m.title;
				dt.m_Reference = m.reference;
				dt.m_ModuleType = m.type;
			}
			gridModules.DataSource = bindList;
			gridModules.DataBind();
		}

		protected void OnEditModule(object sender, CommandEventArgs args)
		{
			((Tab)Parent).EditModule((string)args.CommandArgument);
		}

		protected void OnAddModule(object sender, EventArgs args)
		{
			((Tab)Parent).AddModule(this);
		}

		protected void OnModuleUp(object sender, CommandEventArgs args)
		{
			int idx = Int32.Parse((string)args.CommandArgument);
			((Tab)Parent).MoveModuleUp(idx, this);			
		}
		protected void OnModuleDown(object sender, CommandEventArgs args)
		{
			int idx = Int32.Parse((string)args.CommandArgument);
			((Tab)Parent).MoveModuleDown(idx, this);			
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
