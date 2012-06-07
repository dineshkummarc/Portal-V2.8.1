/// Found at codeproject.com
/// http://www.codeproject.com/useritems/ImageBrowser.asp
/// 
/// Original Author: 
///		Dan Glass
///		http://www.danglass.com/Web/
///		
///	Changed to a Portal Module by:
///		Arthur Zaczek
///		http://www.zaczek.net
///		
/// Features Added by:
///   Andreas Hauri


namespace ImageBrowser.Controls
{
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using Entities;
  using System.Globalization;

  /// <summary>
  ///		Summary description for ImageBrowser.
  /// </summary>
  public partial class ImageBrowser : Portal.API.Module, IPostBackEventHandler
  {
    protected string back;

    private ImageTools imageTools = null;

    private void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        imageTools = new ImageTools(this);
        if (!IsPostBack)
        {
          BuildDirectories(imageTools.cfg.PictureVirtualDirectory, 0);
        }
      }
      catch
      {
      }
    }

    public void RaisePostBackEvent(string args)
    {
      string[] a = args.Split(';');

      if (a[0].ToLower(CultureInfo.InvariantCulture) == "picture")
      {
        DirectoryWrapper data = new DirectoryWrapper(imageTools.GetPath(a[1]), imageTools);
        int index = Convert.ToInt32(a[2]);
        BuildPicture(data, index);
      }
      else if (a[0].ToLower(CultureInfo.InvariantCulture) == "directory")
      {
        BuildDirectories(a[1], Convert.ToInt32(a[2]));
      }
      else if (a[0].ToLower(CultureInfo.InvariantCulture) == "directorysave")
      {
        DirectoryWrapper data = new DirectoryWrapper(imageTools.GetPath(a[1]), imageTools);
        data.Caption = Request[UniqueID + "$txtDirCaption"];
        data.Tooltip = Request[UniqueID + "$txtDirTooltip"];
        data.Description = Request[UniqueID + "$txtDirText"];

        BuildDirectories(a[1], Convert.ToInt32(a[2]));
      }
      else if (a[0].ToLower(CultureInfo.InvariantCulture) == "picturesave")
      {
        int index = Convert.ToInt32(a[2]);
        DirectoryWrapper directory = new DirectoryWrapper(imageTools.GetPath(a[1]), imageTools);

        DirectorySettingsHandler dirSettings = new DirectorySettingsHandler(imageTools.cfg.PictureRootDirectory + "/"
          + imageTools.GetPath(a[1]), a[1]);

        ImageWrapper i = directory.Images[index] as ImageWrapper;
        i.Caption = txtImageCaption.Text;
        i.Tooltip = txtImageTooltip.Text;
        i.Description = txtImageText.Text;

        // Show the next picture.
        if (index < (directory.Images.Count - 1))
          index++;
        BuildPicture(directory, index);
      }
      else if (a[0].ToLower(CultureInfo.InvariantCulture) == "setfolderimage")
      {
        string path = Server.MapPath(a[1]);
        string newFile = path.Substring(0, path.LastIndexOf('\\')) +
            "\\_dirimage." + path.Substring(path.LastIndexOf(".") + 1);
        string thumbFile = path.Substring(0, path.LastIndexOf('\\')) +
            "\\thumbs\\_dirimage." + path.Substring(path.LastIndexOf(".") + 1);

        System.IO.File.Copy(path, newFile, true);
        System.IO.File.Delete(thumbFile);

        BuildDirectories(a[1].Substring(0, a[1].LastIndexOf('/')), Convert.ToInt32(a[2]));
      }
      else if (a[0].ToLower(CultureInfo.InvariantCulture) == "updatepict")
      {
        UpdatePictures(imageTools.GetPath(a[1]));
        BuildDirectories(a[1], Convert.ToInt32(a[2]));
      }
    }

    private void BuildPicture(DirectoryWrapper directory, int index)
    {
      ImageViewPanel.Visible = true;
      ImageBrowserPanel.Visible = false;

      ImageWrapper i = directory.Images[index] as ImageWrapper;

      lnkBack.NavigateUrl = Page.GetPostBackClientHyperlink(this, "directory;" + directory.VirtualPath + ";" + index.ToString());

      int shadowWidth = 0;
      if (imageTools.cfg.PreviewCfg.Shadow)
        shadowWidth = imageTools.cfg.PreviewCfg.ShadowWidth;
      System.Web.UI.WebControls.Image image = null;
      Control imgCtrl = HtmlTools.RenderImagePreview(i, shadowWidth, out image);
      imagePosition.Controls.Add(imgCtrl);
      AddPreloadCode(directory, index, image);

      if (index > 0)
      {
        lnkMovePrevious.NavigateUrl = Page.GetPostBackClientHyperlink(this, "picture;" + directory.VirtualPath + ";"
          + Convert.ToString(index - 1));
        lnkMovePrevious.Visible = true;
      }
      else
        lnkMovePrevious.Visible = false;

      if (index < directory.Images.Count - 1)
      {
        lnkMoveNext.NavigateUrl = Page.GetPostBackClientHyperlink(this, "picture;" + directory.VirtualPath + ";"
          + Convert.ToString(index + 1));
        lnkMoveNext.Visible = true;
      }
      else
        lnkMoveNext.Visible = false;



      if (ModuleHasEditRights)
      {
        txtImageTooltip.Text = i.Tooltip;
        txtImageCaption.Text = i.Caption;
        txtImageText.Text = i.Description;

        lnkSaveImageText.NavigateUrl = Page.GetPostBackClientHyperlink(this, "picturesave;" + directory.VirtualPath + ";" + index.ToString());
        lnkSetFolderImage.NavigateUrl = Page.GetPostBackClientHyperlink(this, "setfolderimage;" + i.FullImageHref + ";" + index.ToString());

        string insertJs = "document.getElementById('{0}').value=document.getElementById('{1}').value";
        btnTTInsSD.Attributes["onClick"] = string.Format(insertJs, txtImageTooltip.ClientID, txtImageCaption.ClientID);
        btnTTInsD.Attributes["onClick"] = string.Format(insertJs, txtImageTooltip.ClientID, txtImageText.ClientID);
        btnSDInsTT.Attributes["onClick"] = string.Format(insertJs, txtImageCaption.ClientID, txtImageTooltip.ClientID);
        btnSDInsD.Attributes["onClick"] = string.Format(insertJs, txtImageCaption.ClientID, txtImageText.ClientID);
        btnDInsTT.Attributes["onClick"] = string.Format(insertJs, txtImageText.ClientID, txtImageTooltip.ClientID);
        btnDInsDS.Attributes["onClick"] = string.Format(insertJs, txtImageText.ClientID, txtImageCaption.ClientID);
        lbImageText.Visible = false;
      }
      else
      {
        lbImageText.Text = i.Description;
        lbImageText.Visible = true;
        PhotoEditOpts.Visible = false;
      }
    }

    private void AddPreloadCode(DirectoryWrapper data, int index, WebControl imgCtrl)
    {
      const string scriptKey = "ImagePreload";
      if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
      {
        string script = "<script language=\"JavaScript\">\n"
          + "function PreLoadSiblings()\n{\n";

        // Script for the Next end the previous picture, to load them to the browser cache.
        if (index > 0)
        {
          ImageWrapper i = data.Images[index - 1] as ImageWrapper;
          string path = ResolveUrl(i.WebImageHref);
          script += "objImageNext = new Image(); objImageNext.src='" + path + "';\n";
        }
        if (index < data.Images.Count - 1)
        {
          ImageWrapper i = data.Images[index + 1] as ImageWrapper;
          string path = ResolveUrl(i.WebImageHref);
          script += "objImageNext = new Image(); objImageNext.src='" + path + "';\n";
        }

        script += "}\n</script>";

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), scriptKey, script);

        // Register the Script for the event when the main Image is loaded.
        imgCtrl.Attributes.Add("onload", "PreLoadSiblings()");
      }

    }

    private void BuildDirectories(string vPath, int currIndex)
    {
      ImageViewPanel.Visible = false;
      ImageBrowserPanel.Visible = true;

      string path = imageTools.GetPath(vPath);
      DirectoryWrapper data = new DirectoryWrapper(path, imageTools);

      if (currIndex >= data.Images.Count)  // Out of range?
        currIndex = data.Images.Count - 1;

      // draw navigation
      HtmlTools.RendenderLinkPath(Path.Controls, path, this, imageTools.cfg);

      directoryContent.Controls.Add(HtmlTools.HR);

      if (ModuleHasEditRights)
      {
        txtDirCaption.Text = data.Caption;
        txtDirTooltip.Text = data.Tooltip;
        txtDirText.Text = data.Description; ;

        lnkSave.NavigateUrl = Page.GetPostBackClientHyperlink(this, "directorysave;" + vPath + ";" + currIndex.ToString());

        lnkSave.Visible = true;
        lnkUpdatePictures.Visible = true;
        lnkUpdatePictures.NavigateUrl = Page.GetPostBackClientHyperlink(this, "updatePict;" + vPath + ";" + currIndex.ToString());

        string insertJs = "document.getElementById('{0}').value=document.getElementById('{1}').value";
        btnDirTTInsSD.Attributes["onClick"] = string.Format(insertJs, txtDirTooltip.ClientID, txtDirCaption.ClientID);
        btnDirTTInsD.Attributes["onClick"] = string.Format(insertJs, txtDirTooltip.ClientID, txtDirText.ClientID);
        btnDirSDInsTT.Attributes["onClick"] = string.Format(insertJs, txtDirCaption.ClientID, txtDirTooltip.ClientID);
        btnDirSDInsD.Attributes["onClick"] = string.Format(insertJs, txtDirCaption.ClientID, txtDirText.ClientID);
        btnDirDInsTT.Attributes["onClick"] = string.Format(insertJs, txtDirText.ClientID, txtDirTooltip.ClientID);
        btnDirDInsDS.Attributes["onClick"] = string.Format(insertJs, txtDirText.ClientID, txtDirCaption.ClientID);
      }
      else
      {
        DirEditOps.Visible = false;

        // pump out the blurb
        Label blurb = new Label();
        blurb.Text = data.Description;
        directoryContent.Controls.Add(blurb);

        // draw a line if appropriate
        if (blurb.Text.Length > 0 && (data.Directories.Count > 0 || data.Images.Count > 0))
        {
          directoryContent.Controls.Add(HtmlTools.HR);
        }
      }


      // draw subdirectories
      directoryContent.Controls.Add(HtmlTools.RenderDirectoryTable(4, data, this));


      // draw a line if appropriate
      if (data.Directories.Count > 0 && data.Images.Count > 0)
      {
        directoryContent.Controls.Add(HtmlTools.HR);
      }

      // Calculate index of the first image on the current page.
      int imageCols = imageTools.cfg.ThumbnailCols;
      int imageRows = imageTools.cfg.ThumbnailRows;
      int startIndex = ((int)(currIndex / (imageCols * imageRows))) * (imageCols * imageRows);

      // draw images
      directoryContent.Controls.Add(HtmlTools.RenderImageTable(imageCols, imageRows, startIndex, data, this));

      // draw the Page Navigation.
      HtmlTools.RendenderPageNavigation(pageNavigation.Controls, path, imageCols * imageRows, data.Images.Count - 1,
                                        startIndex, this);
    }

    private void UpdatePictures(string path)
    {
      CacheUpdater CacheUpd = new CacheUpdater(path, imageTools);
      System.Threading.Thread WorkThread = new System.Threading.Thread(
        new System.Threading.ThreadStart(CacheUpd.UpdateAll));
      //      WorkThread.IsBackground = true;      
      WorkThread.Start();
    }


    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN: This call is required by the ASP.NET Web Form Designer.
      //
      InitializeComponent();
      base.OnInit(e);
    }

    ///		Required method for Designer support - do not modify
    ///		the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion
  }
}
