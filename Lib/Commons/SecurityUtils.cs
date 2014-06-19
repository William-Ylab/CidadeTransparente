using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Commons
{
    public class SecurityUtils
    {
        private static string key = "#$@2014Am4Rr1b0$$";
        private static ICryptoTransform rijndaelDecryptor;
        private static byte[] rawSecretKey = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                              0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        /// <summary>
        /// Criptografa senhas em base MD5
        /// </summary>
        /// <param name="senha">Senha a ser criptografada</param>
        /// <returns>Senha criptografada</returns>
        public static string criptografaSenha(string senha)
        {
            if (!(senha.Equals(String.Empty)))
            {
                System.Security.Cryptography.MD5 _md5Criptographer = System.Security.Cryptography.MD5.Create();
                byte[] _senhaPreCripto = System.Text.Encoding.Default.GetBytes("Am4R51b0_Tr4N$p4r3nC14" + senha);
                byte[] _senhaHashCode = _md5Criptographer.ComputeHash(_senhaPreCripto, 0, _senhaPreCripto.Length);

                StringBuilder _senhaMD5 = new StringBuilder();

                foreach (byte _byte in _senhaHashCode)
                {
                    _senhaMD5.Append(_byte.ToString("x2"));
                }

                return _senhaMD5.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Criptografa valores na base DES
        /// </summary>
        /// <param name="valor">Valor a ser criptografado</param>
        /// <returns>Valor criptografado</returns>
        public static string criptografar(string valor)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(key)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(valor);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                //Set up stream to contain the encryption
                MemoryStream msS = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Descriptografa valores com base DES
        /// </summary>
        /// <param name="valor">Valor a ser descriptografado</param>
        /// <returns>Valor descriptografado</returns>
        public static string descriptografar(string value)
        {
            try
            {
                //Set up the encryption objects
                using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(key)))
                {
                    byte[] RawBytes = Convert.FromBase64String(value);
                    ICryptoTransform ictD = acsp.CreateDecryptor();

                    //RawBytes now contains original byte array, still in Encrypted state

                    //Decrypt into stream
                    MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                    CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                    //csD now contains original byte array, fully decrypted

                    //return the content of msD as a regular string
                    return (new StreamReader(csD)).ReadToEnd();
                }
            }
            catch (Exception)
            {
                //Se a nova encriptação Não funcionar, tentar encriptar do modo antigo
                return descriptografar(Convert.FromBase64String(value));
            }
        }

        public static string descriptografar(byte[] encryptedData)
        {
            byte[] passwordKey = encodeDigest(key);
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Padding = PaddingMode.PKCS7;

            rijndaelDecryptor = rijndael.CreateDecryptor(passwordKey, rawSecretKey);


            byte[] newClearData = rijndaelDecryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.ASCII.GetString(newClearData);
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }

        private static byte[] encodeDigest(string text)
        {
            MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = Encoding.ASCII.GetBytes(text);
            return x.ComputeHash(data);
        }
    }
}
