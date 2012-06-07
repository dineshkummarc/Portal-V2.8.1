<%@ Import namespace="Portal.API" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="System.Xml" %>
<%@ Import namespace="System.Xml.Serialization" %>
<%@ Import namespace="System.Web.Mail" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" Inherits="Portal.Modules.Guestbook.GuestbookControl" CodeFile="Guestbook.ascx.cs"%>
<portal:LinkButton id="m_AddBtn" runat="server" ToolTip="Beitrag ins Gästebuch schreiben." LanguageRef="Add" onclick="OnAdd"></portal:LinkButton>
<table width="80%">
	<colgroup>
		<col class="Label" width="15%"/>
		<col class="Data" width="85%"/>
	</colgroup>
	<tr>
		<td><portal:Label runat="server" LanguageRef="From" id="Label1" /></td>
		<td><asp:textbox id="m_FromTB" runat="server" width="99%"></asp:textbox></td>
	</tr>
	<tr id="m_EmailTR" runat="server">
		<td><portal:Label runat="server" LanguageRef="Email" id="Label2" /></td>
		<td><asp:textbox id="m_EmailTB" runat="server" Width="99%"></asp:textbox></td>
	</tr>
	<tr id="m_UrlTR" runat="server">
		<td><portal:Label runat="server" LanguageRef="Url" id="Label3" /></td>
		<td><asp:textbox id="m_UrlTB" runat="server" Width="99%"></asp:textbox></td>
	</tr>
	<tr>
		<td><portal:Label runat="server" LanguageRef="Title" id="Label4" /></td>
		<td><asp:textbox id="m_TitleTB" runat="server" Width="99%"></asp:textbox></td>
	</tr>
	<tr>
		<td valign="top"><portal:Label runat="server" LanguageRef="Message" id="Label5" /></td>
		<td><asp:textbox id="m_MessageTB" runat="server" Width="99%" TextMode="MultiLine" Height="100px"></asp:textbox></td>
	</tr>
	<tr id="m_CommentTR" runat="server">
		<td valign="top"><portal:Label runat="server" LanguageRef="Comment" id="Label6" /></td>
		<td><asp:textbox id="m_CommentTB" runat="server" Width="99%" TextMode="MultiLine" Height="100px"></asp:textbox></td>
	</tr>
	<tr id="m_EditButtonTR" runat="server">
		<td colspan="2">
			<portal:LinkButton id="m_EditSaveBtn" runat="server" LanguageRef="Save" onclick="OnEditSave"></portal:LinkButton>&nbsp;
			<portal:LinkButton id="m_EditCancelBtn" runat="server" LanguageRef="Cancel" onclick="OnEditCancel"></portal:LinkButton>
		</td>
	</tr>
	<tr id="m_ErrorTR" runat="server">
		<td colspan="2">
			<asp:Label id="m_ErrorLbl" runat="server" ForeColor="Red"></asp:Label>
		</td>
	</tr>
</table>
<div style="WIDTH: 100%; BORDER-BOTTOM: black 1px solid"></div>
<asp:DataGrid id="DataGrid" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
	BorderWidth="0px" ShowHeader="False">
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
				<table width="100%">
					<tr>
						<td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "Created")).ToShortDateString() %>&nbsp;</td>
						<td width="100%"><%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseEmail"]) && (((string)DataBinder.Eval(Container.DataItem, "Email")).Length > 0) ? @"<b><a href='Mailto:":"<b>" %>
							<%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseEmail"]) && (((string)DataBinder.Eval(Container.DataItem, "Email")).Length > 0) ? (string)DataBinder.Eval(Container.DataItem, "Email") : (string)DataBinder.Eval(Container.DataItem, "Name") %>
							<%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseEmail"]) && (((string)DataBinder.Eval(Container.DataItem, "Email")).Length > 0) ? @"'>":"</b>" %>
							<%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseEmail"]) && (((string)DataBinder.Eval(Container.DataItem, "Email")).Length > 0) ? (string)DataBinder.Eval(Container.DataItem, "Name") + @"</a></b>" : "" %>
						</td>
					</tr>
					<tr>
						<td colspan="2"><%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseUrl"]) && (((string)DataBinder.Eval(Container.DataItem, "Url")).Length > 0) ? @"<b><a href='":"" %>
							<%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseUrl"]) && (((string)DataBinder.Eval(Container.DataItem, "Url")).Length > 0) ? (string)DataBinder.Eval(Container.DataItem, "Url") : "" %>
							<%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseUrl"]) && (((string)DataBinder.Eval(Container.DataItem, "Url")).Length > 0) ? @"' target='_blank'>" : "" %>
							<%# ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseUrl"]) && (((string)DataBinder.Eval(Container.DataItem, "Url")).Length > 0) ? (string)DataBinder.Eval(Container.DataItem, "Url") + @"</a></b>" : "" %>
						</td>
					</tr>
					<tr>
						<td colspan="2"><b><%# (string)DataBinder.Eval(Container.DataItem, "Title") %></b></td>
					</tr>
					<tr>
						<td colspan="2"><%# Helper.ActivateWebSiteUrl((string)DataBinder.Eval(Container.DataItem, "Message")) %></td>
					</tr>
					<tr runat="server" visible='<%# ((DataBinder.Eval(Container.DataItem, "Comment") != null) && (DataBinder.Eval(Container.DataItem, "Comment").ToString() != "")) %>' id="Tr1">
						<td valign="top"><i>
								<portal:Label runat="server" LanguageRef="Comment" id="Label7" />:&nbsp;</i></td>
						<td><i><%# DataBinder.Eval(Container.DataItem, "Comment") %></i></td>
					</tr>
					<tr runat="server" visible='<%# ModuleHasEditRights %>' id="Tr2">
						<td colspan="2">
							<portal:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# ((Guid)DataBinder.Eval(Container.DataItem, "Id")).ToString() %>' LanguageRef="Remove" id="Linkbutton1">
							</portal:LinkButton>&nbsp;
							<portal:LinkButton runat="server" CommandName="Edit" CommandArgument='<%# ((Guid)DataBinder.Eval(Container.DataItem, "Id")).ToString() %>' LanguageRef="Edit" id="Linkbutton2">
							</portal:LinkButton>
						</td>
					</tr>
				</table>
				<div style="WIDTH: 100%; BORDER-BOTTOM: black 1px solid"></div>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle NextPageText="&gt;" PrevPageText="&lt;" Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
