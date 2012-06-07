<%@ Control Language="c#" autoeventwireup="true" Inherits="Portal.API.EditModule" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Data" %>
<script runat="server">
DataSet ds = null;

void Page_Load(Object Sender, EventArgs e)
{
	lnkAddRow.Visible = true;
	lnkAddDataRow.Visible = true;
	repAddRow.Visible = true;

	if(!IsPostBack)
	{
		// Cannot use Schema - its dynamic!!
		ds = ReadConfig();
	    if(ds == null)
		{
			ds = new DataSet();
			DataTable ht = new DataTable("header");
			ds.Tables.Add(ht);
			DataColumn colName = new DataColumn("name");
			ht.Columns.Add(colName);
			DataColumn colwidth = new DataColumn("width");
			ht.Columns.Add(colwidth);
			ht = new DataTable("data");
			ds.Tables.Add(ht);
		}

		BindGrid();
	}
	else
	{
		ds = (DataSet)ViewState["DataSet"];
	}
}

protected void OnAddRow(object sender, EventArgs args)
{
	if(sender == lnkAddRow)
	{
		DataTable td = ds.Tables["header"];
		DataRow dr = td.NewRow();
		dr["name"] = "Column Name";
		dr["width"] = "";
		td.Rows.Add(dr);
		SyncDataSet();
		gridDef.EditItemIndex = td.Rows.Count-1;
		lnkAddRow.Visible = false;
		lnkAddDataRow.Visible = false;
		repAddRow.Visible = false;
	}
	else
	{
		DataTable td = ds.Tables["data"];
		DataRow dr = td.NewRow();
		for(int i=0;i<td.Columns.Count;i++)
		{
      if (Portal.API.HtmlAnalyzer.HasScriptTags(((System.Web.UI.WebControls.TextBox)repAddRow.Items[i].FindControl("data")).Text))
			{
				msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
				return;
			}
      dr[i] = ((System.Web.UI.WebControls.TextBox)repAddRow.Items[i].FindControl("data")).Text;
		}
		td.Rows.Add(dr);
	}
	BindGrid();
}

protected void Grid_CartCommand(Object sender, DataGridCommandEventArgs e)
{
	if(e.CommandName == "Delete")
	{
		if(sender == gridDef)
		{
			ds.Tables["header"].Rows.RemoveAt(e.Item.ItemIndex);
			SyncDataSet();
		}
		else
		{
			ds.Tables["data"].Rows.RemoveAt(e.Item.ItemIndex);
		}
		BindGrid();
	}
}

protected void Grid_Edit(Object sender, DataGridCommandEventArgs e) 
{
	DataGrid grid = (DataGrid)sender;
	lnkAddRow.Visible = false;
	lnkAddDataRow.Visible = false;
	repAddRow.Visible = false;
	
	grid.EditItemIndex = e.Item.ItemIndex;
	BindGrid();
}

protected void Grid_Cancel(Object sender, DataGridCommandEventArgs e) 
{
	DataGrid grid = (DataGrid)sender;
	grid.EditItemIndex = -1;
	BindGrid();
}

protected void Grid_Update(Object sender, DataGridCommandEventArgs e) 
{
	DataGrid grid = (DataGrid)sender;
	if(grid == gridDef)
	{
		if(
      Portal.API.HtmlAnalyzer.HasScriptTags(((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Text) ||
      Portal.API.HtmlAnalyzer.HasScriptTags(((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Text)
			)
		{
			msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
			return;
		}
		int idx = grid.EditItemIndex;
		DataRow dr = ds.Tables["header"].Rows[idx];
    dr["name"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Text;
    dr["width"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Text;
		SyncDataSet();
	}
	else
	{
		int idx = grid.EditItemIndex;
		DataRow dr = ds.Tables["data"].Rows[idx];
		for(int i=0;i<ds.Tables["header"].Rows.Count;i++)
		{
      if (Portal.API.HtmlAnalyzer.HasScriptTags(((System.Web.UI.WebControls.TextBox)e.Item.Cells[i + 2].Controls[0]).Text))
			{
				msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
				return;
			}
      dr[i] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[i + 2].Controls[0]).Text;
		}
	}
	grid.EditItemIndex = -1;
	BindGrid();
}

private void SyncDataSet()
{
	int i=0;
	DataTable ht = ds.Tables["header"];
	DataTable dt = ds.Tables["data"];
	int colsToRemove = dt.Columns.Count - ht.Rows.Count;
	if(colsToRemove > 0)
	{
		while(dt.Columns.Count != ht.Rows.Count)
		{
			dt.Columns.RemoveAt(ht.Rows.Count);
		}
	}
	else
	{
		for(i=colsToRemove;i<0;i++)
		{
			dt.Columns.Add(new DataColumn());
		}
	}
	for(i=0;i<ht.Rows.Count;i++)
	{
		dt.Columns[i].ColumnName = (string)ht.Rows[i]["name"];
	}
}

protected void OnUpdate(object sender, EventArgs args)
{
	WriteConfig(ds);
	RedirectBack();
}
protected void OnCancel(object sender, EventArgs args)
{
	RedirectBack();
}

private void BindGrid()
{
	gridDef.DataMember = "header";
	gridDef.DataSource = ds;
	gridDef.DataBind();

	if(ds.Tables["data"].Columns.Count > 0)
	{
		gridData.DataMember = "data";
		gridData.DataSource = ds;        	
		gridData.DataBind();
	
		repAddRow.DataMember = "header";
		repAddRow.DataSource = ds;        	
		repAddRow.DataBind();
	}
	
	
	ViewState["DataSet"] = ds;

	System.IO.StringWriter sw = new System.IO.StringWriter();
	ds.WriteXml(sw);
}
</script>
<portal:Message id="msg" runat="server" />
<h2>
<portal:Label Runat="server" LanguageRef="TitleDefinition" />
</h2>
<portal:LinkButton id="lnkAddRow" runat="server" OnClick="OnAddRow" class="LinkButton" LanguageRef="Add"></portal:LinkButton>
<asp:datagrid id="gridDef" runat="server" AutogenerateColumns="false" OnEditCommand="Grid_Edit" OnCancelCommand="Grid_Cancel" OnUpdateCommand="Grid_Update" OnItemCommand="Grid_CartCommand">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:EditCommandColumn EditText="<img src=PortalImages/edit.gif alt=Edit>" CancelText="<img src=PortalImages/Cancel.gif alt=Cancel>" UpdateText="<img src=PortalImages/save.gif alt=Update>" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ButtonType="LinkButton" HeaderStyle-Width="16px" />
		<asp:ButtonColumn ButtonType="LinkButton" Text="<img src=PortalImages/Delete.gif alt=Delete>" CommandName="Delete" />
		<portal:BoundColumn DataField="name" LanguageRef-HeaderText="ColumnName" HeaderStyle-Width="200px" />
		<portal:BoundColumn DataField="width" LanguageRef-HeaderText="Width" HeaderStyle-Width="100px" />
	</Columns>
</asp:datagrid>
<h2>
	<portal:Label runat="Server" LanguageRef="TitleData" />
</h2>
<portal:LinkButton id="lnkAddDataRow" runat="server" OnClick="OnAddRow" cssclass="LinkButton" LanguageRef="Add"></portal:LinkButton>
<table cellpadding="0" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;">
	<tr>
		<asp:Repeater ID="repAddRow" Runat="server">
			<ItemTemplate>
				<td>
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td class="ListHeader"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
						</tr>
						<tr>
							<td><asp:TextBox Runat="server" ID="data"></asp:TextBox></td>
						</tr>
					</table>
				</td>
			</ItemTemplate>
		</asp:Repeater>
	</tr>
</table>
<asp:datagrid id="gridData" runat="server" AutogenerateColumns="true" OnEditCommand="Grid_Edit" OnCancelCommand="Grid_Cancel" OnUpdateCommand="Grid_Update" OnItemCommand="Grid_CartCommand">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:EditCommandColumn EditText="<img src=PortalImages/edit.gif alt=Edit alt=Edit>" CancelText="<img src=PortalImages/Cancel.gif alt=Cancel>" UpdateText="<img src=PortalImages/save.gif alt=Update>" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ButtonType="LinkButton" />
		<asp:ButtonColumn ButtonType="LinkButton" Text="<img src=PortalImages/Delete.gif alt=Delete>" CommandName="Delete" />
	</Columns>
</asp:datagrid>
<p>
	<hr/>
	<portal:LinkButton id="updateButton" LanguageRef="Update" runat="server" class="LinkButton" BorderStyle="none" OnClick="OnUpdate" />
	&nbsp;
	<portal:LinkButton id="cancelButton" LanguageRef="Cancel" CausesValidation="False" runat="server" class="LinkButton" BorderStyle="none" OnClick="OnCancel" />
</p>
