<%@ Reference Control="~/modules/adminportal/modulelist.ascx" %>
<%@ Control Language="c#" Inherits="Portal.Modules.AdminPortal.TabControl" CodeFile="Tab.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Register TagPrefix="uc1" TagName="ModuleList" Src="ModuleList.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Module" Src="ModuleEdit.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Roles" Src="Roles.ascx" %>
<div class="ModuleTitle" style="BORDER-BOTTOM: black 1px solid">
	<portal:Label runat="server" LanguageRef="TabData" ID="Label3" NAME="Label3"></portal:Label>
</div>
<portal:LinkButton ID="lnkSave" Runat="server" CssClass="LinkButton" OnClick="OnSave" CausesValidation="True" LanguageRef="Save"></portal:LinkButton>
|
<portal:LinkButton ID="lnkCancel" Runat="server" CssClass="LinkButton" OnClick="OnCancel" CausesValidation="False" LanguageRef="Cancel"></portal:LinkButton>
|
<portal:LinkButton ID="lnkDelete" Runat="server" CssClass="LinkButton" OnClick="OnDelete" CausesValidation="False" LanguageRef="Delete"></portal:LinkButton>
<br>
<br>
<asp:Label ID="lbError" Runat="server" CssClass="Error" EnableViewState="False"></asp:Label>
<asp:ValidationSummary EnableClientScript="False" ID="validation" Runat="server"></asp:ValidationSummary>
<table width="100%" cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top">
			<table width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<td class="Label" style="white-space:nowrap;">
						<portal:Label runat="server" LanguageRef="Title" ID="Label1" NAME="Label1"></portal:Label><portal:RequiredFieldValidator EnableClientScript="False" ID="Requiredfieldvalidator1" Runat="server" ControlToValidate="txtTitle"
							LanguageRef="TitleRequired">*</portal:RequiredFieldValidator>
					</td>
					<td class="Data" width="100%">
						<asp:TextBox ID="txtTitle" Runat="server" Width="97%"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="Label" style="white-space:nowrap;">
						<portal:Label runat="server" LanguageRef="Reference" ID="Label2" NAME="Label2"></portal:Label><portal:RequiredFieldValidator EnableClientScript="False" ID="validator1" Runat="server" ControlToValidate="txtReference"
							LanguageRef="ReferenceRequired">*</portal:RequiredFieldValidator>
					</td>
					<td class="Data">
						<asp:TextBox ID="txtReference" Runat="server" Width="97%"></asp:TextBox>
					</td>
				</tr>
				<tr>
				  <td class="Label" style="white-space:nowrap;">
				    <portal:Label runat="server" LanguageRef="ImagePathI" id="Label4"></portal:Label>
				  </td>
					<td class="Data">
						<asp:TextBox ID="txtImagePathI" Runat="server" Width="97%"></asp:TextBox>
					</td>
				</tr>
				<tr>
				  <td class="Label" style="white-space:nowrap;">
				    <portal:Label runat="server" LanguageRef="ImagePathA" id="Label5"></portal:Label>
				  </td>
					<td class="Data">
						<asp:TextBox ID="txtImagePathA" Runat="server" Width="97%"></asp:TextBox>
					</td>
				</tr>
			</table>
		</td>
		<td width="20">&nbsp;</td>
		<td>
			<uc1:Roles id="RolesCtrl" runat="server" ShowRoleType="False"></uc1:Roles>
		</td>
	</tr>
</table>
<uc1:Module id="ModuleEditCtrl" runat="server" Visible="False" OnCancel="OnCancelEditModule" OnDelete="OnDeleteModule" OnSave="OnSaveModule"></uc1:Module>
<uc1:ModuleList id="ModuleListCtrl_Left" runat="server" TitleLanguageRef="ModulesLeft"></uc1:ModuleList>
<uc1:ModuleList id="ModuleListCtrl_Middle" runat="server" TitleLanguageRef="ModulesMiddle"></uc1:ModuleList>
<uc1:ModuleList id="ModuleListCtrl_Right" runat="server" TitleLanguageRef="ModulesRight"></uc1:ModuleList>
