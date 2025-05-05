using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Common
{
    public class AesCryptography
    {
        private static Aes Create(CipherMode cipherMode, PaddingMode paddingMode, int keySize)
        {
            Aes myAes = Aes.Create();
            myAes.Mode = cipherMode;
            myAes.Padding = paddingMode;
            myAes.KeySize = keySize;
            myAes.BlockSize = 128;
            return myAes;
        }

        private static Aes Create(CipherMode cipherMode, PaddingMode paddingMode, int keySize, byte[] key, byte[] iv)
        {
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");
            Aes myAes = Aes.Create();
            myAes.Mode = cipherMode;
            myAes.Padding = paddingMode;
            myAes.KeySize = keySize;
            myAes.BlockSize = 128;

            myAes.Key = key;
            myAes.IV = iv;

            return myAes;
        }

        public static (string key, string iv) Keys(CipherMode cipherMode = CipherMode.CBC,
            PaddingMode paddingMode = PaddingMode.PKCS7, int keySize = 256)
        {
            using Aes myAes = Create(cipherMode, paddingMode, keySize);
            string key = Convert.ToBase64String(myAes.Key);
            string iv = Convert.ToBase64String(myAes.IV);
            return (key, iv);
        }

        public static (string key, byte[] keyBytes) KeysToBytesToHex(CipherMode cipherMode = CipherMode.CBC,
            PaddingMode paddingMode = PaddingMode.PKCS7, int keySize = 256)
        {
            using Aes myAes = Create(cipherMode, paddingMode, keySize);
            var key = myAes.Key;
            StringBuilder hex = new StringBuilder(key.Length * 2);
            foreach (byte b in key)
                hex.AppendFormat("{0:x2}", b);
            return (hex.ToString(), key);
        }

        public static (byte[] key, byte[] iv) KeysToBytes(CipherMode cipherMode = CipherMode.CBC,
            PaddingMode paddingMode = PaddingMode.PKCS7, int keySize = 256)
        {
            using Aes myAes = Create(cipherMode, paddingMode, keySize);
            return (myAes.Key, myAes.IV);
        }

        public static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv,
            CipherMode cipherMode, PaddingMode paddingMode, int keySize)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            using Aes aesAlg = Create(cipherMode, paddingMode, keySize, key, iv);
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();
            return encrypted;
        }

        public static string Encrypt(string plainText, byte[] key, byte[] iv, CipherMode cipherMode,
            PaddingMode paddingMode, int keySize)
        {
            var bytesEncrypt = EncryptStringToBytes(plainText, key, iv, cipherMode, paddingMode, keySize);
            return Convert.ToBase64String(bytesEncrypt);
        }

        public static string EncryptStringFromBytesHex(string plainText, byte[] key, CipherMode cipherMode,
            PaddingMode paddingMode, int keySize)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            using Aes aesAlg = Create(cipherMode, paddingMode, keySize, key, key);
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encrypted = msEncrypt.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherText, byte[] key, byte[] iv, CipherMode cipherMode,
            PaddingMode paddingMode, int keySize = 256)
        {
            byte[] bytesCipher = Convert.FromBase64String(cipherText);
            var plainText = DecryptStringFromBytes(bytesCipher, key, iv, cipherMode, paddingMode, keySize);
            return plainText;
        }

        public static string DecryptFromKeyHex(string cipherText, string key, CipherMode cipherMode,
            PaddingMode paddingMode, int keySize)
        {
            byte[] bytesCipher = Convert.FromBase64String(cipherText);
            var keyByte = hexToBytes(key);
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            string? plaintext = null;
            using Aes aesAlg = Create(cipherMode, paddingMode, keySize, keyByte, keyByte);
            ICryptoTransform decrypt = aesAlg.CreateDecryptor();
            plaintext = Encoding.ASCII.GetString(decrypt.TransformFinalBlock(bytesCipher, 0, bytesCipher.Length));
            return plaintext;
        }

        public static byte[] hexToBytes(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv, CipherMode cipherMode,
            PaddingMode paddingMode, int keySize = 256)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));

            using Aes aesAlg = Create(cipherMode, paddingMode, keySize, key, iv);
            ICryptoTransform decrypt = aesAlg.CreateDecryptor();
            var plaintextBytes = decrypt.TransformFinalBlock(cipherText, 0, cipherText.Length);
            string plaintext = Encoding.UTF8.GetString(plaintextBytes);
            return plaintext;
        }
    }
}
