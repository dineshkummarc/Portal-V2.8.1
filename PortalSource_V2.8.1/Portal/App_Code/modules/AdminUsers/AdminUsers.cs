using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Portal.Modules.AdminUsers
{
    abstract public class AdminUsers : API.Module
    {
        abstract public void EditUser(string account);
        abstract public void ShowUserList();
    }
}
