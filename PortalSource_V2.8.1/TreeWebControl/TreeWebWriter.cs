using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iiuga.Web.UI
{
	/// <summary>
	/// TreeWebWriter class.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 04 January, 2003
	/// </author>
	internal class TreeWebWriter : WebControl, ITreeWebWriter
	{
		private TreeWeb				_tree;

		/// <summary>
		/// 
		/// </summary>
		public TreeWebWriter() : base( HtmlTextWriterTag.Div )
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="tree"></param>
		public void RenderTree( HtmlTextWriter writer, TreeWeb tree )
		{
			_tree = tree;

			RenderControl( writer );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		protected override void RenderContents( HtmlTextWriter writer )
		{	
			writer.AddAttribute( HtmlTextWriterAttribute.Width, "100%" );
			writer.AddAttribute( HtmlTextWriterAttribute.Cellpadding, "0" );
			writer.AddAttribute( HtmlTextWriterAttribute.Cellspacing, "0" );
			writer.RenderBeginTag( HtmlTextWriterTag.Table );

			// render tree Root's children
			RenderChildren( writer );

			writer.RenderEndTag();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		protected override void RenderChildren( HtmlTextWriter writer )
		{
			foreach( TreeElement _elem in _tree.Elements )
				_elem.Render( writer );
		}
	}

}
