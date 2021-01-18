using System.IO;
using System.IO.Compression;
using System.Text;

public class GZipCompress
{
	public static void CopyTo(Stream src, Stream dest)
	{
		byte[] bytes = new byte[4096];

		int cnt;

		while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
		{
			dest.Write(bytes, 0, cnt);
		}
	}

	public static string Unzip(byte[] bytes)
	{
		using MemoryStream msi = new MemoryStream(bytes);
		using MemoryStream mso = new MemoryStream();
		
		using (GZipStream gs = new GZipStream(msi, CompressionMode.Decompress))
		{
			CopyTo(gs, mso);
		}

		return Encoding.UTF8.GetString(mso.ToArray());
	}

	public static string XORCipher(string data, string key)
	{
		int dataLen = data.Length;
		int keyLen = key.Length;
		
		char[] output = new char[dataLen];

		for (int i = 0; i < dataLen; ++i)
		{
			output[i] = (char) (data[i] ^ key[i % keyLen]);
		}

		return new string(output);
	}
}