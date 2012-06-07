<%@ Control Language="c#" Inherits="Portal.API.Module" EnableViewState="false" %>
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
		WebSiteConfig cfg = (WebSiteConfig)ReadConfig(typeof(WebSiteConfig));
        if(cfg != null)
        {
            content.Attributes.Add("src", cfg.URL);
            string height = cfg.Height;
            if(height != "auto")
            {
				content.Attributes.Add("height", height);
			}
        }        
    }
</script>
<iframe id="content" runat="server" width="100%" frameborder="0"></iframe>
