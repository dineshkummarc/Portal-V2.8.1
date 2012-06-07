<%@ Control Language="c#" Inherits="Portal.API.Module" enableViewState="False"%>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<table>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="Browser"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.Browser %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="Version" ID="Label10" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.Version %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="MajorVersion" ID="Label7" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.MajorVersion %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="MinorVersion" ID="Label8" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.MinorVersion %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="Platform" ID="Label9" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.Platform %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="ActiveXControls" ID="Label1" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.ActiveXControls %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="ClrVersion" ID="Label2" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.ClrVersion.ToString() %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="Cookies" ID="Label3" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.Cookies %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="EcmaScriptVersion" ID="Label4" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.EcmaScriptVersion.ToString() %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="Frames" ID="Label5" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.Frames %>
		</td>
	</tr>
	<tr>
		<td>
			<portal:Label runat="server" LanguageRef="JavaScript" ID="Label6" NAME="Label1"></portal:Label>
		</td>
		<td>
			<%=Request.Browser.JavaScript %>
		</td>
	</tr>
</table>
