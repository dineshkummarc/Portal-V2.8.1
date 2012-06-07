<%@ Import namespace="Portal" %>
<%@ Import namespace="Portal.API" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" EnableViewState="false"%>
<script runat="server">
	private void Page_Load(object sender, System.EventArgs e)
	{
    account.Focus();
    
    // Some devices need a Submit button for the login.  
    if (Request.Browser.IsMobileDevice)
      lnkLogin.UseSubmitBehavior = true;
	}

	public override bool IsVisible()
	{
		return !Page.User.Identity.IsAuthenticated;
	}

	void OnLogin(object sender, EventArgs args)
	{
		if(UserManagement.Login(account.Text, password.Text))
		{
			if (m_AutoLogin.Checked)
			{
				HttpCookie cookie = new HttpCookie("PortalUser");
				cookie.Values["AC"] = Crypto.Encrypt(account.Text);
				cookie.Values["PW"] = Crypto.Encrypt(password.Text);
				DateTime dt = DateTime.Now;
				TimeSpan ts = new TimeSpan(100,0,0,0);
				cookie.Expires = dt.Add(ts);
				Response.Cookies.Add(cookie);
			}
			Response.Redirect(Request.RawUrl);
		}
		else
		{
			lError.Text = Portal.API.Language.GetText(this, "InvalidLogin");
		}
	}
</script>
<div>
    &nbsp;</div><div>
    <asp:Panel ID="loginPanel" runat="server" DefaultButton="lnkLogin">
		<portal:Label id="AccountLbl" runat="server" LanguageRef="Account" />
<div>
	<asp:TextBox Width="180px" ID="account" Runat="server" AutoCompleteType="DisplayName"></asp:TextBox>
</div>
<div>
		<portal:Label id="PasswordLbl" runat="server" LanguageRef="Password" />
</div>
<div>
	<asp:TextBox width="180px" ID="password" Runat="server" TextMode="Password"></asp:TextBox>
</div>
<div>
		<portal:Label runat="server" LanguageRef="AutoLogin" id="Label1"></portal:Label>
	<asp:CheckBox id="m_AutoLogin" runat="server" TextAlign="Left"></asp:CheckBox>
</div>
<div>
	<asp:Label ID="lError" Runat="server" CssClass="Error"></asp:Label>
</div>
<div>
	<!--<portal:LinkButton Runat="server" CssClass="LinkButton" ></portal:LinkButton>-->
  <portal:Button ID="lnkLogin" runat="server" 
    OnClick="OnLogin" LanguageRef="Login" CausesValidation="False" 
    CssClass="LoginButton" UseSubmitBehavior="False"/>
</div>
    </asp:Panel>
    &nbsp;</div>
