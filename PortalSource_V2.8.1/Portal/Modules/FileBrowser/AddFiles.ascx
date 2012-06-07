<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddFiles.ascx.cs" Inherits="Portal.Modules.FileBrowser.AddFiles" %>
<%@ Register Assembly="Portal.API" Namespace="Portal.API.Controls" TagPrefix="Portal" %>
<h1><Portal:Label ID="_title" runat="server" LanguageRef="AddFile">Datei hinzufügen</Portal:Label></h1>
<Portal:Label ID="_toDirectory" runat="server" LanguageRef="ToDirectory">Zum Verzeichnis</Portal:Label>:
  <asp:Label ID="dirPath" runat="server" EnableViewState="False" Text="Verzeichnis"></asp:Label>
<br />
<asp:ValidationSummary ID="UploadValidatorSum" runat="server"
  ValidationGroup="FileUpload" DisplayMode="List" />
<table>
  <tr>
    <td>
      <Portal:Label ID="_fileLbl" runat="server" LanguageRef="File">Datei</Portal:Label></td>
    <td>
      <Portal:RequiredFieldValidator ID="fileRequired" runat="server" ControlToValidate="fileSelect"
        ErrorMessage="Keine Datei ausgewählt" SetFocusOnError="True" ValidationGroup="FileUpload" LanguageRef="NoFileSelected">*</Portal:RequiredFieldValidator>&nbsp;<asp:FileUpload ID="fileSelect" runat="server" Width="25em" />&nbsp;
    </td>
    <td>
      <Portal:Label ID="_NameLbl" runat="server" LanguageRef="Name">Name</Portal:Label></td>
    <td>
      <asp:TextBox ID="fileName" runat="server" Width="15em" ValidationGroup="FileUpload"></asp:TextBox>&nbsp;<Portal:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="fileName"
        Display="Dynamic" ErrorMessage="Ungültiger Dateiname" SetFocusOnError="True" ValidationExpression='^[^\\\./:\*\?\"<>\|]{1}[^\\/:\*\?\"<>\|]{0,254}$'
        ValidationGroup="FileUpload" LanguageRef="InvalidFileName">*</Portal:RegularExpressionValidator>&nbsp;<Portal:CustomValidator ID="fileAllreadyExist" runat="server" Display="Dynamic" ErrorMessage="Datei existiert bereits oder Dateiname nicht erlaubt" ControlToValidate="fileName" ValidationGroup="FileUpload" LanguageRef="FileAllreadyExists">*</Portal:CustomValidator></td>
    </tr>
    <tr>
    <td>
      <Portal:Label ID="_DescriptionLbl" runat="server" LanguageRef="Description">Beschreibung</Portal:Label></td>
    <td>
      <asp:TextBox ID="fileDesc" runat="server" Width="30em"></asp:TextBox></td>
      <td>
        <Portal:Label ID="_DateLbl" runat="server" LanguageRef="Date">Datum</Portal:Label></td>
      <td>
        <Portal:JSCalendar ID="modDate" runat="server"></Portal:JSCalendar></td>
  </tr>
</table>
<br />
<asp:CustomValidator ID="uploadException" runat="server"
  ValidationGroup="FileUpload" Display="Dynamic"></asp:CustomValidator><br />
<br />
<Portal:Button ID="OkBtn" runat="server" OnClick="OkBtn_Click" Text="OK" ValidationGroup="FileUpload" LanguageRef="Ok" /><Portal:Button
  ID="CancelBtn" runat="server" OnClick="CancelBtn_Click" Text="Abbrechen" CausesValidation="False" LanguageRef="Cancel" />
