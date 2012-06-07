<%@ Control Language="c#" Inherits="Portal.Modules.AdminRoles.AdminRoles" CodeFile="AdminRoles.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<br>
<div id="msgDeleteBuildInRole" class="Error" runat="server">
	<portal:Label runat=server LanguageRef="DeleteBuildInRole"></portal:Label>
</div>
<asp:TextBox ID="txtNewRole" Runat="server"></asp:TextBox>
<portal:LinkButton CssClass="LinkButton" ID="lnkAddRole" Runat="server" OnClick="OnAddRole" LanguageRef="AddRole"></portal:LinkButton>
<asp:datagrid id="gridRoles" runat="server" AutogenerateColumns="false" OnItemCommand="Grid_CartCommand">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:ButtonColumn ButtonType="LinkButton" Text="<img src='PortalImages/Delete.gif' alt='Delete'>" CommandName="Delete" HeaderStyle-Width="1px" ItemStyle-CssClass="LinkButton" />
		<portal:BoundColumn DataField="name" LanguageRef-HeaderText="Name" HeaderStyle-Width="200px" />
	</Columns>
</asp:datagrid>
