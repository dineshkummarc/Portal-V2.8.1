<%@ Import namespace="Portal" %>
<%@ Import namespace="Portal.API" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.MyUser.MyUser" CodeFile="MyUser.ascx.cs" %>
<div>
  <portal:Message id="msg" runat="server" />
  <portal:LinkButton CssClass="LinkButton" ID="lnkSave" OnClick="OnSave" Runat="server" LanguageRef="Save"></portal:LinkButton>
</div>
<div class="Label" style="float:left;margin-top:4px">
  <portal:Label runat="server" LanguageRef="Account" ID="Label1"></portal:Label>
</div>
<div class="Data" style="margin-left:6em">
  <asp:TextBox Width="125px" ID="txtLogin" Runat="server" ReadOnly="False" Enabled="False"></asp:TextBox>
</div>
<div class="Label" style="float:left;margin-top:4px">
  <portal:Label runat="server" LanguageRef="Password" ID="Label2"></portal:Label>
</div>
<div class="Data" style="margin-left:6em">
  <asp:TextBox Width="125px" ID="txtPassword" Runat="server" TextMode="Password"></asp:TextBox>
</div>
<div class="Label" style="float:left;margin-top:4px">
</div>
<div class="Data" style="margin-left:6em">
  <asp:TextBox Width="125px" ID="txtPassword2" Runat="server" TextMode="Password"></asp:TextBox>
</div>
<div class="Label" style="float:left;margin-top:4px">
  <portal:Label runat="server" LanguageRef="FirstName" ID="Label3"></portal:Label>
</div>
<div class="Data" style="margin-left:6em">
  <asp:TextBox Width="125px" ID="txtFirstName" Runat="server"></asp:TextBox>
</div>
<div class="Label" style="float:left;margin-top:4px">
  <portal:Label runat="server" LanguageRef="SurName" ID="Label4"></portal:Label>
</div>
<div class="Data" style="margin-left:6em">
  <asp:TextBox Width="125px" ID="txtSurName" Runat="server"></asp:TextBox>
</div>
<div class="Label" style="float:left;margin-top:4px">
  <portal:Label runat="server" LanguageRef="EMail" ID="Label5"></portal:Label>
</div>
<div class="Data" style="margin-left:6em">
  <asp:TextBox Width="125px" ID="txtEMail" Runat="server"></asp:TextBox>
</div>
