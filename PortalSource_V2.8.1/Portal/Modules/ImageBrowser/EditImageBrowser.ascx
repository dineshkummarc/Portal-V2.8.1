<%@ Control Language="c#" AutoEventWireup="false" CodeFile="EditImageBrowser.ascx.cs" Inherits="ImageBrowser.Controls.EditImageBrowser" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register tagprefix="ImageBrowser" tagname="ColorPicker" Src="ColorPicker.ascx" %>
<asp:linkbutton id="lnkSave" Runat="server">[Save]</asp:linkbutton><asp:linkbutton id="lnkSaveAndUpdate" Runat="server"> [Start Update cache]</asp:linkbutton><asp:linkbutton id="lnkCancel" runat="server">[Cancel]</asp:linkbutton>
<table width="100%">
  <colgroup>
    <col class="Label" width="100">
    <col class="Data">
  </colgroup>
  <tr>
    <td noWrap width="250">Picture&nbsp;VirtualDirectory</td>
    <td><asp:textbox id="txtPictureVirtualDirectory" Runat="server" Width="100%"></asp:textbox></td>
  </tr>
  <tr>
    <td noWrap width="250">Preview Maximum Size</td>
    <td><asp:textbox id="txtPreviewMax" runat="server"></asp:textbox>&nbsp;<asp:rangevalidator id="RangeValidatorP" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtPreviewMax"
        MinimumValue="50" MaximumValue="4096" Type="Integer">Invalide Preview maximum size</asp:rangevalidator></td>
  </tr>
  <tr>
    <td noWrap width="250">JPEG Quality (0..100)</td>
    <td><asp:textbox id="txtPreviewJpegQuality" runat="server"></asp:textbox>
      <asp:rangevalidator id="Rangevalidator6" runat="server" 
        ErrorMessage="RangeValidator" ControlToValidate="txtPreviewJpegQuality"
        MinimumValue="0" MaximumValue="100" Type="Double">Only range 0..100 ist valid</asp:rangevalidator></td>
  </tr>
</table>
<P>&nbsp;</P>
<table width="100%">
  <tr>
    <td noWrap width="250">Thumbnail Maximum Size</td>
    <td><asp:textbox id="txtThumbnailMax" runat="server"></asp:textbox>&nbsp;<asp:rangevalidator id="RangeValidatorTn" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtThumbnailMax"
        MinimumValue="10" MaximumValue="1000" Type="Integer">Invalid Thumbnail Maximal Size</asp:rangevalidator></td>
  </tr>
  <tr>
    <td noWrap width="250">Thumbnail Columns</td>
    <td><asp:textbox id="txtThumbnailCols" runat="server"></asp:textbox>&nbsp;<asp:rangevalidator id="Rangevalidator1" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtThumbnailCols"
        MinimumValue="1" MaximumValue="4096" Type="Integer">Invalid number of Thumbnail Columns</asp:rangevalidator></td>
  </tr>
  <tr>
    <td noWrap width="250">Thumbnail Rows per Page</td>
    <td><asp:textbox id="txtThumbnailRows" runat="server" Rows="1"></asp:textbox>&nbsp;<asp:rangevalidator id="Rangevalidator4" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtThumbnailRows"
        MinimumValue="0" MaximumValue="4096" Type="Integer">Invalid number of Thumbnail Rows</asp:rangevalidator></td>
  </tr>
  <tr>
    <td noWrap width="250">JPEG Quality (0..100)</td>
    <td><asp:textbox id="txtThumbJpegQuality" runat="server"></asp:textbox>
      <asp:rangevalidator id="Rangevalidator7" runat="server" 
        ErrorMessage="RangeValidator" ControlToValidate="txtThumbJpegQuality"
        MinimumValue="0" MaximumValue="100" Type="Double">Only range 0..100 ist valid</asp:rangevalidator></td>
  </tr>
</table>
<p></p>
<P><asp:checkbox id="chkPreviewShadow" runat="server" Text="Previews with shadow" AutoPostBack="True"></asp:checkbox></P>
<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
  <TR>
    <TD width="50"></TD>
    <TD width="200">Shadow Width [pixel]</TD>
    <TD><asp:textbox id="txtPvShadowWidth" runat="server"></asp:textbox>&nbsp;
      <asp:rangevalidator id="Rangevalidator3" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtPvShadowWidth"
        MinimumValue="0" MaximumValue="1024" Type="Integer">Invalid Shadow Width</asp:rangevalidator></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Shadow Transparency (0..100)</TD>
    <TD><asp:textbox id="txtPvShadowTrans" runat="server"></asp:textbox>&nbsp;
      <asp:rangevalidator id="Rangevalidator5" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtPvShadowTrans"
        MinimumValue="0" MaximumValue="100" Type="Double">Only range 0..100 ist valid</asp:rangevalidator></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Background Color</TD>
    <TD><IMAGEBROWSER:COLORPICKER id="PvBgColor" runat="server" DESIGNTIMEDRAGDROP="686"></IMAGEBROWSER:COLORPICKER></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Shadow Color</TD>
    <TD><IMAGEBROWSER:COLORPICKER id="PvShColor" runat="server" DESIGNTIMEDRAGDROP="716"></IMAGEBROWSER:COLORPICKER></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Border Color</TD>
    <TD><IMAGEBROWSER:COLORPICKER id="PvBColor" runat="server"></IMAGEBROWSER:COLORPICKER></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Soft Shadow</TD>
    <TD><asp:checkbox id="chkPvSoftShadow" runat="server"></asp:checkbox></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200"></TD>
    <TD></TD>
  </TR>
</TABLE>
<p></p>
<P><asp:checkbox id="chkThumbnailShadow" runat="server" Text="Thumbnails with shadow" AutoPostBack="True"></asp:checkbox></P>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
  <TR>
    <TD width="50"></TD>
    <TD width="200">Shadow Width [pixel]</TD>
    <TD><asp:textbox id="txtTnShadowWidth" runat="server"></asp:textbox>&nbsp;
      <asp:rangevalidator id="ShadowWidthValidator" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtTnShadowWidth"
        MinimumValue="0" MaximumValue="1024" Type="Integer">Invalid Shadow Width</asp:rangevalidator></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Border Width [pixel]</TD>
    <TD><asp:textbox id="txtTnBorderWidth" runat="server"></asp:textbox>&nbsp;
      <asp:rangevalidator id="Rangevalidator2" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtTnBorderWidth"
        MinimumValue="0" MaximumValue="1024" Type="Integer">Invalid Border Width</asp:rangevalidator></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Shadow Transparency (0..100)</TD>
    <TD><asp:textbox id="txtTnShadowTrans" runat="server"></asp:textbox>&nbsp;
      <asp:rangevalidator id="TransparencyValidator" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtTnShadowTrans"
        MinimumValue="0" MaximumValue="100" Type="Double">Only range 0..100 ist valid</asp:rangevalidator></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Background Color</TD>
    <TD><IMAGEBROWSER:COLORPICKER id="TnBgColor" runat="server" DESIGNTIMEDRAGDROP="686"></IMAGEBROWSER:COLORPICKER></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Shadow Color</TD>
    <TD><IMAGEBROWSER:COLORPICKER id="TnShColor" runat="server" DESIGNTIMEDRAGDROP="716"></IMAGEBROWSER:COLORPICKER></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Border Color</TD>
    <TD><IMAGEBROWSER:COLORPICKER id="TnBColor" runat="server"></IMAGEBROWSER:COLORPICKER></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200">Soft Shadow</TD>
    <TD><asp:checkbox id="chkTnSoftShadow" runat="server"></asp:checkbox></TD>
  </TR>
  <TR>
    <TD width="50"></TD>
    <TD width="200"></TD>
    <TD></TD>
  </TR>
</TABLE>
<P>&nbsp;</P>
