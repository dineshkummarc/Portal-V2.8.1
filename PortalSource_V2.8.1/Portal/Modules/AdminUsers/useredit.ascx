<%@ Control Language="c#" Inherits="Portal.Modules.AdminUsers.UserEdit" CodeFile="UserEdit.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.Data" %>
<br>
<portal:LinkButton CssClass="LinkButton" ID="lnkBack" OnClick="OnBack" Runat="server" LanguageRef="Back"></portal:LinkButton>
|
<portal:LinkButton CssClass="LinkButton" ID="lnkSave" OnClick="OnSave" Runat="server" LanguageRef="Save"></portal:LinkButton>
|
<portal:LinkButton CssClass="LinkButton" ID="lnkDelete" OnClick="OnDelete" Runat="server" LanguageRef="Delete"></portal:LinkButton>
<br>
<br>
<table width="400px">
	<tr>
		<td class="Label" width="150">
			<portal:Label runat=server LanguageRef="Account"></portal:Label>
		</td>
		<td class="Data"><asp:TextBox Width="100%" ID="txtLogin" Runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="Label">
			<portal:Label runat=server LanguageRef="Password" ID="Label1" NAME="Label1"></portal:Label>
		</td>
		<td class="Data"><asp:TextBox Width="100%" ID="txtPassword" Runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="Label" nowrap>
			<portal:Label runat=server LanguageRef="FirstName" ID="Label2" NAME="Label2"></portal:Label>
		</td>
		<td class="Data"><asp:TextBox Width="100%" ID="txtFirstName" Runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="Label" nowrap>
			<portal:Label runat=server LanguageRef="SurName" ID="Label3" NAME="Label3"></portal:Label>
		</td>
		<td class="Data"><asp:TextBox Width="100%" ID="txtSurName" Runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="Label" nowrap>
			<portal:Label runat=server LanguageRef="EMail" ID="Label4" NAME="Label3"></portal:Label>
		</td>
		<td class="Data"><asp:TextBox Width="100%" ID="txtEMail" Runat="server"></asp:TextBox></td>
	</tr>
</table>
<asp:TextBox Width="100%" Visible="False" ID="txtUserId" Runat="server"></asp:TextBox>
<br>
<asp:datagrid id="gridRoles" runat="server" AutogenerateColumns="false" Width="400px">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<portal:TemplateColumn LanguageRef-HeaderText="Select">
			<ItemTemplate>
				<asp:CheckBox Runat="server" ID="chkRole" Checked=<%#HasRole((DataRowView)Container.DataItem)%> >					
				</asp:CheckBox>
			</ItemTemplate>
		</portal:TemplateColumn>
		<portal:BoundColumn DataField="name" LanguageRef-HeaderText="Role" HeaderStyle-Width="100%" />
	</Columns>
</asp:datagrid>

