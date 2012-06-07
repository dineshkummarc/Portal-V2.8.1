<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Overview.ascx.cs" Inherits="Portal.Modules.StatisticViewer.Overview" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<b><portal:label runat="server" id="labelRequestTitle" languageref="overviewTitle"/><br />
</b>
<asp:Label ID="_errorMsg" runat="server" BackColor="Red" ForeColor="White" Visible="False"
  Width="100%"></asp:Label><br />
<table width="450">
    <tr class="ListHeader">
        <td colspan="5" align="center"><portal:label runat="server" id="labelMonthSummary" languageref="monthSummary"/></td>
    </tr>
    <tr class="ListHeader">
        <td rowspan="2"><portal:label runat="server" id="labelMonth" languageref="month"/></td>
        <td colspan="2" align="center"><portal:label runat="server" id="labelDayAverage" languageref="dayAverage"/></td>
        <td colspan="2" align="center"><portal:label runat="server" id="labelMonthSum" languageref="monthSum"/></td>
    </tr>
    <tr class="ListHeader">
        <td align="center"><portal:label runat="server" id="labelDayRequests" languageref="requests"/></td>
        <td align="center"><portal:label runat="server" id="labelDayVisits" languageref="visits"/></td>
        <td align="center"><portal:label runat="server" id="labelTotalRequests" languageref="requests"/></td>
        <td align="center"><portal:label runat="server" id="labelTotalVisits" languageref="visits"/></td>
    </tr>
    <asp:Repeater ID="repeaterOverview" runat="server" OnItemDataBound="OnRequestItemDataBound" EnableViewState="False">
            <ItemTemplate>
                <tr id="Row" runat="server" class="ListLine"></tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="Row" runat="server" class="AlternatingListLine"></tr>
            </AlternatingItemTemplate>
    </asp:Repeater>
</table>
<br />
<b><portal:label runat="server" id="labelLoginTitle" languageref="loginTitle"/></b>
<br />
<table width="450">
<asp:Repeater ID="repeaterLogin" runat="server" OnItemDataBound="OnLoginItemDataBound" EnableViewState="False">
        <HeaderTemplate>
			<tr id="Row" runat="server" class="ListHeader"></tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr id="Row" runat="server" class="ListLine"></tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr id="Row" runat="server" class="AlternatingListLine"></tr>
        </AlternatingItemTemplate>
</asp:Repeater>
</table>
