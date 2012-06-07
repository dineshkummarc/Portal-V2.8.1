<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.QuickLinks.EditQuickLinks" CodeFile="EditQuickLinks.ascx.cs" %>
<div id="lbLinks" runat="server">
	<portal:linkbutton id="AddLB" runat="server" LanguageRef="Add" onclick="AddLB_Click" />&nbsp;|
	<portal:linkbutton id="SaveLB" runat="server" LanguageRef="Save" onclick="SaveLB_Click" />&nbsp;|
	<portal:linkbutton id="CancelLB" runat="server" LanguageRef="Cancel" onclick="CancelLB_Click" /><BR>
</div>
<asp:datagrid id="LinksGrid" Width="100%" AutoGenerateColumns="False" runat="server">
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<Columns>
		<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="&lt;img src=PortalImages/save.gif&gt;" CancelText="&lt;img src=PortalImages/Cancel.gif&gt;"
			EditText="&lt;img src=PortalImages/edit.gif&gt;">
			<HeaderStyle Width="32px"></HeaderStyle>
		</asp:EditCommandColumn>
		<asp:ButtonColumn Text="&lt;img src=PortalImages/Delete.gif&gt;" CommandName="Delete">
			<HeaderStyle Width="16px"></HeaderStyle>
		</asp:ButtonColumn>
		<asp:TemplateColumn>
			<HeaderStyle Width="16px"></HeaderStyle>
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkUpLink" OnCommand="OnUpLink" CommandArgument='<%# Container.ItemIndex %>' >
					<img src="PortalImages/up.gif" alt="Up">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<HeaderStyle Width="16px"></HeaderStyle>
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkDownLink" OnCommand="OnDownLink" CommandArgument='<%# Container.ItemIndex %>' >
					<img src="PortalImages/down.gif" alt="Down">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<portal:BoundColumn DataField="Text" LanguageRef-HeaderText="Text" />
		<portal:BoundColumn DataField="URL" LanguageRef-HeaderText="URL" />
		<portal:BoundColumn Visible="False" DataField="Position" LanguageRef-HeaderText="Position" />
	</Columns>
</asp:datagrid>
