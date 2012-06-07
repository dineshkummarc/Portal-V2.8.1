<%@ Control Language="c#" Inherits="Portal.Modules.AdminUsers.AdminUsersControl" CodeFile="AdminUsers.ascx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UserList" Src="UserList.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UserEdit" Src="UserEdit.ascx" %>
<uc1:UserList id="ctrlUserList" runat="server"></uc1:UserList>
<uc1:UserEdit id="ctrlUserEdit" runat="server" visible="false"></uc1:UserEdit>
