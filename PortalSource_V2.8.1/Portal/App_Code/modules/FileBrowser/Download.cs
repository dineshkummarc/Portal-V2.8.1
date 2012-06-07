using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Portal.Modules.FileBrowser
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für Download
  /// </summary>
  public class Download : IDownloadProcessor
  {
    public Download()
    {
    }

    #region IDownloadProcessor Member

    public void Process(HttpRequest request, HttpResponse response, string moduleType, string ModuleReference)
    {
      string fileName = Portal.API.Config.GetModuleDataPhysicalPath(moduleType) 
                          + "Module_" + ModuleReference + ".config";

      ModuleConfig cfg = ReadModuleConfig(fileName);

      String file = HttpContext.Current.Server.UrlDecode(request.QueryString["File"]);
      // Soll der Download als Attachment ausgeführt werden?
      bool asAttachment = true;
      asAttachment = (0 != Convert.ToInt32(request.QueryString["Att"]));
      DownloadFile(request, response, cfg, file, asAttachment);     
    }

    private static ModuleConfig ReadModuleConfig(string fileName)
    {
      // Laden der Konfiguration. Wenn sie nicht geladen werden kann, wird angegeben, dass die Datei nicht existiert.
      if (!System.IO.File.Exists(fileName))
        throw new FileNotFoundException();

      XmlTextReader xmlReader = null;
      XmlSerializer xmlSerial = new XmlSerializer(typeof(ModuleConfig));
      object cfgObject = null;
      try
      {
        xmlReader = new XmlTextReader(fileName);
        cfgObject = xmlSerial.Deserialize(xmlReader);
        xmlReader.Close();
      }
      catch (Exception e)
      {
        // Konfiguration ungültig, gilt auch als Datei nicht vorhanden.
        throw new FileNotFoundException();
      }
      
      return (ModuleConfig) cfgObject;
    }

    #endregion



    /// <summary>
    /// Bietet die geforderte Datei zum Download an.
    /// </summary>
    /// <param name="file"></param>
    private void DownloadFile(HttpRequest request, HttpResponse response, ModuleConfig cfg, string file, bool asAttachment)
    {
      // Ermitteln des Filewappers der entsprechenden Datei.
      ConfigAgent cfgAgent = new ConfigAgent(cfg);
      FileWrapper dwnFile = cfgAgent.RootDirectory.GetFile(file);

      if (dwnFile != null)
      {
        response.CacheControl = "public";
        response.Cache.SetCacheability(HttpCacheability.Private);
        if (asAttachment)
          response.AddHeader("Content-Disposition", "attachment; filename=" + dwnFile.FileName);
        response.AddHeader("Content-Length", dwnFile.FileSize.ToString());
        String mimeType = dwnFile.MimeType;
        if (mimeType != null)
          response.ContentType = mimeType;
        response.TransmitFile(dwnFile.PhysicalPath);

        if(!Portal.API.Statistics.StatisticHelper.IsBot(request))
        {
          // In der Statistik zwischenspeichern.
          Statistic FbStatistic = new Statistic(cfgAgent.PhysicalRoot);
          FbStatistic.DownloadFile(file);
        }
      }
      else
        throw new FileNotFoundException();
    }

  }
}