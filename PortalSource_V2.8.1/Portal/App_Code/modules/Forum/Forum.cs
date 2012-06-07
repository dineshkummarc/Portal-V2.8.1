namespace Portal.Modules.Forum
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;

    abstract public class Forum : Portal.API.Module, Portal.Modules.Forum.IStateProcessor
    {
        abstract public StateMachine StateMachine { get; }
        abstract public bool SetEvent(StateEvents newEvent);
        abstract public ConfigAgent ConfigAgent { get; }
    }
}
