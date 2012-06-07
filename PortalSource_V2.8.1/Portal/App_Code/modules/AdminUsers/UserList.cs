using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.AdminUsers
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für UserList
    /// </summary>
    public abstract class UserList : System.Web.UI.UserControl
    {
        public abstract void BindGrid();
    }
}
