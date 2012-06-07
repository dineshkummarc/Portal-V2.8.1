using System;
using System.Collections;
using System.Web.UI;

namespace iiuga.Web.UI
{
	/// <summary>
	/// ImagesCollection class.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 26 January, 2003
	/// </author>
	public class ImagesCollection : ICollection, IStateManager
	{
		private ArrayList			_members;		
		private bool				_marked = false;

		/// <summary>
		/// ImagesCollection constructor.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public ImagesCollection()
		{
		}

		/// <summary>
		/// (IStateManager.IsTrackingViewState)
		/// Gets a value indicating whether the ImagesCollection is tracking its view state changes.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		bool IStateManager.IsTrackingViewState 
		{
			get {
				return _marked;
			}
		}

		/// <summary>
		/// (IStateManager.TrackViewState)
		/// Instructs the ImagesCollection to track changes to its view state.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		void IStateManager.TrackViewState()
		{
			_marked = true;
		}

		/// <summary>
		/// (IStateManager.SaveViewState)
		/// Saves the changes of ImagesCollection's view state to an Object.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		object IStateManager.SaveViewState()
		{
			if( Count == 0 )
				return null;

			object[] membersState = new object[Count];

			for( int index = 0; index < Count; index++ )
				membersState[index] = ((IStateManager)this[index]).SaveViewState();

			return membersState;
		}

		/// <summary>
		/// (IStateManager.LoadViewState)
		/// Loads the ImagesCollection's previously saved view state.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		void IStateManager.LoadViewState( object state )
		{
			if( state != null )
			{
				object[] membersState = (object[])state;
				for( int index = 0; index < membersState.Length; index++ )
				{
					ElementImage _elementImage = new ElementImage();
					Add( _elementImage );

					((IStateManager)_elementImage).TrackViewState();
					((IStateManager)_elementImage).LoadViewState( membersState[index] );
				}			
			}
		}

		/// <summary>
		/// Gets the number of ElementImage objects from the collection.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public int Count
		{
			get {
				if( _members != null )
					return _members.Count;
				else
					return 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the ImagesCollection is synchronized (thread-safe).
		/// [This property is always FALSE.]
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public bool IsSynchronized 
		{
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the ImagesCollection.
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public object SyncRoot 
		{
			get {
				return this;
			}
		}

		/// <summary>
		/// Copies all the elements of the current ImagesCollection to the specified 
		/// one-dimensional Array starting at the specified destination Array index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public void CopyTo( Array array, int index ) 
		{
			for ( IEnumerator e = GetEnumerator(); e.MoveNext(); )
				array.SetValue( e.Current, index++ );
		}

		/// <summary>
		/// Returns an IEnumerator for the ImagesCollection.
		/// </summary>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public IEnumerator GetEnumerator() 
		{
			if ( _members == null ) 
				_members = new ArrayList();

			return _members.GetEnumerator( 0, Count );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public ElementImage this[int index] 
		{
			get {
				if ( _members != null )
					return (ElementImage)( _members[index] );
				else
					return null;
			}
		}
		/// <summary>
		/// Add an image to the ImagesCollection.
		/// </summary>
		/// <param name="imageUrl"></param>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public void Add( string imageUrl ) 
		{
			Add( new ElementImage( imageUrl ) );
		}

		/// <summary>
		/// Add an image to the ImagesCollection.
		/// </summary>
		/// <param name="elementImage"></param>
		/// <author>
		/// Created by Iulian Iuga; 26 January, 2003
		/// </author>
		public void Add( ElementImage elementImage ) 
		{
			if ( _members == null ) 
				_members = new ArrayList();

			_members.Add( elementImage );
		}

	}
}
