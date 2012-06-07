using System;
using System.Web;
using System.IO;

namespace ImageBrowser
{
	public class DirectorySettingsHandler
	{
    string m_szPath = null;
    string m_szDirName = null;
    private DirectorySettingsData dirInfo = null;
    private FilelistSettingsData filelistInfo = null;

		public DirectorySettingsHandler(String szPath, String szDirName)
		{
      m_szPath = szPath;
      m_szDirName = szDirName;
		}

    # region Directory Settings

    /// <summary>
    /// The Caption - of the directory.
    /// </summary>
    public string DirectoryCaption
    {
      get
      {
        if(null == dirInfo)
          LoadDirXml();

        String szCaption;
        if ((dirInfo.DirectorySettings.Rows.Count > 0) && !dirInfo.DirectorySettings[0].IsCaptionNull())
          szCaption = HttpUtility.HtmlDecode(dirInfo.DirectorySettings[0].Caption);
        else 
          szCaption = m_szDirName;
        return szCaption;
      }
      set
      {
        if(null == dirInfo)
          LoadDirXml();

        String szCaption = HttpUtility.HtmlEncode((String) value);
        if(dirInfo.DirectorySettings.Rows.Count == 0)
          dirInfo.DirectorySettings.AddDirectorySettingsRow(szCaption, "", "");
        else
          dirInfo.DirectorySettings[0].Caption = szCaption;
        SaveDirXml();
      }
    }


    /// <summary>
    /// The Description - of the directory.
    /// </summary>
    public string DirectoryDescription
    {
      get
      {
        if(null == dirInfo)
          LoadDirXml();

        String szDescription = "";
        if((dirInfo.DirectorySettings.Rows.Count > 0) && !dirInfo.DirectorySettings[0].IsDescriptionNull())
          szDescription = HttpUtility.HtmlDecode(dirInfo.DirectorySettings[0].Description);
        return szDescription;
      }
      set
      {
        if(null == dirInfo)
          LoadDirXml();

        String szDescription = HttpUtility.HtmlEncode((String) value);
        if(dirInfo.DirectorySettings.Rows.Count == 0)
          dirInfo.DirectorySettings.AddDirectorySettingsRow(m_szDirName, "", szDescription);
        else
          dirInfo.DirectorySettings[0].Description = szDescription;
        SaveDirXml();
      }
    }


    /// <summary>
    /// The Tooltip - of the directory.
    /// </summary>
    public string DirectoryTooltip
    {
      get
      {
        if(null == dirInfo)
          LoadDirXml();

        String szTooltip = "";
        if ((dirInfo.DirectorySettings.Rows.Count > 0) && !dirInfo.DirectorySettings[0].IsTooltipNull())
          szTooltip = HttpUtility.HtmlDecode(dirInfo.DirectorySettings[0].Tooltip);
        return szTooltip;
      }
      set
      {
        if(null == dirInfo)
          LoadDirXml();

        String szTooltip = HttpUtility.HtmlEncode((String) value);
        if(dirInfo.DirectorySettings.Rows.Count == 0)
          dirInfo.DirectorySettings.AddDirectorySettingsRow(m_szDirName, szTooltip, "");
        else
          dirInfo.DirectorySettings[0].Tooltip = szTooltip;
        SaveDirXml();
      }
    }

    #endregion


    #region Filelist Settings.

    
    /// <summary>
    /// Gets the Caption of the file.
    /// </summary>
    public string GetFileCaption(String szFilename)
    {
      // Search the row.
      FilelistSettingsData.FilelistSettingsRow fileData = GetFileData(szFilename);

      if(fileData != null)
        return HttpUtility.HtmlDecode(fileData.Caption);
      else
        return "";
    }


    /// <summary>
    /// Sets the Caption of the file.
    /// </summary>
    public void SetFileCaption(String szFilename, String szNewCaption)
    {
      // Search the row.
      FilelistSettingsData.FilelistSettingsRow fileData = GetFileData(szFilename);

      String szCaption = HttpUtility.HtmlEncode(szNewCaption);
      if(fileData != null)
        fileData.Caption = szCaption;
      else
        filelistInfo.FilelistSettings.AddFilelistSettingsRow(szFilename, szCaption, "", "");
      SaveFilelistXml();
    }


    /// <summary>
    /// Gets the Tooltip of the file.
    /// </summary>
    public string GetFileTooltip(String szFilename)
    {
      // Search the row.
      FilelistSettingsData.FilelistSettingsRow fileData = GetFileData(szFilename);

      if(fileData != null)
        return HttpUtility.HtmlDecode(fileData.Tooltip);
      else
        return "";
    }


    /// <summary>
    /// Sets the Tooltip of the file.
    /// </summary>
    public void SetFileTooltip(String szFilename, String szNewTooltip)
    {
      // Search the row.
      FilelistSettingsData.FilelistSettingsRow fileData = GetFileData(szFilename);

      String szTooltip = HttpUtility.HtmlEncode(szNewTooltip);
      if(fileData != null)
        fileData.Tooltip = szTooltip;
      else
        filelistInfo.FilelistSettings.AddFilelistSettingsRow(szFilename, "", szTooltip, "");
      SaveFilelistXml();
    }


    /// <summary>
    /// Gets the Description of the file.
    /// </summary>
    public string GetFileDescription(String szFilename)
    {
      // Search the row.
      FilelistSettingsData.FilelistSettingsRow fileData = GetFileData(szFilename);

      if(fileData != null)
        return HttpUtility.HtmlDecode(fileData.Description);
      else
        return "";
    }


    /// <summary>
    /// Sets the Description of the file.
    /// </summary>
    public void SetFileDescription(String szFilename, String szNewDescription)
    {
      // Search the row.
      FilelistSettingsData.FilelistSettingsRow fileData = GetFileData(szFilename);

      String szDescription = HttpUtility.HtmlEncode(szNewDescription);
      if(fileData != null)
        fileData.Description = szDescription;
      else
        filelistInfo.FilelistSettings.AddFilelistSettingsRow(szFilename, "", "", szDescription);
      SaveFilelistXml();
    }



    private FilelistSettingsData.FilelistSettingsRow GetFileData(String szFilename)
    {
      // Load the Xml if necessary.
      if(null == filelistInfo)
        LoadFilelistXml();

      // Search the row.
      return (FilelistSettingsData.FilelistSettingsRow) filelistInfo.FilelistSettings.Rows.Find(szFilename);
    }

    #endregion

    #region XML Save an Load.

    private void LoadDirXml()
    {
      dirInfo = new DirectorySettingsData();
      string cfgPath = m_szPath + "/_dirconfig.xml";
      if(File.Exists(cfgPath))
      {
        dirInfo.ReadXml(cfgPath);    
      }
    }

    private void SaveDirXml()
    {
      if(null != dirInfo)
      {
        string cfgPath = m_szPath + "/_dirconfig.xml";
        dirInfo.WriteXml(cfgPath);
      }
    }

    private void LoadFilelistXml()
    {
      filelistInfo = new FilelistSettingsData();
      string cfgPath = m_szPath + "/_fileconfig.xml";
      if(File.Exists(cfgPath))
      {
        filelistInfo.ReadXml(cfgPath);    
      }
    }

    private void SaveFilelistXml()
    {
      if(null != filelistInfo)
      {
        string cfgPath = m_szPath + "/_fileconfig.xml";
        filelistInfo.WriteXml(cfgPath);
      }
    }

    #endregion
	}


}
