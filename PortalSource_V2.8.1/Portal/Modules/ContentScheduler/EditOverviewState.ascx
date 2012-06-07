<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditOverviewState.ascx.cs" Inherits="Portal.Modules.ContentScheduler.EventOverviewState" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>

<portal:LinkButton ID="_backBtn" runat="server" CausesValidation="False" LanguageRef="Back"
  OnClick="OnBack">Zurück</portal:LinkButton>&nbsp; |&nbsp;
<portal:LinkButton ID="_addEvent" runat="server" LanguageRef="AddEvent" OnClick="OnAddEvent" CausesValidation="False">Ereignis hinzufügen</portal:LinkButton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_cleanUpBtn" runat="server" LanguageRef="CleanUp" OnClick="OnCleanUp" CausesValidation="False">Alte Ereignisse entfernen</portal:LinkButton><br />
<hr />
<asp:GridView ID="_contentEventList" runat="server" AutoGenerateColumns="False" ShowHeader="False">
  <Columns>
    <asp:TemplateField>
        <ItemTemplate>
          <portal:ImageButton ID="_editEvent" runat="server" ImageUrl="~/PortalImages/edit.gif"
            OnCommand="OnEditEvent" CommandArgument='<%# Bind("Id") %>' AlternateTextLanguageRef="EditEvent" CausesValidation="False"/>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField>
        <ItemTemplate>
          <portal:ImageButton ID="_previewEvent" runat="server" ImageUrl="~/PortalImages/preview.gif"
            OnCommand="OnPreviewEvent" CommandArgument='<%# Bind("Id")%>' AlternateTextLanguageRef="EventPreviewTooltip" CausesValidation="False"/>
        </ItemTemplate>
    </asp:TemplateField>
      
    <asp:BoundField DataField="ActivationDate" ShowHeader="False" HtmlEncode="False"/>
  
    <asp:TemplateField>
        <ItemTemplate>
          <portal:LinkButton ID="_editPage" runat="server" OnCommand="OnEditPage" CommandArgument='<%# Bind("Id")%>' Text='<%# Bind("Hint")%>' TooltipLanguageRef="EditPage"></portal:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
  </Columns>
</asp:GridView>
