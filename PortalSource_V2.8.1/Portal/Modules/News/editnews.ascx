<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.EditModule" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Data" %>
<script runat="server">
DataSet ds = null;

void Page_Load(Object Sender, EventArgs e)
{
	lbLinks.Visible = true;
	
	if(!IsPostBack)
	{
		ds = ReadConfig();
		BindGrid();
	}
	else
	{
		ds = (DataSet)ViewState["DataSet"];
	}
}

protected void OnAdd(object sender, EventArgs args)
{
	lbLinks.Visible = false;
	DataTable td = ds.Tables["news"];
	DataRow dr = td.NewRow();
	dr["Created"] = DateTime.Today;
	dr["Text"] = "";
	dr["Expires"] = DateTime.Today.AddYears(1);
	td.Rows.Add(dr);

	grid.EditItemIndex = td.Rows.Count-1;
	BindGrid();
}

protected void Grid_CartCommand(Object sender, DataGridCommandEventArgs e)
{
	if(e.CommandName == "Delete")
	{
		ds.Tables["news"].Rows.RemoveAt(e.Item.ItemIndex);
		BindGrid();
	}
}

protected void Grid_Edit(Object sender, DataGridCommandEventArgs e) 
{
	lbLinks.Visible = false;
	grid.EditItemIndex = e.Item.ItemIndex;
	BindGrid();
}

protected void Grid_Cancel(Object sender, DataGridCommandEventArgs e) 
{
	grid.EditItemIndex = -1;
	BindGrid();
}

protected void Grid_Update(Object sender, DataGridCommandEventArgs e) 
{
	int idx = grid.EditItemIndex;
	if(Portal.API.HtmlAnalyzer.HasScriptTags(((System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[1]).Text))
	{
		msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
		return;
	}
	DataRow dr = ds.Tables["news"].Rows[idx];
  dr["Created"] = DateTime.Parse(((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Text);
  dr["Expires"] = DateTime.Parse(((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Text);
  dr["Text"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[1]).Text;

	grid.EditItemIndex = -1;
	BindGrid();
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
	grid.DataSource = ds;
	grid.DataBind();
	
	ViewState["DataSet"] = ds;
}

</script>
<portal:Message id="msg" runat="server" />
<div id="lbLinks" runat="server">
	<portal:LinkButton id="lnkAdd" runat="server" OnClick="OnAdd" cssclass="LinkButton" LanguageRef="Add"></portal:LinkButton> | 
	<portal:LinkButton id="lnkUpdate" runat="server" OnClick="OnUpdate" cssclass="LinkButton" LanguageRef="Save"></portal:LinkButton> | 
	<portal:LinkButton id="lnkCancel" runat="server" OnClick="OnCancel" cssclass="LinkButton" LanguageRef="Cancel"></portal:LinkButton>
</div>
<br>
<asp:datagrid id="grid" runat="server" AutogenerateColumns="false" Width="100%" 
		OnEditCommand="Grid_Edit" OnCancelCommand="Grid_Cancel" 
		OnUpdateCommand="Grid_Update" OnItemCommand="Grid_CartCommand">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:EditCommandColumn ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="List_Header" 
			EditText="<img src=PortalImages/edit.gif>" CancelText="<img src=PortalImages/Cancel.gif>" UpdateText="<img src=PortalImages/save.gif>" 
			ItemStyle-Wrap="false" HeaderStyle-Wrap="false" ButtonType="LinkButton" 
			ItemStyle-CssClass="List_Sub_Cell_1" ItemStyle-HorizontalAlign="Center" 
			HeaderStyle-Width="1%" HeaderText="" />
		<asp:ButtonColumn ButtonType="LinkButton" Text="<img src='PortalImages/Delete.gif' alt='Delete'>" 
			CommandName="Delete"  HeaderText=""
			HeaderStyle-Width="1%" ItemStyle-CssClass="LinkButton"  ItemStyle-VerticalAlign="Top" />
		<portal:BoundColumn DataField="Created" LanguageRef-HeaderText="Created" HeaderStyle-Width="20" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" DataFormatString="{0:d}" />
		<portal:BoundColumn DataField="Expires" LanguageRef-HeaderText="Expires" HeaderStyle-Width="20" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" DataFormatString="{0:d}" />
		<portal:TemplateColumn LanguageRef-HeaderText="Text" HeaderStyle-Width="100%">
			<ItemTemplate>
				<asp:Label Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Text")%>'></asp:Label>
			</ItemTemplate>
			<EditItemTemplate>
				<asp:TextBox Runat="server" Width="100%" TextMode="MultiLine" Height="200px" Text='<%# DataBinder.Eval(Container.DataItem, "Text")%>'></asp:TextBox>
			</EditItemTemplate>
		</portal:TemplateColumn>
	</Columns>
</asp:datagrid>
