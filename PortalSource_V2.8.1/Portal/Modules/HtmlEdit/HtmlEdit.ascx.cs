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
	///		Summary description for Html.
	/// </summary>
	public partial  class Html : Portal.API.Module
	{

		private string GetPath()
		{
			return ModuleDataPhysicalPath + ModuleRef + ".htm";
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Open file            
			if(File.Exists(GetPath()))
			{
				FileStream fs = File.OpenRead(GetPath());
				StreamReader sr = new StreamReader(fs);
				content.InnerHtml = sr.ReadToEnd();
				fs.Close();
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

		}
		#endregion
	}
}
