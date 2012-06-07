<%@ Control Language="c#" Inherits="Portal.Modules.Newsticker.Newsticker" CodeFile="Newsticker.ascx.cs" %>

<asp:Repeater id="m_Repeater" runat="server" OnItemDataBound="OnItemDataBound">
  <ItemTemplate>
    <p runat="server" id="NewsData"></p>
  </ItemTemplate>
</asp:Repeater>
