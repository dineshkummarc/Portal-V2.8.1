<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.Forum.ForumView" CodeFile="ForumView.ascx.cs" %>
<div id="m_ToolbarDiv" runat="server">
  <portal:linkbutton id="m_NewThreadLnk" runat="server" CssClass="LinkButton" LanguageRef="NewThread" onclick="NewThread_Click" /></div>
<div>
  <asp:datagrid id="m_ForumTable" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True">
    <AlternatingItemStyle CssClass="AlternatingListLine"></AlternatingItemStyle>
    <ItemStyle CssClass="ListLine" HorizontalAlign="Left"></ItemStyle>
    <HeaderStyle CssClass="ListHeader"></HeaderStyle>
    <Columns>
      <portal:TemplateColumn LanguageRef-HeaderText="Thread" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
          <table border="0">
            <tr>
              <td width="15px">
                <asp:Image id="ThreadImage" runat="server" ImageUrl="images/thread.gif"></asp:Image>
              </td>
              <td>
                <asp:LinkButton id="ThreadLnk" Runat="server" CommandName="ShowThread" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ThreadFile")%>' CssClass="LinkButton">
                  <%#HttpUtility.HtmlDecode((string)DataBinder.Eval(Container.DataItem, "Title"))%>
                </asp:LinkButton>
                <asp:Image id="NewImage" runat="server" ImageUrl="images/new.gif" visible='<%#IsThreadNew(DataBinder.Eval(Container.DataItem, "dateTime"))%>'>
                </asp:Image>
              </td>
            </tr>
          </table>
        </ItemTemplate>
      </portal:TemplateColumn>
      <portal:TemplateColumn LanguageRef-HeaderText="OpenedBy" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Center">
        <ItemStyle Wrap="False" Width="1%" HorizontalAlign="Center"></ItemStyle>
        <ItemTemplate>
          <asp:Label id="AuthorLbl" Runat="server">
            <%#GetUserName(DataBinder.Eval(Container.DataItem, "Author"), DataBinder.Eval(Container.DataItem, "UserId"))%>
          </asp:Label>
        </ItemTemplate>
      </portal:TemplateColumn>
      <portal:TemplateColumn LanguageRef-HeaderText="Answer" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Center">
        <ItemStyle Wrap="False" Width="1%" HorizontalAlign="Center"></ItemStyle>
        <ItemTemplate>
          <asp:Label id="CommentCountLbl" Runat="server">
            <%#DataBinder.Eval(Container.DataItem, "CommentCount")%>
          </asp:Label>
        </ItemTemplate>
      </portal:TemplateColumn>
      <portal:TemplateColumn LanguageRef-HeaderText="LastArticle" HeaderStyle-Wrap="False" HeaderStyle-HorizontalAlign="Center">
        <ItemStyle Wrap="False" Width="1%" HorizontalAlign="Center"></ItemStyle>
        <ItemTemplate>
          <asp:Label id="LastArticleLbl" Runat="server">
            <%#DataBinder.Eval(Container.DataItem, "DateTime")%>
          </asp:Label>
          <br>
          <asp:Label id="LastPosterLbl" Runat="server">
            <%#GetLastPosterName(DataBinder.Eval(Container.DataItem, "LastPosterName"), DataBinder.Eval(Container.DataItem, "LastPosterId"))%>
          </asp:Label>
          <asp:Image id="LastPostImage" runat="server" ImageUrl="images/last_post.gif" Visible='<%#(int)DataBinder.Eval(Container.DataItem, "CommentCount") > 0%>'>
          </asp:Image>
        </ItemTemplate>
      </portal:TemplateColumn>
    </Columns>
    <PagerStyle Mode="NumericPages"></PagerStyle>
  </asp:datagrid>
  <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
      <td align="right">&nbsp;<portal:Label runat="server" LanguageRef="Search" id="Label1" />
        <asp:TextBox id="m_SearchTB" runat="server" ontextchanged="m_SearchTB_TextChanged"></asp:TextBox>
      </td>
    </tr>
  </table>
</div>
