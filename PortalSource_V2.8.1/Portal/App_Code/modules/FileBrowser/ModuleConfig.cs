using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

namespace Portal.Modules.FileBrowser
{
  [XmlRoot("FileBrowser")]
  public class ModuleConfig
  {
    /// <summary>
    /// Definiert, das virtuelle Root Verzeichnis (Relativ zum ApplicationRoot).
    /// </summary>
    [XmlElement("VirtualRoot")]
    public String VirtualRoot = Portal.API.Config.GetModuleDataVirtualPath() + "Download";

    [XmlElement("SortProperty")]
    public SortProperty SortProperty = SortProperty.Name;

    [XmlElement("SortAscending")]
    public bool SortDirectionAsc = true;
  }
}
