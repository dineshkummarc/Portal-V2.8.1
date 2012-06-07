using System;
using ImageBrowser.Entities;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using ImageTools;
using System.Drawing.Drawing2D;

namespace ImageBrowser
{
	/// <summary>
	/// static helper methods
	/// </summary>
  public class ImageTools
  {
    /// <summary>
    /// 
    /// </summary>
    public ImageTools(Portal.API.Module m)
    {
      currentModule = m;
      cfg = (ImageBrowserConfig)currentModule.ReadConfig(typeof(ImageBrowserConfig));
      if(cfg == null)
        cfg = new ImageBrowserConfig();
    }

    public Portal.API.Module currentModule = null;
    public ImageBrowserConfig cfg = null;

    /// <summary>
    /// Gets a new SubDirectory Wrapper
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public SubDirectoryWrapper GetSubDirectoryWrapper(string dir)
    {
      return new SubDirectoryWrapper(dir, "~/PortalImages/folder.gif", this);
    }

    /// <summary>
    /// Gets a new Image Wrapper
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public ImageWrapper GetImageWrapper(string file, DirectorySettingsHandler dirSettingsHandler)
    {
      return new ImageWrapper(file, this, dirSettingsHandler);
    }

    /// <summary>
    /// Helper function to get the file image path based in the virtual path
    /// </summary>
    /// <param name="queryPath"></param>
    /// <returns></returns>
    public string GetPath(string queryPath)
    {
      string path = "";

      if ( queryPath != null && queryPath.Length > 0 ) path = queryPath.Replace(@"\","/");

      if ( path.StartsWith(cfg.PictureVirtualDirectory) ) path = path.Substring(cfg.PictureVirtualDirectory.Length ,path.Length - cfg.PictureVirtualDirectory.Length );

      return path;
    }

    /// <summary>
    /// Creates a new image of the specified size from the source image
    /// </summary>
    /// <param name="src">Source image path</param>
    /// <param name="dest">Destination image path</param>
    /// <param name="imgCfg">The configuration of the image</param>
    /// <param name="bShadow">Create the image with a shadow</param>
    /// <param name="forceUpdate">Update in every case</param>
    /// <param name="jpegQuality">The Quality for the JPEG File 0..100</param>
    public void CreateImage(String src, string dest, ImageBrowserConfig.ImageCfg imgCfg, bool bShadow, bool forceUpdate, byte jpegQuality)
    {
      if(bShadow)
        CreateShadowedImage(src, dest, imgCfg, forceUpdate, jpegQuality);
      else
        CreateSimpleImage(src, dest, imgCfg, forceUpdate, jpegQuality);
    }


    /// <summary>
    /// Creates a new image of the specified size from the source image
    /// </summary>
    /// <param name="src">Source image path</param>
    /// <param name="dest">Destination image path</param>
    /// <param name="imgCfg">The configuration of the image</param>
    /// <param name="forceUpdate">Update in every case</param>
    /// <param name="jpegQuality">The Quality for the JPEG File 0..100</param>
    protected void CreateSimpleImage( string src, string dest, ImageBrowserConfig.ImageCfg imgCfg, bool forceUpdate, byte jpegQuality )
    {
      try
      {
        if (!forceUpdate && File.Exists(cfg.PictureRootDirectory + "/" + dest))
          return;

        string path = Directory.GetParent(cfg.PictureRootDirectory + "/" + dest).FullName;

        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);

        using (Image thumb = CreateImageInternal(src, imgCfg.MaxSize))
        {
          if (thumb != null)
          {
            EncoderParameters encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long) jpegQuality);
            thumb.Save(cfg.PictureRootDirectory + "/" + dest, GetEncoder(ImageFormat.Jpeg), encParams);
          }
        }
      }
      catch(Exception){}
    }


    /// <summary>
    /// Creates a new image of the specified size from the source image
    /// </summary>
    /// <param name="src">Source image path</param>
    /// <param name="dest">Destination image path</param>
    /// <param name="imgCfg">The configuration of the image</param>
    /// <param name="forceUpdate">Update in every case</param>
    /// <param name="jpegQuality">The Quality for the JPEG File 0..100</param>
    public void CreateShadowedImage(string src, string dest, ImageBrowserConfig.ImageCfg imgCfg, bool forceUpdate, byte jpegQuality)
    {
      try
      {
        if (!forceUpdate && File.Exists(cfg.PictureRootDirectory + "/" + dest) ) 
          return;

        string path = Directory.GetParent(cfg.PictureRootDirectory + "/" + dest).FullName;

        if ( ! Directory.Exists(path)) 
          Directory.CreateDirectory(path);

        // Values for building Shadow.
        Int32 shadowwidth = imgCfg.ShadowWidth;
        Int32 borderwidth = imgCfg.BorderWidth;
        Int32 margin = shadowwidth / 2;
        Int32 shadowdir = 0;
        Double shadowtrans = imgCfg.ShadowTransparency;
        Color bkcolor = Color.FromArgb(imgCfg.BackgroundColor);
        Color shadowcolor = Color.FromArgb(imgCfg.ShadowColor);
        Color bordercolor = Color.FromArgb(imgCfg.BorderColor);
        Boolean softshadow = imgCfg.SoftShadow;

        Image thumb = CreateImageInternal(src, imgCfg.MaxSize);
        if(thumb != null)
        {
          FastBitmap tmp = new FastBitmap(thumb);

          FastBitmap bmp = new FastBitmap(tmp.Width + borderwidth * 2, tmp.Height + borderwidth * 2,
            PixelFormat.Format32bppArgb);

          // add border if necessary
          if (borderwidth > 0) 
          {
            using(SolidBrush br = new SolidBrush(bordercolor))
            using (Graphics g = Graphics.FromImage(bmp._bitmap))
            {
              g.FillRectangle(br, 0, 0, borderwidth * 2 + tmp.Width, borderwidth * 2 + tmp.Height);
            }
          }

          tmp.CopyTo(bmp, borderwidth, borderwidth, 0, 0, tmp.Width, tmp.Height);
          tmp.Dispose();

          // create image

          Int32 width = bmp.Width + shadowwidth + margin * 2;
          Int32 height = bmp.Height + shadowwidth + margin * 2;
          LayeredImage image = new LayeredImage(width, height);

          Int32 shadowx = 0, shadowy = 0, imgx = 0, imgy = 0;

          if (softshadow) 
          {
            switch (shadowdir) 
            {
              case 0:
                shadowx = margin - shadowwidth / 2;
                shadowy = margin - shadowwidth / 2;
                imgx = margin;
                imgy = margin;
                break;
              case 1:
                shadowx = margin + shadowwidth - 3 * (shadowwidth / 2);
                shadowy = margin - shadowwidth / 2;
                imgx = margin + shadowwidth;
                imgy = margin;
                break;
              case 2:
                shadowx = margin + shadowwidth - 3 * (shadowwidth / 2);
                shadowy = margin + shadowwidth - 3 * (shadowwidth / 2);
                imgx = margin + shadowwidth;
                imgy = margin + shadowwidth;
                break;
              case 3:
                shadowx = margin - shadowwidth / 2;
                shadowy = margin + shadowwidth - 3 * (shadowwidth / 2);
                imgx = margin;
                imgy = margin + shadowwidth;
                break;
            }
          } 
          else 
          {
            switch (shadowdir) 
            {
              case 0:
                shadowx = margin;
                shadowy = margin;
                imgx = margin;
                imgy = margin;
                break;
              case 1:
                shadowx = margin - shadowwidth;
                shadowy = margin;
                imgx = margin + shadowwidth;
                imgy = margin;
                break;
              case 2:
                shadowx = margin - shadowwidth;
                shadowy = margin - shadowwidth;
                imgx = margin + shadowwidth;
                imgy = margin + shadowwidth;
                break;
              case 3:
                shadowx = margin;
                shadowy = margin - shadowwidth;
                imgx = margin;
                imgy = margin + shadowwidth;
                break;
            }
          }

          // background
          Layer bg = image.Layers.Add();
          bg.Clear(bkcolor);

          // shadow -- layer must be larger because of blur
          Layer shadow = image.Layers.Add(width + shadowwidth, height + shadowwidth);
          SolidBrush brush = new SolidBrush(shadowcolor);
          shadow.FillRectangle(shadowwidth, shadowwidth, bmp.Width, bmp.Height, brush);
          if (softshadow)
            shadow.Blur(shadowwidth, shadowwidth);
          brush.Dispose();
          shadow.OffsetX = shadowx;
          shadow.OffsetY = shadowy;
          shadow.Opacity = 1.0 - shadowtrans;

          // image
          Layer img = image.Layers.Add(bmp);
          img.OffsetX = imgx;
          img.OffsetY = imgy;

          // result
          FastBitmap result = image.Flatten();

          EncoderParameters encParams = new EncoderParameters(1);
          encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long) jpegQuality);
          result.Save(cfg.PictureRootDirectory + "/" + dest, GetEncoder(ImageFormat.Jpeg), encParams);
          result.Dispose();
        }
      }
      catch(Exception){}
    }

    /// <summary>
    /// Creates the files to build a shadow arround a picture.
    /// </summary>
    /// <param name="imgCfg">the shadow configuration of the picture</param>
    /// <param name="path">the directory where the files should be located</param>
    public void CreateShadowParts(ImageBrowserConfig.ImageCfg imgCfg, String path)
    {
      // Values for building Shadow.
      Int32 shadowwidth = imgCfg.ShadowWidth;
      Int32 margin = shadowwidth / 2;

      // background
      LayeredImage image = new LayeredImage(shadowwidth * 2 + margin + 1, shadowwidth * 2 + margin + 1);
      Layer bg = image.Layers.Add();
      bg.Clear(Color.FromArgb(imgCfg.BackgroundColor));

      // shadow -- layer must be larger because of blur
      Layer shadow = image.Layers.Add(shadowwidth * 2 + margin, shadowwidth * 2 + margin);
      SolidBrush brush = new SolidBrush(Color.FromArgb(imgCfg.ShadowColor));
      shadow.FillRectangle(margin, margin, shadowwidth + margin + 1, shadowwidth + margin + 1, brush);
      if (imgCfg.SoftShadow)
        shadow.Blur(margin * 2, margin * 2);
      brush.Dispose();
      shadow.Opacity = 1.0 - imgCfg.ShadowTransparency;

      // Result with all the parts.
      FastBitmap totalResult = image.Flatten();

      // Create Directory if necessary.
      if ( ! Directory.Exists(path)) 
        Directory.CreateDirectory(path);

      // Save Shadow into splittet files.
      // Top-right.
      FastBitmap imgTR = new FastBitmap(shadowwidth, shadowwidth, PixelFormat.Format32bppArgb);
      totalResult.CopyTo (imgTR, 0, 0, margin + shadowwidth + 1, 0, shadowwidth, shadowwidth);
      imgTR.Save(path + "/tr.jpg", ImageFormat.Jpeg);

      // Right.
      FastBitmap imgR = new FastBitmap(shadowwidth, 1, PixelFormat.Format32bppArgb);
      totalResult.CopyTo (imgR, 0, 0, margin + shadowwidth + 1, margin + shadowwidth, shadowwidth, 1);
      imgR.Save(path + "/r.jpg", ImageFormat.Jpeg);

      // Bottom-right.
      FastBitmap imgBR = new FastBitmap(shadowwidth, shadowwidth, PixelFormat.Format32bppArgb);
      totalResult.CopyTo(imgBR, 0, 0, margin + shadowwidth + 1, margin + shadowwidth + 1, shadowwidth, shadowwidth);
      imgBR.Save(path + "/br.jpg", ImageFormat.Jpeg);

      // Bottom.
      FastBitmap imgB = new FastBitmap(1, shadowwidth, PixelFormat.Format32bppArgb);
      totalResult.CopyTo (imgB, 0, 0, margin + shadowwidth, margin + shadowwidth + 1, 1, shadowwidth);
      imgB.Save(path + "/b.jpg", ImageFormat.Jpeg);

      
      // Bottom-left.
      FastBitmap imgBL = new FastBitmap(shadowwidth, shadowwidth, PixelFormat.Format32bppArgb);
      totalResult.CopyTo(imgBL, 0, 0, 0, margin + shadowwidth + 1, shadowwidth, shadowwidth);
      imgBL.Save(path + "/bl.jpg", ImageFormat.Jpeg);
    }


    /// <summary>
    /// Creates a new image of the specified size from the source image
    /// </summary>
    /// <param name="src">Source image path</param>
    /// <param name="maxSize">Maximal width and height to resize the picture</param>
    protected Image CreateImageInternal( string src, int maxSize)
    {
      Image thumb = null;
      try
      {
        using (Image image = Image.FromFile(cfg.PictureRootDirectory + "/" + src))
        {
          int origX = image.Size.Width;
          int origY = image.Size.Height;
          int x = maxSize;
          int y = maxSize;
          if (image.Size.Width > image.Size.Height)
            y = (int)(((double)((double)maxSize / (double)origX)) * (double)origY);
          else
            x = (int)(((double)((double)maxSize / (double)origY)) * (double)origX);

          if ((maxSize > origX) && (maxSize > origY)) // dont make it larger!
          {
            thumb = image;
          }
          else
          {
            thumb = Resize(new Bitmap(image), x, y);
          }
        }
      }
      catch(Exception){}
      
      return thumb;
    }


    /// <summary>
    /// Resizes an image in HighQualitiy Bicubic Algorithm.
    /// </summary>
    /// <param name="origBitmap">Image to resize</param>
    /// <param name="newWidth">Width to make it</param>
    /// <param name="newHeight">Height to make it</param>
    /// <param name="bBilinear">Whether to use the bilenear method (alot more cpu)</param>
    /// <returns></returns>
    public static Image Resize(Image origImage, int newWidth, int newHeight)
    {
      Image resizedImage = new Bitmap(newWidth, newHeight);
      Graphics graphic = Graphics.FromImage(resizedImage);
      graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphic.SmoothingMode = SmoothingMode.HighQuality;
      graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
      graphic.CompositingQuality = CompositingQuality.HighQuality;
      graphic.DrawImage(origImage, 0, 0, newWidth, newHeight);
      return resizedImage;
    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
      ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

      foreach (ImageCodecInfo codec in codecs)
      {
        if (codec.FormatID == format.Guid)
        {
          return codec;
        }
      }
      return null;
    }
  }

}
