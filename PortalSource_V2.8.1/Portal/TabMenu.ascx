<%@ Control Language="c#" Inherits="Portal.TabMenu" enableViewState="False" CodeFile="TabMenu.ascx.cs" %>
<div class="TabMenu_Top" id="TabMenu_Top" runat="server">
  <asp:Repeater ID="Tabs" runat="server">
    <ItemTemplate>
      <span class="<%# ((bool)DataBinder.Eval(Container.DataItem, "CurrentTab"))?"TabMenu_CurrentTab":"TabMenu_Tab" %>">
        <asp:HyperLink ID="TabItem" runat="server" CssClass='<%# ((bool)DataBinder.Eval(Container.DataItem, "CurrentTab"))?"TabMenu_CurrentLink":"TabMenu_Link" %>'
          NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "URL")%>' Text='<%# ((string)DataBinder.Eval(Container.DataItem, "Text")).Replace(" ", "&nbsp;") %>'
          ImageUrl='<%# ((bool)DataBinder.Eval(Container.DataItem, "CurrentTab"))? DataBinder.Eval(Container.DataItem, "ImgPathActive") : DataBinder.Eval(Container.DataItem, "ImgPathInactive")%>'>
					<%# ((string)DataBinder.Eval(Container.DataItem, "Text")).Replace(" ", "&nbsp;") %>
        </asp:HyperLink></span>
        <img id="TabMenuDelimiterHor" src="PortalImages/blank.gif" alt="" />
    </ItemTemplate>
  </asp:Repeater>
</div>
<div class="TabMenu_SubTab" id="TabMenu_SubTab" runat="server">
  <asp:Repeater ID="SubTabs" runat="server">
    <ItemTemplate>
      <span class="<%# ((bool)DataBinder.Eval(Container.DataItem, "CurrentTab"))?"TabMenu_CurrentTab":"TabMenu_Tab" %>">
        <asp:HyperLink ID="TabItem" runat="server" CssClass='<%# ((bool)DataBinder.Eval(Container.DataItem, "CurrentTab"))?"TabMenu_CurrentLink":"TabMenu_Link" %>'
          NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "URL")%>' Text='<%# ((string)DataBinder.Eval(Container.DataItem, "Text")).Replace(" ", "&nbsp;") %>'
          ImageUrl='<%# ((bool)DataBinder.Eval(Container.DataItem, "CurrentTab"))? DataBinder.Eval(Container.DataItem, "ImgPathActive") : DataBinder.Eval(Container.DataItem, "ImgPathInactive")%>'>
					<%# ((string)DataBinder.Eval(Container.DataItem, "Text")).Replace(" ", "&nbsp;") %>
        </asp:HyperLink>
      </span>
    </ItemTemplate>
  </asp:Repeater>
</div>
