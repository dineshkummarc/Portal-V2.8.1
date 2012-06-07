<%@ Control Language="c#" Inherits="Portal.Modules.TabList.TabTree" CodeFile="TabTree.ascx.cs" %>
<%@ Register TagPrefix="iiuga" Namespace="iiuga.Web.UI" Assembly="TreeWebControl" %>
<iiuga:treeweb id="tree" runat="server" CollapsedElementImage="PortalImages/Plus.jpg" ExpandedElementImage="PortalImages/Minus.jpg">
	<ImageList>
		<iiuga:ElementImage ImageURL="PortalImages/Bullet.gif" />
	</ImageList>
	<Elements>
		<iiuga:treeelement text="Portal" CssClass="" />
	</Elements>
</iiuga:treeweb>
