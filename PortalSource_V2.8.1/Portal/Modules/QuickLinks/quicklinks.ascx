<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" enableViewState="False"%>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Data" %>
<script runat="server">

void Page_Load(Object sender, EventArgs e) 
{
	// Schema exists - cannot be null!
	DataSet ds = ReadConfig();

	links.DataSource = ds.Tables["links"].Select("1=1", "Position");
	links.DataBind();
}

</script>
<table>
	<asp:Repeater id="links" runat="server">
		<ItemTemplate>
			<tr>
				<td width="1px"><img src="PortalImages/Bullet.gif" alt="bullet"></td>
				<td nowrap>
					<a href='<%# ((DataRow)Container.DataItem)["URL"] %>'
						 class="LinkButton"
						 <%# (bool)((DataRow)Container.DataItem)["OpenInNewWindow"] ? " target='_blank' " : "" %> >
					<%# ((DataRow)Container.DataItem)["Text"] %>
				</a></td>
			</tr>			
		</ItemTemplate>
	</asp:Repeater>
</table>
