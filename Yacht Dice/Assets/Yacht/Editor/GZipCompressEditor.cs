using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

#if UNITY_EDITOR

public class GZipCompressEditor : GZipCompress
{
	public static string Hasing(string key, string msg)
	{
		byte[] result;

		byte[] msg_buffer = new ASCIIEncoding().GetBytes(msg);
		byte[] key_buffer = new ASCIIEncoding().GetBytes(key);

		HMACSHA1 h = new HMACSHA1(key_buffer);

		result = h.ComputeHash(msg_buffer);

		return Convert.ToBase64String(result)
				.Replace("\\", "A")
				.Replace(":", "B")
				.Replace("/", "C")
				.Replace("*", "D")
				.Replace("?", "E")
				.Replace("\"", "F")
				.Replace("'", "1")
				.Replace("<", "2")
				.Replace(">", "3")
				.Replace("|", "4")
				.Replace("+", "5")
				.Replace("=", "6")
				.Replace("-", "7")
				.Replace("_", "8")
				.Replace("!", "9")
				.Substring(0, 18)
			;
	}

	public static byte[] Zip(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);

		using MemoryStream msi = new MemoryStream(bytes);
		using MemoryStream mso = new MemoryStream();
		
		using (GZipStream gs = new GZipStream(mso, CompressionMode.Compress))
		{
			CopyTo(msi, gs);
		}

		return mso.ToArray();
	}
	
	public static string ComputeSha256Hash(string rawData)
	{
		// Create a SHA256   
		using SHA256 sha256Hash = SHA256.Create();
		
		// ComputeHash - returns byte array  
		byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
		// Convert byte array to a string   
		StringBuilder builder = new StringBuilder();  
		foreach (byte t in bytes)
		{
			builder.Append(t.ToString("x2"));
		}
		
		return builder.ToString();
	}  
}

#endif