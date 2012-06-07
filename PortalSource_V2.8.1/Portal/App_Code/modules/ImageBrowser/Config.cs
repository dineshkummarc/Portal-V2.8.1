using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Drawing;

namespace ImageBrowser
{
	[XmlRoot("ImageBrowser")]
	public class ImageBrowserConfig
	{
    [XmlIgnore]
    private string PictVDir = "";

    /// <summary>
    /// The virtual directory of the images
    /// ie. /pics
    /// </summary>
    [XmlElement("PictureVirtualDirectory")]
    public string PictureVirtualDirectory
    {
      get 
      { 
        return PictVDir; 
      }
      set 
      { 
        PictVDir = value; 
        PictureRootDirectory = HttpContext.Current.Server.MapPath(PictVDir);
        if(!PictureRootDirectory.EndsWith("\\"))    // XP Home does not include a trailing backslash.
          PictureRootDirectory += "\\";
      }
    }


		/// <summary>
		/// Where the virtual directory maps to on disk
		/// </summary>
		[XmlIgnore]
		public string PictureRootDirectory = "";

    [XmlIgnore]
    public String ShadowPath
    {
      get
      {
        return PictureRootDirectory + "/_Shadow";
      }
    }

    [XmlIgnore]
    public String ShadowVPath
    {
      get
      {
        string absPath = VirtualPathUtility.ToAbsolute(PictureVirtualDirectory);
        return VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.AppendTrailingSlash(absPath) + "_Shadow");
      }
    }   

    [XmlElement("ThumbnailCols")]
    public int ThumbnailCols = 7;

    [XmlElement("ThumbnailRows")]
    public int ThumbnailRows = 5;

    public class ImageCfg
    {
      public ImageCfg()
      {
      }

      public ImageCfg(int maxSize, int shadowWidth)
      {
        MaxSize = maxSize;
        ShadowWidth = shadowWidth;
      }
      [XmlElement("JpegQuality")]
      public byte JpegQuality = 90;

      [XmlElement("MaxSize")]
      public int MaxSize = 100;

      [XmlElement("Shadow")]
      public bool Shadow = false;

      [XmlElement("ShadowWidth")]
      public int ShadowWidth = 8;

      [XmlElement("BorderWidth")]
      public int BorderWidth = 0;

      [XmlElement("ShadowTransparency")]
      public double ShadowTransparency = 0;

      [XmlElement("ShadowColor")]
      public int ShadowColor = Color.FromArgb(0, 0, 0).ToArgb();

      [XmlElement("BackgroundColor")]
      public int BackgroundColor = Color.FromArgb(255, 255, 255).ToArgb();

      [XmlElement("BorderColor")]
      public int BorderColor = Color.FromArgb(255, 255, 255).ToArgb();

      [XmlElement("SoftShadow")]
      public bool SoftShadow = true;
    }

    [XmlElement("ThumbnailCfg")]
    public ImageCfg ThumbCfg = new ImageCfg(120, 8);

    [XmlElement("PreviewCfg")]
    public ImageCfg PreviewCfg = new ImageCfg(600, 25);
	}
}
