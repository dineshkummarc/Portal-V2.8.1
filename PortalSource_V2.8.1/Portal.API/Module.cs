using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace Portal.API
{
    /// <summary>
    /// Base class for each Portal Module Control. Provides the current Tab and Module Definition.
    /// </summary>
    public class Module : UserControl
    {
        /// <summary>
        /// Parent Tab
        /// </summary>
        private string m_TabRef = "";
        /// <summary>
        /// Module Definition from the Portal Definition
        /// </summary>
        private string m_ModuleRef = "";
        /// <summary>
        /// Modules virtual base path.
        /// </summary>
        private string m_ModuleVirtualPath = "";
        /// <summary>
        /// Modules Type.
        /// </summary>
        private string m_ModuleType = "";

        private bool m_HasEditRights;

        /// <summary>
        /// Initializes the Control. Called by the Protal Framework
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="module"></param>
        /// <param name="virtualPath"></param>
        public void InitModule(string tabRef, string moduleRef, string type, string virtualPath, bool hasEditRights)
        {
            m_TabRef = tabRef;
            m_ModuleRef = moduleRef;
            m_ModuleVirtualPath = virtualPath;
            m_HasEditRights = hasEditRights;
            m_ModuleType = type;
        }

        /// <summary>
        /// The Module can control its visibility. The Login Module does so
        /// </summary>
        /// <returns>true if the Module should be visible</returns>
        public virtual bool IsVisible()
        {
            return true;
        }

        /// <summary>
        /// The current Tab reference. Readonly
        /// </summary>
        public string TabRef
        {
            get
            {
                return m_TabRef;
            }
        }
        /// <summary>
        /// The Modules reference. Readonly
        /// </summary>
        public string ModuleRef
        {
            get
            {
                return m_ModuleRef;
            }
        }
        /// <summary>
        /// The Modules type. Readonly
        /// </summary>
        public string ModuleType
        {
            get
            {
                return m_ModuleType;
            }
        }
        /// <summary>
        /// Modules virtual base path. Readonly
        /// </summary>
        public string ModuleDataVirtualPath
        {
            get { return m_ModuleVirtualPath; }
        }
        /// <summary>
        /// Modules physical base path. Readonly
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(true)]
        public string ModuleDataPhysicalPath
        {
            get
            {
                try
                {
                    String ModPhysPath = Server.MapPath(m_ModuleVirtualPath);
                    if (!ModPhysPath.EndsWith("\\"))    // XP Home does not include a trailing backslash.
                        ModPhysPath += "\\";

                    // Make sure the directory exists.
                    if (!System.IO.Directory.Exists(ModPhysPath))
                      System.IO.Directory.CreateDirectory(ModPhysPath);

                    return ModPhysPath;
                }
                catch (ArgumentNullException)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Build a URL to the current Page. Use this method to implement Modules that needs URL Parameter.
        /// </summary>
        /// <param name="parameter">URL Parameter.</param>
        /// <returns>URL with parameter</returns>
        /// <example>Response.Redirect(BuildURL("dir=myPhotos&size=large"));</example>
        public string BuildUrl(string parameter)
        {
            if (null == parameter)
                parameter = "";

            string p = "";
            if (!parameter.StartsWith("&"))
            {
                p = "&" + parameter;
            }
            else
            {
                p = parameter;
            }
            return Config.GetTabUrl(TabRef) + p;
        }

        public bool ModuleHasEditRights
        {
            get { return m_HasEditRights; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ModuleConfigFile
        {
            get
            {
                return ModuleDataPhysicalPath + "Module_" + ModuleRef + ".config";
            }
        }

        public string ModuleConfigSchemaFile
        {
            get
            {
                return Server.MapPath(this.AppRelativeTemplateSourceDirectory + "/" + "Module.xsd");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public object ReadCommonConfig(System.Type t)
        {
            string fileName = ModuleDataPhysicalPath + "/Module.config";

            if (!System.IO.File.Exists(fileName))
                return null;

            XmlTextReader xmlReader = null;
            XmlSerializer xmlSerial = new XmlSerializer(t);
            object o = null;
            Exception e = null;
            try
            {
                xmlReader = new XmlTextReader(fileName);
                o = xmlSerial.Deserialize(xmlReader);
                xmlReader.Close();
                return o;
            }
            catch (FileNotFoundException ex) { e = ex; }
            catch (DirectoryNotFoundException ex) { e = ex; }
            catch (UriFormatException ex) { e = ex; }
            catch (WebException ex) { e = ex; }
            catch (InvalidOperationException ex) { e = ex; }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                // Do not throw exceptions
                Trace.Warn("Module", Language.GetText("ErrorLoadingModulesCommonConfig"), e);
            }

            return o;
        }

        /// <summary>
        /// Reads the config file. The schema is optional.
        /// </summary>
        /// <returns>
        /// Null if config file does not exists and no schema exists,
        /// else the schema is read and a empty DataSet is returned.
        /// </returns>
        public DataSet ReadConfig()
        {
            DataSet ds = null;
            try
            {
                if (System.IO.File.Exists(ModuleConfigFile))
                {
                    ds = new DataSet();
                    if (System.IO.File.Exists(ModuleConfigSchemaFile))
                    {
                        ds.ReadXmlSchema(ModuleConfigSchemaFile);
                    }
                    ds.ReadXml(ModuleConfigFile);
                }
            }
            catch (SecurityException e)
            {
                // Do not throw exceptions
                Trace.Warn("Module", Language.GetText("ErrorLoadingModulesConfigDataset"), e);
                return null;
            }

            if (ds == null && System.IO.File.Exists(ModuleConfigSchemaFile))
            {
                ds = new DataSet();
                ds.ReadXmlSchema(ModuleConfigSchemaFile);
            }

            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public object ReadConfig(System.Type t)
        {
            return ReadConfig(t, ModuleConfigFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public object ReadConfig(System.Type t, string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return null;

            XmlTextReader xmlReader = null;
            XmlSerializer xmlSerial = new XmlSerializer(t);
            object o = null;
            Exception e = null;
            try
            {
                xmlReader = new XmlTextReader(fileName);
                o = xmlSerial.Deserialize(xmlReader);
                xmlReader.Close();
            }
            catch (FileNotFoundException ex) { e = ex; }
            catch (DirectoryNotFoundException ex) { e = ex; }
            catch (UriFormatException ex) { e = ex; }
            catch (WebException ex) { e = ex; }
            catch (InvalidOperationException ex) { e = ex; }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                // Do not throw exceptions
                Trace.Warn("Module", Language.GetText("ErrorLoadingModulesConfig"), e);
            }

            return o;
        }

        public void WriteConfig(DataSet ds)
        {
            if (null == ds)
                throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "ds");
            try
            {
                ds.WriteXml(ModuleConfigFile);
            }
            catch (SecurityException e)
            {
                // Do not throw exceptions
                Trace.Warn("Module", Language.GetText("ErrorWritingModulesConfigDataset"), e);
            }
        }

        public void WriteConfig(object o)
        {
            WriteConfig(o, ModuleConfigFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="fileName"></param>
        public void WriteConfig(object o, string fileName)
        {
            if (null == o)
                return;

            XmlTextWriter xmlWriter = null;
            XmlSerializer xmlSerial = new XmlSerializer(o.GetType());
            Exception e = null;
            try
            {
              // Make sure the directory exists.
              System.IO.FileInfo fileInfo = new FileInfo(fileName);
              if (!fileInfo.Directory.Exists)
                System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName);

              xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
              xmlWriter.Formatting = Formatting.Indented;
              xmlSerial.Serialize(xmlWriter, o);
              xmlWriter.Close();
            }
            catch (ArgumentNullException ex) { e = ex; }
            catch (ArgumentException ex) { e = ex; }
            catch (DirectoryNotFoundException ex) { e = ex; }
            catch (UnauthorizedAccessException ex) { e = ex; }
            catch (SecurityException ex) { e = ex; }
            catch (IOException ex) { e = ex; }
            finally
            {
                if (xmlWriter != null)
                {
                    xmlWriter.Close();
                }
                // Do not throw exceptions
                Trace.Warn("Module", Language.GetText("ErrorWritingModulesConfig"), e);
            }
        }

        public static Module GetModuleControl(Control ctrl)
        {
            if (null == ctrl) return null;

            if (ctrl.GetType().IsSubclassOf(typeof(Module)))
                return (Module)ctrl;

            if (ctrl.Parent == null)
                return null;

            return GetModuleControl(ctrl.Parent);
        }
      }

    /// <summary>
    /// Base class for each Portal Edit Module Control. Derived from Module
    /// </summary>
    public class EditModule : Module
    {
        /// <summary>
        /// Returns the callers URL. 
        /// </summary>
        /// <returns>The BackURL</returns>
        public string BackUrl
        {
            get
            {
                return Config.GetTabUrl(TabRef);
            }
        }

        /// <summary>
        /// Redirects back to the callers URL. Uses the GetBackURL() Method.
        /// </summary>
        public void RedirectBack()
        {
            Response.Redirect(BackUrl);
        }
    }
}
