using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

namespace Portal.Modules.Forum
{
  [XmlRoot("Forum")]
  public class ModuleConfig
  {
    /// <summary>
    /// Definiert, wer die Berechtigung zur Erstellung eines neuen Threads hat.
    /// 0 = Everyone
    /// 1 = User
    /// 2 = Module Admin
    /// </summary>
    [XmlElement("ThreadCreationRight")]
    public int ThreadCreationRight = 0;


    /// <summary>
    /// Definiert, ob f�r Top-Level Beitr�ge der HTML Editor verwendet werden soll.
    /// </summary>
    [XmlElement("UseHTMLEditorOnTopLevel")]
    public bool UseHTMLEditorOnTopLevel = false;

  
    /// <summary>
    /// Definiert, ob f�r  Beitr�ge der restlichen Ebenen der HTML Editor verwendet werden soll.
    /// </summary>
    [XmlElement("UseHTMLEditorOnLowerLevel")]
    public bool UseHTMLEditorOnLowerLevel = false;
  }
}
