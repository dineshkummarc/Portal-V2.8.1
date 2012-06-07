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

namespace Portal.Modules.ContentScheduler
{
  public partial class ContentScheduler : Portal.API.Module
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      ContentSchedulerHandler contentHandler = new ContentSchedulerHandler(this);
      _pageContent.Text = contentHandler.GetCurrentContent();
    }
  }
}
