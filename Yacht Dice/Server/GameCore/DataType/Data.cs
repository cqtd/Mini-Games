using System;
using System.Runtime.InteropServices;

namespace Service.TCP
{
	/// <example>https://www.genericgamedev.com/general/converting-between-structs-and-byte-arrays/</example>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public class Data<T> where T : class
	{
		public Data()
		{
			
		}

		public byte[] Serialize()
		{
			int size = Marshal.SizeOf(typeof(T));
			byte[] array = new byte[size];
			IntPtr ptr = Marshal.AllocHGlobal(size);

			Marshal.StructureToPtr(this, ptr, true);
			Marshal.Copy(ptr, array, 0, size);
			Marshal.FreeHGlobal(ptr);

			return array;
		}

		public static T Deserialize(byte[] array)
		{
			int size = Marshal.SizeOf(typeof(T));
			IntPtr ptr = Marshal.AllocHGlobal(size);
			
			Marshal.Copy(array, 0, ptr, size);
			T s = (T) Marshal.PtrToStructure(ptr, typeof(T));
			Marshal.FreeHGlobal(ptr);

			return s;
		}
	}
}