<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CleanUpState.ascx.cs" Inherits="Portal.Modules.ContentScheduler.CleanUpState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>
<portal:LinkButton ID="_backBtn" runat="server" LanguageRef="Back" OnClick="OnBack" CausesValidation="False">Zurück</portal:LinkButton><br />
<hr />
<br />
&nbsp;<portal:Label ID="_information" runat="server" LanguageRef="CleanUpInfo"></portal:Label>
<br />
<table>
  <tr>
    <td style="height: 43px" valign="top">
<portal:Label ID="_thresholdEventLbl" runat="server" LanguageRef="ThresholdEvent"></portal:Label><br />
    </td>
    <td style="height: 43px" valign="top">
<asp:DropDownList ID="_thresholdEvent" runat="server" AutoPostBack="True">
</asp:DropDownList><br />
      <portal:Button ID="_removeBtn" runat="server" ConfirmationLanguageRef="CleanUpConfirmation"
        LanguageRef="CleanUp" OnClick="OnCleanUp" Text="Entfernen" TooltipLanguageRef="CleanUpTooltip" CausesValidation="False" /></td>
    <td style="height: 43px" valign="top">
<asp:Label ID="_removeInfoLbl" runat="server" Text="Info"></asp:Label></td>
  </tr>
</table>
<br />
&nbsp;&nbsp;
