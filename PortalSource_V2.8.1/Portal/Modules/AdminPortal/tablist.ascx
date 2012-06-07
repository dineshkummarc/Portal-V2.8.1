<%@ Control Language="c#" Inherits="Portal.Modules.AdminPortal.TabList" CodeFile="TabList.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<div class="ModuleTitle" style="border-bottom: solid 1px black;">
	<portal:Label id="SubTabsTitle" runat="server" LanguageRef="SubTabs"></portal:Label>
</div>
<portal:LinkButton ID="lnkAddModule" Runat="server" OnClick="OnAddTab" CssClass="LinkButton" LanguageRef="AddTab"></portal:LinkButton>
<asp:datagrid id="Tabs" runat="server" AutogenerateColumns="false" Width="100%">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="" HeaderStyle-Width="16px">
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkTitle" 
					OnCommand="OnEditTab" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Reference") %>' >
					<img src="PortalImages/edit.gif" alt="Edit">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="" HeaderStyle-Width="16px">
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkUp" 
					OnCommand="OnTabUp" 
					CommandArgument='<%# Container.ItemIndex %>' >
					<img src="PortalImages/up.gif" alt="Up">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="" HeaderStyle-Width="16px">
			<ItemTemplate>
				<asp:LinkButton Runat="server" ID="lnkDown" 
					OnCommand="OnTabDown" 
					CommandArgument='<%# Container.ItemIndex %>' >
					<img src="PortalImages/down.gif" alt="Down">
				</asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateColumn>
		<portal:BoundColumn DataField="Text" LanguageRef-HeaderText="Title" />
		<portal:BoundColumn DataField="URL" LanguageRef-HeaderText="Reference" />
	</Columns>
</asp:datagrid>
