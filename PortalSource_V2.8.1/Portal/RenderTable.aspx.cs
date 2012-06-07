using System;
using Portal.API;

namespace Portal
{
    /// <summary>
    /// Holds the PortalTab and Header Control in the 'Table' Mode
    /// </summary>
    public partial class RenderTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
          Response.Redirect(Config.MainPage);
        }
    }
}
