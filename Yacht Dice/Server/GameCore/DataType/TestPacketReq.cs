using System;
using System.Runtime.InteropServices;

namespace Service.TCP
{
	/// <summary>
	/// Client Request Packet Example
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public class TestPacketReq : Data<TestPacketReq>
	{
		public long m_test_longData;
		public TestStructData m_testData;
		
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public TestStructData[] m_testDataArray = new TestStructData[10];

		public TestPacketReq()
		{
			
		}
	}

}