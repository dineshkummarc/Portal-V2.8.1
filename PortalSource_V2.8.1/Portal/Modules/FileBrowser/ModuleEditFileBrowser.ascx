<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Register Assembly="Portal.API" Namespace="Portal.API.Controls" TagPrefix="Portal" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModuleEditFileBrowser.ascx.cs" Inherits="Portal.Modules.FileBrowser.ModuleEditFileBrowser" %>
<DIV id="DIV1" runat="server"><portal:linkbutton id="SaveLB" runat="server" EnableViewState="False" LanguageRef="Save" onclick="SaveLB_Click" />&nbsp;|
  <portal:linkbutton id="CancelLB" runat="server" EnableViewState="False" LanguageRef="Cancel" /></DIV>
<table border="0">
  <tr>
    <td>
      <Portal:Label ID="RootDirLbl" runat="server" Text="Virtuelles Root Verzeichnis" LanguageRef="VirtualRootDirectory"></Portal:Label>
    </td>
    <td>
      <asp:TextBox ID="VirtualRootEdit" runat="server"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td>
      <Portal:Label ID="SortPropertyLbl" runat="server" Text="Sortierkriterium" LanguageRef="SortBy"></Portal:Label>
    </td>
    <td>
      <asp:DropDownList ID="SortPropertyCombo" runat="server">
      </asp:DropDownList>
      <Portal:CheckBox ID="SortDirCheck" runat="server" Text="aufsteigend" LanguageRef="Ascending" />
    </td>
  </tr>
</table>
