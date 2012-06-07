<%@ Register TagPrefix="ucm" TagName="OVM" Src="OverlayMenu.ascx" %>
<%@ Register TagPrefix="portal" Namespace="Portal" %>
<%@ Control Language="c#" Inherits="Portal.ModuleHeaderControl" enableViewState="False" CodeFile="ModuleHeader.ascx.cs" %>
<div runat="server" id="HeaderContainer" class="ModuleTitle" visible="false">
  <div id="ModuleMenuDiv" class="ModuleMenu" runat="server">
	  <portal:EditLink Class="LinkButton" id="lnkEditLink" runat="server"></portal:EditLink>
	  <ucm:OVM id="ovm" runat="server" LanguageRef="EditLink_Text">
		  <MenuItem LanguageRef="EditMenu_EditContent" Icon="PortalImages/edit.gif" OnClick="OnEditContent"></MenuItem>
		  <MenuItem LanguageRef="EditMenu_EditModule" Icon="PortalImages/edit.gif" OnClick="OnEditModule"></MenuItem>
		  <SeparatorItem></SeparatorItem>
		  <MenuItem LanguageRef="EditMenu_MoveUp" Icon="PortalImages/up.gif" OnClick="OnMoveUp"></MenuItem>
		  <MenuItem LanguageRef="EditMenu_MoveDown" Icon="PortalImages/down.gif" OnClick="OnMoveDown"></MenuItem>
		  <MenuItem LanguageRef="EditMenu_MoveLeft" Icon="PortalImages/left.gif" OnClick="OnMoveLeft"></MenuItem>
		  <MenuItem LanguageRef="EditMenu_MoveRight" Icon="PortalImages/right.gif" OnClick="OnMoveRight"></MenuItem>
	  </ucm:OVM>
  </div>
  <div runat="server" id="ModuleTitle" visible="false">
  </div>
</div>