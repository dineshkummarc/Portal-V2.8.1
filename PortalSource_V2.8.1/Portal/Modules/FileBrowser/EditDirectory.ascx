<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditDirectory.ascx.cs"
  Inherits="Portal.Modules.FileBrowser.EditDirectory" %>
<%@ Register Assembly="Portal.API" Namespace="Portal.API.Controls" TagPrefix="Portal" %>
<asp:Panel ID="editDirectoryPnl" runat="server"  DefaultButton="OkBtn">
  <table border="0">
    <tr>
      <td>
        <Portal:Label ID="_directoryName" runat="server" LanguageRef="Directoryname">Verzeichnisname </Portal:Label>
      </td>
      <td>
        <asp:TextBox ID="dirName" runat="server" Width="15em"></asp:TextBox></td>
    </tr>
    <tr>
      <td>
        <Portal:Label ID="_DescriptionLbl" runat="server" LanguageRef="Description">Beschreibung</Portal:Label></td>
      <td>
        <asp:TextBox ID="description" runat="server" Width="30em"></asp:TextBox></td>
    </tr>
    <tr>
      <td align="left">
        <Portal:Button ID="OkBtn" runat="server" Text="OK" OnClick="OkBtn_Click" LanguageRef="Ok" /><Portal:Button
          ID="CancelBtn" runat="server" Text="Abbrechen" OnClick="CancelBtn_Click" LanguageRef="Cancel" />
      </td>
      <td align="right">
        <Portal:Button ID="DeleteBtn" runat="server" OnClick="DeleteBtn_Click" Text="Verzeichnis löschen"
          Visible="False" LanguageRef="DeleteDirectory" ConfirmationLanguageRef="DirectoryDeleteQuestion" />
      </td>
    </tr>
  </table>
</asp:Panel>
