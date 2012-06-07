<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ViewFiles.ascx.cs" Inherits="Portal.Modules.FileBrowser.ViewFiles" %>
<%@ Register Assembly="Portal.API" Namespace="Portal.API.Controls" TagPrefix="Portal" %>
<div runat="server" id="currPathDiv">
  <Portal:label id="_currDirLbl" runat="server" languageref="CurrentDirectory">Aktuelles Verzeichnis</Portal:label>:
<asp:PlaceHolder ID="currPath" runat="server"></asp:PlaceHolder>
</div>
<div runat="server" id="pathNotFound" visible="false">
  <asp:Label ID="directoryCreateQuestion" runat="server" Text="Nicht gefunden - erstellen?"></asp:Label>
  <Portal:Button
    ID="createRootDir" runat="server" Text="Verzeichnis erstellen" OnClick="createRootDir_Click" LanguageRef="CreateDirectory" />
</div>
<asp:Panel ID="EditTools" runat="server" Width="100%" Visible="False">
  <Portal:LinkButton ID="editDirectoryBtn" runat="server" OnClick="editDirectoryBtn_Click" LanguageRef="EditDirectory">Verzeichnis bearbeiten</Portal:LinkButton> |
  <Portal:LinkButton ID="newDirectoryBtn" runat="server" OnClick="newDirectoryBtn_Click" LanguageRef="CreateSubDirectory">Unterverzeichnis erzeugen</Portal:LinkButton>
  |
  <Portal:LinkButton ID="addFileBtn" runat="server" OnClick="OnAddFiles" LanguageRef="AddFile">Datei hinzufügen</Portal:LinkButton><br />
</asp:Panel>
<asp:LinkButton ID="dirUpBtn" runat="server" CommandArgument="1" OnCommand="ChangeDirUp" Visible="False">[..]</asp:LinkButton><br />
<asp:Panel ID="directoryArea" runat="server" Width="100%">
  <asp:GridView ID="directoryList" runat="server" AutoGenerateColumns="False" GridLines="None"
    ShowHeader="False">
    <Columns>
      <asp:TemplateField HeaderText="Icon">
        <ItemTemplate>
          <asp:Image runat="server" ImageUrl="~/PortalImages/Extensions/folder.gif" />
        </ItemTemplate>
      </asp:TemplateField>
      <asp:TemplateField>
        <ItemTemplate>
          <asp:ImageButton ID="editSubDirectoryBtn" runat="server" ImageUrl="~/PortalImages/edit.gif"
            OnCommand="editSubDirectoryBtn_Click" CommandArgument='<%# Bind("VirtualPath")%>'/>
        </ItemTemplate>
      </asp:TemplateField>
      <asp:TemplateField HeaderText="Name">
        <ItemTemplate>
          <asp:LinkButton ID="dir" runat="server" Text='<%# Bind("PresentationName") %>' OnCommand="ChangeDir"
            CommandArgument='<%# Bind("VirtualPath")%>'></asp:LinkButton>
        </ItemTemplate>
      </asp:TemplateField>
      <asp:BoundField DataField="Description" HeaderText="Beschreibung" />
    </Columns>
  </asp:GridView>
  <br />
</asp:Panel>
<asp:GridView ID="fileList" runat="server" AutoGenerateColumns="False" ShowHeader="False"
  GridLines="None" CellPadding="0" CssClass="FileList">
  <Columns>
    <asp:TemplateField HeaderText="Icon">
      <ItemTemplate>
        <asp:Image runat="server" ImageUrl='<%# GetImageUrl(DataBinder.Eval(Container.DataItem, "Extension").ToString()) %>' />
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField>
      <ItemTemplate>
        <asp:ImageButton ID="editFileBtn" runat="server" ImageUrl="~/PortalImages/edit.gif" OnCommand="editFileBtn_Click"
          CommandArgument='<%# Bind("FileName")%>' />
      </ItemTemplate>
      <ControlStyle />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Name">
      <ItemTemplate>
        <asp:HyperLink ID="file" runat="server" Text='<% # Bind("FileName")%>' NavigateUrl='<%# GetDownloadUrl(DataBinder.Eval(Container.DataItem, "VirtualPath").ToString()) %>'></asp:HyperLink>
      </ItemTemplate>
      <ControlStyle />
    </asp:TemplateField>
    <asp:BoundField DataField="Description" HeaderText="Beschreibung" >
      <ControlStyle />
    </asp:BoundField>
    <asp:BoundField DataField="ModificationDate" HtmlEncode="False" >
      <ControlStyle />
    </asp:BoundField>
  </Columns>
</asp:GridView>
