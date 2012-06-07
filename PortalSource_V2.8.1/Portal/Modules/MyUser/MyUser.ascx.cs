namespace Portal.Modules.MyUser
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Web.Security;
    using System.Security;
    using System.Security.Principal;
    using Portal.API;

    /// <summary>
    ///		Summary description for Login.
    /// </summary>
    public partial class MyUser : Portal.API.Module
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            Portal.API.Principal principal = (Portal.API.Principal)Page.User;

            txtLogin.Text = principal.Login;
            txtPassword.Text = "";
            txtPassword2.Text = "";
            txtFirstName.Text = HttpUtility.HtmlDecode(principal.FirstName);
            txtSurName.Text = HttpUtility.HtmlDecode(principal.SurName);
            txtEMail.Text = HttpUtility.HtmlDecode(principal.EMail);
        }

        protected void OnSave(object sender, EventArgs args)
        {
            try
            {
                string pwd = "";
                if (txtPassword.Text != "")
                {
                    if (txtPassword.Text != txtPassword2.Text)
                    {
                        msg.Error = Portal.API.Language.GetText(this, "InvalidPassword");
                        return;
                    }
                    pwd = txtPassword.Text;
                }
                Portal.API.Principal principal = (Portal.API.Principal)Page.User;
                UserManagement.SaveUser(
                    Page.User.Identity.Name,
                    pwd, txtFirstName.Text, txtSurName.Text, txtEMail.Text,
                    new System.Collections.ArrayList(principal.Roles), principal.Id);
                msg.Success = Portal.API.Language.GetText(this, "SuccessfullySaved");

                HttpContext.Current.User = UserManagement.GetUser(principal.Login);

                Bind();
            }
            catch (Exception e)
            {
                msg.Error = e.Message;
            }
        }

        public override bool IsVisible()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
