<%@ Page language="c#" Inherits="Portal.EditModuleTable" CodeFile="EditModuleTable.aspx.cs" %>
<%@ Register TagPrefix="portal" TagName="PortalHeader" Src="PortalHeader.ascx" %>
<%@ Register TagPrefix="portal" TagName="PortalFooter" Src="PortalFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ModuleEdit" Src="Modules/AdminPortal/ModuleEdit.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 //EN" >
<HTML>
	<HEAD runat="server">
		<title>EditModuleTable</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link runat="server" id="cssLink" rel="stylesheet" href="Portal.css" type="text/css"/>
	</HEAD>
	<body>
		<form method="post" runat="server">
			<div id="container">
				<div id="PortalHeader">
					<portal:PortalHeader id="header" runat="server"></portal:PortalHeader>
				</div>
				<div id="PortalEditContainer">
					<div id="PortalEditModule">
						<uc1:ModuleEdit id="ModuleEditCtrl" runat="server" OnSave="OnSave" OnCancel="OnCancel" OnDelete="OnDelete"></uc1:ModuleEdit>
					</div>
				</div>
				<div id="PortalFooter">
					<portal:PortalFooter id="footer" runat="server"></portal:PortalFooter>
				</div>
			</div>
		</form>
	</body>
</HTML>
