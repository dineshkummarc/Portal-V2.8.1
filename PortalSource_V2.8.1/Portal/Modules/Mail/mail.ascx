<%@ Control Language="c#" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
				txtBody.Text = cfg.DefaultBody;
			}
		}
    }
    
    protected void OnSend(object sender, EventArgs args)
    {
		Page.Validate();
		if(!Page.IsValid) return;
		
		try
		{
			MailConfig cfg = (MailConfig)ReadConfig(typeof(MailConfig));
			if(cfg != null)
			{
				string body = Language.GetText(this, "BodyMailFrom") + txtFromAddress.Text + "\n\n" + txtBody.Text;
				System.Web.Mail.SmtpMail.Send(
					cfg.FromAddress, cfg.ToAddress, txtSubject.Text, body);
				
				preSuccess.InnerText = Language.GetText(this, "MailSendSuccessfully");
			}
			else
			{
				preError.InnerText = Language.GetText(this, "NotConfigured");
			}
		}
		catch(Exception e)
		{
			preError.InnerText = Language.GetText(this, "Error");
		}
    }

</script>
<pre id="preSuccess" runat="server" style="color: green;font-weight:bold;"></pre>
<pre id="preError" runat="server" style="color: red;font-weight:bold;"></pre>
<asp:ValidationSummary Runat="server"></asp:ValidationSummary>
<table width="100%">
	<colgroup>
		<col class="Label" width="200">
		<col class="Data">
	</colgroup>
	<tr>
		<td nowrap>
			<portal:Label runat=server LanguageRef="From"></portal:Label>&nbsp;<portal:RequiredFieldValidator Runat="server" ControlToValidate="txtFromAddress" LanguageRef="FromAddressMissing">*</portal:RequiredFieldValidator><portal:RegularExpressionValidator Runat="server" ControlToValidate="txtFromAddress" LanguageRef="FromAddressInvalid" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</portal:RegularExpressionValidator>
		</td>
		<td><asp:TextBox ID="txtFromAddress" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
	<tr>
		<td>
			<portal:Label runat=server LanguageRef="Subject"></portal:Label>&nbsp;<portal:RequiredFieldValidator Runat="server" ControlToValidate="txtSubject" LanguageRef="SubjectMissing" ID="Requiredfieldvalidator1" NAME="Requiredfieldvalidator1">*</portal:RequiredFieldValidator>
		</td>
		<td><asp:TextBox ID="txtSubject" Runat="server" Width="100%"></asp:TextBox></td>
	</tr>
	<tr valign="top">
		<td>
			<portal:Label runat=server LanguageRef="Message"></portal:Label>&nbsp;<portal:RequiredFieldValidator Runat="server" ControlToValidate="txtBody" LanguageRef="BodyMissing" ID="Requiredfieldvalidator2" NAME="Requiredfieldvalidator2">*</portal:RequiredFieldValidator>
		</td>
		<td><asp:TextBox ID="txtBody" Runat="server" Width="100%" Height="300" TextMode="MultiLine"></asp:TextBox></td>
	</tr>
</table>
<div align="right">
	<portal:Button ID="btnSend" Runat="server" LanguageRef="Send" OnClick="OnSend"></portal:Button>
</div>
