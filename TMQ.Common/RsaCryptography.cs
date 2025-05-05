using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMQ.Common
{
    public class RsaCryptography
    {

        public static (string privateKey, string publicKey) GetKeys()
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(4096);
            var privateKey = cryptoServiceProvider.ExportParameters(true);
            var publicKey = cryptoServiceProvider.ExportParameters(false);
            return (RSAParametersToString(privateKey), RSAParametersToString(publicKey));
        }

        private static string RSAParametersToString(RSAParameters rsaParameters)
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, rsaParameters);
            return sw.ToString();
        }

        private static RSAParameters RSAParametersFromString(string key)
        {
            var xs = new XmlSerializer(typeof(RSAParameters));

            using TextReader reader = new StringReader(key);
            var obj = xs.Deserialize(reader);
            if (obj != null)
            {
                var rsaParameters = (RSAParameters)obj;
                return rsaParameters;
            }

            throw new Exception("key invalid");
        }

        public static string Encrypt(RSAParameters publicKey, string plainText)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(publicKey);
            var data = Encoding.UTF8.GetBytes(plainText);
            var cypher = cryptoServiceProvider.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public static string Encrypt(string publicKeyXml, string plainText)
        {
            RSAParameters publicKey = RSAParametersFromString(publicKeyXml);
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(publicKey);
            var data = Encoding.UTF8.GetBytes(plainText);
            var cypher = cryptoServiceProvider.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public static string Encrypt(string publicKey, string plainText, bool keyIsPemFile)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            if (keyIsPemFile)
            {
                cryptoServiceProvider.ImportFromPem(publicKey);
            }
            else
            {
                var key = RSAParametersFromString(publicKey);
                cryptoServiceProvider.ImportParameters(key);
            }

            var data = Encoding.UTF8.GetBytes(plainText);
            var cypher = cryptoServiceProvider.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public static string Encrypt(string publicKey, byte[] plainBytes, bool keyIsPemFile)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            if (keyIsPemFile)
            {
                cryptoServiceProvider.ImportFromPem(publicKey);
            }
            else
            {
                var key = RSAParametersFromString(publicKey);
                cryptoServiceProvider.ImportParameters(key);
            }

            var cypher = cryptoServiceProvider.Encrypt(plainBytes, false);
            return Convert.ToBase64String(cypher);
        }

        public static string Decrypt(RSAParameters privateKey, string cypherText)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(privateKey);
            var dataBytes = Convert.FromBase64String(cypherText);
            var plainText = cryptoServiceProvider.Decrypt(dataBytes, false);
            return Encoding.UTF8.GetString(plainText);
        }

        public static string Decrypt(string privateKeyXml, string cypherText)
        {
            RSAParameters privateKey = RSAParametersFromString(privateKeyXml);
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(privateKey);
            var dataBytes = Convert.FromBase64String(cypherText);
            var plainText = cryptoServiceProvider.Decrypt(dataBytes, false);
            return Encoding.UTF8.GetString(plainText);
        }
        public static byte[] DecryptToBytes(string privateKeyXml, string cypherText)
        {
            RSAParameters privateKey = RSAParametersFromString(privateKeyXml);
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(privateKey);
            var dataBytes = Convert.FromBase64String(cypherText);
            var plainText = cryptoServiceProvider.Decrypt(dataBytes, false);
            return plainText;
        }
        public static string Decrypt(string privateKey, string cypherText, bool keyIsPemFile)
        {
            //RSAParameters privateKey = RSAParametersFromString(privateKeyXml);
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            if (keyIsPemFile)
            {
                cryptoServiceProvider.ImportFromPem(privateKey);
            }
            else
            {
                var key = RSAParametersFromString(privateKey);
                cryptoServiceProvider.ImportParameters(key);
            }

            var dataBytes = Convert.FromBase64String(cypherText);
            var plainText = cryptoServiceProvider.Decrypt(dataBytes, false);
            return Encoding.UTF8.GetString(plainText);
        }

        public static string Decrypt(string privateKeyXml, byte[] cypherBytes)
        {
            RSAParameters privateKey = RSAParametersFromString(privateKeyXml);
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(privateKey);
            var plainText = cryptoServiceProvider.Decrypt(cypherBytes, false);
            return Encoding.UTF8.GetString(plainText);
        }

        public static byte[]? DecryptToBytes(string privateKeyXml, byte[] cypherBytes)
        {
            RSAParameters privateKey = RSAParametersFromString(privateKeyXml);
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(privateKey);
            var plainText = cryptoServiceProvider.Decrypt(cypherBytes, false);
            return plainText;
        }

        public static string SignData(string privateKey, string plainText, bool keyIsPemFile)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            if (keyIsPemFile)
            {
                cryptoServiceProvider.ImportFromPem(privateKey);
            }
            else
            {
                var key = RSAParametersFromString(privateKey);
                cryptoServiceProvider.ImportParameters(key);
            }

            var data = Encoding.UTF8.GetBytes(plainText);
            var cypher = cryptoServiceProvider.SignData(data, SHA1.Create());
            return Convert.ToBase64String(cypher);
        }

        public static bool VerifyData(string publicKey, string cypherText, string signature, bool keyIsPemFile)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            if (keyIsPemFile)
            {
                cryptoServiceProvider.ImportFromPem(publicKey);
            }
            else
            {
                var key = RSAParametersFromString(publicKey);
                cryptoServiceProvider.ImportParameters(key);
            }

            var data = Encoding.UTF8.GetBytes(cypherText);
            var signatureBytes = Convert.FromBase64String(signature);
            var isVerifyData = cryptoServiceProvider.VerifyData(data, SHA1.Create(), signatureBytes);
            return isVerifyData;
        }
    }
}
