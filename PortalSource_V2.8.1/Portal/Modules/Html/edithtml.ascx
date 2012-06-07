<%@ Control Language="c#" autoeventwireup="true" Inherits="Portal.API.EditModule" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<script runat="server">

    private string GetPath()
    {
        return ModuleDataPhysicalPath + ModuleRef + ".htm";
    }

    void Page_Load(object sender, EventArgs args)
    {
        if(!IsPostBack)
        {
            // Open file            
            if(File.Exists(GetPath()))
            {
                using(FileStream fs = File.OpenRead(GetPath()))
                {
					StreamReader sr = new StreamReader(fs);
					txt.Text = sr.ReadToEnd();
                }
            }
        }
    }
    
    void OnSave(object sender, EventArgs args)
    {
		if(Portal.API.HtmlAnalyzer.HasScriptTags(txt.Text))
		{
			msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
			return;
		}
        using(FileStream fs = new FileStream(GetPath(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        {
			fs.SetLength(0); // Truncate
			using(StreamWriter sw = new StreamWriter(fs))
			{
				sw.Write(txt.Text);
			}
        }
        RedirectBack();
    }

</script>
<portal:Message id="msg" runat="server" />
<asp:TextBox id="txt" Width="100%" Height="300px" TextMode="MultiLine" Runat="server"></asp:TextBox>
<portal:LinkButton CssClass="LinkButton" runat="server" OnClick="OnSave" ID="Linkbutton1" NAME="Linkbutton1" LanguageRef="SaveBack"></portal:LinkButton>
