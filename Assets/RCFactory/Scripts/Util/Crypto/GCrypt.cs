using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public class GCrypt
{
	static public string RSAEncrypt(byte[] szValue, byte[] e, byte[] n)
	{
		// RSA 암호화.
		
		byte[]						encryptedData;		
		RSACryptoServiceProvider 	rsa 		= new RSACryptoServiceProvider();
		RSAParameters 				RSAKeyInfo	= new RSAParameters();
		
		RSAKeyInfo.Exponent = e;
		RSAKeyInfo.Modulus 	= n;
		
		rsa.ImportParameters(RSAKeyInfo);
		encryptedData = rsa.Encrypt(szValue, false);

		StringBuilder sBuilder = new StringBuilder();
		for(int i=0; i<encryptedData.Length; ++i)
		{
			sBuilder.Append(encryptedData[i].ToString("x2"));
		}
		return sBuilder.ToString();
	}
	
	static public string RSAEncrypt(string sValue, string sPubKey)
	{
		// RSA 암호화.

		byte [] keybuf = Convert.FromBase64String(sPubKey);		
		sPubKey = (new UTF8Encoding()).GetString(keybuf);
		System.Security.Cryptography.RSACryptoServiceProvider oEnc = new RSACryptoServiceProvider(); //암호화
		
		oEnc.FromXmlString(sPubKey);

		byte [] inbuf = (new UTF8Encoding()).GetBytes(sValue);
		byte [] encbuf = oEnc.Encrypt(inbuf, false);

		return Convert.ToBase64String(encbuf);
	}
	
	static public string RSADecrypt(byte[] szValue, byte[] e, byte[] n, byte[] d, byte[] p, byte[] q, byte[] dmp, byte[] dmq, byte[] coeff)
	{
		// RSA 복호화.
		
		byte[]						decryptedData;
		RSACryptoServiceProvider 	rsa 		= new RSACryptoServiceProvider();
		RSAParameters 				RSAKeyInfo	= new RSAParameters();
		
		RSAKeyInfo.Exponent = e;
		RSAKeyInfo.Modulus 	= n;
		RSAKeyInfo.D 		= d;
		RSAKeyInfo.P 		= p;
		RSAKeyInfo.Q 		= q;
		RSAKeyInfo.DP 		= dmp;
		RSAKeyInfo.DQ 		= dmq;
		RSAKeyInfo.InverseQ = coeff;
		
		rsa.ImportParameters(RSAKeyInfo);
		decryptedData = rsa.Decrypt(szValue, false);
		return Convert.ToBase64String(decryptedData);
	}
	
	static public string RSADecrypt(string sValue, string sPrvKey)
	{
		// RSA 복호화.

		byte [] inbuf = Convert.FromBase64String(sPrvKey);
		sPrvKey =  (new UTF8Encoding()).GetString(inbuf);

		System.Security.Cryptography.RSACryptoServiceProvider oDec = new System.Security.Cryptography.RSACryptoServiceProvider(); //복호화
		oDec.FromXmlString(sPrvKey);

		byte [] srcbuf = Convert.FromBase64String(sValue);
		byte [] decbuf = oDec.Decrypt(srcbuf, false);

		string sDec = (new UTF8Encoding()).GetString(decbuf,0,decbuf.Length);
		return sDec;
	}
	
	static public String AES_encrypt(String Input, String key)
	{
		// AES 암호화.
		
		if( 0 == Input.Length )
			return Input;
		
		RijndaelManaged aes = new RijndaelManaged();

		aes.KeySize 	= 256;
		aes.BlockSize 	= 128;
		aes.Mode 		= CipherMode.CBC;
		aes.Padding 	= PaddingMode.PKCS7;
		aes.Key 		= Encoding.UTF8.GetBytes(key);        
		aes.IV 			= new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		
		var 	encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
		byte[] 	xBuff 	= null;
		
		using (var ms = new MemoryStream())
		{
			using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
			{
				byte[] xXml = Encoding.UTF8.GetBytes(Input);
				cs.Write(xXml, 0, xXml.Length);
			}
			
			xBuff = ms.ToArray();
		}
		
		String Output = Convert.ToBase64String(xBuff);		
		aes = null;		
		return Output;
	}
	
	static public String AES_decrypt(String Input, String key)
	{
		// AES 복호화.
		
		if( 0 == Input.Length )
			return Input;
		
		RijndaelManaged aes = new RijndaelManaged();
		aes.KeySize 	= 256;
		aes.BlockSize 	= 128;
		aes.Mode 		= CipherMode.CBC;
		aes.Padding 	= PaddingMode.PKCS7;
		aes.Key 		= Encoding.UTF8.GetBytes(key);
		aes.IV 			= new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		
		var 	decrypt = aes.CreateDecryptor();
		byte[] 	xBuff = null;
		
		using (var ms = new MemoryStream())
		{
			using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
			{
				byte[] xXml = Convert.FromBase64String(Input);
				cs.Write(xXml, 0, xXml.Length);
			}
			
			xBuff = ms.ToArray();
		}
		
		String Output = Encoding.UTF8.GetString(xBuff);		
		aes = null;		
		return Output;
	}
}
