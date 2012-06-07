<%@ Control Language="c#" autoeventwireup="true" Inherits="Portal.API.EditModule" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="Portal" %>
<%@ Import namespace="Portal.API" %>
<script runat="server">

	public class MailConfig
	{
		public string ToAddress = "";
		public string FromAddress = "";
		public string DefaultBody = "";
	}

	void Page_Load(object sender, EventArgs args)
    {
		if(!IsPostBack)
		{
			MailConfig cfg = (MailConfig)ReadConfig(typeof(MailConfig));
			if(cfg != null)
			{
				txtToAddress.Text = cfg.ToAddress;
				txtFromAddress.Text = cfg.FromAddress;
				txtDefaultBody.Text = cfg.DefaultBody;
			}
		}
    }

    void OnSave(object sender, EventArgs args)
    {
		MailConfig cfg = (MailConfig)ReadConfig(typeof(MailConfig));
		if(cfg == null)
		{
			cfg = new MailConfig();
		}

		cfg.ToAddress = txtToAddress.Text;
		cfg.FromAddress = txtFromAddress.Text;
		cfg.DefaultBody = txtDefaultBody.Text;

		WriteConfig(cfg);
		RedirectBack();
    }
    
</script>
<portal:LinkButton CssClass="LinkButton" runat="server" OnClick="OnSave" ID="Linkbutton1" NAME="Linkbutton1" LanguageRef="SaveBack"></portal:LinkButton>
<table width="100%">
	<colgroup>
		<col class="Label" width="200">
			<col class="Data">
	</colgroup>
	<tr>
		<td nowrap>
			<portal:Label runat=server LanguageRef="EditFromAddress"></portal:Label>
		</td>
		<td><asp:TextBox ID="txtFromAddress" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
	<tr>
		<td>
			<portal:Label runat=server LanguageRef="EditToAddress" ID="Label1" NAME="Label1"></portal:Label>
		</td>
		<td><asp:TextBox ID="txtToAddress" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
	<tr valign="top">
		<td>
			<portal:Label runat=server LanguageRef="EditDefaultMessage" ID="Label2" NAME="Label1"></portal:Label>			
		</td>
		<td><asp:TextBox ID="txtDefaultBody" Runat="server" Width="100%" Height="300" TextMode="MultiLine"></asp:TextBox></td>
	</tr>
</table>
