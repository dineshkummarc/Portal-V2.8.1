<%@ Control Language="c#" Inherits="Portal.API.Module" enableViewState="False"%>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Data" %>
<script runat="server">
void Page_Load(Object sender, EventArgs e) 
{
	DataSet ds = ReadConfig();
	if(ds != null)
	{
		grid.DataMember = "data";
		grid.DataSource = ds;

		DataTable ht = ds.Tables["header"];
		if(ht != null)
		{
			for(int i=0;i<ht.Rows.Count;i++)
			{
				BoundColumn bc = new BoundColumn();
				bc.HeaderText = (string)ht.Rows[i]["name"];
				bc.DataField = (string)ht.Rows[i]["name"];
				bc.HeaderStyle.Width = new Unit((string)ht.Rows[i]["width"]);
				grid.Columns.Add(bc);
			}
			grid.DataBind();
		}
	}
}
</script>
<asp:datagrid id="grid" runat="server" AutogenerateColumns="false">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
</asp:datagrid>
