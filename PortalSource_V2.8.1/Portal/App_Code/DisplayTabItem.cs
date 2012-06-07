using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal
{
  /// <summary>
  /// Wrapper Class for the Tab Object.
  /// </summary>
  public class DisplayTabItem
  {
    public DisplayTabItem(PortalDefinition.Tab t, bool currTab)
    {
      m_Text = t.title;
      m_CurrentTab = currTab;
      m_URL = Helper.GetTabLink(t.reference);
      m_Reference = t.reference;

      // Check if image exist.
      if (t.imgPathInactive.Trim() != string.Empty)
      {
        // if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(t.imgPathInactive)))
        m_ImgPathI = t.imgPathInactive;
      }
      if (t.imgPathActive.Trim() != string.Empty)
      {
        // if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(t.imgPathActive)))
        m_ImgPathA = t.imgPathActive;
      }
    }

    public DisplayTabItem()
    {
    }

    /// <summary>
    /// Menus Text
    /// </summary>
    public string Text
    {
      get { return m_Text; }
      set { m_Text = value; }
    }
    /// <summary>
    /// Tabs URL.
    /// </summary>
    public string URL
    {
      get { return m_URL; }
      set { m_URL = value; }
    }

    /// <summary>
    /// Tabs Reference.
    /// </summary>
    public string Reference
    {
      get { return m_Reference; }
      set { m_Reference = value; }
    }


    /// <summary>
    /// Relative Path to the image (Inactive State), if the tab is represented by an image.
    /// </summary>
    public string ImgPathInactive
    {
      get { return m_ImgPathI; }
      set { m_ImgPathI = value; }
    }

    /// <summary>
    /// Relative Path to the image (Active State), if the tab is represented by an image.
    /// </summary>
    public string ImgPathActive
    {
      get 
      {
        if (string.IsNullOrEmpty(m_ImgPathA))
          return m_ImgPathI;
        else
          return m_ImgPathA; 
      }
      set 
      { 
        m_ImgPathA = value; 
      }
    }

    /// <summary>
    /// True if the menu item represents the current Tab
    /// </summary>
    public bool CurrentTab
    {
      get { return m_CurrentTab; }
      set { m_CurrentTab = value; }
    }

    internal string m_Text = "";
    internal string m_URL = "";
    internal string m_Reference = "";
    internal bool m_CurrentTab = false;
    internal string m_ImgPathI = "";
    internal string m_ImgPathA = "";
  }
}