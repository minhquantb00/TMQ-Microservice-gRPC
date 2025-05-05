using static System.Net.WebRequestMethods;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using OtpNet;
using QRCoder;

namespace TMQ.AccountManager.Services
{
    public class OtpUtility
    {
        public const int OtpEmailStep = 300;
        public const int OtpStep = 300;
        public const int OtpSize = 6;
        public const OtpHashMode OtpHashMode = OtpNet.OtpHashMode.Sha512;

        public string GenerateRandomKey()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            var base32String = Base32Encoding.ToString(key);
            return base32String;
            //var base32Bytes = Base32Encoding.ToBytes(base32String);
        }

        public string GenerateOtp(string secretKey, int otpStep = OtpStep, OtpHashMode otpHashMode = OtpHashMode,
            int otpSize = OtpSize)
        {
            var otp = new Totp(Base32Encoding.ToBytes(secretKey), otpStep, otpHashMode, otpSize);
            var date = DateTime.UtcNow;
            var code = otp.ComputeTotp(date);
            return code;
        }

        public bool VerifyOtp(string secretKey, string otpCode, int otpStep = OtpStep,
            OtpHashMode otpHashMode = OtpHashMode, int otpSize = OtpSize)
        {
            var otp = new Totp(Base32Encoding.ToBytes(secretKey), otpStep, otpHashMode, otpSize);
            var date = DateTime.UtcNow;
            bool valid = otp.VerifyTotp(date, otpCode, out long timeStepMatched, new VerificationWindow(2, 2));
            return valid;
        }

        public string GenerateBarcodeUrl(string secretKey, string user, int otpStep = OtpStep,
            OtpHashMode otpHashMode = OtpHashMode, int otpSize = OtpSize)
        {
            string barcodeUrl =
                $"{KeyUrl.GetTotpUrl(Base32Encoding.ToBytes(secretKey), user, otpStep, otpHashMode, otpSize)}";
            return HttpUtility.UrlEncode(barcodeUrl);
        }

        //https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#311-one-time-password
        //https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#317-sms
        //https://github.com/codebude/QRCoder
        public byte[] GenerateQRCode(string secretKey, string user)
        {
            string barcodeUrl = GenerateBarcodeUrl(secretKey, user);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(barcodeUrl, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
            //QRCode qrCode = new QRCode(qrCodeData);
            // Bitmap qrCodeImage = qrCode.GetGraphic(20);
            //return ImageToByte(qrCodeImage);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public byte[] GenerateQrCode(string code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string ToBase64String(byte[] item)
        {
            return Convert.ToBase64String(item);
        }

        // public static byte[] ImageToByte(Image img)
        // {
        //     using (var stream = new MemoryStream())
        //     {
        //         img.Save(stream, ImageFormat.Png);
        //         return stream.ToArray();
        //     }
        // }
    }

    public enum OtpType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// HOTP
        /// </summary>
        Hotp,

        /// <summary>
        /// TOTP
        /// </summary>
        Totp
    }

    public static class KeyUrl
    {
        /// <summary>
        /// Several constants used for the URL format
        /// </summary>
        private class UrlConstants
        {
            public const string SecretParameter = "secret";

            public const string AlgorithmParameter = "algorithm";

            public const string PeriodParameter = "period";

            public const string CounterParameter = "counter";

            public const string DigitsParameter = "digits";
            public const string ParameterCreation = "&{0}={1}";
            public const string UrlValidationPatterm = @"^[^:]+://[^/]+/[^/\?]+(/?\?[^/]+)?$";
        }

        /// <summary>
        /// Get a url for a TOTP key
        /// </summary>
        /// <param name="key">Plaintext key</param>
        /// <param name="user">The username</param>
        /// <param name="step">Timestep</param>
        /// <param name="mode">Hash mode</param>
        /// <param name="totpSize">Digits</param>
        /// <returns>URL</returns>
        public static string GetTotpUrl(byte[] key, string user, int step = 30, OtpHashMode mode = OtpHashMode.Sha1,
            int totpSize = 6)
        {
            var url = GetBaseKeyUrl(key, user, OtpType.Totp, totpSize);
            if (mode != OtpHashMode.Sha1)
                url += CreateParameter(UrlConstants.AlgorithmParameter, mode);
            if (step != 30)
                url += CreateParameter(UrlConstants.PeriodParameter, step);

            return url;
        }

        /// <summary>
        /// Get a URL for HOTP
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="user">user</param>
        /// <param name="counter">Current Counter</param>
        /// <param name="hotpSize">Digits</param>
        /// <returns></returns>
        public static string GetHotpUrl(byte[] key, string user, long counter, int hotpSize = 6)
        {
            var url = GetBaseKeyUrl(key, user, OtpType.Hotp, hotpSize);

            return url + CreateParameter(UrlConstants.CounterParameter, counter);
        }

        /// <summary>
        /// Gets a URL that conforms to the de-facto standard
        /// created and used by Google
        /// </summary>
        private static string GetBaseKeyUrl(byte[] key, string user, OtpType otpType, int size)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");
            if (key == null || key.Length == 0)
                throw new ArgumentNullException("key");
            if (size != 6 && size != 8)
                throw new ArgumentException("size must be 6 or 8");
            var url = string.Format("otpauth://{0}/{1}?{2}={3}", otpType.ToString().ToLowerInvariant(),
                WebUtility.UrlEncode(user), UrlConstants.SecretParameter, Base32Encoding.ToString(key));
            if (size == 8)
                url += CreateParameter(UrlConstants.DigitsParameter, size);
            return url;
        }


        /// <summary>
        /// Creates a new parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">value</param>
        /// <returns>URL Encoded name value</returns>
        private static string CreateParameter(string name, object value)
        {
            return string.Format(UrlConstants.ParameterCreation, name, WebUtility.UrlEncode(value.ToString()));
        }


        /// <summary>
        /// Takes a URL and converts it 
        /// </summary>
        /// <param name="rawUrl">the url to convert</param>
        /// <returns>An otp object</returns>
        public static Otp FromUrl(string rawUrl)

        {
            string user;

            return FromUrl(rawUrl, out user);
        }


        /// <summary>
        /// Takes a URL and converts it 
        /// </summary>
        /// <param name="rawUrl">the url to convert</param>
        /// <param name="user">The username</param>
        /// <returns>An otp object</returns>
        public static Otp FromUrl(string rawUrl, out string user)

        {
            user = null;

            if (string.IsNullOrWhiteSpace(rawUrl))

                throw new ArgumentNullException("rawUrl");


            if (!Regex.IsMatch(rawUrl, UrlConstants.UrlValidationPatterm))

                throw new ArgumentException("rawUrl is invalid");


            var url = new Uri(rawUrl);


            if (url.Scheme != "otpauth")

                throw new ArgumentException(string.Format("invalid scheme {0}. Must be otpauth://", url.Scheme));


            var otpTypeString = url.Authority;

            OtpType type;

            if (!Enum.TryParse<OtpType>(otpTypeString, true, out type))

                type = OtpType.Unknown;


            switch (type)

            {
                case OtpType.Hotp:

                    return HotpFromUrl(url);

                case OtpType.Totp:

                    return TotpFromUrl(url);

                default:

                    throw new ArgumentException(
                        string.Format("rawUrl contains an invalid operation {0}. Must be hotp or totp", otpTypeString));
            }
        }


        private static Hotp HotpFromUrl(Uri url)
        {
            int digits;
            var collection = ParseAndValidateQueryString(url, out digits);

            if (!ValidateQueryStringFields(collection,
                    UrlConstants.AlgorithmParameter,
                    UrlConstants.CounterParameter,
                    UrlConstants.DigitsParameter,
                    UrlConstants.SecretParameter))

                throw new ArgumentException("Invalid parameter in query string");

            throw new NotImplementedException("HOTP isn't yet implemented");
        }


        private static Totp TotpFromUrl(Uri url)

        {
            int digits;

            var collection = ParseAndValidateQueryString(url, out digits);


            if (!ValidateQueryStringFields(collection,
                    UrlConstants.AlgorithmParameter,
                    UrlConstants.DigitsParameter,
                    UrlConstants.PeriodParameter,
                    UrlConstants.SecretParameter))

                throw new ArgumentException("Invalid parameter in query string");


            OtpHashMode algorithm = OtpHashMode.Sha1;

            if (collection.AllKeys.Contains(UrlConstants.AlgorithmParameter))

            {
                var algorithmRaw = collection[UrlConstants.AlgorithmParameter];

                if (!Enum.TryParse<OtpHashMode>(algorithmRaw, true, out algorithm))

                    throw new ArgumentException(string.Format("Invalid Algorithm {0}", algorithmRaw));
            }


            int period = 30; // the spec indicates that 30 is the default 

            if (collection.AllKeys.Contains(UrlConstants.PeriodParameter))

            {
                int tempPeriod;

                if (int.TryParse(collection[UrlConstants.PeriodParameter], out tempPeriod))

                {
                    if (tempPeriod < 1)

                        throw new ArgumentException(string.Format("Invalid Period {0}, must be at least 1",
                            tempPeriod));

                    else

                        period = tempPeriod;
                }

                else

                    throw new ArgumentException(string.Format("Invalid digits {0}, must be a number",
                        collection[UrlConstants.DigitsParameter]));
            }


            var key = Base32Encoding.ToBytes(collection[UrlConstants.SecretParameter]);

            return new Totp(key,
                step: period,
                mode: algorithm,
                totpSize: digits);
        }

        public static Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index < 0) continue;
                rc[Uri.UnescapeDataString(row.Substring(0, index))] =
                    Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }

            return rc;
        }

        private static NameValueCollection ParseAndValidateQueryString(Uri uri, out int digits)

        {
            if (string.IsNullOrEmpty(uri.Query))

                throw new ArgumentException("Must have a query string");

            var collection = ParseQueryString(uri.Query);


            if (!collection.Keys.Contains(UrlConstants.SecretParameter))

                throw new ArgumentException("must contain secret");


            // digits defaults to 6 according to the URL spec

            int localDigits = 6;

            if (collection.Keys.Contains(UrlConstants.DigitsParameter))

            {
                if (int.TryParse(collection[UrlConstants.DigitsParameter], out localDigits))

                {
                    if (localDigits != 6 && localDigits != 8)

                        throw new ArgumentException(string.Format("Invalid Digits {0}, must be 6 or 8", localDigits));

                    else

                        digits = localDigits;
                }

                else

                    throw new ArgumentException(string.Format("Invalid digits {0}, must be a number",
                        collection[UrlConstants.DigitsParameter]));
            }

            else

                digits = localDigits;


            var res = new NameValueCollection();

            foreach (var c in collection)

            {
                res.Add(c.Key, c.Value);
            }


            return res;
        }
        internal static bool ValidateQueryStringFields(NameValueCollection query, params string[] acceptableValues)

        {
            if (query == null)

                throw new ArgumentNullException("query");


            if (acceptableValues != null)

            {
                var intersection = from k in query.AllKeys
                                   where !acceptableValues.Contains(k)
                                   select k;

                return !intersection.Any();
            }

            else

                return false;
        }
    }
}
