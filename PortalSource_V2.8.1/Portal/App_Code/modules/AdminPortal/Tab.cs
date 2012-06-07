namespace Portal.Modules.AdminPortal
 {
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

    abstract public class Tab :  System.Web.UI.UserControl
    {
      abstract public void LoadData(PortalDefinition.Tab t);
	    abstract public  void EditModule(string reference);
	    abstract public  void AddModule(ModuleList list);
	    abstract public  void MoveModuleUp(int idx, ModuleList list);
	    abstract public  void MoveModuleDown(int idx, ModuleList list);
    }
}
