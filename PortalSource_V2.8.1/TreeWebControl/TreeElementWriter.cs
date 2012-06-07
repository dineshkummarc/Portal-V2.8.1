using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace iiuga.Web.UI
{
	/// <summary>
	/// TreeElementWriter class
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 04 January, 2003
	/// </author>
	internal class TreeElementWriter: ITreeElementWriter
	{
		static readonly string[]	_expcol = new string[2] { "+", "-" };
		static readonly string		_indentationStep = "&nbsp;";

		private TreeElement			_element;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="element"></param>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		public void RenderElement( HtmlTextWriter writer, TreeElement element )
		{
			_element = element;

			Render( writer );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		protected void Render( HtmlTextWriter writer )
		{
			RenderContents( writer );

			if( _element.HasElements && _element.IsExpanded )
				RenderChildren( writer );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 30 December, 2002
		/// </author>		
		protected void RenderContents( HtmlTextWriter writer )
		{
			TableRow _tr = new TableRow();
			TableCell _td = new TableCell();

			_tr.RenderBeginTag( writer );
			
			// apply CssClass for current element
			if( _element.TreeWeb.DefaultElementCssClass.Length > 0 )
				_td.CssClass = _element.TreeWeb.DefaultElementCssClass;
			if( _element.CssClass.Length > 0 )
				_td.CssClass = _element.CssClass;

			//_td.Style.Add( "display", "none" );
			_td.Attributes.Add( "nowrap", "yes" );
			_td.RenderBeginTag( writer );

			// render a &nbsp; at the begining of each element
			writer.Write( "&nbsp;" );

			// render element indentation based on element level
			string _indentation = "";
      int nIndentationPerStep = _element.TreeWeb.Indentation;
			for( int index = 0; index < (_element.Level * nIndentationPerStep); index++ )
				_indentation += _indentationStep;
			writer.Write( _indentation );

			// render the expand/collapse link if the element has child elements and is expandable.
			if( _element.HasElements )
			{
				HyperLink _link = new HyperLink();
				Image _image = new Image();

        if( _element.TreeWeb.Expandable && _element.Expandable )
        {
          if( _element.IsExpanded )
          {
            _link.Text = _expcol[1];
            if( _element.TreeWeb.ExpandedElementImage.Length > 0 )
              _image.ImageUrl = _element.TreeWeb.ExpandedElementImage;
          }
          else
          {
            _link.Text = _expcol[0];
            if( _element.TreeWeb.CollapsedElementImage.Length > 0 )
              _image.ImageUrl = _element.TreeWeb.CollapsedElementImage;
          }
        }

				string scriptCode = "javascript:";
				scriptCode += _element.TreeWeb.Page.GetPostBackEventReference( _element.TreeWeb, _element.ID );
				_link.NavigateUrl = scriptCode;

				if( _image.ImageUrl.Length > 0 )
				{
					_link.RenderBeginTag( writer );
					_image.RenderControl( writer );
					_link.RenderEndTag( writer );
				}
				else
					_link.RenderControl( writer );

				_image = null;
				_link = null;
				writer.Write( "&nbsp;" );
			}

			// render checkbox
			if( _element.TreeWeb.CheckBoxes || _element.CheckBox )
			{
				CheckBox _checkbox = new CheckBox();
				
				_checkbox.ID = _element.ID + TreeElement._separator + TreeElement._checkboxIDSufix;
				_checkbox.RenderControl( writer );

				_checkbox = null;
				
				// write a non-breaking space before the element text
				writer.Write( "&nbsp;" );
			}

			// render element's image if it has one
			if( _element.ImageIndex > -1 )
			{
				ElementImage _elementImage = _element.TreeWeb.ImageList[_element.ImageIndex];
				if( _elementImage != null )
				{
					Image _image = new Image();

					_image.ImageUrl = _elementImage.ImageUrl;
					_image.RenderControl( writer );

					_image = null;

					// write a non-breaking space before the element text
					writer.Write( "&nbsp;" );
				}
			}
			
			// render element text as a link if NavigateUrl is present or otherwise as normal text
			if( _element.NavigateUrl.Length > 0 )
			{
				HyperLink _linkNavigateUrl = new HyperLink();
				
				_linkNavigateUrl.Text = _element.Text;
				_linkNavigateUrl.NavigateUrl = _element.NavigateUrl;
				if( _element.TreeWeb.Target.Length > 0 )
					_linkNavigateUrl.Target = _element.TreeWeb.Target;
				if( _element.Target.Length > 0 )
					_linkNavigateUrl.Target = _element.Target;
				if( _element.ToolTip.Length > 0 )
					_linkNavigateUrl.ToolTip = _element.ToolTip;
				_linkNavigateUrl.RenderControl( writer );
				
				_linkNavigateUrl = null;
			}
			else
			{
				Label _label = new Label();

				_label.Text = _element.Text;
				if( _element.ToolTip.Length > 0 )
					_label.ToolTip = _element.ToolTip;
				_label.RenderControl( writer );

				_label = null;
			}

			_td.RenderEndTag( writer );
			_tr.RenderEndTag( writer );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		protected void RenderChildren( HtmlTextWriter writer )
		{
			foreach( TreeElement _elem in _element.Elements )
				_elem.Render( writer );
		}

	}
}
