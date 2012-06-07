using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class WebSolutionControl : WebControl, INamingContainer
	{
		private bool				_initialised = false;
		private int					_documentID;
		private int					_webID;
		private string				_webFolder;
		private WebSolutionObject	_webSolutionObject;

		public WebSolutionControl()
		{
		}

		protected virtual void Initialise()
		{
			_webID = WebSolutionDB.GetWebIDAndFolder(Page.Request.Url.Host, out _webFolder);
			_documentID = Convert.ToInt32(Page.Request.QueryString["DocumentID"]);
			_initialised = true;
		}

		protected override void OnLoad(System.EventArgs e)
		{
			Initialise();
		}

		protected override void EnsureChildControls()
		{
			if (!_initialised)
				Initialise();
			base.EnsureChildControls();
		}

		protected virtual void CreateObject()
		{
		}

		protected override void CreateChildControls()
		{
			CreateObject();
			if (_webSolutionObject != null)
				_webSolutionObject.CreateChildControls();
		}

		protected override void OnPreRender(System.EventArgs e)
		{
			if (_webSolutionObject != null)
				_webSolutionObject.OnPreRender();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (_webSolutionObject != null)
				_webSolutionObject.Render(writer);
		}

		public int DocumentID
		{
			get
			{
				return _documentID;
			}
			set
			{
				_documentID = value;
			}
		}

		public int WebID
		{
			get
			{
				return _webID;
			}
		}

		public string WebFolder
		{
			get
			{
				return _webFolder == null ? string.Empty : _webFolder;
			}
		}

		protected WebSolutionObject WebSolutionObject
		{
			get
			{
				return _webSolutionObject;
			}
			set
			{
				_webSolutionObject = value;
			}
		}
	}
}
