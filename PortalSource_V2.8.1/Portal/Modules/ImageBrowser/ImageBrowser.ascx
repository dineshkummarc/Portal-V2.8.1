<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" AutoEventWireup="false" CodeFile="ImageBrowser.ascx.cs" Inherits="ImageBrowser.Controls.ImageBrowser" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" enableViewState="True"%>
<asp:panel id="ImageBrowserPanel" runat="server" Width="100%">
  <asp:PlaceHolder id="Path" runat="server"></asp:PlaceHolder>
  <BR>
  <DIV id="DirEditOps" align="left" runat="server">
    <TABLE cellSpacing="1" cellPadding="1" border="0">
      <TR>
        <TD style="WIDTH: 116px">
          <asp:Label id="lblDirCaption" runat="server">Caption</asp:Label></TD>
        <TD>
          <asp:TextBox id="txtDirCaption" Width="500px" runat="server"></asp:TextBox></TD>
        <TD><INPUT id="btnDirSDInsTT" style="WIDTH: 60px; HEIGHT: 24px" type="button" value="Tooltip"
            name="btnDirSDInsTT" runat="server">&nbsp;&nbsp; <INPUT id="btnDirSDInsD" style="WIDTH: 60px" type="button" value="Desc" name="btnDirSDInsD"
            runat="server"></TD>
      </TR>
      <TR>
        <TD>
          <asp:Label id="lblDirTooltip" runat="server">Tooltip</asp:Label></TD>
        <TD>
          <asp:TextBox id="txtDirTooltip" Width="500px" runat="server"></asp:TextBox></TD>
        <TD><INPUT id="btnDirTTInsSD" style="WIDTH: 60px; HEIGHT: 24px" type="button" value="Caption"
            name="btnDirTTInsSD" runat="server">&nbsp;&nbsp; <INPUT id="btnDirTTInsD" style="WIDTH: 60px" type="button" value="Desc" name="btnDirTTInsD"
            runat="server"></TD>
      </TR>
      <TR>
        <TD>Description</TD>
        <TD>
          <asp:TextBox id="txtDirText" Width="500px" Runat="server" Height="50px" TextMode="MultiLine"></asp:TextBox></TD>
        <TD vAlign="top"><INPUT id="btnDirDInsTT" style="WIDTH: 60px" type="button" value="Tooltip" name="btnDirDInsTT"
            runat="server">&nbsp;&nbsp; <INPUT id="btnDirDInsDS" style="WIDTH: 60px" type="button" value="Caption" name="btnDirDInsDS"
            runat="server"></TD>
      </TR>
    </TABLE>
    <asp:HyperLink id="lnkSave" runat="server" Visible="False">Save</asp:HyperLink>&nbsp;&nbsp;&nbsp;
    <asp:HyperLink id="lnkUpdatePictures" runat="server" Visible="False">Update Pictures</asp:HyperLink></DIV>
  <asp:PlaceHolder id="directoryContent" runat="server"></asp:PlaceHolder>
  <P align="right">
    <asp:PlaceHolder id="pageNavigation" runat="server"></asp:PlaceHolder></P>
</asp:panel><asp:panel id="ImageViewPanel" Runat="server"><BR>
  <DIV align="center">
    <asp:HyperLink id="lnkMovePrevious" runat="server" ImageUrl="NavPrevious.gif"></asp:HyperLink>&nbsp;&nbsp;&nbsp; 
    &nbsp;
    <asp:HyperLink id="lnkBack" runat="server" ImageUrl="NavBack.gif"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp; 
    &nbsp;
    <asp:HyperLink id="lnkMoveNext" runat="server" ImageUrl="NavNext.gif"></asp:HyperLink></DIV>
  <DIV align="center">&nbsp;</DIV>
  <DIV align="center">
    <asp:PlaceHolder id="imagePosition" runat="server"></asp:PlaceHolder></DIV>
  <DIV align="center"><BR>
    <asp:Label id="lbImageText" Runat="server"></asp:Label></DIV>
  <DIV id="PhotoEditOpts" align="left" runat="server">
    <TABLE cellSpacing="1" cellPadding="1" border="0">
      <TR>
        <TD style="WIDTH: 116px">
          <asp:Label id="lblCaption" runat="server">Caption</asp:Label></TD>
        <TD>
          <asp:TextBox id="txtImageCaption" Width="500px" runat="server"></asp:TextBox></TD>
        <TD><INPUT id="btnSDInsTT" style="WIDTH: 60px; HEIGHT: 24px" type="button" value="Tooltip"
            runat="server">&nbsp;&nbsp; <INPUT id="btnSDInsD" style="WIDTH: 60px" type="button" value="Desc" runat="server"></TD>
      </TR>
      <TR>
        <TD>
          <asp:Label id="lblTooltip" runat="server">Tooltip</asp:Label></TD>
        <TD>
          <asp:TextBox id="txtImageTooltip" Width="500px" runat="server"></asp:TextBox></TD>
        <TD><INPUT id="btnTTInsSD" style="WIDTH: 60px; HEIGHT: 24px" type="button" value="Caption"
            runat="server">&nbsp;&nbsp; <INPUT id="btnTTInsD" style="WIDTH: 60px" type="button" value="Desc" runat="server"></TD>
      </TR>
      <TR>
        <TD>Description</TD>
        <TD>
          <asp:TextBox id="txtImageText" Width="500px" Runat="server" Height="50px" TextMode="MultiLine"></asp:TextBox></TD>
        <TD vAlign="top"><INPUT id="btnDInsTT" style="WIDTH: 60px" type="button" value="Tooltip" runat="server">&nbsp;&nbsp;
          <INPUT id="btnDInsDS" style="WIDTH: 60px" type="button" value="Caption" runat="server"></TD>
      </TR>
    </TABLE>
    <asp:HyperLink id="lnkSaveImageText" Runat="server">Save</asp:HyperLink>&nbsp;&nbsp;
    <asp:HyperLink id="lnkSetFolderImage" Runat="server">Set as Folder Image</asp:HyperLink>
    <DIV style="WIDTH: 100%; BORDER-BOTTOM: black 1px solid"></DIV>
  </DIV>
</asp:panel>
