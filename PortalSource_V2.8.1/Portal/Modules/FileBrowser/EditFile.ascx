<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditFile.ascx.cs" Inherits="Portal.Modules.FileBrowser.EditFile" %>
<%@ Register Assembly="Portal.API" Namespace="Portal.API.Controls" TagPrefix="Portal" %>
<h1>
  fileAllreadyExist<Portal:Label ID="_title" runat="server" LanguageRef="EditFile">Datei ändern</Portal:Label>&nbsp;</h1>
<table border="0">
  <tr>
    <td colspan="2">
      <p>
        <Portal:Label ID="_toDirectory" runat="server" LanguageRef="ToDirectory">Zum Verzeichnis</Portal:Label>:
        <asp:Label ID="dirPath" runat="server" EnableViewState="False" Text="Verzeichnis"></asp:Label></p>
      &nbsp;
      <asp:ValidationSummary ID="ValidatorSum" runat="server" ValidationGroup="FileChange"
        DisplayMode="List" ShowSummary="False" />
      <table>
        <tr>
          <td>
            <Portal:Label ID="_fileLbl" runat="server" LanguageRef="File">Datei</Portal:Label></td>
          <td>
            <asp:TextBox ID="fileName" runat="server" Width="15em" ValidationGroup="FileChange"></asp:TextBox>&nbsp;&nbsp;<Portal:RegularExpressionValidator
              ID="RegularExpressionValidator1" runat="server" ControlToValidate="fileName" Display="Dynamic"
              ErrorMessage="Ungültiger Dateiname" LanguageRef="InvalidFileName" SetFocusOnError="True"
              ValidationExpression='^[^\\\./:\*\?\"<>\|]{1}[^\\/:\*\?\"<>\|]{0,254}$' ValidationGroup="FileChange">*</Portal:RegularExpressionValidator><Portal:CustomValidator
                ID="fileAllreadyExist" runat="server" ControlToValidate="fileName" Display="Dynamic"
                ErrorMessage="Datei existiert bereits oder Dateiname nicht erlaubt" LanguageRef="FileAllreadyExists"
                ValidationGroup="FileChange">*</Portal:CustomValidator></td>
          <td>
            <Portal:Label ID="_DescriptionLbl" runat="server" LanguageRef="Description">Beschreibung</Portal:Label></td>
          <td>
            <asp:TextBox ID="fileDesc" runat="server" Width="30em"></asp:TextBox></td>
        </tr>
        <tr>
          <td style="height: 24px">
            <Portal:Label ID="_fileReplaceBylbl" runat="server" LanguageRef="FileReplaceBy">Datei ersetzen durch</Portal:Label></td>
          <td style="height: 24px">
            <asp:FileUpload ID="fileSelect" runat="server" /></td>
          <td style="height: 24px">
            <Portal:Label ID="_DateLbl" runat="server" LanguageRef="Date">Datum</Portal:Label></td>
          <td style="height: 24px">
            <Portal:JSCalendar ID="modDate" runat="server"></Portal:JSCalendar></td>
        </tr>
      </table>
      <br />
      &nbsp;<asp:CustomValidator ID="uploadException" runat="server" Display="Dynamic"
        ValidationGroup="FileChange"></asp:CustomValidator><br />
    </td>
  </tr>
  <tr>
    <td>
      &nbsp;<Portal:Button ID="OkBtn" runat="server" LanguageRef="Ok" OnClick="OkBtn_Click"
        Text="OK" ValidationGroup="FileUpload" /><Portal:Button ID="CancelBtn" runat="server"
          CausesValidation="False" LanguageRef="Cancel" OnClick="CancelBtn_Click" Text="Abbrechen" /></td>
    <td align="right">
      <Portal:Button ID="DeleteBtn" runat="server" OnClick="DeleteBtn_Click" Text="Datei löschen" LanguageRef="DeleteFile" ConfirmationLanguageRef="FileDeleteQuestion" />&nbsp;</td>
  </tr>
</table>
