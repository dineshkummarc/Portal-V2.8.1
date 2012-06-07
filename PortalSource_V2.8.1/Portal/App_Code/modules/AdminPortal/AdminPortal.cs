using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;


namespace Portal.Modules.AdminPortal
{
    abstract public class AdminPortal :  Module, IPostBackEventHandler
    {
	    abstract public  void BuildTree();
	    abstract public void SelectTab(string reference);
	    abstract public void AddTab();
	    abstract public  void MoveTabUp(int index);
	    abstract public  void MoveTabDown(int index);
	    abstract public void RaisePostBackEvent(string eventArgument);
    }
}
