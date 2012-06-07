using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Diagnostics;

namespace iiuga.Web.UI
{
	// TODO: Implement the TreeElement as an template control, to be able to include other controls in its body.
	// TODO: Add "Attributes" collection to the node. (these attributes can be displayed on the same line with the node)

	/// <summary>
	/// Element types. 
	/// The elements can be added at design time or dinamically at run time.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 19 January, 2003
	/// </author>
	internal enum _ElementType : byte
	{
		_designTimeElement,
		_runTimeElement
	}

	/// <summary>
	/// Represents a tree element. It is displayed as part of TreeWeb control.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 27 December, 2002
	/// </author>
	public class TreeElement : IStateManager
	{
		internal static readonly char	_separator = ':';
		internal static readonly string	_checkboxIDSufix = "checkbox";
		static readonly string			_idPrefix = "element";

		string					_id;
		int						_level;
		ElementsCollection		_elements;
		TreeElement				_parent;
		TreeWeb					_treeWeb;
		StateBag				_state;
		_ElementType			_elementType;
		bool					_marked = false;

		#region TreeElement public properties

		/// <summary>
		/// Gets the ID of the TreeElement.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public string ID 
		{ 
			get { 
				if( _id != null && _id.Length > 0)
					return _id;

				_id = Parent.ID + _separator + _idPrefix + Parent.Elements.IndexOf( this );
				return _id; 
			} 
		}

		/// <summary>
		/// Gets or sets the text to be displayed for the TreeElement.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string Text 
		{ 
			get {
				string _text = (string) ViewState["Text"];
				return( ( _text == null ) ? String.Empty : _text );
			}
			set {
				ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Gets the ElementsCollection for the TreeElement.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		[
		Browsable( true ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public ElementsCollection Elements
		{
			get {
				if( _elements == null )
					_elements = new ElementsCollection( this );

				return _elements;
			}
		}

		/// <summary>
		/// Gets the TreeElement parent object for the current element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 December, 2002
		/// </author>
		[
		Browsable( false )
		]
		public TreeElement Parent
		{
			get {
				return _parent;
			}
		}

		/// <summary>
		/// Gets the TreeWeb object.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 December, 2002
		/// </author>
		[
		Browsable( false )
		]
		public TreeWeb TreeWeb
		{
			get {
				return _treeWeb;
			}
		}

		/// <summary>
		/// Gets whether the element is expanded or not.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 December, 2002
		/// </author>
		[
		DefaultValue( false ),
		Browsable( false )
		]
		public bool IsExpanded 
		{
			get {
        bool bExpanded = true;
        if(TreeWeb.Expandable && Expandable)
        {
          object _expanded = ViewState["IsExpanded"];
          bExpanded =  ( ( _expanded == null ) ? false : (bool)_expanded );
        }
        return bExpanded;
			}
		}

		/// <summary>
		/// Gets whether the element has or not children elements.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 December, 2002
		/// </author>
		[
		DefaultValue( false ),
		Browsable( false )
		]
		public bool HasElements
		{
			get {
				return ( _elements != null && _elements.Count > 0 );
			}
		}

		/// <summary>
		/// Gets the level where the element is in the tree.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		[
		Browsable( false )
		]
		public int Level
		{
			get {
				return _level;
			}
		}

		/// <summary>
		/// Gets whether the element is checked or not.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 05 January, 2003
		/// </author>
		[
		DefaultValue( false ),
		Browsable( false )
		]
		public bool IsChecked 
		{
			get {
				object _checked = ViewState["IsChecked"];
				return ( ( _checked == null ) ? false : (bool)_checked );
			}
		}

		/// <summary>
		/// Gets or sets specific data for the tree element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 18 January, 2003
		/// </author>
		[
		Bindable( true ),
		Browsable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public object Key 
		{ 
			get {
				return (object) ViewState["Key"];
			}
			set {
				ViewState["Key"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the action for the tree element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 18 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string NavigateUrl
		{
			get {
				string _navigateUrl = (string) ViewState["NavigateUrl"];
				return( ( _navigateUrl == null ) ? String.Empty : _navigateUrl );
			}
			set {
				ViewState["NavigateUrl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the target of the tree element action.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 25 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
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
    /// Gets or sets whether the current element is expandable.
    /// </summary>
    /// <author>
    /// Created by Andreas Hauri, 28 May, 2005.
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
		/// Gets or sets whether a check box is displayed next to the current element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 20 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( false ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public bool CheckBox
		{
			get {
				object _checkBox = ViewState["CheckBox"];
				return ( ( _checkBox == null ) ? false : (bool)_checkBox );
			}
			set {
				ViewState["CheckBox"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ToolTip display text for the current element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string ToolTip 
		{
			get {
				string _toolTip = (string) ViewState["ToolTip"];
				return( ( _toolTip == null ) ? String.Empty : _toolTip );
			}
			set {
				ViewState["ToolTip"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the current element is enabled or not.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( true ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public bool Enabled
		{
			get {
				object _enabled = ViewState["Enabled"];
				return ( ( _enabled == null ) ? true : (bool)_enabled );
			}
			set {
				ViewState["Enabled"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the default CssClass for an element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string CssClass
		{
			get {
				string _cssClass = (string) ViewState["CssClass"];
				return ( ( _cssClass == null ) ? String.Empty : _cssClass );
			}
			set {
				ViewState["CssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the default CssClass for the element when OnMouseOver client event occure.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string CssClassOver
		{
			get {
				string _cssClassOver = (string) ViewState["CssClassOver"];
				return ( ( _cssClassOver == null ) ? String.Empty : _cssClassOver );
			}
			set {
				ViewState["CssClassOver"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the image index to be showen before the element in the tree.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 2 February, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( -1 ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public int ImageIndex 
		{
			get {
				object _index = ViewState["ImageIndex"];
				return( ( _index == null ) ? -1 : (int)_index );
			}
			set {
				ViewState["ImageIndex"] = value;
			}
		}

		#endregion

		#region TreeElement private properties

		/// <summary>
		/// Gets the writer object for the tree element.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		private ITreeElementWriter ElementWriter
		{
			get {
				return new TreeElementWriter();
			}
		}

		#endregion

		#region TreeElement internal properties

		/// <summary>
		/// Internaly gets the control StateBag object. The StateBag object is case sensitive.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		internal StateBag ViewState 
		{
			get {
				if( _state == null ) 
				{
					_state = new StateBag( true );
					if( ((IStateManager)this).IsTrackingViewState )
						((IStateManager)_state).TrackViewState();
				}
				return _state;
			}
		}

		/// <summary>
		/// Internaly gets or sets the type of the element. (i.e. design time or run time added)
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 19 January, 2003
		/// </author>
		internal _ElementType ElementType
		{
			get {
				return _elementType;
			}
			set {
				_elementType = value;
			}
		}

		#endregion

		#region TreeElement public methods

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public TreeElement()
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public TreeElement( string text )
		{
			if( text == null )
				throw new ArgumentNullException();

			((IStateManager)this).TrackViewState();
			Text = text;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 18 January, 2003
		/// </author>
		public TreeElement( string text, string navigateUrl )
		{
			if( text == null || navigateUrl == null )
				throw new ArgumentNullException();

			((IStateManager)this).TrackViewState();
			Text = text;
			NavigateUrl = navigateUrl;
		}

		/// <summary>
		/// Expand current element, it has effect only if it has elements.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		public void Expand()
		{
			if( HasElements )
			{
				ViewState["IsExpanded"] = true;
				TreeWeb.OnExpand( new TreeWeb.TreeWebEventArgs( this ) );
			}
		}

		/// <summary>
		/// Collapse current element, it has effect only if it has elements.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		public void Collapse()
		{
			if( HasElements )
			{
				ViewState["IsExpanded"] = false;
				TreeWeb.OnCollapse( new TreeWeb.TreeWebEventArgs( this ) );
			}
		}

		/// <summary>
		/// Finds a specified child element of current tree element.
		/// 
		/// Note: The search algorithm can be optimized.
		/// </summary>
		/// <param name="ID">The searched element id.</param>
		/// <returns>The child element coresponding to the ID parameter value.</returns>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		public TreeElement FindElement( string ID )
		{
			// TODO: optimize the search algorithm.

			foreach( TreeElement element in Elements )
			{
				if( element.ID == ID )
					return element;
			}

			foreach( TreeElement element in Elements )
			{
				TreeElement _found = element.FindElement( ID );
				if( _found != null )
					return _found;
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public virtual void Render( HtmlTextWriter writer ) //, TreeRenderState treeState
		{
			ElementWriter.RenderElement( writer, this );
		}

		/// <summary>
		/// (IStateManager.SaveViewState)
		/// Saves the changes of TreeElement's view state to an Object.
		/// </summary>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		object IStateManager.SaveViewState()
		{
			// save _state state
			object _stateState = null;
			if( _state != null ) 
				_stateState = ((IStateManager)_state).SaveViewState();

			// save _elements state
			object _elementsState = null;
			if( _elements != null ) 
				_elementsState = ((IStateManager)_elements).SaveViewState();

			if ( _stateState == null && _elementsState == null) 
				return null;

			object[] _newState = new object[2];

			_newState[0] = _stateState;
			_newState[1] = _elementsState;

			return _newState;
		}

		/// <summary>
		/// (IStateManager.TrackViewState)
		/// Instructs the TreeElement to track changes to its view state.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		void IStateManager.TrackViewState()
		{
			_marked = true;

			if( _state != null )
				((IStateManager)_state).TrackViewState();
			if( _elements != null ) 
				((IStateManager)_elements).TrackViewState();
		}

		/// <summary>
		/// (IStateManager.LoadViewState)
		/// Loads the TreeElement's previously saved view state.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		void IStateManager.LoadViewState( object state )
		{
			if( state != null )
			{
				object[] _newState = (object[])state;

				if( _newState[0] != null )
					((IStateManager)ViewState).LoadViewState( _newState[0] );
				if( _newState[1] != null )
					((IStateManager)Elements).LoadViewState( _newState[1] );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		bool IStateManager.IsTrackingViewState 
		{
			get {
				return _marked;
			}
		}

		#endregion

		#region TreeElement internal methods

		/// <summary>
		/// Internaly sets the element id.
		/// </summary>
		/// <param name="elementID"></param>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		internal void SetElementID( string elementID )
		{
			_id = elementID;
		}

		/// <summary>
		/// Internaly sets the node parent.
		/// </summary>
		/// <param name="parent"></param>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		internal void SetParent( TreeElement parent ) 
		{
			_parent = parent;
		}

		/// <summary>
		/// Internaly sets the TreeWeb control which contains the node.
		/// </summary>
		/// <param name="treeWeb"></param>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		internal void SetTreeWeb( TreeWeb treeWeb ) 
		{
			_treeWeb = treeWeb;
		}

		/// <summary>
		/// Internaly sets the level where the node is in the tree.
		/// </summary>
		/// <param name="level"></param>
		/// <author>
		/// Created by Iulian Iuga; 04 January, 2003
		/// </author>
		internal void SetLevel( int level ) 
		{
			_level = level;
		}

		#endregion

		#region TreeElement private methods


		#endregion	
	}

}
