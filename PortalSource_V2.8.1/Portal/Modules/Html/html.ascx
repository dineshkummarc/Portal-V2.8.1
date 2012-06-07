<%@ Control Language="c#" Inherits="Portal.API.Module" enableViewState="False"%>
<%@ Import namespace="System.IO" %>
<script runat="server">
    private string GetPath()
    {
        return ModuleDataPhysicalPath + ModuleRef + ".htm";
    }

    void Page_Load(object sender, EventArgs args)
    {
        // Open file            
        if(File.Exists(GetPath()))
        {
            FileStream fs = File.OpenRead(GetPath());
            StreamReader sr = new StreamReader(fs);
            content.InnerHtml = sr.ReadToEnd();
            fs.Close();
        }        
    }

</script>
<div id="content" runat="server">
</div>
