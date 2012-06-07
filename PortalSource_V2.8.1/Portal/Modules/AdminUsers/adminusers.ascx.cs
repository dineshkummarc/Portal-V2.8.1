//===========================================================================
// Diese Datei wurde als Teil einer ASP.NET 2.0-Webprojektkonvertierung geändert.
// Der Klassenname wurde geändert, und die Klasse wurde so geändert, dass sie von der abstrakten 
//-Basisklasse in Datei "App_Code\Migrated\modules\adminusers\Stub_adminusers_ascx_cs.cs" erbt.
// Andere Klassen in der Webanwendung können während der Laufzeit die Code-Behind-Seite
//" mithilfe der abstrakten Basisklasse binden und darauf zugreifen.
// Die zugehörige Inhaltsseite "modules\adminusers\adminusers.ascx" wurde ebenfalls geändert und verweist auf den neuen Klassennamen.
// Weitere Informationen zu diesem Codemuster erhalten Sie unter http://go.microsoft.com/fwlink/?LinkId=46995. 
//===========================================================================
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.AdminUsers
{

	/// <summary>
	///		Summary description for AdminUsers.
	/// </summary>
	public partial  class AdminUsersControl : AdminUsers
	{

		protected void Page_Load(object Sender, EventArgs e)
		{
		}

//		internal void EditUser(string account)
		override public   void EditUser(string account)
		{
			ctrlUserList.Visible = false;
			ctrlUserEdit.Visible = true;
			ctrlUserEdit.EditUser(account);
		}

//		internal void ShowUserList()
		override public   void ShowUserList()
		{
			ctrlUserList.Visible = true;
			ctrlUserEdit.Visible = false;
			ctrlUserList.BindGrid();
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
