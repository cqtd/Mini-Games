using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

public static class GZipCompress
{
	public static void CopyTo(Stream src, Stream dest) {
		byte[] bytes = new byte[4096];

		int cnt;

		while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
			dest.Write(bytes, 0, cnt);
		}
	}

	public static byte[] Zip(string str) {
		var bytes = Encoding.UTF8.GetBytes(str);

		using (var msi = new MemoryStream(bytes))
		using (var mso = new MemoryStream()) {
			using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
				//msi.CopyTo(gs);
				CopyTo(msi, gs);
			}

			return mso.ToArray();
		}
	}

	public static string Unzip(byte[] bytes) {
		using (var msi = new MemoryStream(bytes))
		using (var mso = new MemoryStream()) {
			using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
				//gs.CopyTo(mso);
				CopyTo(gs, mso);
			}

			return Encoding.UTF8.GetString(mso.ToArray());
		}
	}
	
	public static string XORCipher(string data, string key)
	{
		int dataLen = data.Length;
		int keyLen = key.Length;
		char[] output = new char[dataLen];

		for (int i = 0; i < dataLen; ++i)
		{
			output[i] = (char)(data[i] ^ key[i % keyLen]);
		}

		return new string(output);
	}

	public static string Hasing(string key, string msg)
	{
		byte[] result;

		byte[] msg_buffer = new ASCIIEncoding().GetBytes(msg);
		byte[] key_buffer = new ASCIIEncoding().GetBytes(key);

		HMACSHA1 h = new HMACSHA1(key_buffer);

		result = h.ComputeHash(msg_buffer);

		return Convert.ToBase64String(result)
				.Replace("\\","")
				.Replace(":","")
				.Replace("/","")
				.Replace("*","")
				.Replace("?","")
				.Replace("\"","")
				.Replace("'","")
				.Replace("<","")
				.Replace(">","")
				.Replace("|","")
				.Replace("+","")
				.Replace("=","")
				.Replace("-","")
				.Replace("_","")
				.Replace("!","")
				.Substring(0, 18)
			;
	}
	
}