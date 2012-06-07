<%@ Control Language="c#" autoeventwireup="true" Inherits="Portal.API.EditModule" %>
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
        if(!IsPostBack)
        {
			ForumConfig cfg = (ForumConfig)ReadConfig(typeof(ForumConfig));
            if(cfg != null)
            {
                txtForumID.Text = cfg.ForumID.ToString();
            }
        }
    }
    
    void OnSave(object sender, EventArgs args)
    {
		ForumConfig cfg = new ForumConfig();
		
        cfg.ForumID = Int32.Parse(txtForumID.Text);
        WriteConfig(cfg);
        
        RedirectBack();
    }
</script>
<table width="100%">
	<tr>
		<td class="Label">Forum ID</td>
		<td class="Data"><asp:TextBox ID="txtForumID" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
</table>
<asp:LinkButton CssClass="LinkButton" runat="server" OnClick="OnSave" ID="Linkbutton1">Save & Back</asp:LinkButton>
