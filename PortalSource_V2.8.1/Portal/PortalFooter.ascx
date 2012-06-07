<%@ Import namespace="System.IO" %>
<%@ Control Language="c#" AutoEventWireup="true" EnableViewState="false" %>
<script runat="server">

  protected void OnPreRenderFooter(object sender, EventArgs e)
  {
      // Load File.            
    string fileName = Portal.API.Config.GetModuleDataPhysicalPath() + "PortalFooter.htm";
      if (File.Exists(fileName))
      {
        FileStream fs = File.OpenRead(fileName);
        StreamReader sr = new StreamReader(fs);
        footerContent.Text = sr.ReadToEnd();
        fs.Close();
      }
  }
</script>

<asp:Literal ID="footerContent" runat="server" EnableViewState="False" OnPreRender="OnPreRenderFooter"></asp:Literal>
