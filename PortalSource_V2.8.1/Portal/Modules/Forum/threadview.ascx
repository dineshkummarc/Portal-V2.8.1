<%@ Reference Control="~/modules/forum/forum.ascx" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.Forum.ThreadView" CodeFile="ThreadView.ascx.cs" %>
<div id="m_Toolbar" runat="server">
  <portal:LinkButton id="BackLB" runat="server" CssClass="LinkButton" LanguageRef="Back" onclick="BackLB_Click" /><BR>
  <asp:DataGrid id="m_ThreadTable" runat="server" Width="100%" AutoGenerateColumns="False">
    <AlternatingItemStyle CssClass="AlternatingListLine"></AlternatingItemStyle>
    <ItemStyle CssClass="ListLine"></ItemStyle>
    <HeaderStyle CssClass="ListHeader"></HeaderStyle>
    <Columns>
      <portal:TemplateColumn LanguageRef-HeaderText="Author">
        <ItemStyle Wrap="False" Width="1%" VerticalAlign="Top"></ItemStyle>
        <ItemTemplate>
          <a runat="server" name='<%#DataBinder.Eval(Container.DataItem, "Id")%>'/>
          <asp:Label Runat="server" Font-Bold="True" ID="Label1">
            <%#GetUserName(DataBinder.Eval(Container.DataItem, "Author"), DataBinder.Eval(Container.DataItem, "UserId"))%>
          </asp:Label>
        </ItemTemplate>
      </portal:TemplateColumn>
      <portal:TemplateColumn LanguageRef-HeaderText="Articles">
        <ItemStyle VerticalAlign="Top"></ItemStyle>
        <ItemTemplate>
          <table border="0" width="100%" cellspacing="0" cellpadding="0">
            <tr>
              <td align="right" width='<%#GetColumnWidth(DataBinder.Eval(Container.DataItem, "Depth"))%>'>
                <asp:Image id="MessageImg" runat="server" ImageUrl="images/message.gif"></asp:Image>
              </td>
              <td>
                <asp:label id="TitleLbl" Runat="server" Font-Bold="True">
                  <%#DataBinder.Eval(Container.DataItem, "Title")%>
                </asp:label>
                <asp:Image id="NewImage" runat="server" ImageUrl="images/new.gif" visible='<%#IsThreadNew(DataBinder.Eval(Container.DataItem, "dateTime"))%>'>
                </asp:Image>
              </td>
              <td align="right">
                <asp:label id="DateTimeLbl" Runat="server">
                  <%#DataBinder.Eval(Container.DataItem, "DateTime")%>
                </asp:label>
              </td>
            </tr>
            <tr>
              <td></td>
              <td colspan="2">
                <asp:label id="TextLbl" Runat="server">
                  <%#GetArticleText(DataBinder.Eval(Container.DataItem, "Text"))%>
                </asp:label>
              </td>
            </tr>
            <tr>
              <td></td>
              <td colspan="2">
                <portal:linkbutton id="AnswerLB" Runat="server" CommandName="Answer" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Id")%>' CssClass="LinkButton" LanguageRef="Answer" />
                <asp:label id="SpacerLbl" Runat="server" Visible='<%#configAgent.Module.ModuleHasEditRights%>'> | </asp:label>
                <portal:linkbutton id="DeleteLB" Runat="server" CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Id")%>' Visible='<%#configAgent.Module.ModuleHasEditRights%>' CssClass="LinkButton" LanguageRef="Remove" />
              </td>
            </tr>
          </table>
        </ItemTemplate>
      </portal:TemplateColumn>
    </Columns>
  </asp:DataGrid>
</div>
