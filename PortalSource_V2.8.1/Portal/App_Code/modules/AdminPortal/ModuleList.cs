using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.AdminPortal
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für ModuleList
    /// </summary>
    public abstract class ModuleList : System.Web.UI.UserControl
    {
        public string TitleLanguageRef = "";

        /// <summary>
        /// Wrapper Class for the Tab Object.
        /// </summary>
        public abstract class DisplayModuleItem
        {
            /// <summary>
            /// Tabs Text
            /// </summary>
            public abstract string Title
            {
                get;
            }
            /// <summary>
            /// Tabs Reference
            /// </summary>
            public abstract string Reference
            {
                get;
            }
            /// <summary>
            /// Modules Type
            /// </summary>
            public abstract string ModuleType
            {
                get;
            }
        }

        abstract public void LoadData(ArrayList modules);
    }
}