<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditErrorState.ascx.cs" Inherits="Portal.Modules.FormBuilder.EditErrorState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<portal:linkbutton id="_SaveBack" runat="server" languageref="SaveBack" onclick="OnSaveBack" CausesValidation="False" EnableViewState="False">Speichern und zur&uuml;ck</portal:linkbutton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_SaveBtn" runat="server" LanguageRef="Save" OnClick="OnSave" CausesValidation="False" EnableViewState="False">Speichern</portal:LinkButton>&nbsp;
|&nbsp;
<portal:linkbutton id="_CancelBtn" runat="server" languageref="Cancel" onclick="OnCancel" CausesValidation="False" EnableViewState="False">Abbrechen</portal:linkbutton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_resetBtn" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="ResetToDefault" OnClick="OnResetToDefault">Auf Standard zurücksetzen</portal:LinkButton>
<hr />
<portal:Label ID="_commonErrorMsgLbl" runat="server" LanguageRef="CommonErrorMessage">Allgemeine Fehlermeldung:</portal:Label>
<asp:TextBox ID="_commonErrorMsg" runat="server" Width="30em"></asp:TextBox><br />
<asp:Label ID="_descriptionLbl" runat="server"></asp:Label><br />

<div style="WIDTH: 100%;HEIGHT: 600px" align="center">
<FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" EnableViewState="False" Height="590px" Width="100%" ToolbarSet="Inplace" />
</div>
<br />
<br />
