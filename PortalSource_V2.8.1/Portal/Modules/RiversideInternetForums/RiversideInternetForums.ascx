<%@ Control Language="c#" autoeventwireup="true" Inherits="Portal.API.Module" %>
<%@ Register TagPrefix="WS" Namespace="RiversideInternet.WebSolution" Assembly="RiversideInternetForums" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Xml" %>
<%@ Import namespace="System.Xml.Serialization" %>

<script runat="server">

	[XmlRoot("ForumConfig")]
	public class ForumConfig
	{
		[XmlElement("ForumID")]
		public int ForumID = 1;
	}

    void Page_Load(object sender, EventArgs args)
    {
		ForumConfig cfg = (ForumConfig)ReadConfig(typeof(ForumConfig));
        if(cfg != null)
        {
	        forum.ForumID = cfg.ForumID;
        }
        if(Page.User.Identity.IsAuthenticated)
        {
			lnkSettings.Visible = true;
        }
        else
        {
			lnkSettings.Visible = false;
        }
    }
    protected void OnSettings(object sender, EventArgs args)
    {
		forum.Visible = false;
		userManagement.Visible = true;
		lnkSettings.Visible = false;
    }
    protected void OnSaveSettings(object sender, EventArgs args)
    {
		forum.Visible = true;
		userManagement.Visible = false;
		lnkSettings.Visible = true;
    }
</script>

<asp:LinkButton id="lnkSettings" runat="server" OnClick="OnSettings">My Settings</asp:LinkButton>
<br>
<br>
<WS:Forum ForumID="1" Runat="server" id="forum" />
<WS:UserManagement Runat="server" id="userManagement" Visible="false" OnSave="OnSaveSettings" />
