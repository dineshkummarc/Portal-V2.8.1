using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;

namespace Portal.Modules.AdminUsers
{
	/// <summary>
	///		Summary description for UserEdit.
	/// </summary>
	public partial  class UserEdit : System.Web.UI.UserControl
	{

		private Users.UserRow user = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		protected void OnBack(object sender, System.EventArgs args)
		{
			((AdminUsers)Parent).ShowUserList();
		}
		protected void OnSave(object sender, System.EventArgs args)
		{
			ArrayList roles = new ArrayList();
			Users.RoleDataTable rolesTbl = UserManagement.Users.Role;
			foreach(DataGridItem gi in gridRoles.Items)
			{
				CheckBox chkBox = (CheckBox)gi.Cells[0].Controls[1];
				if(chkBox.Checked)
				{
					roles.Add(rolesTbl[gi.DataSetIndex].name);
				}
			}

			UserManagement.SaveUser(txtLogin.Text, txtPassword.Text, 
				HttpUtility.HtmlEncode(txtFirstName.Text), 
				HttpUtility.HtmlEncode(txtSurName.Text),
				HttpUtility.HtmlEncode(txtEMail.Text), 
				roles, 
				new Guid(txtUserId.Text));
			((AdminUsers)Parent).ShowUserList();
		}
		protected void OnDelete(object sender, System.EventArgs args)
		{
			UserManagement.DeleteUser(txtLogin.Text);
			((AdminUsers)Parent).ShowUserList();
		}

		protected bool HasRole(DataRowView item)
		{
			if(user == null) return false;

			Users.RoleRow role = (Users.RoleRow)item.Row;
			Users.UserRoleRow[] roles = user.GetUserRoleRows();
			foreach(Users.UserRoleRow r in roles)
			{
				if(r.name == role.name)
				{
					return true;
				}
			}

			return false;
		}

		public void EditUser(string account)
		{
			if(account != "")
			{
				user = UserManagement.Users.User.FindBylogin(account);
				txtLogin.Text = user.login;
				txtPassword.Text = user.password;
				txtFirstName.Text = HttpUtility.HtmlDecode(user.IsfirstNameNull()?"":user.firstName);
				txtSurName.Text = HttpUtility.HtmlDecode(user.IssurNameNull()?"":user.surName);
				txtUserId.Text = user.id.ToString();
				txtEMail.Text = user.IsemailNull()?"":user.email;
				txtLogin.Enabled = false;
			}
			else
			{
				txtLogin.Text = "";
				txtPassword.Text = "";
				txtFirstName.Text = "";
				txtSurName.Text = "";
				txtEMail.Text = "";
				txtUserId.Text = Guid.NewGuid().ToString();
				txtLogin.Enabled = true;
			}

			gridRoles.DataSource = UserManagement.Users.Role;
			gridRoles.DataBind();
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
