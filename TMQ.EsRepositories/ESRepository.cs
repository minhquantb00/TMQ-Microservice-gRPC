using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.EsRepositories
{
    public class ESRepository : IESRepository
    {
        private readonly ILogger<ESRepository> _logger;
        private readonly EsUrlConfig[] _esUrlConfigs;
        private readonly IHttpClient _httpClient;
        private int _index = 0;
        private const string IndexType = "_doc";
        private readonly string _environment;

        public ESRepository(ILogger<ESRepository> logger, string urls, IHttpClient httpClient, string environment)
        {
            _logger = logger;
            _httpClient = httpClient;
            _environment = environment;
            var urlConfigs = urls.Split(',');
            EsUrlConfig[] esUrlConfigs = new EsUrlConfig[urlConfigs.Length];
            int i = 0;
            foreach (var urlConfig in urlConfigs)
            {
                esUrlConfigs[i] = new EsUrlConfig()
                {
                    IsOk = true,
                    Index = i,
                    Url = urlConfigs[i]
                };
                i++;
            }

            _esUrlConfigs = esUrlConfigs;
        }

        private string Url()
        {
            int i = 0;
            foreach (var esUrlConfig in _esUrlConfigs)
            {
                if (_index >= _esUrlConfigs.Length)
                {
                    _index = 0;
                }

                if (i >= _index)
                {
                    if (esUrlConfig.IsOk)
                    {
                        _index++;
                        return esUrlConfig.Url;
                    }
                }

                i++;
            }

            string? url = _esUrlConfigs.FirstOrDefault(p => p.IsOk)?.Url;
            if (url?.Length > 0)
            {
                return url;
            }

            throw new Exception("Cannot connect to ES");
        }

        private string GetIndexName(string indexName)
        {
            /*if (IsMultipleWebsite)
            {
                return $"es{_environment.AsEmpty()}_{indexName}_{CacheVersion}".ToLower();
            }*/

            return $"es{_environment.AsEmpty()}_{indexName}".ToLower();
        }

        public async Task<string> Add<T>(string indexName, string id, T indexObject,
            Func<string, string, string, Task>? addLog = null, Dictionary<string, string>? parameters = null)
        {
            string urlConfig = Url();
            indexName = GetIndexName(indexName);
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{indexName}/{IndexType}/{id}"
                : $"{urlConfig}/{indexName}/{IndexType}/{id}").ToLower();
            if (parameters?.Count > 0)
            {
                url += "?";
                foreach (var parameter in parameters.Select((value, index) => new { index, value }))
                {
                    if (parameter.index == parameters.Count - 1)
                    {
                        url += parameter.value.Key + "=" + parameter.value.Value;
                    }
                    else
                    {
                        url += parameter.value.Key + "=" + parameter.value.Value + "&";
                    }
                }
            }

            string data = Serialize.JsonSerializeObject(indexObject);
            using var response = await _httpClient.Post(url, data);

            if (addLog != null)
            {
                _ = addLog(url, data, $"{(int)response.StatusCode} ({response.ReasonPhrase})");
            }

            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            try
            {
                var p = await response.Content.ReadAsStringAsync();
                _logger.LogError(p);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> Add<T>((string, string, T)[] indexEses)
        {
            string urlConfig = Url();
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{EsMethodName._bulk}"
                : $"{urlConfig}/{EsMethodName._bulk}").ToLower();
            StringBuilder script = new StringBuilder();
            foreach (var item in indexEses)
            {
                script.AppendLine(
                    $"{{ \"index\" : {{ \"_index\" : \"{GetIndexName(item.Item1)}\",  \"_id\" : \"{item.Item2}\" }} }}"
                        .ToLower());
                string data = Serialize.JsonSerializeObject(item.Item3);
                script.AppendLine(data);
            }

            using var response = await _httpClient.Post(url, script.ToString());
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            try
            {
                var p = await response.Content.ReadAsStringAsync();
                _logger.LogError(p);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> Add<T>((string, string, T)[] indexEses, Dictionary<string, string> parameters = null)
        {
            string urlConfig = Url();
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{EsMethodName._bulk}"
                : $"{urlConfig}/{EsMethodName._bulk}").ToLower();
            if (parameters?.Count > 0)
            {
                url += "?";
                foreach (var parameter in parameters.Select((value, index) => new { index, value }))
                {
                    if (parameter.index == parameters.Count - 1)
                    {
                        url += parameter.value.Key + "=" + parameter.value.Value;
                    }
                    else
                    {
                        url += parameter.value.Key + "=" + parameter.value.Value + "&";
                    }
                }
            }

            StringBuilder script = new StringBuilder();
            foreach (var item in indexEses)
            {
                script.AppendLine(
                    $"{{ \"index\" : {{ \"_index\" : \"{GetIndexName(item.Item1)}\",  \"_id\" : \"{item.Item2}\" }} }}"
                        .ToLower());
                string data = Serialize.JsonSerializeObject(item.Item3);
                script.AppendLine(data);
            }

            using var response = await _httpClient.Post(url, script.ToString());
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            try
            {
                var p = await response.Content.ReadAsStringAsync();
                _logger.LogError(p);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> Remove<T>((string, string, T)[] indexEses)
        {
            string urlConfig = Url();
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{EsMethodName._bulk}"
                : $"{urlConfig}/{EsMethodName._bulk}").ToLower();
            string script = string.Empty;
            foreach (var item in indexEses)
            {
                script =
                    $"{{ \"delete\" : {{ \"_index\" : \"{GetIndexName(item.Item1)}\",  \"_id\" : \"{item.Item2}\" }} }}"
                        .ToLower();
                script += Environment.NewLine;
                string data = Serialize.JsonSerializeObject(item.Item3);
                script += data;
                script += Environment.NewLine;
            }

            using var response = await _httpClient.Post(url, script);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> Remove(string indexName, string script)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}" : $"{urlConfig}/{indexName}")}/_delete_by_query?conflicts=proceed"
                    .ToLower();
            using var response = await _httpClient.Post(url, script);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url} --- {script}");
        }

        public async Task<string> RemoveById(string indexName, string id)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/")
                    ? $"{urlConfig}{indexName}"
                    : $"{urlConfig}/{indexName}")}/_doc/{id}"
                    .ToLower();
            using var response = await _httpClient.Delete(url);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url} --- {id}");
        }

        public async Task<string> Update(string indexName, string id, string script)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}" : $"{urlConfig}/{indexName}")}/_update/{id}"
                    .ToLower();
            using var response = await _httpClient.Post(url, script);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> Update(string indexName, string script)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}" : $"{urlConfig}/{indexName}")}/_update_by_query?conflicts=proceed"
                    .ToLower();
            using var response = await _httpClient.Post(url, script);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> UpdateBulk<T>((string, string, T)[] indexEses)
        {
            string urlConfig = Url();
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{EsMethodName._bulk}"
                : $"{urlConfig}/{EsMethodName._bulk}").ToLower();

            StringBuilder script = new StringBuilder();
            foreach (var item in indexEses)
            {
                script.AppendLine(
                    $"{{ \"update\" : {{ \"_index\" : \"{GetIndexName(item.Item1)}\",  \"_id\" : \"{item.Item2}\" }} }}"
                        .ToLower());
                string data = Serialize.JsonSerializeObject(item.Item3);
                script.AppendLine(data);
            }

            using var response = await _httpClient.Post(url, script.ToString());
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> AddWithShard<T>((string, string, T)[] indexEses, string shard)
        {
            string urlConfig = Url();
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{EsMethodName._bulk}"
                : $"{urlConfig}/{EsMethodName._bulk}").ToLower();
            url += "?routing=" + shard;
            StringBuilder script = new StringBuilder();
            foreach (var item in indexEses)
            {
                script.AppendLine(
                    $"{{ \"index\" : {{ \"_index\" : \"{GetIndexName(item.Item1)}\",  \"_id\" : \"{item.Item2}\" }} }}"
                        .ToLower());
                string data = Serialize.JsonSerializeObject(item.Item3);
                script.AppendLine(data);
            }

            using var response = await _httpClient.Post(url, script.ToString());
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            try
            {
                var p = await response.Content.ReadAsStringAsync();
                _logger.LogError(p);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> UpdateValue(string indexName, string id, string data)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}" : $"{urlConfig}/{indexName}")}/_update/{id}"
                    .ToLower();
            var script = $"{{ \"script\" : {{ \"source\" : \"ctx._source.{data}\", \"lang\" : \"painless\" }} }}";
            using var response = await _httpClient.Post(url, script);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> UpdateValueBulk<T>((string, string, T)[] indexEses)
        {
            string urlConfig = Url();
            var url = (urlConfig.EndsWith("/")
                ? $"{urlConfig}{EsMethodName._bulk}"
                : $"{urlConfig}/{EsMethodName._bulk}").ToLower();

            StringBuilder script = new StringBuilder();
            foreach (var item in indexEses)
            {
                script.AppendLine(
                    $"{{ \"update\" : {{ \"_index\" : \"{GetIndexName(item.Item1)}\",  \"_id\" : \"{item.Item2.ToLower()}\" }} }}");
                string data = $"{{ \"doc\" : {item.Item3} }}";
                script.AppendLine(data);
            }

            var scriptBulk = script.ToString();
            using var response = await _httpClient.Post(url, scriptBulk);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> GetById(string indexName, string id)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}/_doc" : $"{urlConfig}/{indexName}/_doc")}/{id}"
                    .ToLower();
            var response = await _httpClient.Get(url);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> GetById(string indexName, string[] ids)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}/_doc" : $"{urlConfig}/{indexName}/_doc")}/_search"
                    .ToLower();
            var script = $"{{\"query\": {{\"ids\" : {{\"type\" : \"documents\",\"values\" :" +
                         Serialize.JsonSerializeObject(ids.Distinct()) + "}}}}}}";
            using var response = await _httpClient.Post(url, script);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url}");
        }

        public async Task<string> Search(string indexName, string script, bool isAggs = false,
            Func<string, string, string, Task>? addLog = null)
        {
            indexName = GetIndexName(indexName);
            string urlConfig = Url();
            var url = $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{indexName}" : $"{urlConfig}/{indexName}")}/_search"
                .ToLower();
            if (isAggs)
            {
                url += "?scroll=100m";
            }

            using var response = await _httpClient.Post(url, script);
            if (addLog != null)
            {
                addLog(url, script, $"{(int)response.StatusCode} ({response.ReasonPhrase})");
            }

            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url} --- {script}");
        }

        public async Task<string> Search(string[] indexNames, string script, bool isAggs = false,
            Func<string, string, string, Task>? addLog = null, bool isValidatedIndex = false)
        {
            string urlConfig = Url();
            var existsIndexNames = isValidatedIndex ? indexNames : await GetExistsIndexes(indexNames);
            if (existsIndexNames.Length == 0)
            {
                addLog?.Invoke(urlConfig, script, $"IndexNames is missing - {indexNames.AsArrayJoin()}");
                return string.Empty;
            }

            var mergedIndexName = existsIndexNames.AsArrayJoin();
            var url =
                $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{mergedIndexName}" : $"{urlConfig}/{mergedIndexName}")}/_search"
                    .ToLower();

            if (isAggs)
            {
                url += "?scroll=100m";
            }

            using var response = await _httpClient.Post(url, script);
            addLog?.Invoke(url, script, $"{(int)response.StatusCode} ({response.ReasonPhrase})");

            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url} --- {script}");
        }

        public async Task<string> MultiSearch((string, string)[] indexesAndQueries, bool isAggs = false,
            Func<string, string, string, Task>? addLog = null)
        {
            if (indexesAndQueries.Length == 0)
            {
                return string.Empty;
            }

            var targetIndex = GetIndexName(indexesAndQueries[0].Item1);

            // Build script
            string scripts = string.Empty;
            foreach (var item in indexesAndQueries)
            {
                var indexName = GetIndexName(item.Item1);
                var script = item.Item2;
                scripts += $"{{\"index\":\"{indexName}\"}}";
                scripts += Environment.NewLine;
                scripts += script;
                scripts += Environment.NewLine;
            }

            string urlConfig = Url();
            var url = $"{(urlConfig.EndsWith("/") ? $"{urlConfig}{targetIndex}" : $"{urlConfig}/{targetIndex}")}/_msearch"
                .ToLower();
            if (isAggs)
            {
                url += "?scroll=100m";
            }

            using var response = await _httpClient.Post(url, scripts);
            if (addLog != null)
            {
                addLog(url, scripts, $"{(int)response.StatusCode} ({response.ReasonPhrase})");
            }

            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return p;
            }

            throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {url} --- {scripts}");
        }

        public async Task<string[]> GetExistsIndexes(string[] indexNames)
        {
            var existsIndexes = new List<string>();
            var url = Url();
            foreach (var index in indexNames)
            {
                var indexName = GetIndexName(index);
                var urlCreateIndex = url.EndsWith("/") ? $"{url}{indexName}" : $"{url}/{indexName}";
                var urlCheckIndex = $"{urlCreateIndex}?pretty";
                var responseCheckIndex = await _httpClient.Get(urlCheckIndex);
                if (responseCheckIndex.IsSuccessStatusCode)
                {
                    existsIndexes.Add(indexName);
                }
            }

            return existsIndexes.ToArray();
        }

        public async Task<(bool, string)> CreateIndex(string indexName, string indexFile, int numberOfShards)
        {
            string fullFilePath = Path.Combine(Environment.CurrentDirectory, "Script", $"{indexFile}.txt");
            if (!File.Exists(fullFilePath))
            {
                return (false, $"Create index {indexName} fail - scriptFilePath {indexFile} is not found !");
            }

            indexName = GetIndexName(indexName);
            var url = Url();
            var urlCreateIndex = url.EndsWith("/") ? $"{url}{indexName}" : $"{url}/{indexName}";
            var urlCheckIndex = $"{urlCreateIndex}?pretty";
            var responseCheckIndex = await _httpClient.Get(urlCheckIndex);
            if (responseCheckIndex.IsSuccessStatusCode)
            {
                var p = await responseCheckIndex.Content.ReadAsStringAsync();
                return (true, p);
            }

            var content = await System.IO.File.ReadAllTextAsync(fullFilePath);
            if (numberOfShards is > 0 and <= 10)
            {
                content = content.Replace("\"number_of_shards\": 5,", $"\"number_of_shards\": {numberOfShards},");
            }

            var response = await _httpClient.Put(urlCreateIndex, content);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadAsStringAsync();
                return (true, p);
            }

            try
            {
                var p = await response.Content.ReadAsStringAsync();
                _logger.LogError(p);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return (false, $"{(int)response.StatusCode} ({response.ReasonPhrase}) --- {urlCreateIndex}");
        }
    }
}
