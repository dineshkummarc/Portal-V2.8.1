<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="System.Xml" %>
<%@ Import namespace="System.Xml.Serialization" %>
<%@ Import namespace="System.Web.Mail" %>
<%@ Import namespace="System.Text.RegularExpressions" %>
<script runat="server">

[XmlRoot("Feedback")]
public class ModuleConfig
{
	public string MailFrom = "";
	public string MailTo = "";
	public string MailSubject = "";
	public string MailServer = "";
}

void Bind()
{
	// Schema exists - cannot be null!
	DataSet ds = ReadConfig();

	FeedBack.DataSource = ds.Tables["Feedback"].Select("1=1", "Created DESC");
	FeedBack.DataBind();
}

void Page_Load(Object sender, EventArgs e) 
{
	Bind();
}

void OnAdd(Object sender, EventArgs args) 
{
	try
	{
		// Check
		if(
			Portal.API.HtmlAnalyzer.HasScriptTags(txtMessage.Text) ||
      Portal.API.HtmlAnalyzer.HasScriptTags(txtFrom.Text) ||
      Portal.API.HtmlAnalyzer.HasScriptTags(txtTitle.Text))
		{
			msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
			return;
		}
		
		txtMessage.Text = txtMessage.Text.Trim();
		if(txtMessage.Text == "") return;
		
		// Save
		DataSet ds = ReadConfig();
		DataRow r = ds.Tables["Feedback"].NewRow();
		r["Created"] = DateTime.Now;
		r["From"] = txtFrom.Text;
		r["Title"] = txtTitle.Text;
		r["Message"] = txtMessage.Text;
	
		ds.Tables["Feedback"].Rows.Add(r);
		
		WriteConfig(ds);
	
		// Send Mail
		ModuleConfig cfg = (ModuleConfig)ReadCommonConfig(typeof(ModuleConfig));
		
		if(cfg.MailServer != "")
		{		
			MailMessage m = new MailMessage();
			m.To = cfg.MailTo;
			m.From = cfg.MailFrom;
			m.Subject = cfg.MailSubject;

			string body = "";
			if(txtFrom.Text != "")
			{
				body += "Name: " + txtFrom.Text + "\n";
			}
			if(txtTitle.Text != "")
			{
				body += "Title: " + txtTitle.Text + "\n";
			}
			if(body != "")
			{
				body += "\n";
			}
			body += txtMessage.Text;

			m.Body = body;
			SmtpMail.SmtpServer = cfg.MailServer;
			SmtpMail.Send(m);
		}
		
		txtFrom.Text = "";
		txtTitle.Text = "";
		txtMessage.Text = "";
		
		Bind();
	}
	catch(Exception e)
	{
		throw new Exception(e.Message, e); 
	}	
}

</script>
<portal:Message id="msg" runat="server" />
<portal:LinkButton Runat="server" OnClick="OnAdd" LanguageRef="Add"></portal:LinkButton>
<table width="80%">
	<colgroup>
		<col class="Label" width="20%">
		<col class="Data" width="80%">
	</colgroup>
	<tr>
		<td>
			<portal:Label runat=server LanguageRef="From"></portal:Label>
		</td>
		<td><asp:TextBox ID="txtFrom" Runat="server" Width="99%"></asp:TextBox></td>
	</tr>
	<tr>
		<td>
			<portal:Label runat=server LanguageRef="Title" ID="Label1" NAME="Label1"></portal:Label>
		</td>
		<td><asp:TextBox ID="txtTitle" Runat="server" Width="99%"></asp:TextBox></td>
	</tr>
	<tr>
		<td nowrap>
			<portal:Label runat=server LanguageRef="Message" ID="Label2" NAME="Label2"></portal:Label>
		</td>
		<td><asp:TextBox ID="txtMessage" Runat="server" TextMode="MultiLine" Width="99%" Height="100"></asp:TextBox></td>
	</tr>
</table>
<div style="border-bottom:solid 1px black;width: 100%"></div>
<table>
	<asp:Repeater ID="FeedBack" Runat="server">
		<ItemTemplate>
			<tr>
				<td><%# ((DateTime)((DataRow)Container.DataItem)["Created"]).ToShortDateString() %>&nbsp;</td>
				<td width="100%"><%# ((DataRow)Container.DataItem)["From"] %></td>
			</tr>
			<tr>
				<td colspan="2"><b><%# ((DataRow)Container.DataItem)["Title"] %></b></td>
			</tr>
			<tr>
				<td colspan="2"><%# ((DataRow)Container.DataItem)["Message"] %></td>
			</tr>
		</ItemTemplate>
		<SeparatorTemplate>
			<tr>
				<td style="border-bottom:solid 1px black;width: 100%" colspan="2">&nbsp;</td>
			</tr>
		</SeparatorTemplate>
	</asp:Repeater>
</table>
