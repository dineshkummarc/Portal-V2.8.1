<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditSuccessState.ascx.cs" Inherits="Portal.Modules.FormBuilder.EditSuccessState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<portal:linkbutton id="_SaveBack" runat="server" languageref="SaveBack" onclick="OnSaveBack" CausesValidation="False" EnableViewState="False">Speichern und zur&uuml;ck</portal:linkbutton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_SaveBtn" runat="server" LanguageRef="Save" OnClick="OnSave" CausesValidation="False" EnableViewState="False">Speichern</portal:LinkButton>&nbsp;
|&nbsp;
<portal:linkbutton id="_CancelBtn" runat="server" languageref="Cancel" onclick="OnCancel" CausesValidation="False" EnableViewState="False">Abbrechen</portal:linkbutton>
<hr />
<portal:CheckBox ID="_showSuccess" runat="server" EnableViewState="False" LanguageRef="ShowSuccess" /><br />

<div style="WIDTH: 100%;HEIGHT: 600px" align="center">
<FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" EnableViewState="False" Height="590px" Width="100%" ToolbarSet="Inplace" />
</div>