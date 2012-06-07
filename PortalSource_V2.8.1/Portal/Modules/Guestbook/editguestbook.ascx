<%@ Control Language="c#" Inherits="Portal.Modules.Guestbook.EditGuestbook" CodeFile="EditGuestbook.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<P>
  <portal:LinkButton id="m_SpeichernBtn" runat="server" LanguageRef="Save" onclick="m_SpeichernBtn_Click" />&nbsp;
  <portal:LinkButton id="m_CancelBtn" runat="server" LanguageRef="Cancel" onclick="m_CancelBtn_Click" /><BR>
</P>
<table>
  <tr>
    <td colspan="3"><b><portal:Label runat="server" LanguageRef="AdditionalUserInfos" id="Label1" /></b></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="Email" id="Label2" /></td>
    <td style="WIDTH: 180px"><asp:CheckBox id="m_EmailCB" runat="server"></asp:CheckBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="Url" id="Label3" /></td>
    <td style="WIDTH: 180px"><asp:CheckBox id="m_UrlCB" runat="server"></asp:CheckBox></td>
  </tr>
  <tr>
    <td colspan="3">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="3"><b><portal:Label runat="server" LanguageRef="NewMessageNotification" id="Label4" /></b></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="EmailNotification" id="Label5" /></td>
    <td style="WIDTH: 180px"><asp:CheckBox id="m_EmailNotificationCB" runat="server"></asp:CheckBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="EmailFrom" id="Label6" /></td>
    <td style="WIDTH: 180px"><asp:TextBox id="m_EmailFromTB" runat="server" Width="100%"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="EmailTo" id="Label7" /></td>
    <td style="WIDTH: 180px"><asp:TextBox id="m_EmailToTB" runat="server" Width="100%"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="Subject" id="Label8" /></td>
    <td style="WIDTH: 180px"><asp:TextBox id="m_EmailSubjectTB" runat="server" Width="100%"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="EmailServer" id="Label9" /></td>
    <td style="WIDTH: 180px"><asp:TextBox id="m_EmailServerTB" runat="server" Width="100%"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="AuthentificationRequired" id="Label10" /></td>
    <td style="WIDTH: 180px"><asp:CheckBox id="m_EmailAuthentification" runat="server"></asp:CheckBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="Username" id="Label11" /></td>
    <td style="WIDTH: 180px"><asp:TextBox id="m_EmailUserName" runat="server" Width="100%"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 20px">&nbsp;</td>
    <td style="WIDTH: 220px"><portal:Label runat="server" LanguageRef="Password" id="Label12" /></td>
    <td style="WIDTH: 180px"><asp:TextBox id="m_EmailPassword" runat="server" Width="100%" TextMode="Password"></asp:TextBox></td>
  </tr>
</table>
