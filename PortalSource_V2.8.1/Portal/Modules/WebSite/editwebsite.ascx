<%@ Control Language="c#" autoeventwireup="true" Inherits="Portal.API.EditModule" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Xml" %>
<%@ Import namespace="System.Xml.Serialization" %>
<script runat="server">

	[XmlRoot("WebSite")]
	public class WebSiteConfig
	{
		[XmlElement("URL")]
		public string URL = "";
		[XmlElement("Height")]
		public string Height = "";
	}

    void Page_Load(object sender, EventArgs args)
    {
        if(!IsPostBack)
        {
			WebSiteConfig cfg = (WebSiteConfig)ReadConfig(typeof(WebSiteConfig));
            if(cfg != null)
            {
                txtURL.Text = cfg.URL;
                txtHeight.Text = cfg.Height;
            }
        }
    }
    
    void OnSave(object sender, EventArgs args)
    {
		WebSiteConfig cfg = new WebSiteConfig();
		
        cfg.URL = txtURL.Text;
        cfg.Height = txtHeight.Text;
        WriteConfig(cfg);
        
        RedirectBack();
    }
</script>
<table width="100%">
	<tr>
		<td class="Label">
			<portal:Label runat=server LanguageRef="URL" />
		</td>
		<td class="Data"><asp:TextBox ID="txtURL" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="Label">
			<portal:Label runat=server LanguageRef="Height" />
		</td>
		<td class="Data"><asp:TextBox ID="txtHeight" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
</table>
<portal:LinkButton CssClass="LinkButton" runat="server" OnClick="OnSave" ID="Linkbutton1" LanguageRef="SaveBack"></portal:LinkButton>
