using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;

namespace Portal
{
  /// <summary>
  /// Manages the PortalDefinition.
  /// </summary>
  [XmlRoot("portal"), Serializable]
  public class PortalDefinition
  {
    private static XmlSerializer xmlPortalDef = new XmlSerializer(typeof(PortalDefinition));

    public PortalDefinition()
    {
    }

    private Tab InternalGetTab(ArrayList tabList, string reference)
    {
      reference = reference.ToLower(CultureInfo.InvariantCulture);
      if (tabList == null) return null;

      foreach (Tab t in tabList)
      {
        if (string.Compare(t.reference, reference, true, CultureInfo.InvariantCulture) == 0)
        {
          if (UserManagement.HasViewRights(HttpContext.Current.User, t.roles))
            return t;
          else
            return null;
        }
        Tab tb = InternalGetTab(t.tabs, reference);
        if (tb != null) return tb;
      }

      return null;
    }

    /// <summary>
    /// Returns a Tab by a reference. 
    /// If not reference is provided it returns the default (first) Tab.
    /// </summary>
    /// <param name="reference">Tabs reference</param>
    /// <returns>null if Tab not found or the default Tab if no reference is provided</returns>
    public Tab GetTab(string reference)
    {
      Tab currTab = null;
      if (!string.IsNullOrEmpty(reference))
        currTab = InternalGetTab(tabs, reference);

      if (currTab == null)
      {
        for (int i = 0; i < tabs.Count; i++)
        {
          currTab = (Tab)tabs[i];
          if (UserManagement.HasViewRights(HttpContext.Current.User, currTab.roles))
            return currTab;
        }
      }

      return currTab;
    }

    /// <summary>
    /// Returns the current Tab. 
    /// The current HTTPContext is used to determinate the current Tab (TabRef=[ref])
    /// </summary>
    /// <returns>The current Tab or the default Tab</returns>
    public static Tab CurrentTab
    {
      get
      {
        PortalDefinition pd = Load();
        return pd.GetTab(HttpContext.Current.Request["TabRef"]);
      }
    }

    public static void DeleteTab(string reference)
    {
      PortalDefinition pd = PortalDefinition.Load();
      PortalDefinition.Tab t = pd.GetTab(reference);

      if (t.parent == null) // Root Tab
      {
        for (int i = 0; i < pd.tabs.Count; i++)
        {
          if (((PortalDefinition.Tab)pd.tabs[i]).reference == reference)
          {
            pd.tabs.RemoveAt(i);
            break;
          }
        }
      }
      else
      {
        PortalDefinition.Tab pt = t.parent;
        for (int i = 0; i < pt.tabs.Count; i++)
        {
          if (((PortalDefinition.Tab)pt.tabs[i]).reference == reference)
          {
            pt.tabs.RemoveAt(i);
            break;
          }
        }
      }

      pd.Save();
    }

    /// <summary>
    /// Sets the parent Tab object
    /// </summary>
    /// <param name="tabs">Collection of child tabs</param>
    /// <param name="parent">Parrent Tab or null if there is no parrent Tab (root collection)</param>
    public static void UpdatePortalDefinitionProperties(ArrayList tabs, Tab parent)
    {
      if (tabs == null) return;

      foreach (Tab t in tabs)
      {
        t.parent = parent;
        UpdatePortalDefinitionProperties(t.tabs, t);
      }
    }

    /// <summary>
    /// Throws an PortalException if the Data in not valid
    /// </summary>
    public void IsValid()
    {
      try
      {
        Hashtable tabRefList = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        foreach (Tab t in tabs)
        {
          t.IsValid(tabRefList);
        }
      }
      catch
      {
        System.Web.HttpContext.Current.Cache.Remove("PortalSettings");
        throw;
      }
    }

    /// <summary>
    /// Loads the Portal Definition
    /// </summary>
    /// <returns>Portal Definition</returns>
    public static PortalDefinition Load()
    {
      // Lookup in Cache
      PortalDefinition pd = (PortalDefinition)System.Web.HttpContext.Current.Cache["PortalSettings"];
      if (pd != null) return pd;

      // Load Portaldefinition
      XmlTextReader xmlReader = null;
      try
      {
        xmlReader = new XmlTextReader(Config.PortalDefinitionPhysicalPath);
        pd = (PortalDefinition)xmlPortalDef.Deserialize(xmlReader);

        UpdatePortalDefinitionProperties(pd.tabs, null);

        // Add to Cache
        System.Web.HttpContext.Current.Cache.Insert("PortalSettings", pd,
  new System.Web.Caching.CacheDependency(Config.PortalDefinitionPhysicalPath));
      }
      finally
      {
        if (xmlReader != null)
        {
          xmlReader.Close();
        }
      }

      return pd;
    }

    public void Save()
    {
      IsValid();

      XmlTextWriter xmlWriter = null;
      try
      {
        xmlWriter = new XmlTextWriter(Config.PortalDefinitionPhysicalPath, System.Text.Encoding.UTF8);
        xmlWriter.Formatting = Formatting.Indented;
        xmlPortalDef.Serialize(xmlWriter, this);
      }
      finally
      {
        if (xmlWriter != null)
        {
          xmlWriter.Close();
        }
      }
    }

    /// <summary>
    /// Array of root Tabs
    /// </summary>
    [XmlArray("tabs"), XmlArrayItem("tab", typeof(Tab))]
    public ArrayList tabs = new ArrayList();

    /// <summary>
    /// Tab Definition Object
    /// </summary>
    [Serializable]
    public class Tab
    {
      /// <summary>
      /// Parent Tab. null if it is a root Tab
      /// </summary>
      [XmlIgnore]
      public Tab parent;

      /// <summary>
      /// Tabs reference. Must be unique!
      /// </summary>
      [XmlAttribute("ref")]
      public string reference = "";

      /// <summary>
      /// Tabs title.
      /// </summary>
      [XmlElement("title")]
      public string title = "";

      /// <summary>
      /// Relative path to an image that represents the inactive Tab. (optional).
      /// </summary>
      [XmlElement("imgPathInactive")]
      public string imgPathInactive = "";

      /// <summary>
      /// Relative path to an image that represents the active tab. (optional).
      /// </summary>
      [XmlElement("imgPathActive")]
      public string imgPathActive = "";

      /// <summary>
      /// Collection of view and edit roles.
      /// A View Role is represented by a 'ViewRole' class, a Edit Role by a 'EditRole' class
      /// </summary>
      [XmlArray("roles"),
      XmlArrayItem("view", typeof(ViewRole)),
      XmlArrayItem("edit", typeof(EditRole))]
      public ArrayList roles = new ArrayList();

      /// <summary>
      /// Sub Tab collection.
      /// </summary>
      [XmlArray("tabs"), XmlArrayItem("tab", typeof(Tab))]
      public ArrayList tabs = new ArrayList();

      /// <summary>
      /// Left Modules collection
      /// </summary>
      [XmlArray("left"), XmlArrayItem("module", typeof(Module))]
      public ArrayList left = new ArrayList();

      /// <summary>
      /// Middle Modules collection
      /// </summary>
      [XmlArray("middle"), XmlArrayItem("module", typeof(Module))]
      public ArrayList middle = new ArrayList();

      /// <summary>
      /// Right Modules collection
      /// </summary>
      [XmlArray("right"), XmlArrayItem("module", typeof(Module))]
      public ArrayList right = new ArrayList();

      /// <summary>
      /// Returns the Tabs root Tab or this if it is a root Tab
      /// </summary>
      /// <returns>Root Tab or this</returns>
      public Tab RootTab
      {
        get
        {
          if (parent == null) return this;
          return parent.RootTab;
        }
      }

      /// <summary>
      /// Returns a Tabs Module by reference
      /// </summary>
      /// <param name="ModuleRef">Module reference</param>
      /// <returns>Module or null</returns>
      public Module GetModule(string moduleRef)
      {
        if (null == moduleRef)
          throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "moduleRef");

        moduleRef = moduleRef.ToLower(CultureInfo.InvariantCulture);
        foreach (Module m in left)
        {
          if (string.Compare(m.reference, moduleRef, true, CultureInfo.InvariantCulture) == 0)
          {
            return m;
          }
        }
        foreach (Module m in middle)
        {
          if (string.Compare(m.reference, moduleRef, true, CultureInfo.InvariantCulture) == 0)
          {
            return m;
          }
        }
        foreach (Module m in right)
        {
          if (string.Compare(m.reference, moduleRef, true, CultureInfo.InvariantCulture) == 0)
          {
            return m;
          }
        }

        return null;
      }

      public bool DeleteModule(string ModuleRef)
      {
        for (int i = 0; i < left.Count; i++)
        {
          if (((PortalDefinition.Module)left[i]).reference == ModuleRef)
          {
            left.RemoveAt(i);
            return true;
          }
        }
        for (int i = 0; i < middle.Count; i++)
        {
          if (((PortalDefinition.Module)middle[i]).reference == ModuleRef)
          {
            middle.RemoveAt(i);
            return true;
          }
        }
        for (int i = 0; i < right.Count; i++)
        {
          if (((PortalDefinition.Module)right[i]).reference == ModuleRef)
          {
            right.RemoveAt(i);
            return true;
          }
        }
        return false;
      }

      public void IsValid(Hashtable tabRefList)
      {
        if (null == tabRefList)
          throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "tabRefList");

        if (tabRefList.ContainsKey(reference))
        {
          throw new PortalException(Language.GetText("exception_DuplicateTabReferenceFound"));
        }
        tabRefList.Add(reference, reference);

        foreach (Tab t in tabs)
        {
          t.IsValid(tabRefList);
        }

        Hashtable moduleRefList = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

        foreach (Module m in left)
        {
          m.IsValid(moduleRefList);
        }
        foreach (Module m in middle)
        {
          m.IsValid(moduleRefList);
        }
        foreach (Module m in right)
        {
          m.IsValid(moduleRefList);
        }
      }

      public static Tab Create()
      {
        Tab t = new Tab();
        t.reference = Guid.NewGuid().ToString();
        t.title = Language.GetText("NewTabTitle");

        return t;
      }
    }

    /// <summary>
    /// Module Definition Object
    /// </summary>
    [Serializable]
    public class Module
    {
      /// <summary>
      /// Modules reference
      /// </summary>
      [XmlAttribute("ref")]
      public string reference = "";

      /// <summary>
      /// Modules Title
      /// </summary>
      [XmlElement("title")]
      public string title = "";

      /// <summary>
      /// Modules Ctrl Type
      /// </summary>
      [XmlElement("type")]
      public string type = "";

      /// <summary>
      /// Collection of view and edit roles.
      /// A View Role is represented by a 'ViewRole' class, a Edit Role by a 'EditRole' class
      /// </summary>
      [XmlArray("roles"),
       XmlArrayItem("view", typeof(ViewRole)),
       XmlArrayItem("edit", typeof(EditRole))]
      public ArrayList roles = new ArrayList();

      /// <summary>
      /// Module Settings Object. Loaded by the internal Method 'LoadModuleSettings'
      /// </summary>
      [XmlIgnore]
      public ModuleSettings moduleSettings;

      /// <summary>
      /// Loads the Modules Settings represented by the 'ModuleSettings.config' File.
      /// Called by the Methods 'Helper.GetEditControl()' and 'PortalTab.RenderModules()'
      /// </summary>
      public void LoadModuleSettings()
      {
        string path = Config.GetModulePhysicalPath(type) + "ModuleSettings.config";
        if (File.Exists(path))
        {
          // Lookup in Cache
          string msk = "ModuleSettings_" + path;
          moduleSettings = (ModuleSettings)System.Web.HttpContext.Current.Cache[msk];
          if (moduleSettings != null) return;

          XmlTextReader xmlReader = new XmlTextReader(path);
          moduleSettings = (ModuleSettings)ModuleSettings.XmlModuleSettings.Deserialize(xmlReader);
          xmlReader.Close();

          // Add to Cache
          System.Web.HttpContext.Current.Cache.Insert(msk, moduleSettings,
            new System.Web.Caching.CacheDependency(path));
        }
        else
        {
          moduleSettings = null;
        }
      }

      public void IsValid(Hashtable moduleRefList)
      {
        if (null == moduleRefList)
          throw new ArgumentException(Language.GetText("exception_NullReferenceParameter"), "moduleRefList");

        if (moduleRefList.ContainsKey(reference))
        {
          throw new PortalException(Language.GetText("exception_DuplicateModuleReferenceFound"));
        }
        moduleRefList.Add(reference, reference);
      }

      public static Module Create()
      {
        PortalDefinition.Module m = new PortalDefinition.Module();
        m.reference = Guid.NewGuid().ToString();
        m.type = "HtmlEdit";

        return m;
      }
    }
  }

  /// <summary>
  /// Base class of a Role. Abstract.
  /// </summary>
  [Serializable]
  public abstract class Role
  {
    /// <summary>
    /// Name of the Role
    /// </summary>
    [XmlText]
    public string name = "";
  }

  /// <summary>
  /// Edit Role. Derived from Role.
  /// </summary>
  [Serializable]
  public class EditRole : Role
  {
  }
  /// <summary>
  /// View Role. Derived from Role.
  /// </summary>
  [Serializable]
  public class ViewRole : Role
  {
  }

  /// <summary>
  /// Module Settings Object. Loaded from the Modules 'ModulesSettings.xml' File.
  /// </summary>
  [XmlRoot("module"), Serializable]
  public class ModuleSettings
  {
    private static XmlSerializer xmlModuleSettings = new XmlSerializer(typeof(ModuleSettings));

    /// <summary>
    /// Returns the modules XmlSerializer.
    /// </summary>
    public static XmlSerializer XmlModuleSettings
    {
      get
      {
        return xmlModuleSettings;
      }
    }

    /// <summary>
    /// Modules View .ascx Control.
    /// </summary>
    [XmlElement("ctrl")]
    public string ctrl = "";

    /// <summary>
    /// Modules Edit .ascx Control. 'none' if the Module has no Edit Control.
    /// </summary>
    [XmlElement("editCtrl")]
    public EditControl editCtrl = new EditControl();

    /// <summary>
    /// How the module appears in edit mode. ("fullscreen" / "inplace"). (case sensitive!)
    /// </summary>
    [XmlIgnore]
    public bool IsInplaceEdit
    {
      get { return editCtrl.editType == "inplace"; }
    }

    /// <summary>
    /// True if the Module has no Edit Control. Property editCtrl mus be set to 'none' (case sensitive!)
    /// </summary>
    [XmlIgnore]
    public bool HasEditCtrl
    {
      get { return editCtrl.value != "none"; }
    }

    [Serializable]
    public class EditControl
    {
      [XmlText]
      public string value = "";

      /// <summary>
      /// How the module appears in edit mode. ("fullscreen" / "inplace"). (case sensitive!)
      /// </summary>
      [XmlAttribute("editType")]
      public string editType = "";

      public override string ToString()
      {
        return value;
      }
    }
  }
}
