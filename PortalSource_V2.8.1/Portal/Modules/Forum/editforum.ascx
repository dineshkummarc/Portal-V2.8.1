<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.Forum.EditForum" enableViewState="True" CodeFile="EditForum.ascx.cs" %>
<DIV runat="server"><portal:linkbutton id="SaveLB" runat="server" EnableViewState="False" LanguageRef="Save" onclick="SaveLB_Click" />&nbsp;|
  <portal:linkbutton id="CancelLB" runat="server" EnableViewState="False" LanguageRef="Cancel" /></DIV>
<DIV runat="server">&nbsp;</DIV>
<DIV runat="server">
  <table>
    <tr>
      <td noWrap><portal:Label runat="server" LanguageRef="DefineThreadCreationRight" id="DefineThreadCreationRightLbl" /></td>
      <td><asp:dropdownlist id="m_ThreadCreationRightCombo" runat="server" Width="128px">
          <asp:ListItem Value="0" Selected="True">Everyone</asp:ListItem>
          <asp:ListItem Value="1">User</asp:ListItem>
          <asp:ListItem Value="2">Module Admin</asp:ListItem>
        </asp:dropdownlist></td>
    </tr>
    <tr>
      <td noWrap><portal:Label runat="server" LanguageRef="UseHTMLEditorOnTopLevel" id="UseHTMLEditorOnTopLevelLbl" /></td>
      <td><asp:checkbox id="m_TopLevelCheck" runat="server" Enabled="False"></asp:checkbox></td>
    </tr>
    <tr>
      <td noWrap><portal:Label runat="server" LanguageRef="UseHTMLEditorOnOtherLevels" id="UseHTMLEditorOnOtherLevelsLbl" /></td>
      <td><asp:checkbox id="m_LowerLevelCheck" runat="server" Enabled="False"></asp:checkbox></td>
    </tr>
  </table>
</DIV>
