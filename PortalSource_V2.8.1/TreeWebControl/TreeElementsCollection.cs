using System;
using System.Collections;
using System.Web.UI;

namespace iiuga.Web.UI
{
	/// <summary>
	/// ElementsCollection class.
	/// 
	/// Copyright © Iulian Iuga, 2003. All Rights Reserved.
	/// </summary>
	/// <author>
	/// Created by Iulian Iuga; 27 December, 2002
	/// </author>
	public class ElementsCollection : ICollection, IStateManager
	{
		private TreeElement			_proprietor;
		private ArrayList			_members;
		
		private bool				_marked = false;

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		internal ElementsCollection( TreeElement proprietor ) 
		{
			if( proprietor == null )
				throw new ArgumentNullException();

			_proprietor = proprietor;
		}

		#region ElementsCollection public properties

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
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
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public bool IsSynchronized 
		{
			get {
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public object SyncRoot 
		{
			get {
				return this;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public TreeElement this[int index] 
		{
			get {
				if ( _members != null )
					return (TreeElement)( _members[index] );
				else
					return null;
			}
		}

		#endregion

		#region ElementsCollection public methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
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
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <author>
		/// Created by Iulian Iuga; 27 December, 2002
		/// </author>
		public void CopyTo( Array array, int index ) 
		{
			for ( IEnumerator e = GetEnumerator(); e.MoveNext(); )
				array.SetValue( e.Current, index++ );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public int Add( TreeElement element ) 
		{
			// add the element at the end of the elements list
			return AddAt( -1, element );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public int Add( string text ) 
		{
			// add the element at the end of the elements list
			return AddAt( -1, text );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="element"></param>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public int AddAt( int index, TreeElement element ) 
		{
			if( _members == null )
				_members =  new ArrayList();

			int elementIndex = index;
			if( index == -1 )
				elementIndex = _members.Add( element );
			else
				_members.Insert( index, element );

			// configure the new added element
			ConfigureNewElement( element );

			return elementIndex;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public int AddAt( int index, string text ) 
		{
			TreeElement _element = new TreeElement( text );

			return AddAt( index, _element );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		public int IndexOf( TreeElement element ) 
		{
			if( element != null && _members != null )
				return _members.IndexOf( element, 0, Count );

			return -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public void Remove( TreeElement element )
		{
			if(_members == null) return;
			_members.Remove(element);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public void RemoveAt( int index )
		{
			if(_members == null) return;
			_members.RemoveAt(index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 28 December, 2002
		/// </author>
		public void Clear( )
		{
			if(_members == null) return;
			_members.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <author>
		/// Created by Iulian Iuga; 11 January, 2003
		/// </author>
		object IStateManager.SaveViewState()
		{
			if( Count == 0 )
				return null;

			object[] membersState = new object[Count];

			int index = 0;
			foreach( TreeElement _element in this )
				membersState[index] = new object[3] { 
											((IStateManager)_element).SaveViewState(), 
											_element.ElementType,
											index++ };

			return membersState;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 11 January, 2003
		/// </author>
		void IStateManager.LoadViewState( object state )
		{
			if( state != null )
			{
				object[] membersState = (object[])state;
				for( int index = 0; index < membersState.Length; index++ )
				{
					object[] stateElement = (object[])membersState[index];
					if( stateElement[0] != null )
					{
						if( (byte)stateElement[1] == (byte)_ElementType._designTimeElement )
						{
							((IStateManager)this[index]).LoadViewState( stateElement[0] );
						}
						else if( (byte)stateElement[1] == (byte)_ElementType._runTimeElement )
						{
							TreeElement _element = new TreeElement();
							int _elementIndex = AddAt( (int)stateElement[2], _element );

							((IStateManager)this[_elementIndex]).TrackViewState();
							((IStateManager)this[_elementIndex]).LoadViewState( stateElement[0] );
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 11 January, 2003
		/// </author>
		void IStateManager.TrackViewState()
		{
			_marked = true;

			foreach( TreeElement _element in this )
				((IStateManager)_element).TrackViewState();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <author>
		/// Created by Iulian Iuga; 11 January, 2003
		/// </author>
		bool IStateManager.IsTrackingViewState 
		{
			get {
				return _marked;
			}
		}

		#endregion

		#region ElementsCollection private methods

		/// <summary>
		/// Configure internaly the required properties for a new tree element.
		/// </summary>
		/// <param name="element"></param>
		/// <author>
		/// Created by Iulian Iuga; 01 January, 2003
		/// </author>
		private void ConfigureNewElement( TreeElement element )
		{
			element.SetParent( _proprietor );
			element.SetTreeWeb( _proprietor.TreeWeb );
			element.SetLevel( _proprietor.Level + 1 );
			element.ElementType = _ElementType._runTimeElement;

			// configure element children too
			foreach( TreeElement _element in element.Elements )
				ConfigureNewElement( _element );
		}

		#endregion
	}
}
