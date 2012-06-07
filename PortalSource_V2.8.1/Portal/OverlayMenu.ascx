<%@ Control Language="c#" Inherits="Portal.OverlayMenu" CodeFile="OverlayMenu.ascx.cs" EnableViewState="false" %>
<div id="MenuRoot" runat="server" class="OverlayMenuRoot">
	<a href="javascript:OpenOverlayMenu(document.getElementById('<%=Menu.ClientID%>'), document.getElementById('<%=MenuRoot.ClientID%>'))">
		<%=RootText%>
	</a>
</div>
<div id="Menu" runat="server" style="DISPLAY: none;" class="OverlayMenu">
	<asp:Repeater id="MenuRepeater" Runat="server">
		<ItemTemplate>
			<div nowrap
				runat="server"
				visible='<%# Container.DataItem as Portal.OverlayMenuSeparatorItem == null && ((Portal.OverlayMenuItem)Container.DataItem).Visible %>'
				class="OverlayMenuItem"
				onmouseover="javascript:OverlayMenuOnMouseOver(this)" 
				onmouseout="javascript:OverlayMenuOnMouseOut(this)"									
				onclick='<%# Page.GetPostBackClientHyperlink(this, DataBinder.Eval(Container.DataItem, "MenuItemIndex").ToString() ) %>'>
				<img src='<%# DataBinder.Eval(Container.DataItem, "Icon") %>' > &nbsp;
				<%# DataBinder.Eval(Container.DataItem, "Text") %>
			</div>
			<div nowrap
				class="OverlayMenuSeparator"
				runat="server"
				visible='<%# Container.DataItem as Portal.OverlayMenuSeparatorItem != null && ((Portal.OverlayMenuItem)Container.DataItem).Visible %>'>
				<hr>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
