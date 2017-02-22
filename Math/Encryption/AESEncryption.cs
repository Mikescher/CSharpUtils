using System.IO;
using System.Security.Cryptography;

namespace MSHC.Math.Encryption
{
	public static class AESEncryption
	{
		public static byte[] DecryptCBC256(byte[] data, byte[] key, byte[] iv)
		{
			if (iv == null) iv = new byte[16];

			using (var rijndaelManaged = new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
			using (var memoryStream = new MemoryStream(data))
			using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(key, iv), CryptoStreamMode.Read))
			{
				byte[] buffer = new byte[16 * 1024];
				using (MemoryStream ms = new MemoryStream())
				{
					int read;
					while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						ms.Write(buffer, 0, read);
					}
					return ms.ToArray();
				}
			}
		}
	}
}
