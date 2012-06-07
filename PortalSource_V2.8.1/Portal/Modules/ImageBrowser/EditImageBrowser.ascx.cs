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

  /// <summary>
  ///		Summary description for ImageBrowser.
  /// </summary>
  public partial class EditImageBrowser : Portal.API.EditModule
  {
    private void Page_Load(object sender, System.EventArgs e)
    {
      if (!IsPostBack)
      {
        ImageBrowserConfig cfg = (ImageBrowserConfig)ReadConfig(typeof(ImageBrowserConfig));
        if (cfg == null)
          cfg = new ImageBrowserConfig();

        txtPictureVirtualDirectory.Text = cfg.PictureVirtualDirectory;
        txtThumbnailCols.Text = cfg.ThumbnailCols.ToString();
        txtThumbnailRows.Text = cfg.ThumbnailRows.ToString();
        txtPreviewMax.Text = cfg.PreviewCfg.MaxSize.ToString();
        txtPreviewJpegQuality.Text = cfg.PreviewCfg.JpegQuality.ToString();
        txtThumbnailMax.Text = cfg.ThumbCfg.MaxSize.ToString();
        txtThumbJpegQuality.Text = cfg.ThumbCfg.JpegQuality.ToString();

        chkPreviewShadow.Checked = cfg.PreviewCfg.Shadow;
        txtPvShadowWidth.Text = cfg.PreviewCfg.ShadowWidth.ToString();
        txtPvShadowTrans.Text = (cfg.PreviewCfg.ShadowTransparency * 100).ToString();
        PvBgColor.SelectedColor = Color.FromArgb(cfg.PreviewCfg.BackgroundColor);
        PvShColor.SelectedColor = Color.FromArgb(cfg.PreviewCfg.ShadowColor);
        PvBColor.SelectedColor = Color.FromArgb(cfg.PreviewCfg.BorderColor);
        chkPvSoftShadow.Checked = cfg.PreviewCfg.SoftShadow;

        chkThumbnailShadow.Checked = cfg.ThumbCfg.Shadow;
        txtTnShadowWidth.Text = cfg.ThumbCfg.ShadowWidth.ToString();
        txtTnBorderWidth.Text = cfg.ThumbCfg.BorderWidth.ToString();
        txtTnShadowTrans.Text = (cfg.ThumbCfg.ShadowTransparency * 100).ToString();
        TnBgColor.SelectedColor = Color.FromArgb(cfg.ThumbCfg.BackgroundColor);
        TnShColor.SelectedColor = Color.FromArgb(cfg.ThumbCfg.ShadowColor);
        TnBColor.SelectedColor = Color.FromArgb(cfg.ThumbCfg.BorderColor);
        chkTnSoftShadow.Checked = cfg.ThumbCfg.SoftShadow;

        chkThumbnailShadow_CheckedChanged(null, null);
        chkPreviewShadow_CheckedChanged(null, null);
      }
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
      this.lnkSave.Click += new System.EventHandler(this.lnkSave_Click);
      this.lnkSaveAndUpdate.Click += new System.EventHandler(this.lnkSaveUpdate_Click);
      this.lnkCancel.Click += new System.EventHandler(this.lnkCancel_Click);
      this.chkPreviewShadow.CheckedChanged += new System.EventHandler(this.chkPreviewShadow_CheckedChanged);
      this.chkThumbnailShadow.CheckedChanged += new System.EventHandler(this.chkThumbnailShadow_CheckedChanged);
      this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion

    private void lnkSave_Click(object sender, System.EventArgs e)
    {
      if (Page.IsValid)
      {
        Save();

        RedirectBack();
      }
    }

    private void lnkSaveUpdate_Click(object sender, System.EventArgs e)
    {
      Save();

      ImageTools it = new ImageTools(this);
      string path = it.GetPath(it.cfg.PictureVirtualDirectory);
      CacheUpdater CacheUpd = new CacheUpdater(path, it);
      System.Threading.Thread WorkThread = new System.Threading.Thread(
        new System.Threading.ThreadStart(CacheUpd.UpdateAll));
      //      WorkThread.IsBackground = true;      
      WorkThread.Start();

      RedirectBack();
    }


    private void Save()
    {
      // Save Configuration.
      ImageBrowserConfig cfg = (ImageBrowserConfig)ReadConfig(typeof(ImageBrowserConfig));
      if (cfg == null)
      {
        cfg = new ImageBrowserConfig();
      }

      cfg.PictureVirtualDirectory = txtPictureVirtualDirectory.Text;
      cfg.ThumbnailCols = Convert.ToInt32(txtThumbnailCols.Text);
      cfg.ThumbnailRows = Convert.ToInt32(txtThumbnailRows.Text);
      cfg.PreviewCfg.MaxSize = Convert.ToInt32(txtPreviewMax.Text);
      cfg.PreviewCfg.JpegQuality = Convert.ToByte(txtPreviewJpegQuality.Text);
      cfg.ThumbCfg.MaxSize = Convert.ToInt32(txtThumbnailMax.Text);
      cfg.ThumbCfg.JpegQuality = Convert.ToByte(txtThumbJpegQuality.Text);

      cfg.PreviewCfg.Shadow = chkPreviewShadow.Checked;
      cfg.PreviewCfg.ShadowWidth = Convert.ToInt32(txtPvShadowWidth.Text);
      cfg.PreviewCfg.ShadowTransparency = Convert.ToDouble(txtPvShadowTrans.Text) / 100;
      cfg.PreviewCfg.BackgroundColor = PvBgColor.SelectedColor.ToArgb();
      cfg.PreviewCfg.ShadowColor = PvShColor.SelectedColor.ToArgb();
      cfg.PreviewCfg.BorderColor = PvBColor.SelectedColor.ToArgb();
      cfg.PreviewCfg.SoftShadow = chkPvSoftShadow.Checked;

      cfg.ThumbCfg.Shadow = chkThumbnailShadow.Checked;
      cfg.ThumbCfg.ShadowWidth = Convert.ToInt32(txtTnShadowWidth.Text);
      cfg.ThumbCfg.BorderWidth = Convert.ToInt32(txtTnBorderWidth.Text);
      cfg.ThumbCfg.ShadowTransparency = Convert.ToDouble(txtTnShadowTrans.Text) / 100;
      cfg.ThumbCfg.BackgroundColor = TnBgColor.SelectedColor.ToArgb();
      cfg.ThumbCfg.ShadowColor = TnShColor.SelectedColor.ToArgb();
      cfg.ThumbCfg.BorderColor = TnBColor.SelectedColor.ToArgb();
      cfg.ThumbCfg.SoftShadow = chkTnSoftShadow.Checked;
      WriteConfig(cfg);

      // Build shadow files.
      ImageTools it = new ImageTools(this);
      it.CreateShadowParts(cfg.PreviewCfg, cfg.ShadowPath);
    }

    private void lnkCancel_Click(object sender, System.EventArgs e)
    {
      RedirectBack();
    }

    private void chkThumbnailShadow_CheckedChanged(object sender, System.EventArgs e)
    {
      bool Checked = chkThumbnailShadow.Checked;
      txtTnShadowWidth.Enabled = Checked;
      txtTnBorderWidth.Enabled = Checked;
      txtTnShadowTrans.Enabled = Checked;
      TnBgColor.Enabled = Checked;
      TnBColor.Enabled = Checked;
      TnShColor.Enabled = Checked;
      chkTnSoftShadow.Enabled = Checked;
    }

    private void chkPreviewShadow_CheckedChanged(object sender, System.EventArgs e)
    {
      bool Checked = chkPreviewShadow.Checked;
      txtPvShadowWidth.Enabled = Checked;
      txtPvShadowTrans.Enabled = Checked;
      PvBgColor.Enabled = Checked;
      PvBColor.Enabled = Checked;
      PvShColor.Enabled = Checked;
      chkPvSoftShadow.Enabled = Checked;
    }

  }
}
