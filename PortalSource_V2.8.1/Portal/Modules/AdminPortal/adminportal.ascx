<%@ Register TagPrefix="iiuga" Namespace="iiuga.Web.UI" Assembly="TreeWebControl" %>
<%@ Register TagPrefix="uc1" TagName="Tab" Src="Tab.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TabList" Src="TabList.ascx" %>
<%@ Control Language="c#" Inherits="Portal.Modules.AdminPortal.AdminPortalControl" CodeFile="AdminPortal.ascx.cs" %>
<table>
	<tr>
		<td width="1" valign="top">
			<iiuga:treeweb id="tree" runat="server" CollapsedElementImage="PortalImages/Plus.jpg" ExpandedElementImage="PortalImages/Minus.jpg">
				<ImageList>
					<iiuga:ElementImage ImageURL="PortalImages/Bullet.gif" />
				</ImageList>
				<Elements>
					<iiuga:treeelement text="Portal" CssClass="" />
				</Elements>
			</iiuga:treeweb>
		</td>
		<td width="40">&nbsp;</td>
		<td valign="top">
			<uc1:Tab id="TabCtrl" runat="server" OnSave="OnSave" OnCancel="OnCancel" OnDelete="OnDelete"></uc1:Tab>
			<!--			<hr> -->
			<uc1:TabList id="TabListCtrl" runat="server"></uc1:TabList>
		</td>
	</tr>
</table>
