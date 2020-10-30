using System;
using System.Runtime.InteropServices;

namespace Service.TCP
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public class TestPacketRes : Data<TestPacketReq>
	{
		public bool m_success;
		public int m_testIntValue;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string m_message;

		public TestPacketRes()
		{
			
		}
	}
}