namespace Portal
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Zusammenfassungsbeschreibung für ModuleHeader
    /// </summary>
    public abstract class ModuleHeader : System.Web.UI.UserControl
    {
        public abstract void SetModuleConfig(PortalDefinition.Module md);
    }
}