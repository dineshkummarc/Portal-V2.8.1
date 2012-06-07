<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMainState.ascx.cs" Inherits="Portal.Modules.FormBuilder.EditMainState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>


<h1><portal:Label ID="_title" LanguageRef="ConfigurationMainTitle" runat="server" Text="Die Konfigurationsmöglichkeiten"></portal:Label></h1>
<li><portal:LinkButton ID="_editFormBtn" LanguageRef="EditForm" Text="Formular" runat="server" OnClick="EditFormBtn_Click"></portal:LinkButton></li>
<li><portal:LinkButton ID="_editResultSuccessBtn" LanguageRef="EditResultSuccess" Text="Erfolgsmeldung"  runat="server" OnClick="EditResultSuccessBtn_Click"></portal:LinkButton></li>
<li><portal:LinkButton ID="_editResultErrorBtn" LanguageRef="EditResultError" Text="Fehlermeldung"  runat="server" OnClick="EditResultErrorBtn_Click"></portal:LinkButton></li>
<li>
  <portal:LinkButton ID="LinkButton1" runat="server" LanguageRef="EditValidation" OnClick="EditValidation_Click"
    Text="Gültigkeitsprüfung der Daten"></portal:LinkButton></li>
<li><portal:LinkButton ID="_editEmailBtn" LanguageRef="EditEmail" Text="Benachrichtigungs E-Mail"  runat="server" OnClick="EditEmailBtn_Click"></portal:LinkButton></li>
<hr style="width: 20em; position: static; text-align: left" />
</li>
<li><portal:LinkButton ID="_backBtn" runat="server" CausesValidation="False" LanguageRef="Back" OnClick="OnBack">Zurück</portal:LinkButton></li>

