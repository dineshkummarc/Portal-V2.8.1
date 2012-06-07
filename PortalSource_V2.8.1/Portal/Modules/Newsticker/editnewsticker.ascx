<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.Newsticker.EditNewsticker" CodeFile="EditNewsticker.ascx.cs" %>
<div id="lbLinks" runat="server"><portal:LinkButton id="lnkAdd" runat="server" LanguageRef="Add" onclick="OnAdd" />&nbsp;|
	<portal:LinkButton id="lnkSave" runat="server" LanguageRef="Save" onclick="OnUpdate" />&nbsp;|
	<portal:LinkButton id="lnkCancel" runat="server" LanguageRef="Cancel" onclick="OnCancel" /></div>
<br/>
<asp:DataGrid id="grid" runat="server" Width="100%" AutoGenerateColumns="False">
	<HeaderStyle CssClass="ListHeader"></HeaderStyle>
	<ItemStyle CssClass="ListLine"></ItemStyle>
	<Columns>
		<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="&lt;img src=PortalImages/save.gif&gt;" CancelText="&lt;img src=PortalImages/Cancel.gif&gt;"
			EditText="&lt;img src=PortalImages/edit.gif&gt;"></asp:EditCommandColumn>
		<asp:ButtonColumn Text="&lt;img src=PortalImages/Delete.gif&gt;" CommandName="Delete"></asp:ButtonColumn>
		<portal:BoundColumn DataField="Url" LanguageRef-HeaderText="Url" />
		<portal:BoundColumn DataField="Name" LanguageRef-HeaderText="Title" />
		<portal:BoundColumn DataField="MaxCount" LanguageRef-HeaderText="MaxNewsCount" />
	</Columns>
</asp:DataGrid>
