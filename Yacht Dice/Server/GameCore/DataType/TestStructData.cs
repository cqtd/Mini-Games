using System;
using System.Runtime.InteropServices;

namespace Service.TCP
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct TestStructData
	{
		public ETestType m_testEnum;

		public long m_long;
		public float m_float;
		public bool m_bool;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string m_name;

		public TestStructData(long v_long,
			string v_name,
			float v_float,
			bool v_bool,
			ETestType v_testEnum)
		{
			this.m_long = v_long;
			this.m_float = v_float;
			this.m_bool = v_bool;
			this.m_name = v_name;
			this.m_testEnum = v_testEnum;
		}
	}
	
	[Serializable]
	public enum ETestType
	{
		TEST_TYPE_NONE = -1,
		
		TEST_TYPE_1,
		TEST_TYPE_2,
		TEST_TYPE_3,
		
		TEST_TYPE_COUNT,
	}
}