using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TMQ.Common
{
    public class UrlExtensions
    {
        public static string ValidRequest(string currentPageUrl, HashSet<string>? queriesValidate,
            Dictionary<string, string> queriesValue)
        {
            if (string.IsNullOrEmpty(currentPageUrl))
            {
                return string.Empty;
            }

            if (currentPageUrl.Length > 0)
            {
                if (currentPageUrl.StartsWith("https://localhost"))
                {
                    currentPageUrl = currentPageUrl.Substring(23);
                }

                if (currentPageUrl.StartsWith("https://"))
                {
                    var index = currentPageUrl.IndexOf("/", 8, StringComparison.Ordinal);
                    currentPageUrl = index >= 0 ? currentPageUrl.Substring(index) : "/";
                }

                currentPageUrl = currentPageUrl.Replace("{", string.Empty);
                currentPageUrl = currentPageUrl.Replace("}", string.Empty);
            }

            var urlDetail = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))
                : currentPageUrl;

            string? queryString = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(currentPageUrl.IndexOf('?'))
                : null;

            if (queryString?.Length > 0)
            {
                if (queriesValidate is not { Count: > 0 })
                {
                    queryString = string.Empty;
                }
                else
                {
                    var newQueryString = HttpUtility.ParseQueryString(queryString);
                    var queryStringInvalid =
                        newQueryString.AllKeys?.Where(p =>
                                (
                                    p?.Length > 0 &&
                                    !queriesValidate!.Contains(p.ToLower())
                                ) || string.IsNullOrEmpty(p)
                            )
                            ?.ToArray();
                    if (queryStringInvalid?.Length > 0)
                    {
                        foreach (var key in queryStringInvalid)
                        {
                            newQueryString.Remove(key);
                        }
                    }

                    queryString = newQueryString.ToString();
                }

                if (queriesValue?.Count > 0 && queryString?.Length > 0)
                {
                    var newQueryString = HttpUtility.ParseQueryString(queryString);
                    foreach (var key in newQueryString.AllKeys)
                    {
                        if (key?.Length > 0)
                        {
                            if (queriesValue.TryGetValue(key.ToLower(), out string? value))
                            {
                                newQueryString[key] = value;
                            }
                        }
                    }

                    queryString = newQueryString.ToString();
                }
            }

            var newUrl = queryString?.Length > 0 ? $"{urlDetail}?{queryString}" : urlDetail;
            newUrl = HttpUtility.UrlDecode(newUrl);
            return newUrl;
        }

        public static string ReplaceQueryStringParam(string currentPageUrl, string paramToReplace, string newValue)
        {
            if (currentPageUrl?.Length > 0)
            {
                if (currentPageUrl.StartsWith("https://localhost"))
                {
                    currentPageUrl = currentPageUrl.Substring(23);
                }

                if (currentPageUrl.StartsWith("https://"))
                {
                    var index = currentPageUrl.IndexOf("/", 8, StringComparison.Ordinal);
                    currentPageUrl = index >= 0 ? currentPageUrl.Substring(index) : "/";
                }

                currentPageUrl = currentPageUrl.Replace("{", string.Empty);
                currentPageUrl = currentPageUrl.Replace("}", string.Empty);
            }

            string urlWithoutQuery = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))
                : currentPageUrl;

            string queryString = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(currentPageUrl.IndexOf('?'))
                : null;

            var queryParamList = queryString != null
                ? HttpUtility.ParseQueryString(queryString)
                : HttpUtility.ParseQueryString(string.Empty);

            if (newValue?.Length > 0)
            {
                if (queryParamList[paramToReplace] != null)
                {
                    queryParamList[paramToReplace] = newValue;
                }
                else
                {
                    queryParamList.Add(paramToReplace, newValue);
                }
            }
            else
            {
                if (queryParamList[paramToReplace] != null)
                {
                    queryParamList.Remove(paramToReplace);
                }
            }


            string newUrl = $"{urlWithoutQuery}?{queryParamList}";
            newUrl = HttpUtility.UrlDecode(newUrl);
            return newUrl;
        }

        public static string ReplacePagingQueryStringParam(string currentPageUrl, string newValue,
            HashSet<string>? queriesValidate, string prefix = "page")
        {
            if (currentPageUrl?.Length > 0)
            {
                if (currentPageUrl.StartsWith("https://localhost"))
                {
                    currentPageUrl = currentPageUrl.Substring(23);
                }

                if (currentPageUrl.StartsWith("https://"))
                {
                    var index = currentPageUrl.IndexOf("/", 8, StringComparison.Ordinal);
                    currentPageUrl = index >= 0 ? currentPageUrl.Substring(index) : "/";
                }

                currentPageUrl = currentPageUrl.Replace("{", string.Empty);
                currentPageUrl = currentPageUrl.Replace("}", string.Empty);
            }

            string? urlWithoutQuery = currentPageUrl?.IndexOf('?') >= 0
                ? currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))
                : currentPageUrl;

            string? queryString = currentPageUrl?.IndexOf('?') >= 0
                ? currentPageUrl.Substring(currentPageUrl.IndexOf('?'))
                : null;
            if (queryString?.Length > 0)
            {
                if (queriesValidate is not { Count: > 0 })
                {
                    queryString = string.Empty;
                }
                else
                {
                    var newQueryString = HttpUtility.ParseQueryString(queryString);
                    var queryStringInvalid =
                        newQueryString.AllKeys?.Where(p =>
                                (
                                    p?.Length > 0 &&
                                    !queriesValidate!.Contains(p.ToLower())
                                ) || string.IsNullOrEmpty(p)
                            )
                            ?.ToArray();
                    if (queryStringInvalid?.Length > 0)
                    {
                        foreach (var key in queryStringInvalid)
                        {
                            newQueryString.Remove(key);
                        }
                    }

                    queryString = newQueryString.ToString();
                }
            }


            string template = $"{prefix}\\d{{1,10}}$";
            Regex rx = new Regex(template, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = rx.Match(urlWithoutQuery);
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                if (groups.Count < 1)
                {
                    return string.Empty;
                }

                string page = groups[0].Value;
                urlWithoutQuery = urlWithoutQuery.Substring(0, urlWithoutQuery.Length - page.Length - 1);
            }

            if (queryString?.Length > 0 && !queryString.StartsWith("?"))
            {
                queryString = $"?{queryString}";
            }

            var newUrl = $"{urlWithoutQuery}-{prefix}{newValue}{queryString}";
            newUrl = HttpUtility.UrlDecode(newUrl);
            return newUrl;
        }

        public static string ReplacePagingDetailQueryStringParam(string currentPageUrl, string newValue,
            HashSet<string>? queriesValidate,
            string exrhtml = ".html", string prefix = "page")
        {
            if (currentPageUrl?.Length > 0)
            {
                if (currentPageUrl.StartsWith("https://localhost"))
                {
                    currentPageUrl = currentPageUrl.Substring(23);
                }

                if (currentPageUrl.StartsWith("https://"))
                {
                    var index = currentPageUrl.IndexOf("/", 8, StringComparison.Ordinal);
                    currentPageUrl = index >= 0 ? currentPageUrl.Substring(index) : "/";
                }

                currentPageUrl = currentPageUrl.Replace("{", string.Empty);
                currentPageUrl = currentPageUrl.Replace("}", string.Empty);
            }

            var urlDetail = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))
                : currentPageUrl;

            string urlWithoutQuery = urlDetail.Replace(".html", "");

            string? queryString = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(currentPageUrl.IndexOf('?'))
                : null;
            if (queryString?.Length > 0)
            {
                if (queriesValidate is not { Count: > 0 })
                {
                    queryString = string.Empty;
                }
                else
                {
                    var newQueryString = HttpUtility.ParseQueryString(queryString);
                    var queryStringInvalid =
                        newQueryString.AllKeys?.Where(p =>
                                (
                                    p?.Length > 0 &&
                                    !queriesValidate!.Contains(p.ToLower())
                                ) || string.IsNullOrEmpty(p)
                            )
                            ?.ToArray();
                    if (queryStringInvalid?.Length > 0)
                    {
                        foreach (var key in queryStringInvalid)
                        {
                            newQueryString.Remove(key);
                        }
                    }

                    queryString = newQueryString.ToString();
                }
            }

            string template = $"{prefix}\\d{{1,10}}$";
            Regex rx = new Regex(template, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = rx.Match(urlWithoutQuery);
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                if (groups.Count < 1)
                {
                    return string.Empty;
                }

                string page = groups[0].Value;
                urlWithoutQuery = urlWithoutQuery.Substring(0, urlWithoutQuery.Length - page.Length - 1);
            }

            if (queryString?.Length > 0 && !queryString.StartsWith("?"))
            {
                queryString = $"?{queryString}";
            }

            var newUrl = $"{urlWithoutQuery}-{prefix}{newValue}{exrhtml}{queryString}";
            newUrl = HttpUtility.UrlDecode(newUrl);
            return newUrl;
        }

        public static string SplitUrlByStringParamKeepParam(string currentPageUrl)
        {
            if (string.IsNullOrEmpty(currentPageUrl))
                return string.Empty;

            if (currentPageUrl.StartsWith("https://localhost"))
            {
                currentPageUrl = currentPageUrl.Substring(23);
            }

            if (currentPageUrl.StartsWith("https://"))
            {
                var index = currentPageUrl.IndexOf("/", 8, StringComparison.Ordinal);
                currentPageUrl = index >= 0 ? currentPageUrl.Substring(index) : "/";
            }

            if (currentPageUrl.StartsWith("http://"))
            {
                var index = currentPageUrl.IndexOf("/", 7, StringComparison.Ordinal);
                currentPageUrl = currentPageUrl.Substring(index);
            }

            return currentPageUrl;
        }

        public static string SplitUrlByReMoveLastByChar(string currentPageUrl, string st = "")
        {
            if (string.IsNullOrEmpty(currentPageUrl))
                return string.Empty;

            if (currentPageUrl?.Length > 0)
            {
                if (currentPageUrl.StartsWith("https://localhost"))
                {
                    currentPageUrl = currentPageUrl.Substring(23);
                }

                if (currentPageUrl.StartsWith("https://"))
                {
                    var index = currentPageUrl.IndexOf("/", 8, StringComparison.Ordinal);
                    currentPageUrl = index >= 0 ? currentPageUrl.Substring(index) : "/";
                }

                string urlWithoutQuery = currentPageUrl.IndexOf('?') >= 0
                    ? currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))
                    : currentPageUrl;

                var split = urlWithoutQuery.Split(st);
                var results = string.Join(st, split.Take(split.Length - 1));
                results = HttpUtility.UrlDecode(results);
                return results;
            }

            return string.Empty;
        }

        public static string ImageCreateThumpUrl(string fullUrl, int width, string hashKey, out bool isGif)
        {
            isGif = false;
            if (fullUrl is not { Length: > 0 })
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(hashKey))
            {
                return fullUrl;
            }

            if (!fullUrl.StartsWith("http://") && !fullUrl.StartsWith("https://"))
            {
                return fullUrl;
            }

            Uri baseUri = new Uri(fullUrl);
            StringBuilder newUrl = new StringBuilder($"{baseUri.Scheme}://{baseUri.Host}{baseUri.AbsolutePath}");
            //if (width > 0)
            {
                string path = $"{baseUri.AbsolutePath}{width}{hashKey}";
                //isGif = baseUri.AbsolutePath.ToLower().EndsWith(".gif");
                using var md5 = MD5.Create();
                var md5Hash = md5.ComputeHash(Encoding.ASCII.GetBytes(path));
                string base64 = Convert.ToBase64String(md5Hash);
                string base64Replace = base64.Replace("+", "-").Replace("/", "_").Replace("=", string.Empty);
                newUrl.Append($"?width={width}&s={base64Replace}");
                if (isGif)
                {
                    newUrl.Append("&t=video");
                }
            }

            return newUrl.ToString();
        }

        public static string ImageCreateThumpUrl(string fullUrl, int width, string hashKey)
        {
            if (fullUrl is not { Length: > 0 })
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(hashKey))
            {
                return fullUrl;
            }

            if (!fullUrl.StartsWith("http://") && !fullUrl.StartsWith("https://"))
            {
                return fullUrl;
            }

            Uri baseUri = new Uri(fullUrl);
            string path = $"{baseUri.AbsolutePath}{width}{hashKey}";
            using var md5 = MD5.Create();
            var md5Hash = md5.ComputeHash(Encoding.ASCII.GetBytes(path));
            string base64 = Convert.ToBase64String(md5Hash);
            string base64Replace = base64.Replace("+", "-").Replace("/", "_").Replace("=", string.Empty);
            string newUrl = $"{baseUri.Scheme}://{baseUri.Host}{baseUri.AbsolutePath}?width={width}&s={base64Replace}";
            return newUrl;
        }

        public static string ImageCreateThumpUrl(Uri uri, int width, string hashKey)
        {
            if (string.IsNullOrEmpty(hashKey))
            {
                return uri.ToString();
            }

            string path = $"{uri.AbsolutePath}{width}{hashKey}";
            using var md5 = MD5.Create();
            var md5Hash = md5.ComputeHash(Encoding.ASCII.GetBytes(path));
            string base64 = Convert.ToBase64String(md5Hash);
            string base64Replace = base64.Replace("+", "-").Replace("/", "_").Replace("=", string.Empty);
            string newUrl = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}?width={width}&s={base64Replace}";
            return newUrl;
        }

        public static string ImageCreateThumpUrlByAbsolutePath(string absolutePath, int width, string hashKey)
        {
            string path = $"{absolutePath}{width}{hashKey}";
            using var md5 = MD5.Create();
            var md5Hash = md5.ComputeHash(Encoding.ASCII.GetBytes(path));
            string base64 = Convert.ToBase64String(md5Hash);
            string base64Replace = base64.Replace("+", "-").Replace("/", "_").Replace("=", string.Empty);
            string newAbsolutePath = $"{absolutePath}?width={width}&s={base64Replace}";
            return newAbsolutePath;
        }

        public static string UrlWithToken(string fullUrl, string hashKey)
        {
            if (fullUrl is not { Length: > 0 })
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(hashKey))
            {
                return fullUrl;
            }

            Uri baseUri = new Uri(fullUrl);
            string base64Replace = GeneratorToken(baseUri.AbsolutePath, hashKey);
            string newUrl = $"{baseUri.Scheme}://{baseUri.Host}{baseUri.AbsolutePath}?s={base64Replace}";
            return newUrl;
        }

        public static string UrlWithToken(Uri uri, string hashKey)
        {
            if (uri.Host is not { Length: > 0 })
            {
                return string.Empty;
            }

            string base64Replace = GeneratorToken(uri.AbsolutePath, hashKey);
            string newUrl = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}?s={base64Replace}";
            if (uri.Query.Length > 0)
            {
                var queriesDictionary = HttpUtility.ParseQueryString(uri.Query);
                foreach (string? item in queriesDictionary)
                {
                    if (item == "s")
                    {
                        continue;
                    }

                    newUrl += $"&{item}={queriesDictionary[item]}";
                }
            }

            return newUrl;
        }

        public static string GeneratorToken(string absolutePath, string hashKey)
        {
            string path = $"{absolutePath}{hashKey}";
            using var md5 = MD5.Create();
            var md5Hash = md5.ComputeHash(Encoding.ASCII.GetBytes(path));
            string base64 = Convert.ToBase64String(md5Hash);
            string base64Replace = base64.Replace("+", "-").Replace("/", "_").Replace("=", string.Empty);
            return base64Replace;
        }
    }
}
