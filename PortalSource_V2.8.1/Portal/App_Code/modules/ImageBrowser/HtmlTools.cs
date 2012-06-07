using System;
using System.Web.UI.WebControls;
using System.Drawing;

using ImageBrowser.Entities;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;

namespace ImageBrowser
{
	/// <summary>
	/// static helper methods
	/// </summary>
	public sealed class HtmlTools
	{
		/// <summary>
		/// Dont want anyone to instantiate
		/// </summary>
		private HtmlTools(){}

		/// <summary>
		/// This creates a Table object with all the thumbnails in it
		/// </summary>
		/// <param name="nofCols">Picture Columns</param>
		/// <param name="nofRows">Picture Rows</param>
		/// <param name="startIndex">index of the first displayed picture</param>
		/// <param name="data">The directory to render</param>
		/// <param name="url">The URL to use in the links</param>
		/// <returns></returns>
		public static Table RenderImageTable(int nofCols, int nofRows, int startIndex, DirectoryWrapper data, 
                                         System.Web.UI.Control ctrl)
		{
			Table table = new Table();
			table.Width = Unit.Percentage(100);

			TableRow tr = null;
      if(((data.Images.Count - 1) - startIndex) <= 0) // Are some pictures in this range?
        startIndex = 0;   // No.
      int lastIndex = System.Math.Min((data.Images.Count - 1), startIndex + nofCols * nofRows - 1);

      for(int index = startIndex; index <= lastIndex; index++)
			{
				if ( tr == null ) 
          tr = new TableRow();
        ImageWrapper image = data.Images[index] as ImageWrapper;
				HyperLink h = new HyperLink();
				h.ImageUrl = image.ThumbHref;
				h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "picture;" + data.VirtualPath + ";" + index);
				h.Text = image.Tooltip;
				h.CssClass = "LinkButton";

				Label lbText = new Label();
				lbText.Text = image.Caption;


				TableCell td = new TableCell();
				td.Attributes.Add("align", "center");
				td.Controls.Add(h);
				if(lbText.Text != "")
				{
					lbText.Text = @"<div align=""center"" width=""100%"">" + lbText.Text + @"</div>";
					td.Controls.Add(lbText);
				}
				tr.Cells.Add(td);

				if ( tr.Cells.Count == nofCols )
				{
					table.Rows.Add(tr);
					tr = null;
				}
			}

			if ( tr != null ) table.Rows.Add(tr);

			return table;
		}

		/// <summary>
		///  This creates a Table object with all the subdirectories in it
		/// </summary>
		/// <param name="x">Subdirectories wide</param>
		/// <param name="data">The directory to render</param>
		/// <param name="url">The URL to use in the links</param>
		/// <returns></returns>
		public static Table RenderDirectoryTable(int x, DirectoryWrapper data/*, string url*/, System.Web.UI.Control ctrl)
		{
			Table table = new Table();
			table.CellPadding = 10;
			table.CellSpacing = 10;
			table.Width = Unit.Percentage(100);

			TableRow tr = null;

			foreach ( SubDirectoryWrapper dir in data.Directories )
			{
				if ( tr == null ) tr = new TableRow();

				TableCell td = new TableCell();
				td.Attributes["align"] = "center";

				HyperLink h = new HyperLink();

				h.ImageUrl = dir.Src;
				h.CssClass = "LinkButton";
				h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "directory;" + dir.HREF + ";0");
				h.Text = dir.Tooltip;

				td.Controls.Add(h);
				
				td.Controls.Add(BR);

				h = new HyperLink();
				h.CssClass = "LinkButton";
				h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "directory;" + dir.HREF + ";0");
				h.Text = dir.Caption;
				

				td.Controls.Add(h);

				tr.Cells.Add(td);

				if ( tr.Cells.Count == x )
				{
					table.Rows.Add(tr);
					tr = null;
				}
			}

			if ( tr != null ) table.Rows.Add(tr);

			return table;
		}

    /// <summary>
    ///  This creates the Preview of the image with the optional shadow.
    /// </summary>
    /// <param name="data">The image to render</param>
    /// <param name="shadowWidth">The width of the shadow, if a shadow is displayed</param>
    /// <returns></returns>
    public static Control RenderImagePreview(ImageWrapper data, int  shadowWidth, out System.Web.UI.WebControls.Image img)
    {
      HyperLink lnk = new HyperLink();
      lnk.NavigateUrl = data.FullImageHref;
      lnk.Target = "_blank";

      img = new System.Web.UI.WebControls.Image();
      img.ImageUrl = data.WebImageHref;
      img.Attributes.CssStyle.Add("display", "block");
      lnk.Controls.Add(img);

      Control ResultCtrl = null;
      if(shadowWidth > 0)
      {
        // Get the size.
        System.Drawing.Image imgData = System.Drawing.Image.FromFile(data.WebImagePath);
        int imageHeight = imgData.Height;
        int imageWidth = imgData.Width;
        imgData.Dispose();

        HtmlGenericControl container = new HtmlGenericControl("div");
        container.Attributes.CssStyle.Add("width", (imageWidth + shadowWidth).ToString() + "px");
        container.Attributes.CssStyle.Add("height", (imageHeight + shadowWidth).ToString() + "px");

        // Add Image.
        lnk.Attributes.CssStyle.Add("float", "left");        
        container.Controls.Add(lnk);

        // Right Side Shadow.
        HtmlGenericControl rightBorder = new HtmlGenericControl("div");
        rightBorder.Attributes.CssStyle.Add("width", shadowWidth.ToString() + "px");
        rightBorder.Attributes.CssStyle.Add("height", imageHeight.ToString() + "px");
        rightBorder.Attributes.CssStyle.Add("float", "right");
        rightBorder.Attributes.CssStyle.Add("background-image", "url(" + VirtualPathUtility.Combine(data.ShadowVPath, "r.jpg") + ")");
        rightBorder.Attributes.CssStyle.Add("background-repeat", "top right repeat-y");
        container.Controls.Add(rightBorder);

        // Top Right Corner Shadow.
        System.Web.UI.WebControls.Image topRightCorner = new System.Web.UI.WebControls.Image();
        topRightCorner.ImageUrl = VirtualPathUtility.Combine(data.ShadowVPath, "tr.jpg");
        rightBorder.Controls.Add(topRightCorner);

        // Bottom Shadow.
        HtmlGenericControl bottomBorder = new HtmlGenericControl("div");
        bottomBorder.Attributes.CssStyle.Add("width", (imageWidth + shadowWidth).ToString() + "px");
        bottomBorder.Attributes.CssStyle.Add("height", shadowWidth.ToString() + "px");
        bottomBorder.Attributes.CssStyle.Add("float", "left");
        bottomBorder.Attributes.CssStyle.Add("background-image", "url(" + VirtualPathUtility.Combine(data.ShadowVPath, "b.jpg") + ")");
        bottomBorder.Attributes.CssStyle.Add("background-repeat", "top right repeat-x");
        container.Controls.Add(bottomBorder);

        // Bottom Left Corner Shadow.
        System.Web.UI.WebControls.Image bottomLeftCorner = new System.Web.UI.WebControls.Image();
        bottomLeftCorner.ImageUrl = VirtualPathUtility.Combine(data.ShadowVPath, "bl.jpg");
        bottomLeftCorner.Attributes.CssStyle.Add("float", "left");
        bottomBorder.Controls.Add(bottomLeftCorner);

        // Bottom Right Corner Shadow.
        System.Web.UI.WebControls.Image bottomRightCorner = new System.Web.UI.WebControls.Image();
        bottomRightCorner.ImageUrl = VirtualPathUtility.Combine(data.ShadowVPath, "br.jpg");
        bottomRightCorner.Attributes.CssStyle.Add("float", "right");
        bottomBorder.Controls.Add(bottomRightCorner);

        ResultCtrl = container;
      }
      else
        ResultCtrl = lnk;
      
      return ResultCtrl;
    }
		

		/// <summary>
		/// Outputs some navigation links to the page.
		/// </summary>
		/// <param name="controlCollection">the pages' controls</param>
		/// <param name="path">The path of the current image directory being browsed</param>
		/// <param name="url">The URL to use in the links</param>
		public static void RendenderLinkPath(System.Web.UI.ControlCollection controlCollection, string path, System.Web.UI.Control ctrl, ImageBrowserConfig cfg)
		{

			HyperLink h = null;
			Literal l = null;

      DirectorySettingsHandler RootSettings = new DirectorySettingsHandler(cfg.PictureRootDirectory, "My Pictures");

			if ( path != null && path.Length > 0 )
				path = path.Replace(@"\","/");
			else
			{
				h = new HyperLink();
				h.NavigateUrl = "";
				h.Text = RootSettings.DirectoryCaption;
				h.Attributes.Add("class","LinkButton");
				controlCollection.Add(h);
				return;
			}

			string[] paths = path.Split('/');

			paths[0] = RootSettings.DirectoryCaption;

			for ( int i = 1; i <= paths.Length; i++ )
			{
        DirectorySettingsHandler DirSetting = new DirectorySettingsHandler(
          cfg.PictureRootDirectory + "\\" + string.Join("\\",paths,0,i).Replace(RootSettings.DirectoryCaption,""), paths[i-1]);
        if ( i < paths.Length )
				{
					h = new HyperLink();
					h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "directory;" + string.Join("/",paths,0,i).Replace(RootSettings.DirectoryCaption,"") + ";0");
					h.Text = DirSetting.DirectoryCaption;
					h.Attributes.Add("class","LinkButton");
					controlCollection.Add(h);

					l = new Literal();
					l.Text = " &raquo; \n";
					controlCollection.Add(l);
				}
				else
				{
					h = new HyperLink();
					h.NavigateUrl = "";
					h.Text = DirSetting.DirectoryCaption;
					h.Attributes.Add("class","LinkButton");
					controlCollection.Add(h);
				}
			}
		}


    /// <summary>
    /// Outputs the page navigation to the page.
    /// </summary>
    /// <param name="controlCollection">the pages' controls</param>
    /// <param name="path">The path of the current image directory being browsed</param>
    /// <param name="pageSize">The number of items on a page</param>
    /// <param name="maxIndex">The index of the last item on the last page</param>
    /// <param name="currIndex">The index of the first item in this page</param>
    public static void RendenderPageNavigation(System.Web.UI.ControlCollection controlCollection, string path, 
      int pageSize, int maxIndex, int currIndex, System.Web.UI.Control ctrl)
    {
      if(maxIndex >= pageSize)
      {
        HyperLink h = null;
        
        // Previous-Button.
        if(currIndex >= pageSize)
        {
          h = new HyperLink();
          int previousPageFirst = ((int)(currIndex / pageSize) - 1)  * pageSize;
          h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "directory;" + path + ";" + previousPageFirst.ToString());
          h.Text = "1";
          
          System.Web.UI.WebControls.Image prevImg = new System.Web.UI.WebControls.Image();
          prevImg.ImageUrl = "NavPreviousSmall.gif";
          prevImg.Attributes.Add("align", "middle");
          h.Controls.Add(prevImg);
          controlCollection.Add(h);
        }

        // List all Pages.
        for(int index = 0; index <= maxIndex; index += pageSize)
        {
          h = new HyperLink();
          if((currIndex < index) || (currIndex >= index + pageSize))
          {
            h.Text = (index / pageSize + 1).ToString() + "  ";
            h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "directory;" + path + ";" + index.ToString());
          }
          else
          {
            h.Text = "[" + (index / pageSize + 1).ToString() + "]  ";
          }
          h.Attributes.Add("class","LinkButton");
          controlCollection.Add(h);
        }

        // Next Button.
        if(currIndex < ((int)(maxIndex / pageSize)) * pageSize)
        {
          h = new HyperLink();
          int nextPageFirst = ((int)(currIndex / pageSize) + 1) * pageSize;
          h.NavigateUrl = ctrl.Page.GetPostBackClientHyperlink(ctrl, "directory;" + path + ";" + nextPageFirst.ToString());
          h.Text = "1";
          
          System.Web.UI.WebControls.Image nextImg = new System.Web.UI.WebControls.Image();
          nextImg.ImageUrl = "NavNextSmall.gif";
          nextImg.Attributes.Add("align", "middle");
          h.Controls.Add(nextImg);
          controlCollection.Add(h);
        }
      }
    }

		/// <summary>
		/// Helper property
		/// </summary>
		public static Literal BR
		{
			get
			{
				Literal l = new Literal();
				l.Text = "<BR>";
				return l;
			}
		}

		/// <summary>
		/// Helper property
		/// </summary>
		public static Literal HR
		{
			get
			{
				Literal l = new Literal();
				l.Text = "<div width=\"100%\" style=\"border-bottom: solid 1px black;font-size: 5px;\">&nbsp;</div>";
				return l;
			}
		}
	}
}
