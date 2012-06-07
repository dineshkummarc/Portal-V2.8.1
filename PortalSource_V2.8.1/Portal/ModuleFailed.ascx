<%@ Control Language="c#" Inherits="Portal.ModuleFailed" CodeFile="ModuleFailed.ascx.cs" %>
<table style="BORDER: red 3px solid; color: red;">
	<tr>
		<td style="font-weight:bold;">Error loading Module!</td>
	</tr>
	<tr>
		<td><asp:Label ID="lbMessage" Runat="server"></asp:Label></td>
	</tr>
</table>