namespace HtmlEdit
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.IO;

	/// <summary>
	///		Summary description for EditHtml.
	/// </summary>
	public partial  class EditHtml : Portal.API.EditModule
	{

		private string GetPath()
		{
			return ModuleDataPhysicalPath + ModuleRef + ".htm";
		}


        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Get the application directory.
            string applicationDir = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
            if (!applicationDir.EndsWith("/"))
                applicationDir += "/";

            FCKeditor1.BasePath = applicationDir + "FCKeditor/";
            FCKeditor1.EditorAreaCSS = Portal.Helper.CssEditPath;
            if (!IsPostBack)
            {
                // Open file            
                if (File.Exists(GetPath()))
                {
                    FileStream fs = File.OpenRead(GetPath());
                    StreamReader sr = new StreamReader(fs);
                    FCKeditor1.Value = sr.ReadToEnd();
                    fs.Close();
                }
            }
        }


        protected void Save()
        {
            if (Portal.API.HtmlAnalyzer.HasScriptTags(FCKeditor1.Value))
            {
                msg.Error = Portal.API.Language.GetText(this, "ErrorScriptTags");
                return;
            }
            using (FileStream fs = new FileStream(GetPath(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                fs.SetLength(0); // Truncate
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(FCKeditor1.Value);
                }
            }
        }

        protected void OnSaveBack(object sender, EventArgs e)
        {
          Save();
          RedirectBack();
        }

        protected void OnSave(object sender, EventArgs e)
        {
          Save();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
          RedirectBack();
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

        }
        #endregion
 
}
}
