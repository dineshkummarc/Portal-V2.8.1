<%@ Reference Control="~/ModuleFailed.ascx" %>
<%@ Reference Control="~/ModuleHeader.ascx" %>
<%@ Register TagPrefix="ucm" TagName="OVM" Src="OverlayMenu.ascx" %>
<%@ Control Language="c#" Inherits="Portal.PortalTab" CodeFile="PortalTab.ascx.cs" %>
<div id="TabPath" runat="server">
</div>
<div id="TabMainMenuContainer" runat="server">
	<div id="TabMainMenu">
		<ucm:OVM id="ovm" runat="server" LanguageRef="EditTabLink_Text">
			<MenuItem LanguageRef="EditTabMenu_AddTab" Icon="PortalImages/new.gif" OnClick="OnAddTab"></MenuItem>
			<MenuItem LanguageRef="EditTabMenu_EditTab" Icon="PortalImages/edit.gif" OnClick="OnEditTab"></MenuItem>
			<MenuItem LanguageRef="EditTabMenu_DeleteTab" Icon="PortalImages/Delete.gif" OnClick="OnDeleteTab"></MenuItem>
			<SeparatorItem></SeparatorItem>
			<MenuItem LanguageRef="EditTabMenu_AddLeftModule" Icon="PortalImages/new.gif" OnClick="OnAddLeftModule"></MenuItem>
			<MenuItem LanguageRef="EditTabMenu_AddMiddleModule" Icon="PortalImages/new.gif" OnClick="OnAddMiddleModule"></MenuItem>
			<MenuItem LanguageRef="EditTabMenu_AddRightModule" Icon="PortalImages/new.gif" OnClick="OnAddRightModule"></MenuItem>
		</ucm:OVM>
	</div>
	<div>&nbsp;</div>
</div>
<table id="TabContainer">
  <tr>
    <td id="TabLeft" runat="server" class="TabLeft">
    </td>
    <td id="TabMiddle" runat="server" class="TabMiddle">
    </td>
    <td id="TabRight" runat="server" class="TabRight">
    </td>
  </tr>
</table>
