using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.FormBuilder
{
  public partial class EditMainState : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void EditFormBtn_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.EditForm);
    }

    protected void OnBack(object sender, EventArgs e)
    {
      Response.Redirect(Helper.GetCancelEditLink());
    }

    protected void EditResultSuccessBtn_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.EditResultSuccess);
    }

    protected void EditResultErrorBtn_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.EditResultError);
    }

    protected void EditEmailBtn_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.EditEmail);
    }

    protected void EditValidation_Click(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.EditValidation);
    }
}
}
