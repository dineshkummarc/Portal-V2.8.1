<%@ Control Language="c#" Inherits="Portal.Modules.AdminPortal.ModuleEdit" CodeFile="ModuleEdit.ascx.cs" %>
<%@ Register TagPrefix="uc1" TagName="Roles" Src="Roles.ascx" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<div class="ModuleTitle" style="BORDER-BOTTOM: black 1px solid">
	<portal:Label runat="server" LanguageRef="ModuleData"></portal:Label>
</div>
<portal:LinkButton ID="lnkSave" Runat="server" CssClass="LinkButton" OnClick="OnSave" CausesValidation="True"
	LanguageRef="Save"></portal:LinkButton>
|
<portal:LinkButton ID="lnkCancel" Runat="server" CssClass="LinkButton" OnClick="OnCancel" CausesValidation="False"
	LanguageRef="Cancel"></portal:LinkButton>
|
<portal:LinkButton ID="lnkDelete" Runat="server" CssClass="LinkButton" OnClick="OnDelete" CausesValidation="False"
	LanguageRef="Delete"></portal:LinkButton>
<br>
<br>
<asp:Label ID="lbError" Runat="server" CssClass="Error" EnableViewState="False"></asp:Label>
<asp:ValidationSummary EnableClientScript="False" ID="validation" Runat="server"></asp:ValidationSummary>
<table cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top">
			<table width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<td class="Label">
						<portal:Label runat="server" LanguageRef="Title"></portal:Label>
					</td>
					<td class="Data"><asp:TextBox ID="txtTitle" Runat="server" Width="100%"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="Label" nowrap>
						<portal:Label runat="server" LanguageRef="Reference"></portal:Label><portal:RequiredFieldValidator EnableClientScript="False" ID="validator1" Runat="server" ControlToValidate="txtReference"
							LanguageRef="ReferenceRequired">*</portal:RequiredFieldValidator>
					</td>
					<td class="Data"><asp:TextBox ID="txtReference" Runat="server" Width="100%"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="Label"><portal:Label runat="server" LanguageRef="Type"></portal:Label><portal:CustomValidator ID="validator2" Runat="server" OnServerValidate="OnValidateCBType" EnableClientScript="False"
							LanguageRef="TypeRequired">*</portal:CustomValidator>
					</td>
					<td class="Data"><asp:DropDownList ID="cbType" Runat="server"></asp:DropDownList></td>
				</tr>
			</table>
		</td>
		<td width="20">&nbsp;</td>
		<td>
			<uc1:Roles id="RolesCtrl" runat="server"></uc1:Roles>
		</td>
	</tr>
</table>
<br>
