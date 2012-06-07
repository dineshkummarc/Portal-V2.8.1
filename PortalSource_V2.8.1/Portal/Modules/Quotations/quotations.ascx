<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import namespace="System.IO" %>
<script runat="server">
    private string GetPath()
    {
        return ModuleDataPhysicalPath + ModuleRef + ".txt";
    }

    void Page_Load(object sender, EventArgs args)
    {
		// Random Numbers
		Random r = new Random(DateTime.Now.Millisecond);
    
        // Open file            
        if(File.Exists(GetPath()))
        {
            FileStream fs = File.OpenRead(GetPath());
            StreamReader sr = new StreamReader(fs);
            string[] lines = sr.ReadToEnd().Split('\n');
            content.InnerHtml = lines[r.Next(lines.Length)];
            fs.Close();
        }        
    }

</script>
<span id="content" runat="server">
</span>
