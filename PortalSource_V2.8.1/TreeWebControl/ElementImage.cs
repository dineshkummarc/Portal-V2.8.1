using System;
using System.Web.UI;
using System.ComponentModel;

namespace iiuga.Web.UI
{
	/// <summary>
	/// ElementImage class.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; January 30, 2003
	/// </author>
	public class ElementImage : IStateManager
	{
		bool					_marked = false;
		StateBag				_state;

		/// <summary>
		/// ElementImage constructor.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		public ElementImage()
		{}

		/// <summary>
		/// ElementImage constructor.
		/// </summary>
		/// <param name="imageUrl"></param>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		public ElementImage( string imageUrl )
		{
			if( imageUrl == null )
				throw new ArgumentNullException();

			((IStateManager)this).TrackViewState();
			ImageUrl = imageUrl;
		}

		/// <summary>
		/// (IStateManager.IsTrackingViewState)
		/// Gets a value indicating whether the ElementImage is tracking its view state changes.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		bool IStateManager.IsTrackingViewState 
		{
			get {
				return _marked;
			}
		}

		/// <summary>
		/// (IStateManager.TrackViewState)
		/// Instructs the ElementImage to track changes to its view state.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		void IStateManager.TrackViewState()
		{
			_marked = true;
		}

		/// <summary>
		/// (IStateManager.SaveViewState)
		/// Saves the changes of ElementImage's view state to an Object.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		object IStateManager.SaveViewState()
		{
			// save _state state
			object _stateState = null;
			if( _state != null ) 
				_stateState = ((IStateManager)_state).SaveViewState();

			return _stateState;
		}

		/// <summary>
		/// (IStateManager.LoadViewState)
		/// Loads the ElementImage's previously saved view state.
		/// </summary>
		/// <param name="state"></param>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		void IStateManager.LoadViewState( object state )
		{
			if( state != null )
				((IStateManager)ViewState).LoadViewState( state );
		}

		/// <summary>
		/// Gets or sets the location of an image to display in the TreeElement control.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
		/// </author>
		[
		Bindable( true ),
		DefaultValue( "" ),
		PersistenceMode( PersistenceMode.Attribute )
		]
		public string ImageUrl
		{
			get {
				string _imageUrl = (string)ViewState["ImageUrl"];
				return( ( _imageUrl == null ) ? String.Empty : _imageUrl );
			}
			set {
				ViewState["ImageUrl"] = value;
			}
		}

		/// <summary>
		/// Internaly gets the control StateBag object. The StateBag object is case sensitive.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 30 January, 2003
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

	}
}
