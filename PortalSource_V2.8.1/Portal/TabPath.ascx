<%@ Control Language="c#" Inherits="Portal.TabPath" CodeFile="TabPath.ascx.cs" EnableViewState="false" %>
<asp:Repeater ID="tabpath" Runat="server">
	<HeaderTemplate>
<table width="100%" class="TabPath" border="0" cellpadding="0" cellspacing="0">
			<tr><td>
	</HeaderTemplate>
	<ItemTemplate>
		<asp:HyperLink CssClass="TabPathButton" ID="lnktext" Runat="server"
			NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "URL") %>'>
				<%# DataBinder.Eval(Container.DataItem, "Text") %>
			</asp:HyperLink>
	</ItemTemplate>
	<SeparatorTemplate>&nbsp;>>&nbsp;</SeparatorTemplate>
	<FooterTemplate>
		</td></tr></table>
	</FooterTemplate>
</asp:Repeater>