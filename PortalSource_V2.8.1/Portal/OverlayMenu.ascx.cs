namespace Portal
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for OverlayMenu.
	/// </summary>
	[ControlBuilder(typeof(OverlayMenuItemCtrlBuilder)), ParseChildren(false)]
	public partial  class OverlayMenu : System.Web.UI.UserControl, IPostBackEventHandler
	{
		public string RootText = "";

		private string languageRef = "";
		public string LanguageRef
		{
			get { return languageRef; }
			set { languageRef = value; }
		}

		ArrayList miList = new ArrayList();
	
		public ArrayList Items
		{
			get { return miList; }
		}


		protected override void AddParsedSubObject(object obj)
		{
			OverlayMenuItem m = obj as OverlayMenuItem;
			if(m != null)
			{
				m.MenuItemIndex = miList.Add(m);
			}
			else
			{
				base.AddParsedSubObject(obj);
			}
		}

		public void RaisePostBackEvent(string args)
		{
			int i = Int32.Parse(args);
			OverlayMenuItem mi = (OverlayMenuItem)miList[i];
			mi.InvokeClick();
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(languageRef != "")
				RootText = Portal.API.Language.GetText(languageRef);

			MenuRepeater.DataSource = miList;
			MenuRepeater.DataBind();

			Page.ClientScript.RegisterStartupScript(GetType(), "OverlayMenu", "<script language=\"javascript\" src=\"OverlayMenu.js\"></script>");
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
