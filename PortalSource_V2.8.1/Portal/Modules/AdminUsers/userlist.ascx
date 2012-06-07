<%@ Control Language="c#" Inherits="Portal.Modules.AdminUsers.UserListControl" CodeFile="UserList.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.Data" %>
<br>
<portal:LinkButton CssClass="LinkButton" ID="lnkAddUser" Runat="server" OnClick="OnAddUser" LanguageRef="AddUser"></portal:LinkButton>
<asp:datagrid id="gridUsers" runat="server" AutogenerateColumns="false" OnItemCommand="Grid_CartCommand">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:ButtonColumn ButtonType="LinkButton" Text="<img src='PortalImages/edit.gif' alt='Edit'>" CommandName="Edit" HeaderStyle-Width="16px" HeaderText="&nbsp;" ItemStyle-CssClass="LinkButton" />
		<portal:BoundColumn DataField="login" LanguageRef-HeaderText="Account" HeaderStyle-Width="100px" />
		<portal:BoundColumn DataField="firstName" LanguageRef-HeaderText="FirstName" HeaderStyle-Width="200px" />
		<portal:BoundColumn DataField="surName" LanguageRef-HeaderText="SurName" HeaderStyle-Width="200px" />
		<portal:TemplateColumn LanguageRef-HeaderText="Roles">
			<ItemTemplate>
				<asp:Label Runat="server" ID="Label">
					<%# GetRoles((DataRowView)Container.DataItem) %>
				</asp:Label>
			</ItemTemplate>
		</portal:TemplateColumn>
	</Columns>
</asp:datagrid>
