<%@ Control Language="c#" Inherits="HtmlEdit.EditHtml" CodeFile="EditHtmlEdit.ascx.cs" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<portal:linkbutton id="_SaveBack" runat="server" languageref="SaveBack" onclick="OnSaveBack" CausesValidation="False">Speichern und zur&uuml;ck</portal:linkbutton>
&nbsp;
<portal:LinkButton ID="_SaveBtn" runat="server" LanguageRef="Save" OnClick="OnSave" CausesValidation="False">Speichern</portal:LinkButton>
&nbsp;
<portal:linkbutton id="_CancelBtn" runat="server" languageref="Cancel" onclick="OnCancel" CausesValidation="False">Abbrechen</portal:linkbutton>
<br /><br />

<div style="WIDTH: 100%;HEIGHT: 600px" align="center">
<portal:message id="msg" runat="server"></portal:message>
<FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" EnableViewState="False" Height="590px" Width="100%" ToolbarSet="InplaceNoForm" />
</div>
