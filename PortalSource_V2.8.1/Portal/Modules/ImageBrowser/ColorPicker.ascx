<%@ Control Language="c#" AutoEventWireup="false" CodeFile="ColorPicker.ascx.cs" Inherits="ImageBrowser.ColorPicker" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:TextBox ID="TextBoxColor" runat="server" />&nbsp;
<a id="LinkPick" href="" runat="server">Pick</a>&nbsp;
<asp:CustomValidator ID="RGBValidator" runat="server" 
  ErrorMessage="CustomValidator" ControlToValidate="TextBoxColor" 
  onservervalidate="OnServerValidate" SetFocusOnError="True">Value must be like #AABBCC</asp:CustomValidator>
