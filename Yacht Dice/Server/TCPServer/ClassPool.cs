using System.Collections.Generic;
using CQ;

namespace Service.TCP.Server
{
	public class ClassPool<T> : Singleton<ClassPool<T>> 
	{
		Stack<T> container = new Stack<T>();
		
		public T Pop()
		{
			return container.Pop();
		}

		public void Push(T t)
		{
			container.Push(t);
		}
	}
}