<%@ Register TagPrefix="uc1" TagName="TabMenu" Src="TabMenu.ascx" %>
<%@ Register TagPrefix="portal" TagName="PortalHeader" Src="PortalHeader.ascx" %>
<%@ Register TagPrefix="portal" TagName="PortalFooter" Src="PortalFooter.ascx" %>
<%@ Page language="c#" Inherits="Portal.EditPageTable" CodeFile="EditPageTable.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 //EN" >
<HTML>
  <HEAD runat="server">
    <title>EditPageTable</title>
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
        <div id="TabMenuContainer">
          <uc1:TabMenu id="TabMenu" runat="server"></uc1:TabMenu>
        </div>
        <div id="PortalEditContainer">
          <div id="divEdit" class="PortalEditPage" runat="server">
          </div>
        </div>
        <div id="PortalFooter">
          <portal:PortalFooter id="footer" runat="server"></portal:PortalFooter>
        </div>
      </div>
    </form>
  </body>
</HTML>
