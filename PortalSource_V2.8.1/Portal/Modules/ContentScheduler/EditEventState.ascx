<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditEventState.ascx.cs"
  Inherits="Portal.Modules.ContentScheduler.EditEventState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>

<portal:LinkButton ID="_editSaveBtn" runat="server" LanguageRef="Save" OnClick="OnSave" CausesValidation="False">Speichern</portal:LinkButton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_editCancelBtn" runat="server" LanguageRef="Cancel" OnClick="OnCancel" CausesValidation="False">Abbrechen</portal:LinkButton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_deleteBtn" runat="server" LanguageRef="Delete" OnClick="OnDelete" ConfirmationLanguageRef="EventDeleteConfirmation" CausesValidation="False">Löschen</portal:LinkButton>
<hr />
<table>
  <tr>
    <td>
      <portal:Label ID="_dateLabel" runat="server" LanguageRef="EventDate">Zeitpunkt</portal:Label></td>
    <td>
      <portal:JSCalendar ID="_activationDate" CssClass="date" runat="server" DateTimeValue="2007-05-01"
        ShowTime="True" ShowWeekNumber="True" /></td>
    <td>
    </td>
  </tr>
  <tr>
    <td>
      <portal:Label ID="_hintLabel" runat="server" LanguageRef="EventHint">Hinweis zum Ereignis</portal:Label></td>
    <td>
      <asp:TextBox ID="_hintEdit" runat="server"></asp:TextBox>
    </td>
    <td>
    </td>
  </tr>
    <tr>
    <td>
      <portal:Label ID="_pageLabel" runat="server" LanguageRef="ContentPage">Zugehörige Seite</portal:Label></td>
    <td>
      <portal:Button ID="_createPage" runat="server" LanguageRef="CreatePageFromTemplate" Text="Von Vorlage erzeugen" OnClick="OnCreatePage_Click" TooltipLanguageRef="CreatePageFromTemplateTooltip" CausesValidation="False" />
      <portal:LinkButton ID="_editPage" runat="server" LanguageRef="EditPage" Visible="False" OnClick="OnEditPage_Click" CausesValidation="False">Seite bearbeiten</portal:LinkButton>
    </td>
      <td>
        <asp:DropDownList ID="_templateSelection" runat="server">
        </asp:DropDownList></td>
  </tr>
</table>
