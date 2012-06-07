using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.StatisticViewer
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für ConfigAgent
    /// </summary>
    public class ConfigAgent
    {
        #region Members

        private DateTime month;

        #endregion

        #region Construction / Destruction

        public ConfigAgent()
        {
        }

        #endregion

        #region Properties

        public DateTime Month
        {
            get { return month; }
            set { month = value; }
        }

        #endregion
    }
}