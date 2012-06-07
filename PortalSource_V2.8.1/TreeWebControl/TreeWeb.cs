using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Xml;

namespace iiuga.Web.UI
{	
	/// <summary>
	/// TreeWebBuilder class.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 05 January, 2003
	/// </author>
	public class TreeWebBuilder : ControlBuilder 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="attribs"></param>
		/// <returns></returns>
		public override Type GetChildControlType( string tagName, IDictionary attribs ) 
		{
			if( tagName.ToUpper().EndsWith( "TREEELEMENT" ) )
				return typeof( TreeElement );

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public override void AppendLiteralString( string s ) 
		{
			// override to ignore literals between items
		}
	}

	// TODO: Add Design-Time support.

	/// <summary>
	/// TreeWeb class.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 27 December, 2002
	/// </author>
	[
	ControlBuilderAttribute( typeof( TreeWebBuilder ) ),
	DefaultProperty( "Elements" )
	]
	public class TreeWeb : WebControl, IPostBackEventHandler
	{
		/// <summary>
		/// TreeWebEventArgs
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 23 February, 2003
		/// </author>
		public class TreeWebEventArgs : EventArgs
		{     
			private TreeElement _element;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="element"></param>
			/// <author>
			/// Created by Iulian Iuga; 23 February, 2003
			/// </author>
			public TreeWebEventArgs( TreeElement element )
			{
				this._element = element;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <author>
			/// Created by Iulian Iuga; 23 February, 2003
			/// </author>
			public TreeElement Element 
			{
				get {
					return _element;
				}
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 23 February, 2003
		/// </author>
		public delegate void TreeWebEventHandler( object source, TreeWebEventArgs e );

		/// <summary>
		/// 
		/// </summary>
		public event TreeWebEventHandler Expand;
		/// <summary>
		/// 
		/// </summary>
		public event TreeWebEventHandler Collapse;
		
		private TreeElement			_root;
		private ImagesCollection	_images;

		#region TreeWeb public properties

		/// <summary>
		/// Gets the elements collection of the tree.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		[
		Browsable( true ),
		DefaultValue( null ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public ElementsCollection Elements
		{
			get {
				return Root.Elements;
			}
		}




		/// <summary>
		/// Gets or sets the URL to the image that is showen for a collapsed element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 12 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string CollapsedElementImage
		{
			get {
				object _url = ViewState["CollapsedElementImage"];
				return ( ( _url == null ) ? String.Empty : (string)_url );
			}
			set {
				ViewState["CollapsedElementImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the URL to the image that is showen for an expanded element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 12 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string ExpandedElementImage
		{
			get {
				object _url = ViewState["ExpandedElementImage"];
				return ( ( _url == null ) ? String.Empty : (string)_url );
			}
			set {
				ViewState["ExpandedElementImage"] = value;
			}
		}


    /// <summary>
    /// Gets or sets the default property, for the Expandable feature.
    /// </summary>
    /// <author>
    /// Created by Andreas Hauri, 28 May, 2005
    /// </author>
    [
    Bindable( true ),
    DefaultValue( true ),
    PersistenceMode( PersistenceMode.Attribute )
    ]
    public bool Expandable
    {
      get 
      {
        object _expandable = ViewState["Expandable"];
        return ( ( _expandable == null ) ? true : (bool)_expandable );
      }
      set 
      {
        ViewState["Expandable"] = value;
      }
    }


    /// <summary>
    /// Gets or sets the indentation per level.
    /// </summary>
    /// <author>
    /// Created by Andreas Hauri, 28 May, 2005
    /// </author>
    [
    Bindable( true ),
    DefaultValue( 3 ),
    PersistenceMode( PersistenceMode.Attribute )
    ]
    public int Indentation
    {
      get 
      {
        object _indentation = ViewState["Indentation"];
        int nIndentation = ( ( _indentation == null ) ? 3 : (int)_indentation );
        if(nIndentation < 1)
          nIndentation = 1;
        return nIndentation;
      }
      set 
      {
        ViewState["Indentation"] = value;
      }
    }

		/// <summary>
		/// Gets or sets whether check boxes are displayed next to the tree elements
		/// in the treeweb control.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 20 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( false ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public bool CheckBoxes
		{
			get {
				object _checkBoxes = ViewState["CheckBoxes"];
				return ( ( _checkBoxes == null ) ? false : (bool)_checkBoxes );
			}
			set {
				ViewState["CheckBoxes"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the default target for all tree elements action.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 25 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" )
		]
		public string Target
		{
			get {
				string _target = (string) ViewState["Target"];
				return( ( _target == null ) ? String.Empty : _target );
			}
			set {
				ViewState["Target"] = value;
			}
		}

		/// <summary>
		/// Gets the collection of images urls for the TreeWeb control.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Browsable( true ),
		DefaultValue( null ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public ImagesCollection ImageList
		{
			get {
				if( _images == null )
				{
					_images = new ImagesCollection();
					if( IsTrackingViewState )
						((IStateManager)_images).TrackViewState();
				}

				return _images;
			}
		}

		/// <summary>
		/// Gets or sets the default CssClass for the tree elements.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string DefaultElementCssClass
		{
			get {
				string _defaultElementCssClass = (string) ViewState["DefaultElementCssClass"];
				return ( ( _defaultElementCssClass == null ) ? String.Empty : _defaultElementCssClass );
			}
			set {
				ViewState["DefaultElementCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the default CssClass for the tree elements when OnMouseOver client event occure.
		/// 
		/// [Not Implemented]
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string DefaultElementCssClassOver
		{
			get {
				string _defaultElementCssClassOver = (string) ViewState["DefaultElementCssClassOver"];
				return ( ( _defaultElementCssClassOver == null ) ? String.Empty : _defaultElementCssClassOver );
			}
			set {
				ViewState["DefaultElementCssClassOver"] = value;
			}
		}

		#endregion

		#region TreeWeb protected properties

		#endregion

		#region TreeWeb private properties

		/// <summary>
		/// Gets the tree's root element. (Note: creates the root object on demand)
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		private TreeElement Root
		{
			get {
				if( _root == null ) 
				{
					_root = new TreeElement( "TreeRoot" );
					_root.SetTreeWeb( this );
					_root.SetElementID( UniqueID );
					_root.SetLevel( -1 );
					_root.ViewState["IsExpanded"] = true;
				}
			
				return _root;
			}
		}

		/// <summary>
		/// Gets the writer object for the tree.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		private ITreeWebWriter TreeWriter
		{
			get {
				return new TreeWebWriter();
			}
		}
		
		#endregion

		#region TreeWeb public methods

		/// <summary>
		/// TreeWeb constructor
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public TreeWeb()
		{} 

		/// <summary>
		/// (IPostBackEventHandler.RaisePostBackEvent)
		/// Raise control specific events on post back. 
		/// </summary>
		/// <param name="eventArgument"></param>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		public virtual void RaisePostBackEvent( string eventArgument ) 
		{
			// TODO: raise specific control events
			
			TreeElement element = Root.FindElement( eventArgument );
			if( element != null )
			{
				if( element.IsExpanded )
					element.Collapse();
				else
					element.Expand();
			}

			return;
		}

		/// <summary>
		/// Loads the tree structure from a XML file. [Not Implemented]
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		public void LoadXML( string xmlPath ) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Loads the tree structure from a XML structure. [Not Implemented]
		/// </summary>
		/// <param name="xmlReader"></param>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		public void LoadXML( XmlReader xmlReader ) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Saves the structure of the tree in a XML file. [Not Implemented]
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		public void SaveXML( string xmlPath ) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Saves the structure of the tree in a XML structure. [Not Implemented]
		/// </summary>
		/// <param name="xmlWriter"></param>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		public void SaveXML( XmlWriter xmlWriter ) 
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// OnExpand
		/// </summary>
		/// <param name="e"></param>
		/// <author>
		/// Created by Iulian Iuga; 23 February, 2003
		/// </author>
		public virtual void OnExpand( TreeWebEventArgs e )
		{
			if( Expand != null )
				Expand( this, e );
		}

		/// <summary>
		/// OnCollapse
		/// </summary>
		/// <param name="e"></param>
		/// <author>
		/// Created by Iulian Iuga; 23 February, 2003
		/// </author>
		public virtual void OnCollapse( TreeWebEventArgs e )
		{
			if( Collapse != null )
				Collapse( this, e );
		}

		#endregion

		#region TreeWeb protected methods

		/// <summary>
		/// Render the TreeWeb control.
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		protected override void Render( HtmlTextWriter writer )
		{	
			TreeWriter.RenderTree( writer, this );
		} 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <author>
		/// Created by Iulian Iuga; 11 January, 2003
		/// </author>
		protected override void OnInit( EventArgs e ) 
		{
			base.OnInit(e);

			// configure all parsed elements
			ConfigureParsedElements( Root );
		}

		/// <summary>
		/// (IStateManager.SaveViewState)
		/// Saves the changes of TreeWeb's view state to an Object.
		/// </summary>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		protected override object SaveViewState() 
		{
			object _baseState = base.SaveViewState();
			object _treeState = ((IStateManager)Root).SaveViewState();
			object _imagesState = ((IStateManager)ImageList).SaveViewState();

			object[] _newState = new object[3];
			_newState[0] = _baseState;
			_newState[1] = _treeState;
			_newState[2] = _imagesState;

			return _newState;
		}

		/// <summary>
		/// (IStateManager.LoadViewState)
		/// Loads the TreeWeb's previously saved view state.
		/// </summary>
		/// <param name="state"></param>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		protected override void LoadViewState( object state ) 
		{
			if( state != null ) 
			{
				object[] _newState = (object[])state;

				if( _newState[0] != null )
					base.LoadViewState( _newState[0] );
				if( _newState[1] != null )
					((IStateManager)Root).LoadViewState( _newState[1] );
				if( _newState[2] != null )
					((IStateManager)ImageList).LoadViewState( _newState[2] );
			}
		}

		/// <summary>
		/// (IStateManager.TrackViewState)
		/// Instructs the TreeWeb to track changes to its view state.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		protected override void TrackViewState() 
		{
			// ensure that the base class is tracking the changes to its state
			base.TrackViewState();
			// instructs the _root element to track changes to its view state
			((IStateManager)Root).TrackViewState();
		}

		#endregion

		#region TreeWeb private methods
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <author>
		/// Created by Iulian Iuga; 18 January, 2003
		/// </author>
		private void ConfigureParsedElements( TreeElement element )
		{
			foreach( TreeElement _element in element.Elements ) 
			{
				_element.SetParent( element );
				_element.SetTreeWeb( this );
				_element.SetLevel( element.Level + 1 );
				_element.ElementType = _ElementType._designTimeElement;

				ConfigureParsedElements( _element );
			}
		}

		#endregion
	}
}
