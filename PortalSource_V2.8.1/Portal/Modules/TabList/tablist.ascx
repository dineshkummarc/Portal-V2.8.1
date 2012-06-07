<%@ Control Language="c#" Inherits="Portal.Modules.TabList.TabList" CodeFile="TabList.ascx.cs" EnableViewState="false" %>
<table>
	<asp:Repeater ID="Tabs" Runat="server">
		<ItemTemplate>
			<tr>
				<td width="1px">
					<img src="PortalImages/Bullet.gif">
				</td>
				<td>
					<asp:HyperLink Runat="server" 
						CssClass="LinkButton"
						NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "URL")%>'>
						<%# DataBinder.Eval(Container.DataItem, "Text") %>
					</asp:HyperLink>
				</td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</table>
