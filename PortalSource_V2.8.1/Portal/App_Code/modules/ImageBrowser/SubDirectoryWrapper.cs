using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using System.Collections;
using System.Globalization;

namespace ImageBrowser.Entities
{
  /// <summary>
  /// Wraps a directory object ( not to be confused with the directory that your in.
  /// </summary>
  public class SubDirectoryWrapper
  {
    private string directory;
    private string defaultImage;
    private ImageTools imageTools = null;
    private DirectorySettingsHandler dirSettings = null;

    public SubDirectoryWrapper(string dir, string defaultFolderImage, ImageTools imgTools)
    {
      directory = dir;
      defaultImage = defaultFolderImage;
      imageTools = imgTools;

      string[] dirs = directory.Replace(@"\","/").Split('/');
      dirSettings = new DirectorySettingsHandler(imageTools.cfg.PictureRootDirectory + "/" + directory, 
        dirs[dirs.Length - 1]);
    }

    /// <summary>
    /// Name of the subdirectory
    /// </summary>
    public string Caption
    {
      get
      {
        return dirSettings.DirectoryCaption;
      }
    }


    /// <summary>
    /// Tooltip for the subdirectory.
    /// </summary>
    public string Tooltip
    {
      get
      {
        return dirSettings.DirectoryTooltip;
      }
    }

    /// <summary>
    /// Image to use to represent the subdirectory
    /// </summary>
    public string Src
    {
      get
      {
        String file = PreviewSrc;
        if ( file != null )
        {
          string[] filename = file.Replace(@"\",@"/").Split('/');

          string thumb = string.Join("/",filename,0,filename.Length - 1) + "/thumbs/" + filename[filename.Length - 1];

          imageTools.CreateImage(file,thumb, imageTools.cfg.ThumbCfg, imageTools.cfg.ThumbCfg.Shadow, false, imageTools.cfg.ThumbCfg.JpegQuality);
          return imageTools.cfg.PictureVirtualDirectory + thumb;
        }
        return defaultImage;
      }
    }

    protected string PreviewSrc
    {
      get
      {
        string file = null;
        string parent = imageTools.cfg.PictureRootDirectory + @"\" + directory;
        string[] files = Directory.GetFiles( parent, @"_dirimage.*");

        if ( files.Length > 0 )
        {
          foreach ( string s in files )
          {
            string extension = null;
            if (s.IndexOf(".") > 0)
            {
              string[] parts = s.Split('.');
              extension = parts[parts.Length - 1];
            }
            if ( extension == null ) continue;

            extension = extension.ToLower(CultureInfo.InvariantCulture);

            if ( extension == "jpg" ||
              extension == "png" ||
              extension == "gif" )
            {
              string[] filepath = s.Replace(@"\","/").Split('/');
              file = directory + "/" + filepath[filepath.Length - 1];
              break;
            }
          }
        }
        return file;
      }
    }

    /// <summary>
    /// The link to use to get to the subdirectory
    /// </summary>
    public string HREF
    {
      get
      {
        return imageTools.cfg.PictureVirtualDirectory + directory;
      }
    }


    /// <summary>
    /// Updates the Thumbnail in the cache.
    /// </summary>
    public void UpdateCache()
    {
      String file = PreviewSrc;
      if(file != null)
      {
        string[] filename = file.Replace(@"\",@"/").Split('/');

        string thumb = string.Join("/",filename,0,filename.Length - 1) + "/thumbs/" + filename[filename.Length - 1];

        imageTools.CreateImage(file, thumb, imageTools.cfg.ThumbCfg, imageTools.cfg.ThumbCfg.Shadow, true, imageTools.cfg.ThumbCfg.JpegQuality);
      }
    }

  }
}