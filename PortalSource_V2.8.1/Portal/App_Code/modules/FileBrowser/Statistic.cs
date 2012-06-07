using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.FileBrowser
{
  /// <summary>
  /// Erfasst spezifische Statistiken im Bereich des FileBrowsers.
  /// </summary>
  public class Statistic
  {
    private string dataPath;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataPath">Physikalischen Root Pfad des Daten-Verzeichnis. </param>
    public Statistic(string dataPath)
    {
      this.dataPath = dataPath;
      if (!this.dataPath.EndsWith("\\"))
        this.dataPath += "\\";
    }

    /// <summary>
    /// Erfasst einen Download einer Datei.
    /// </summary>
    /// <param name="fileName"></param>
    public void DownloadFile(string fileName)
    {
      lock(new object())
      {
        FileBrowserStatistic stat = GetAllData();
        FileBrowserStatistic.DownloadsRow fileData = stat.Downloads.FindByFile(fileName);

        if (fileData == null)
        {
          fileData = stat.Downloads.NewDownloadsRow();
          fileData.File = fileName;
          stat.Downloads.AddDownloadsRow(fileData);
        }

        fileData.NumOfDownloads++;
        fileData.LastDownload = DateTime.Now;

        SaveAllData(stat);
      }
    }

    private FileBrowserStatistic GetAllData()
    {
      FileBrowserStatistic data = new FileBrowserStatistic();

      // Existiert das File, wird es eingelesen.
      if (System.IO.File.Exists(DataFile))
        data.ReadXml(DataFile);

      return data;
    }

    private void SaveAllData(FileBrowserStatistic data)
    {
      data.WriteXml(DataFile);
    }

    private string DataFile
    {
      get { return this.dataPath + "_statistic.xml"; }
    }
  }
}