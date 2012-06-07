<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditEmailState.ascx.cs" Inherits="Portal.Modules.FormBuilder.EditEmailState" %>
<%@ Register Assembly="Portal.API" Namespace="Portal.API.Controls" TagPrefix="portal" %>

<portal:LinkButton ID="_SaveBack" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="SaveBack" OnClick="OnSaveBack">Speichern und zur&uuml;ck</portal:LinkButton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_SaveBtn" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="Save" OnClick="OnSave">Speichern</portal:LinkButton>&nbsp; |&nbsp;
<portal:LinkButton ID="_CancelBtn" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="Cancel" OnClick="OnCancel">Abbrechen</portal:LinkButton><br />
<hr />

<table style="width: 100%">
  <tr>
  <td style="height: 22px" colspan="3">
    <portal:Label ID="_sendEmail" runat="server" LanguageRef="SendEmail">Formular per Email versenden:</portal:Label><asp:CheckBox ID="_sendEmailCheck" runat="server" /></td>
  </tr>
  <tr>
    <td style="height: 21px">
      <portal:Label ID="_serverLbl" runat="server" LanguageRef="EmailServer">E-Mail Server:</portal:Label></td>
    <td style="height: 21px" colspan="2">
      <asp:TextBox ID="_serverEdit" runat="server" Width="20em"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="height: 21px">
      <portal:Label ID="_serverNeedAuthLbl" runat="server" LanguageRef="ServerNeedAuth">Server benötigt Authentifizierung:</portal:Label></td>
    <td style="height: 21px" colspan="2">
      <asp:CheckBox ID="_serverNeedAuthCheck" runat="server" /></td>
  </tr>
  <tr>
    <td style="height: 21px">
      <portal:Label ID="_userNameLbl" runat="server" LanguageRef="EmailUserName">E-Mail Benutzername:</portal:Label></td>
    <td style="height: 21px" colspan="2">
      <asp:TextBox ID="_userNameEdit" runat="server" Width="20em"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="height: 21px">
      <portal:Label ID="_emailPasswordLbl" runat="server" LanguageRef="EmailPassword">E-Mail Passwort:</portal:Label></td>
    <td style="height: 21px" colspan="2">
      <asp:TextBox ID="_passwordEdit" runat="server" TextMode="Password" Width="20em"></asp:TextBox></td>
  </tr>
  <tr>
    <td colspan="3" style="height: 21px"><hr /></td>
  </tr>
  <tr>
    <td style="height: 21px">
      <portal:Label ID="_senderLbl" runat="server" LanguageRef="Sender">Absender:</portal:Label></td>
    <td style="height: 21px" colspan="2">
      <portal:TextBox ID="_senderEdit" runat="server" AutoCompleteType="Email" Width="20em" TooltipLanguageRef="FormPlaceholderAllowed" OnTextChanged="_senderEdit_TextChanged"></portal:TextBox>
      <portal:Button ID="_insertSenderBtn" runat="server" CausesValidation="False" Enabled="False"
        EnableViewState="False" LanguageRef="InsertPlaceholder" Text="<< Platzhalter" /></td>
  </tr>
	<tr>
		<td style="height: 21px">
      <portal:label id="_toLbl" runat="server" languageref="To">An:</portal:label></td>
		<td style="height: 21px" colspan="2">
      <portal:TextBox ID="_toEdit" runat="server" AutoCompleteType="Email" TooltipLanguageRef="SemicolonSepEmail"
        Width="20em"></portal:TextBox>
      <portal:Button ID="_insertToBtn" runat="server" CausesValidation="False" Enabled="False"
        EnableViewState="False" LanguageRef="InsertPlaceholder" Text="<< Platzhalter" /></td>
	</tr>
	<tr>
		<td><portal:label id="_ccLbl" runat="server" languageref="Cc">Cc:</portal:label></td>
		<td colspan="2">
      <portal:TextBox ID="_ccEdit" runat="server" AutoCompleteType="Email" TooltipLanguageRef="SemicolonSepEmail"
        Width="20em"></portal:TextBox>
      <portal:Button ID="_insertCcBtn" runat="server" CausesValidation="False" Enabled="False"
        EnableViewState="False" LanguageRef="InsertPlaceholder" Text="<< Platzhalter" /></td>
	</tr>
  <tr>
    <td>
      <portal:label id="_bccLbl" runat="server" languageref="Bcc">Bcc:</portal:label>
    </td>
    <td colspan="2">
      <portal:TextBox ID="_bccEdit" runat="server" AutoCompleteType="Email" TooltipLanguageRef="SemicolonSepEmail"
        Width="20em"></portal:TextBox>
      <portal:Button ID="_insertBccBtn" runat="server" CausesValidation="False" Enabled="False"
        EnableViewState="False" LanguageRef="InsertPlaceholder" Text="<< Platzhalter" /></td>
  </tr>
  <tr>
    <td>
    </td>
    <td colspan="2">
    </td>
  </tr>
  <tr>
    <td>
      <portal:Label ID="_subjectLbl" runat="server" LanguageRef="Subject">Betreff:</portal:Label></td>
    <td colspan="2">
      <portal:TextBox ID="_subjectEdit" runat="server" Width="20em" TooltipLanguageRef="FormPlaceholderAllowed" OnTextChanged="_subjectEdit_TextChanged"></portal:TextBox>
      <portal:Button ID="_insertSubjectBtn" runat="server" CausesValidation="False" Enabled="False"
        EnableViewState="False" LanguageRef="InsertPlaceholder" Text="<< Platzhalter" /></td>
  </tr>
  <tr>
    <td>
      <portal:Label ID="_emailBodyLbl" runat="server" LanguageRef="EmailBody">E-Mail Inhalt:</portal:Label></td>
    <td colspan="2">
    </td>
  </tr>
  <tr>
    <td colspan="3">
      <portal:TextBox ID="_body" runat="server" Height="15em" TextMode="MultiLine" TooltipLanguageRef="FormPlaceholderAllowed"
        Width="99%"></portal:TextBox></td>
  </tr>
      <tr>
    <td style="height: 22px">
      <portal:Label ID="_bodyAsHtmlLbl" runat="server" LanguageRef="BodyAsHtml">Inhalt als HTML versenden:</portal:Label></td>
    <td style="height: 22px"><asp:CheckBox ID="_bodyAsHtmlCheck" runat="server" /><td align="right" style="height: 22px">
        <portal:Button ID="_insertBodyBtn" runat="server" CausesValidation="False" Enabled="False"
        EnableViewState="False" LanguageRef="InsertPlaceholder" Text="<< Platzhalter" /></td>
      </tr>
  <tr>
    <td colspan="3" style="height: 21px">
      <hr />
    </td>
  </tr>
  <tr>
    <td valign="top">
      <portal:Label ID="_insertPlaceholder" runat="server" LanguageRef="SelectPlaceholder">Platzhalter auswählen:</portal:Label>
    </td>
    <td valign="top" colspan="2">
      <asp:ListBox ID="_availablePlaceholder" runat="server" Height="8em" Width="20em">
      </asp:ListBox>
      </td>
  </tr>
</table>
<br />
