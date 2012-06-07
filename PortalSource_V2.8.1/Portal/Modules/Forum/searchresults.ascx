<%@ Control Language="c#" Inherits="Portal.Modules.Forum.SearchResultsControl" CodeFile="SearchResults.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<div id="m_ToolbarDiv" runat="server">
  <portal:LinkButton id="m_CancelBtn" runat="server" LanguageRef="Back" onclick="m_CancelBtn_Click" />
</div>
<asp:DataGrid id="m_SearchResultsTable" runat="server" Width="100%" AutoGenerateColumns="False">
  <AlternatingItemStyle CssClass="AlternatingListLine"></AlternatingItemStyle>
  <ItemStyle CssClass="ListLine" HorizontalAlign="Left"></ItemStyle>
  <HeaderStyle CssClass="ListHeader"></HeaderStyle>
  <Columns>
    <portal:TemplateColumn LanguageRef-HeaderText="SearchResults" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Left">
      <ItemTemplate>
        <table border="0">
          <tr>
            <td width="15px">
              <asp:Image id="ThreadImage" runat="server" ImageUrl="images/thread.gif"></asp:Image>
            </td>
            <td>
              <asp:LinkButton id="ThreadLnk" Runat="server" CommandName="ShowThread" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ThreadFile") + ":" + DataBinder.Eval(Container.DataItem, "Id")%>' CssClass="LinkButton">
                <%#HttpUtility.HtmlDecode((string)DataBinder.Eval(Container.DataItem, "Title"))%>
              </asp:LinkButton>
              <asp:Image id="NewImage" runat="server" ImageUrl="images/new.gif" visible='<%#IsThreadNew(DataBinder.Eval(Container.DataItem, "dateTime"))%>'>
              </asp:Image>
            </td>
          </tr>
        </table>
      </ItemTemplate>
    </portal:TemplateColumn>
    <portal:TemplateColumn LanguageRef-HeaderText="Author" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Left">
      <ItemTemplate>
        <asp:Label id="AuthorLbl" Runat="server">
          <%#GetUserName(DataBinder.Eval(Container.DataItem, "Author"), DataBinder.Eval(Container.DataItem, "UserId"))%>
        </asp:Label>
      </ItemTemplate>
    </portal:TemplateColumn>
    <portal:TemplateColumn LanguageRef-HeaderText="Date" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Left">
      <ItemTemplate>
        <asp:Label id="DateLbl" Runat="server">
          <%#DataBinder.Eval(Container.DataItem, "dateTime")%>
        </asp:Label>
      </ItemTemplate>
    </portal:TemplateColumn>
  </Columns>
</asp:DataGrid>
<asp:Label id="m_StateLbl" runat="server"></asp:Label>
