using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using System.Collections;

namespace ImageBrowser.Entities
{
  /// <summary>
  /// Wraps an image object for when you ar looking at it in a directory
  /// </summary>
  public class ImageWrapper
  {
    private string file;
    private string name;
    private ImageTools imageTools = null;
    private DirectorySettingsHandler dirSettings = null;

    public ImageWrapper(string fileName, ImageTools imgTools, DirectorySettingsHandler dirSettingsHandler)
    {
      imageTools = imgTools;
      file = fileName;
      string[] dirs = file.Replace(@"\","/").Split('/');
      name = dirs[dirs.Length - 1];
      dirSettings = dirSettingsHandler;
    }
	
    /// <summary>
    /// Name of the file
    /// </summary>
    public string Name
    {
      get
      {
        return name;
      }
    }

    /// <summary>
    /// Link to use when they click on it
    /// This will take them to a 'web friendly' version of the image
    /// </summary>
    public string WebImageHref
    {
      get
      {
        string[] name = file.Split('/');

        string thumb = string.Join("/",name,0,name.Length - 1) + "/webpics/" + name[name.Length - 1];

        string path = imageTools.GetPath(thumb);
        imageTools.CreateImage(imageTools.GetPath(file), path, imageTools.cfg.PreviewCfg, false, false, imageTools.cfg.PreviewCfg.JpegQuality);
				
        return imageTools.cfg.PictureVirtualDirectory + path;
      }
    }

    public string WebImagePath
    {
      get
      {
        string[] name = file.Split('/');
        string thumb = string.Join("\\",name,0,name.Length - 1) + "\\webpics\\" + name[name.Length - 1];
        return imageTools.GetPath(imageTools.cfg.PictureRootDirectory + imageTools.GetPath(thumb));
      }
    }

    /// <summary>
    /// The path of the orgional image
    /// </summary>
    public string FullImageHref
    {
      get
      {
        return imageTools.cfg.PictureVirtualDirectory + imageTools.GetPath(file);
      }
    }


    /// <summary>
    /// The path of the thumbnail
    /// </summary>
    public string ThumbHref
    {
      get
      {
        string[] name = file.Split('/');

        string thumb = string.Join("/",name,0,name.Length - 1) + "/thumbs/" + name[name.Length - 1];

        imageTools.CreateImage(imageTools.GetPath(file),imageTools.GetPath(thumb), imageTools.cfg.ThumbCfg,
          imageTools.cfg.ThumbCfg.Shadow, false, imageTools.cfg.ThumbCfg.JpegQuality);

        return imageTools.cfg.PictureVirtualDirectory + imageTools.GetPath(thumb);
      }
    }
    
    public string Caption
    {
      get
      {
        return dirSettings.GetFileCaption(Name);
      }
      set
      {
        dirSettings.SetFileCaption(Name, value);
      }
    }

    public string Tooltip
    {
      get
      {
        return dirSettings.GetFileTooltip(Name);
      }
      set
      {
        dirSettings.SetFileTooltip(Name, value);
      }
    }


    public string Description
    {
      get
      {
        return dirSettings.GetFileDescription(Name);
      }
      set
      {
        dirSettings.SetFileDescription(Name, value);
      }
    }


    /// <summary>
    /// Updates the Thumbnail and the Preview pictures in the cache.
    /// </summary>
    public void UpdateCache()
    {
      string[] name = file.Split('/');
      string PathPattern = string.Join("/",name,0,name.Length - 1) + "/{0}/" + name[name.Length - 1];

      // Create Thumbnail.
      imageTools.CreateImage(imageTools.GetPath(file),imageTools.GetPath(String.Format(PathPattern,
        "thumbs")), imageTools.cfg.ThumbCfg, imageTools.cfg.ThumbCfg.Shadow, true, imageTools.cfg.ThumbCfg.JpegQuality);

      // Create the web friendly Preview.
      imageTools.CreateImage(imageTools.GetPath(file),imageTools.GetPath(String.Format(PathPattern, "webpics")),
        imageTools.cfg.PreviewCfg, false, true, imageTools.cfg.PreviewCfg.JpegQuality);
    }


    public String ShadowPath
    {
      get
      {
        return imageTools.cfg.ShadowPath;
      }
    }

    public String ShadowVPath
    {
      get
      {
        return imageTools.cfg.ShadowVPath;
      }
    }    
  }
}
