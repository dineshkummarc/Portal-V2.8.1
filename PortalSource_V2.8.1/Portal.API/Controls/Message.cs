using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Portal.API.Controls
{
	/// <summary>
	/// Summary description for Message.
	/// </summary>
	[DefaultProperty("Error")] 
	[ToolboxData(@"<{0}:Message runat=""server""></{0}:Message>")]
	public class Message : System.Web.UI.WebControls.WebControl
	{
		private string error = "";
        private string success = "";
        private string info = "";

        [DefaultValue("")]
        public string Error
		{
			get { return error; }
			set { error = value; }
		}

        [DefaultValue("")]
        public string Success
		{
			get { return success; }
			set { success = value; }
		}

        [DefaultValue("")]
        public string Info
		{
			get { return info; }
			set { info = value; }
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter writer)
		{
			if(!string.IsNullOrEmpty(error))
			{
                writer.Write("<pre class=\"Error\">{0}</pre>", error);
			}
			else if(!string.IsNullOrEmpty(success))
			{
                writer.Write("<pre class=\"Success\">{0}</pre>", success);
			}
			else if(!string.IsNullOrEmpty(info))
			{
                writer.Write("<pre class=\"Info\">{0}</pre>", info);
			}
		}
	}
}
