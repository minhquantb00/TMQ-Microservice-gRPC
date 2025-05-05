using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace TMQ.Common
{
    public static class Extension
    {
        public const string DateFormatVN = "dd/MM/yyyy";

        public const string DateFormatOther = "MM/dd/yyyy";
        // transform object into Identity data type (integer).

        public static int AsId(this object item, int defaultId = -1)
        {
            if (item == null)
                return defaultId;

            if (!int.TryParse(item.ToString(), out var result))
                return defaultId;

            return result;
        }

        // transform object into integer data type.

        public static int AsInt(this object? item, int defaultInt = default(int))
        {
            if (item == null)
                return defaultInt;

            if (!int.TryParse(item.ToString(), out var result))
                return defaultInt;

            return result;
        }

        public static int AsEnumToInt(this object? item, int defaultInt = default(int))
        {
            if (item == null)
                return defaultInt;
            return (int)item;
        }

        public static long AsEnumToLong(this object? item, long defaultInt = default(long))
        {
            if (item == null)
                return defaultInt;
            return (long)item;
        }

        public static long AsLong(this object item, long defaultInt = default(long))
        {
            if (item == null)
                return defaultInt;

            if (!long.TryParse(item.ToString(), out var result))
                return defaultInt;

            return result;
        }

        public static long DoubleAsLong(this double item, long defaultInt = default(long))
        {
            return Convert.ToInt64(item);
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.int64.tryparse?view=net-6.0
        public static long AsLong(this object item, NumberStyles styles, long defaultInt = default(long))
        {
            if (item == null)
                return defaultInt;

            return CallTryParse(item.ToString(), styles, defaultInt);
        }

        private static long CallTryParse(string stringToConvert, NumberStyles styles, long defaultInt = default(long))
        {
            CultureInfo provider;
            if ((styles & NumberStyles.AllowCurrencySymbol) > 0)
                provider = new CultureInfo("en-US");
            else
                provider = CultureInfo.InvariantCulture;

            bool success = Int64.TryParse(stringToConvert, styles, provider, out long number);
            if (success)
                return number;
            return defaultInt;
        }

        // transform object into double data type

        public static double AsDouble(this object item, double defaultDouble = default(double))
        {
            if (item == null)
                return defaultDouble;

            if (!double.TryParse(item.ToString(), out var result))
                return defaultDouble;

            return result;
        }

        public static decimal AsDecimal(this object? item, decimal defaultDecimal = default(decimal))
        {
            if (item == null)
                return defaultDecimal;

            if (!decimal.TryParse(item.ToString(), out var result))
                return defaultDecimal;

            return result;
        }

        public static decimal? AsDecimalRound(this object? item, int decimalNumber = 2,
            decimal defaultDecimal = default(decimal))
        {
            if (item == null)
                return null;

            return decimal.Round(item.AsDecimal(), 2, MidpointRounding.AwayFromZero);
        }

        public static decimal? AsDecimalNullable(this object? item)
        {
            if (item == null)
                return null;

            if (!decimal.TryParse(item.ToString(), out var result))
                return null;

            return result;
        }

        public static short AsShort(this object item, short defaultShort = default(short))
        {
            if (item == null)
                return defaultShort;

            if (!short.TryParse(item.ToString(), out var result))
                return defaultShort;

            return result;
        }

        public static byte AsByte(this object item, byte defaultByte = default(byte))
        {
            if (item == null)
                return defaultByte;

            if (!byte.TryParse(item.ToString(), out var result))
                return defaultByte;

            return result;
        }
        // transform object into string data type

        public static string AsString(this object? item, string defaultString = default(string))
        {
            if (item == null || item.Equals(DBNull.Value))
                return defaultString;
            var value = item.ToString().Trim();
            return string.IsNullOrEmpty(value) ? defaultString : value;
        }

        public static bool IsNullOrEmpty(this string? source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static string AsEmpty(this object? item)
        {
            return item == null ? string.Empty : item.ToString().Trim();
        }

        public static string AsNull(this string? item)
        {
            return string.IsNullOrEmpty(item) ? null : item.Trim();
        }

        public static string AsMoney(this decimal item, bool haveCurrency = true)
        {
            if (item > 0)
            {
                if (haveCurrency)
                {
                    return item.ToString("#,##.## VND", new CultureInfo("vi-VN"));
                }
                else
                {
                    return item.ToString("#,##.##", new CultureInfo("vi-VN"));
                }
            }

            if (haveCurrency)
            {
                return string.Format("0 VND");
            }
            else
            {
                return string.Format("0");
            }
        }

        public static string AsMoney(this int item, bool haveCurrency = true)
        {
            if (item > 0)
            {
                if (haveCurrency)
                {
                    return item.ToString("#,##.## VND", new CultureInfo("vi-VN"));
                }
                else
                {
                    return item.ToString("#,##.##", new CultureInfo("vi-VN"));
                }
            }

            if (haveCurrency)
            {
                return string.Format("0 VND");
            }
            else
            {
                return string.Format("0");
            }
        }

        public static string AsMoney(this decimal? item, bool haveCurrency = true)
        {
            if (item.HasValue)
            {
                if (haveCurrency)
                {
                    return item.Value.ToString("#,##.## VND", new CultureInfo("vi-VN"));
                }
                else
                {
                    return item.Value.ToString("#,##.##", new CultureInfo("vi-VN"));
                }
            }

            if (haveCurrency)
            {
                return string.Format("0 VND");
            }
            else
            {
                return string.Format("0");
            }
        }

        public static string AsMoney(this int? item, bool haveCurrency = true)
        {
            if (item.HasValue)
            {
                if (haveCurrency)
                {
                    return item.Value.ToString("#,##.## VND", new CultureInfo("vi-VN"));
                }
                else
                {
                    return item.Value.ToString("#,##.##", new CultureInfo("vi-VN"));
                }
            }

            if (haveCurrency)
            {
                return string.Format("0 VND");
            }
            else
            {
                return string.Format("0");
            }
        }

        public static string AsMoneyBackEnd(this decimal item, bool haveCurrency = true)
        {
            if (item > 0)
            {
                if (haveCurrency)
                {
                    return item.ToString("#,##.## VNĐ", new CultureInfo("en-US"));
                }
                else
                {
                    return item.ToString("#,##.##", new CultureInfo("en-US"));
                }
            }

            if (haveCurrency)
            {
                return string.Format("0 VND");
            }
            else
            {
                return string.Format("0");
            }
        }

        public static string AsDateView(this DateTime item)
        {
            return item.ToString("dd/MM/yyyy");
        }

        public static string AsDateView(this DateTime? item)
        {
            if (item.HasValue)
                return item.Value.ToString("dd/MM/yyyy");
            return string.Empty;
        }

        public static string AsDateView(this DateTime? item, string format)
        {
            if (item.HasValue)
                return item.Value.ToString(format);
            return string.Empty;
        }

        public static string AsDateView(this DateTime item, string format)
        {
            return item.ToString(format);
        }

        public static string? AsDateView(this string? dateString, string originalFormat, string displayFormat)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return dateString;
            }

            try
            {
                var date = DateTime.ParseExact(dateString, originalFormat, CultureInfo.InvariantCulture);
                return date.ToString(displayFormat);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string AsDateTimeView(this DateTime? item)
        {
            if (item.HasValue)
                return item.Value.ToString("dd/MM/yyyy HH:mm:ss");
            return string.Empty;
        }

        public static string AsDateTimeView(this DateTime item)
        {
            return item.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string AsDateTimeViewISO8601(this DateTime item)
        {
            return item.ToString("yyyy-MM-ddTHH:mm:ss.fff zzz");
        }

        public static string AsDateTimeFullDateTimeView(this DateTime item)
        {
            // Mon, 14 Mar 2022 14:48:26 +0700
            return item.ToString("ddd, dd MMMM yyyy HH:mm:ss zzz");
        }

        public static string AsDateTimeRFC822(this DateTime date)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString().PadLeft(2, '0');
            if (offset < 0)
            {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }

            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'));
        }

        public static string AsDateTimeViewISO8601(this DateTime? item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return item.Value.AsDateTimeViewISO8601();
        }

        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public static DateTime GetCurrentDateUtc()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
        // transform object into DateTime data type.

        public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            if (!DateTime.TryParse($"{item:yyyy-MM-dd HH:mm:ss.fff}", out var result))
                return defaultDateTime;

            return result;
        }

        public static DateTime AsLongToDateTime(this long item)
        {
            try
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddMilliseconds(item);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static long AsLongToGMT(this long? item, int timeZone)
        {
            var time = item ?? 0;
            try
            {
                return time == 0 ? 0 : time - timeZone * 60 * 60 * 1000;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DateTime? AsLongToDateTimeNullable(this long item, DateTime? defaultDateTime = null)
        {
            if (item == 0) return defaultDateTime;
            try
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddMilliseconds(item);
            }
            catch (Exception)
            {
                return defaultDateTime;
            }
        }

        public static long AsUnixTimeStamp(this DateTime item)
        {
            try
            {
                return (long)item.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
            catch (Exception)
            {
                return (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
        }

        public static long? AsUnixTimeStampNullable(this DateTime? item, long? defaultDateTime = null)
        {
            if (item == null) return defaultDateTime;
            try
            {
                return (long)item?.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
            catch (Exception)
            {
                return defaultDateTime;
            }
        }

        public static DateTime AsDateTime(this string item, string fomat, DateTime defaultDateTime = default(DateTime))
        {
            if (item == null || string.IsNullOrEmpty(item))
                return defaultDateTime;
            try
            {
                var result = DateTime.ParseExact(item, fomat, CultureInfo.InvariantCulture);
                return result;
            }
            catch (Exception)
            {
                return defaultDateTime;
            }
        }

        public static DateTime? AsDateTimeNullable(this string item, DateTime? defaultDateTime = null)
        {
            return AsDateTimeNullable(item, "yyyy-MM-dd HH:mm:ss.fff", defaultDateTime);
        }

        public static DateTime? AsDateTimeNullable2(this string item, DateTime? defaultDateTime = null)
        {
            return AsDateTimeNullable(item, "yyyy-MM-ddTHH:mm:ss.fffZ", defaultDateTime);
        }

        public static DateTime? AsDateTimeNullable3(this string? item, DateTime? defaultDateTime = null)
        {
            if (item == null || string.IsNullOrEmpty(item))
                return defaultDateTime;
            try
            {
                var result = Convert.ToDateTime(item);
                return result;
            }
            catch (Exception)
            {
                return defaultDateTime;
            }
        }

        public static DateTime? AsDateTimeNullable(this string? item, string format, DateTime? defaultDateTime = null)
        {
            if (item == null || string.IsNullOrEmpty(item))
                return defaultDateTime;
            try
            {
                var result = DateTime.ParseExact(item, format, CultureInfo.InvariantCulture);
                return result;
            }
            catch (Exception)
            {
                return defaultDateTime;
            }
        }

        public static DateTime? AsToUniversalTime(this DateTime? dateTime)
        {
            return dateTime?.ToUniversalTime();
        }

        public static DateTime? AsToLocalTime(this DateTime? dateTime)
        {
            return dateTime?.ToLocalTime();
        }
        // transform object into bool data type

        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        public static bool AsBool(this object? item, bool defaultBool = default(bool))
        {
            if (item == null)
                return defaultBool;

            return new List<string>() { "yes", "y", "true", "1" }.Contains(item!.ToString()!.ToLower());
        }

        // transform string into byte array

        public static byte[] AsByteArray(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Convert.FromBase64String(s);
        }

        // transform object into base64 string.

        public static string AsBase64String(this object item)
        {
            if (item == null)
                return null;
            ;

            return Convert.ToBase64String((byte[])item);
        }

        // transform object into Guid data type

        public static Guid AsGuid(this object item)
        {
            try
            {
                return new Guid(item.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        // concatenates SQL and ORDER BY clauses into a single string

        public static string OrderBy(this string sql, string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
                return sql;

            return sql + " ORDER BY " + sortExpression;
        }

        public static string AsArrayJoin(this string[]? strings)
        {
            if (strings?.Length > 0)
            {
                return string.Join(",", strings);
            }

            return string.Empty;
        }

        public static string AsArrayJoin(this List<string> strings)
        {
            if (strings?.Count > 0)
            {
                return string.Join(",", strings);
            }

            return string.Empty;
        }

        public static string AsArrayJoin(this string[]? strings, string separator)
        {
            if (strings?.Length > 0)
            {
                return string.Join(separator, strings);
            }

            return string.Empty;
        }

        public static string AsArrayJoin(this long[] strings)
        {
            if (strings?.Length > 0)
            {
                return string.Join(",", strings);
            }

            return string.Empty;
        }

        public static string AsArrayJoin(this ulong[] strings)
        {
            if (strings?.Length > 0)
            {
                return string.Join(",", strings);
            }

            return string.Empty;
        }

        public static string AsArrayJoinWithSeparator(this string[] strings, string separator)
        {
            if (strings?.Length > 0)
            {
                return string.Join(separator, strings);
            }

            return string.Empty;
        }

        public static string AsArrayJoin(this int[]? strings)
        {
            if (strings?.Length > 0)
            {
                return string.Join(",", strings);
            }

            return string.Empty;
        }

        public static string AsSqlLike(this string strings)
        {
            if (strings?.Length > 0)
            {
                return $"%{strings}%";
            }

            return string.Empty;
        }
        // takes an enumerable source and returns a comma separate string.
        // handy for building SQL Statements (for example with IN () statements) from object collections

        public static string CommaSeparate<T, U>(this IEnumerable<T> source, Func<T, U> func)
        {
            return string.Join(",", source.Select(s => func(s).ToString()).ToArray());
        }

        public static string ByteArrayToString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static DateTime? StringVniPaseDateTime(string strDateTime, string format)
        {
            try
            {
                DateTime dt;
                DateTime.TryParseExact(strDateTime, format, new CultureInfo("en-US"), DateTimeStyles.None, out dt);
                return dt == DateTime.MinValue ? DateTime.MaxValue : dt;
            }
            catch (Exception)
            {
                return DateTime.MaxValue;
            }
        }

        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant |
            RegexOptions.Compiled);

        public static string StripHtml(this string target)
        {
            if (!String.IsNullOrEmpty(target))
            {
                return StripHTMLExpression.Replace(target, string.Empty);
            }

            return target;
        }


        public static string Sha256Encrypt(string password, string salt)
        {
            string saltAndPwd = String.Concat(password, salt);
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(saltAndPwd));
            string hashedPwd = String.Concat(byteArrayToString(hashedDataBytes), salt);
            return hashedPwd;
        }

        public static string CreateHmac(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacmd5 = new HMACMD5(keyByte))
            {
                byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }

            return output.ToString();
        }

        public static string CreateSalt(string userName)
        {
            var random = Guid.NewGuid().ToString();
            string username = String.Concat(userName, random);
            byte[] userBytes;
            string salt;
            userBytes = ASCIIEncoding.ASCII.GetBytes(username);
            long XORED = 0x00;
            foreach (int x in userBytes)
                XORED = XORED ^ x;
            Random rand = new Random(Convert.ToInt32(XORED));
            salt = rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            return salt;
        }

        public const string UniChars =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯqwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_-";

        public static HashSet<char> UnicodeCharter;
        public const string DefaultSqlSearchText = "DefaultSqlSearchText";
        public const string DefaultSqlSearchTextParameter = "@DefaultSqlSearchText";

        public static string AsSqlSearchText(this string item, string defaultValue = DefaultSqlSearchText)
        {
            if (string.IsNullOrEmpty(item))
            {
                return defaultValue;
            }

            if (UnicodeCharter == null)
            {
                UnicodeCharter = new HashSet<char>(UniChars.Select(p => p));
            }

            StringBuilder stringBuilder = new StringBuilder();
            bool lastCharterValid = true;
            int i = 0;
            foreach (var s in item)
            {
                if (UnicodeCharter.Contains(s))
                {
                    stringBuilder.Append(s);
                    lastCharterValid = true;
                    i++;
                }
                else
                {
                    if (!lastCharterValid || i == 0)
                    {
                        continue;
                    }

                    if (s == ' ')
                    {
                        stringBuilder.Append(" NEAR ");
                    }
                    else
                    {
                        stringBuilder.Append(" OR ");
                    }

                    lastCharterValid = false;
                }
            }

            string text = stringBuilder.ToString().Trim();
            if (text.EndsWith("NEAR"))
            {
                text = text.Remove(text.Length - 4, 4);
            }
            else if (text.EndsWith("OR"))
            {
                text = text.Remove(text.Length - 2, 2);
            }

            return text;
        }

        public static bool IsValidEmail(string strIn)
        {
            try
            {
                MailAddress m = new MailAddress(strIn);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            // Return true if strIn is in valid e-mail format.
            //return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static void ExceptionAddParam(this Exception exception, string method, object input)
        {
            exception.Data["Method"] = method;
            exception.Data["Input"] = input;
        }

        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEqualsWith<T>(this T dest, T source)
        {
            foreach (var propDest in dest.GetType().GetProperties())
            {
                var sourcePropertyValue = source.GetType().GetProperty(propDest.Name).GetValue(source, null);
                var destPropertyValue = propDest.GetValue(dest, null);
                if (!Equals(sourcePropertyValue, destPropertyValue))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEqualsWith<T>(this List<T> dest, List<T> source)
        {
            if (dest.Count != source.Count)
            {
                return false;
            }

            for (int i = 0; i < dest.Count; i++)
            {
                foreach (var propDest in dest[i].GetType().GetProperties())
                {
                    var sourcePropertyValue = source[i].GetType().GetProperty(propDest.Name).GetValue(source[i], null);
                    var destPropertyValue = propDest.GetValue(dest[i], null);
                    if (!Equals(sourcePropertyValue, destPropertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string PriceToString(this decimal n)
        {
            if (n <= 0)
            {
                return "0";
            }

            return n.ToString();
        }

        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public static string DecodeBase64(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }

        public static string EncodeXml(string title)
        {
            string unxml = title;
            if (!string.IsNullOrEmpty(unxml))
            {
                unxml = unxml.Replace("'", "&apos;");
                unxml = unxml.Replace("\"", "&quot;");
                unxml = unxml.Replace("&", "&amp;");
                unxml = unxml.Replace("", string.Empty);
                //unxml = unxml.Replace("’",string.Empty );
            }

            return unxml;
        }

        public static DateTime AsDateTimeWithoutSecond(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }
    }
}
