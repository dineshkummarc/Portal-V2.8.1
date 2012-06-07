<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.EditModule" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<script runat="server">

    private string GetPath()
    {
        return ModuleDataPhysicalPath + ModuleRef + ".csv";
    }

    void Page_Load(object sender, EventArgs args)
    {
        if(!IsPostBack)
        {
            // Open file            
            if(File.Exists(GetPath()))
            {
                FileStream fs = File.OpenRead(GetPath());
                StreamReader sr = new StreamReader(fs);
                txt.Text = sr.ReadToEnd();
                fs.Close();
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
        FileStream fs = null;
        try
        {
            fs = new FileStream(GetPath(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            fs.SetLength(0); // Truncate
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(txt.Text);
            sw.Close();
            
        }
        finally
        {
            if(fs != null)
            {
                fs.Close();
            }
        }
        RedirectBack();
    }

</script>
<portal:Message id="msg" runat="server" />
<asp:TextBox id="txt" Width="100%" Height="300px" TextMode="MultiLine" Runat="server"></asp:TextBox>
<portal:LinkButton CssClass="LinkButton" runat="server" OnClick="OnSave" ID="Linkbutton1" NAME="Linkbutton1" LanguageRef="SaveBack"></portal:LinkButton>
