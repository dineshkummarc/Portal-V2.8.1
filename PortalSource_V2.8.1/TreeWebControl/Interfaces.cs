using System;
using System.Web.UI;

namespace iiuga.Web.UI
{
	/// <summary>
	/// ITreeElementWriter interface declaration. All the objects which want to implement
	/// a writer class for the TreeElement should inherit from this interface.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 04 January, 2003
	/// </author>
	internal interface ITreeElementWriter
	{
		/// <summary>
		/// When implemented renders an element inside the tree.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="element"></param>
		void RenderElement( HtmlTextWriter writer, TreeElement element );
	}


	/// <summary>
	/// ITreeWebWriter interface declaration. All the objects which want to implement
	/// a writer class for the TreeWeb should inherit from this interface.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 04 January, 2003
	/// </author>
	internal interface ITreeWebWriter
	{
		/// <summary>
		/// When implemented renders the tree.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="tree"></param>
		void RenderTree( HtmlTextWriter writer, TreeWeb tree );
	}

}
