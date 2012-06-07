<%@ Control Language="c#" Inherits="Portal.Modules.AdminPortal.ModuleListControl" CodeFile="ModuleList.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<div class="ModuleTitle" id="divTitle" runat="server" style="border-bottom: solid 1px black;"></div>
<portal:LinkButton ID="lnkAddModule" Runat="server" OnClick="OnAddModule" CssClass="LinkButton" LanguageRef="AddModule"></portal:LinkButton>
<asp:datagrid id="gridModules" runat="server" AutogenerateColumns="false" Width="100%">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderStyle-Width="16px">
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkModule" 
					OnCommand="OnEditModule" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Reference") %>' >
					<img src="PortalImages/edit.gif" alt="Edit">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderStyle-Width="16px">
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkUp" 
					OnCommand="OnModuleUp" 
					CommandArgument='<%# Container.ItemIndex %>' >
					<img src="PortalImages/up.gif" alt="Up">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderStyle-Width="16px">
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkDown" 
					OnCommand="OnModuleDown" 
					CommandArgument='<%# Container.ItemIndex %>' >
					<img src="PortalImages/down.gif" alt="Down">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<portal:BoundColumn DataField="Title" LanguageRef-HeaderText="Title" />
		<portal:BoundColumn DataField="Reference" LanguageRef-HeaderText="Reference" />
		<portal:BoundColumn DataField="ModuleType" LanguageRef-HeaderText="Type" />
	</Columns>
</asp:datagrid>
<br>