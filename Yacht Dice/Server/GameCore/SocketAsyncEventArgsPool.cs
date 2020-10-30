using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Service.TCP
{
	public class SocketAsyncEventArgsPool
	{
		static SocketAsyncEventArgsPool instance { get; set; }
		public static SocketAsyncEventArgsPool Instance {
			get
			{
				if (instance == null)
				{
					instance = new SocketAsyncEventArgsPool(10);
				}

				return instance;
			}
		}
		
		Stack<SocketAsyncEventArgs> m_pool;
 
		// Initializes the object pool to the specified size
		//
		// The "capacity" parameter is the maximum number of
		// SocketAsyncEventArgs objects the pool can hold
		public SocketAsyncEventArgsPool(int capacity)
		{
			m_pool = new Stack<SocketAsyncEventArgs>(capacity);
		}
 
		// Add a SocketAsyncEventArg instance to the pool
		//
		// The "item" parameter is the SocketAsyncEventArgs instance
		// to add to the pool
		public void Push(SocketAsyncEventArgs item)
		{
			if (item == null) { throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
			lock (m_pool)
			{
				m_pool.Push(item);
			}
		}
 
		// Removes a SocketAsyncEventArgs instance from the pool
		// and returns the object removed from the pool
		public SocketAsyncEventArgs Pop()
		{
			lock (m_pool)
			{
				return m_pool.Pop();
			}
		}
 
		// The number of SocketAsyncEventArgs instnaces in the pool
		public int Count
		{
			get { return m_pool.Count; }
		}
	}
}