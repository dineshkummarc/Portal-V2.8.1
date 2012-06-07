<%@ Register TagPrefix="portal" TagName="PortalFooter" Src="PortalFooter.ascx" %>
<%@ Register TagPrefix="portal" TagName="PortalHeader" Src="PortalHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TabMenu" Src="TabMenu.ascx" %>
<%@ Page Language="c#" Inherits="Portal.StartPage" CodeFile="default.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">

<html>
	<head id="Head1" runat="server">
		<link runat="server" id="cssLink" rel="stylesheet" type="text/css"/>
		<link runat="server" id="favicon" rel="SHORTCUT ICON" href="" type="image/ico" visible="false"/>
		<meta runat="server" id="metaDescription" name="description" content="" visible="false" />
		<meta runat="server" id="metaAuthor" name="author" content="" visible="false" />
		<meta runat="server" id="metaKeywords" name="keywords" content="" visible="false" />
		<meta runat="server" id="metaRobots" name="robots" content="" visible="false" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
			<div id="container">
				<div id="PortalHeader">
					<portal:PortalHeader id="header" runat="server"></portal:PortalHeader>
				</div>
				<div id="TabMenuContainer">
					<uc1:TabMenu id="TabMenu" runat="server"></uc1:TabMenu>
				</div>
				<div id="PortalMain">
				  <asp:PlaceHolder ID="ContentPlace" runat="server"></asp:PlaceHolder>
				</div>
				<div runat="server" id="PortalFooter">
					<portal:PortalFooter id="PortalFooter1" runat="server"></portal:PortalFooter>
				</div>
			</div>
		</form>
	</body>
</html>

