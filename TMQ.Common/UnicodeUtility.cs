using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TMQ.Common
{
    public class UnicodeUtility
    {
        public const string UniChars =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";

        public const string KoDauChars =
            //"aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyaaaaaaaaaaaaaaaaaeeeeeeeeeeediiiooooooooooooooooooouuuuuuuuuuuyyyyyaadoou";

        public const string UniCharsWithSegments =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ/.";

        public const string KoDauCharsWithSegments =
            //"aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyaaaaaaaaaaaaaaaaaeeeeeeeeeeediiiooooooooooooooooooouuuuuuuuuuuyyyyyaadoou/.";

        public const string KeyBoardChars = " `1234567890-=~!@#$%^&*()_+qwertyuiop[]{}|asdfghjkl;':zxcvbnm,./<>?*-+";

        //public const string KeyBoardCharsNonSpecialCharacters = "1234567890qwertyuiopasdfghjklzxcvbnm";
        public const string KeyBoardCharsNonSpecialCharacters = " 1234567890qwertyuiopasdfghjklzxcvbnm";

        public static string UnicodeToKoDau(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            string retVal = string.Empty;
            foreach (var t in s)
            {
                var pos = UniChars.IndexOf(t.ToString(), StringComparison.Ordinal);
                if (pos >= 0)
                {
                    retVal += KoDauChars[pos];
                }
                else
                {
                    if (KeyBoardChars.IndexOf(t.ToString().ToLower(), StringComparison.Ordinal) >= 0)
                    {
                        retVal += t.ToString();
                    }
                }
            }

            return retVal;
        }

        public static string RemoveUnicodeAndSpecialCharacters(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            string retVal = string.Empty;
            foreach (var t in s)
            {
                var pos = UniChars.IndexOf(t.ToString(), StringComparison.Ordinal);
                if (pos >= 0)
                {
                    retVal += KoDauChars[pos];
                }
                else
                {
                    if (KeyBoardCharsNonSpecialCharacters.IndexOf(t.ToString().ToLower(), StringComparison.Ordinal) >=
                        0)
                    {
                        retVal += t.ToString();
                    }
                }
            }

            return retVal;
        }

        public static string RemoveUnicodeAndSpecialCharactersAndSpace(string s)
        {
            var result = RemoveUnicodeAndSpecialCharacters(s);
            return result.Replace(" ", "");
        }

        public static string ToTitleCaseFirstCharacter(string str)
        {
            return str?.First().ToString().ToUpper() + (str?.Length > 1 ? str?.Substring(1).ToLower() : string.Empty);
        }

        public static string UnicodeToKoDauAndGach(string? s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            string retVal = string.Empty;
            int pos;

            for (int i = 0; i < s.Length; i++)
            {
                pos = UniChars.IndexOf(s[i].ToString(), StringComparison.Ordinal);
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }

            String temp = retVal;
            for (int i = 0; i < retVal.Length; i++)
            {
                pos = Convert.ToInt32(retVal[i]);
                if (!((pos >= 97 && pos <= 122) || (pos >= 65 && pos <= 90) || (pos >= 48 && pos <= 57) || pos == 32 ||
                      pos == 45 || pos == 95))
                    temp = temp.Replace(retVal[i].ToString(), "");
            }

            temp = temp.Replace("_", "-");
            temp = temp.Replace(" ", "-");
            while (temp.EndsWith("-"))
                temp = temp.Substring(0, temp.Length - 1);

            while (temp.IndexOf("--", StringComparison.Ordinal) >= 0)
                temp = temp.Replace("--", "-");

            retVal = temp;

            return retVal.ToLower();
        }

        public static string UnicodeToKoDauAndGachWithSegments(string? s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            string retVal = string.Empty;
            int pos;

            for (int i = 0; i < s.Length; i++)
            {
                pos = UniCharsWithSegments.IndexOf(s[i].ToString(), StringComparison.Ordinal);
                if (pos >= 0)
                    retVal += KoDauCharsWithSegments[pos];
                else
                    retVal += s[i];
            }

            string temp = retVal;
            for (int i = 0; i < retVal.Length; i++)
            {
                pos = Convert.ToInt32(retVal[i]);
                if (!((pos >= 97 && pos <= 122) || (pos >= 65 && pos <= 90) || (pos >= 48 && pos <= 57) || pos == 32 ||
                      pos == 45 || pos == 95 || pos == 47 || pos == 46))
                    temp = temp.Replace(retVal[i].ToString(), "");
            }

            temp = temp.Replace("_", "-");
            temp = temp.Replace(" ", "-");
            while (temp.EndsWith("-"))
                temp = temp.Substring(0, temp.Length - 1);

            while (temp.IndexOf("--", StringComparison.Ordinal) >= 0)
                temp = temp.Replace("--", "-");

            retVal = temp;

            return retVal.ToLower();
        }

        public static string HtmlTabRemove(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            input = WebUtility.HtmlDecode(input);
            input = Regex.Replace(input, "<style(.|\n)*?>(.|\n)*?</style>", string.Empty);
            input = Regex.Replace(input, @"<xml(.|\n)*?>(.|\n)*?</xml>",
                string.Empty); // remove all <xml></xml> tags and anything inbetween.  
            input = Regex.Replace(input, @"<script(.|\n)*?>(.|\n)*?</script>", string.Empty);
            input = Regex.Replace(input, @"<(.|\n)*?>",
                string.Empty); // remove any tags but not there content "<p>bob<span> johnson</span></p>" becomes "bob johnson"

            input = Regex.Replace(input, @"\s{2,}", " ");
            input = input.Replace("\"", string.Empty);
            input = input.Replace("\n", string.Empty);
            //input = WebUtility.JavaScriptStringEncode(input);
            return input;
        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }

        public static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        public static string[] GetAllTextBetween(string input, string firstString, string lastString)
        {
            Regex rg = new Regex($"{firstString}(.+?){lastString}");
            MatchCollection matched = rg.Matches(input);
            List<string> results = new List<string>();
            if (matched.Count > 0)
            {
                for (int count = 0; count < matched.Count; count++)
                {
                    if (matched[count].Groups?.Count > 0)
                    {
                        string value = matched[count].Groups[1].Value.AsEmpty();
                        results.Add(value);
                    }
                }
            }

            return results.Distinct().ToArray();
        }

        public static string RemoveHtmlCharacters(string? s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return s.Replace("<", string.Empty)
                .Replace(">", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("'", string.Empty);
        }
    }
}
