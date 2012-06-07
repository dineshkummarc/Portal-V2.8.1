namespace Portal.Modules.Newsticker
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for EditNewsticker.
	/// </summary>
	public partial class EditNewsticker : Portal.API.EditModule
	{
		private DataSet m_Data = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lbLinks.Visible = true;
	
			if(!IsPostBack)
			{
				m_Data = ReadConfig();
				BindGrid();
			}
			else
			{
				m_Data = (DataSet)ViewState["DataSet"];
			}
		}

		private void BindGrid()
		{
			grid.DataSource = m_Data;
			grid.DataBind();
	
			ViewState["DataSet"] = m_Data;
		}

		#region Vom Web Form-Designer generierter Code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist für den ASP.NET Web Form-Designer erforderlich.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Erforderliche Methode für die Designerunterstützung
		///		Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.grid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grid_ItemCommand);
			this.grid.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grid_CancelCommand);
			this.grid.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grid_EditCommand);
			this.grid.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grid_UpdateCommand);

		}
		#endregion

		protected void OnCancel(object sender, System.EventArgs e)
		{
			RedirectBack();
		}

		protected void OnUpdate(object sender, System.EventArgs e)
		{
			WriteConfig(m_Data);
			RedirectBack();
		}

		protected void OnAdd(object sender, System.EventArgs e)
		{
			lbLinks.Visible = false;
			DataTable td = m_Data.Tables["news"];
			DataRow dr = td.NewRow();
			dr["Url"] = "http://";
			dr["Name"] = "";
			dr["MaxCount"] = "20";
			td.Rows.Add(dr);

			grid.EditItemIndex = td.Rows.Count-1;
			BindGrid();
		}

		private void grid_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			lbLinks.Visible = false;
			grid.EditItemIndex = e.Item.ItemIndex;
			BindGrid();
		}

		private void grid_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			grid.EditItemIndex = -1;
			BindGrid();
		}

		private void grid_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			int idx = grid.EditItemIndex;
			DataRow dr = m_Data.Tables["news"].Rows[idx];
			dr["Url"] = ((TextBox)e.Item.Cells[2].Controls[0]).Text;
			dr["Name"] = ((TextBox)e.Item.Cells[3].Controls[0]).Text;
			dr["MaxCount"] = Int32.Parse(((TextBox)e.Item.Cells[4].Controls[0]).Text);

			grid.EditItemIndex = -1;
			BindGrid();
		}

		private void grid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "Delete")
			{
				m_Data.Tables["news"].Rows.RemoveAt(e.Item.ItemIndex);
				BindGrid();
			}
		}
	}
}
