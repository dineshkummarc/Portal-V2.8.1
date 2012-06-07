<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonthlyRequestStatistic.ascx.cs" Inherits="Portal.Modules.StatisticViewer.MonthlyRequestStatistic" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<portal:LinkButton ID="linkButtonBack" runat="server" OnClick="linkButtonBack_Click" languageref="back"/>
<br />
<table width="450">
    <tr class="ListHeader">
        <td colspan="2" align="center"><asp:Label runat="server" id="labelMonthlySummary" /></td>
     </tr>
    <asp:Repeater ID="repeaterSummary" runat="server" OnItemDataBound="OnSummaryItemDataBound" EnableViewState="false">
        <ItemTemplate>
            <tr id="Row" runat="server" class="ListLine"></tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr id="Row" runat="server" class="AlternatingListLine"></tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
<br />
<table width="450">
    <tr class="ListHeader">
        <td colspan="4" align="center"><asp:Label runat="server" id="labelMonthlyRequests"></asp:Label></td>
    </tr>
    <tr class="ListHeader">
        <td align="center">#</td>
        <td colspan="2" align="center"><portal:label runat="server" id="labelRequests" languageref="requests"/></td>
        <td align="left"><portal:label runat="server" id="labelUrl" languageref="url"/></td>
    </tr>
    <asp:Repeater ID="repeaterTopUrls" runat="server" OnItemDataBound="OnTopUrlsItemDataBound" EnableViewState="False">
        <ItemTemplate>
            <tr id="Row" runat="server" class="ListLine"></tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr id="Row" runat="server" class="AlternatingListLine"></tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>

