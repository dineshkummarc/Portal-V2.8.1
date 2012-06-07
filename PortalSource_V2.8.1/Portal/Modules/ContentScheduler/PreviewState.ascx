<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PreviewState.ascx.cs" Inherits="Portal.Modules.ContentScheduler.PreviewState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>

<asp:Literal ID="_pageContent" runat="server" EnableViewState="False"></asp:Literal>
<p>
<portal:LinkButton ID="_backBtn" runat="server" LanguageRef="Back" OnClick="OnBack" >Zurück</portal:LinkButton>&nbsp;
  |&nbsp;<portal:LinkButton ID="_previousEvent" runat="server" LanguageRef="PreviousEvent"
    OnClick="OnPreviousEvent_Click" TooltipLanguageRef="PreviousEventTooltip"><< Vorhergehendes Ereignis</portal:LinkButton>&nbsp;
  |&nbsp;
  <portal:LinkButton ID="_currentInfo" runat="server" Text="--" TooltipLanguageRef="EditPage" OnClick="OnEditPage_Click"></portal:LinkButton >&nbsp; |&nbsp;
  <portal:LinkButton ID="_nextEvent" runat="server" LanguageRef="NextEvent" OnClick="OnNextEvent_Click"
    TooltipLanguageRef="TextEventTooltip">Nächstes Event >></portal:LinkButton></p>