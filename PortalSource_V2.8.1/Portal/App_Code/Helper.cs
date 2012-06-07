using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Portal.API;

namespace Portal
{
    /// <summary>
    /// Collection of Helper Methods. For internal use only.
    /// </summary>
    public sealed class Helper
    {
        /// <summary>
        /// Helper-Class must not be instantiated, so set the constructor to private.
        /// </summary>
        private Helper() { }

        /// <summary>
        /// Returns the path to the Portal CSS-File.
        /// </summary>
        public static string CssPath
        {
            get 
            {
              string filePath;
              HttpContext context = HttpContext.Current;
              if (context != null)
              {
                filePath = context.Request.ApplicationPath;
                if (!filePath.EndsWith("/"))
                  filePath += "/";
              }
              else
                filePath = "";
              
              return filePath + System.Configuration.ConfigurationManager.AppSettings["CssFile"]; 
            }
        }

        /// <summary>
        /// Retruns the path to the Portal CSS-File for the HTML-Edit Mode.
        /// </summary>
        public static string CssEditPath
        {
          get
          {
            string subPath = System.Configuration.ConfigurationManager.AppSettings["CssFileForEdit"];
            if (string.IsNullOrEmpty(subPath))
            {
              subPath = "Data/Resources/PortalEdit.css";
            }
            string filePath;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
              filePath = context.Request.ApplicationPath;
              if (!filePath.EndsWith("/"))
                filePath += "/";
            }
            else
              filePath = "";

            return filePath + subPath;
          }
        }

        /// <summary>
        /// Returns the proper edit ascx Control. Uses the current Page to load the Control.
        /// If the user has no right a error control is returned
        /// </summary>
        /// <param name="p">The current Page</param>
        /// <returns>Edit ascx Control</returns>
        public static Control GetEditControl(Page p)
        {
            PortalDefinition.Tab tab = PortalDefinition.CurrentTab;
            PortalDefinition.Module m = tab.GetModule(p.Request["ModuleRef"]);

            if (!UserManagement.HasEditRights(HttpContext.Current.User, m.roles))
            {
                return GetNoAccessControl();
            }
            m.LoadModuleSettings();
            Module em = null;
            if (m.moduleSettings != null)
            {
                // Module Settings are present, use custom ascx Control
                em = (Module)p.LoadControl(Config.GetModuleVirtualPath(m.type) + m.moduleSettings.editCtrl);
            }
            else
            {
                // Use default ascx control (Edit[type].ascx)
                em = (Module)p.LoadControl(Config.GetModuleVirtualPath(m.type) + "Edit" + m.type + ".ascx");
            }

            // Initialize the control
            em.InitModule(
                tab.reference,
                m.reference,
                m.type,
                Config.GetModuleDataVirtualPath(m.type),
                true);

            return em;
        }

        public static Control GetNoAccessControl()
        {
            // No rights, return a error Control
            System.Web.UI.WebControls.Label l = new System.Web.UI.WebControls.Label();
            l.CssClass = "Error";
            l.Text = "Access denied!";
            return l;
        }

        /// <summary>
        /// Returns the requested Tab.
        /// If the user has no right null is returned.
        /// </summary>
        /// <param name="tabRef"></param>
        /// <returns></returns>
        public static PortalDefinition.Tab GetEditTab(string tabRef)
        {
            PortalDefinition.Tab editTab = null;

            if (HttpContext.Current.User.IsInRole(Config.AdminRole))
            {
                PortalDefinition pd = PortalDefinition.Load();
                editTab = pd.GetTab(tabRef);
            }
            return editTab;
        }

        public static string GetEditLink(PortalDefinition.Module module)
        {
          // Link depends on the Edit Type (Inplace / Fullscreen).
          bool isInplace = module.moduleSettings != null && module.moduleSettings.IsInplaceEdit;
          if (isInplace)
          {
            return Config.GetTabUrl(HttpContext.Current.Request["TabRef"]) + "?Edit=Content&ModuleRef="
                                                                        + module.reference + "&TabRef=";
          }
          else
          {
            return "EditPageTable.aspx?ModuleRef=" + module.reference + "&TabRef=" + HttpContext.Current.Request["TabRef"];
          }
        }

        public static string GetCancelEditLink()
        {
          return GetTabLink(HttpContext.Current.Request["TabRef"]);
        }

        public static string GetEditModuleLink(string ModuleRef)
        {
          // Portal Type Table
          return "EditModuleTable.aspx?ModuleRef=" + ModuleRef + "&TabRef=" + HttpContext.Current.Request["TabRef"];
        }

        public static string GetEditTabLink()
        {
            return GetEditTabLink(HttpContext.Current.Request["TabRef"]);
        }

        public static string GetEditTabLink(string tabRef)
        {
          return "default.aspx?Edit=Tab&TabRef=" + tabRef;
        }

        public static string GetTabLink(string reference)
        {
           return Config.GetTabUrl(reference);
        }

        /// <summary>
        /// Checks if this module is requested to appear in edit mode.
        /// </summary>
        public static bool IsEditModuleRequested(PortalDefinition.Module module)
        {
          HttpRequest request = HttpContext.Current.Request;
          string editMode = request["Edit"];
          string moduleRef = request["ModuleRef"];
          bool dataAvailable = !string.IsNullOrEmpty(editMode) && !string.IsNullOrEmpty(moduleRef);
          return dataAvailable  && editMode == "Content" && moduleRef == module.reference
            && module.moduleSettings.HasEditCtrl;
        }
    }
}
