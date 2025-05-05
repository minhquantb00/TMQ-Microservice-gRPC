
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using TMQ.BaseApplication.Models;
using TMQ.BaseApplication.Services;
using TMQ.BaseReadModels;
using TMQ.Common;
using TMQ.Config;
using TMQ.Exceptions;

namespace TMQ.BaseApplication.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors(Constant.CorsPolicy)]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BaseApiController(
    ILogger<BaseApiController> logger,
    ContextService contextService
) : ControllerBase
    {
        protected readonly ContextService ContextService = contextService;

        protected async Task<BaseResponse<T>> ProcessRequest<T>(Func<BaseResponse<T>, Task> processFunc)
        {
            return await ProcessRequest(null, processFunc);
        }

        protected async Task<BaseResponse<T>> ProcessRequest<T>(object? request, Func<BaseResponse<T>, Task> processFunc)
        {
            BaseResponse<T> response = new BaseResponse<T>();
            try
            {
                await processFunc(response);
            }
            catch (Exception e)
            {
                HandlerException(e, response);
            }

            return response;
        }

        protected async Task<BaseResponse> ProcessRequest(Func<BaseResponse, Task> processFunc)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                await processFunc(response);
            }
            catch (Exception e)
            {
                HandlerException(e, response);
            }

            return response;
        }

        protected async Task<BaseResponse> ProcessRequest(object? request, Func<BaseResponse, Task> processFunc,
            bool? validateRequest = false)
        {
            var startTime = Stopwatch.GetTimestamp();

            var connectionId = HttpContext.Connection.Id;
            var requestId = HttpContext.TraceIdentifier;
            var requestPath = HttpContext.Request.Path;
            BaseResponse response = new BaseResponse();
            try
            {
                await processFunc(response);
            }
            catch (Exception e)
            {
                HandlerException(e, response);
            }

            return response;
        }

        private void HandlerException<T>(Exception e, BaseResponse<T> response)
        {
            if (e.GetType() == typeof(RpcException) &&
                ((RpcException)e).StatusCode == global::Grpc.Core.StatusCode.Unauthenticated)
            {
                response.SetFail(ErrorCodeEnum.Unauthorized);
                return;
            }

            string ex;
            if (e.GetType() == typeof(TMQException))
            {
                ex = ((TMQException)e).Message;
                LogError(e, ex);
                response.SetFail(ex);
                return;
            }

            if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
            {
                ex = e.Data[Constant.ErrorCodeEnum].AsString();
            }
            else
            {
                ex = e.Message;
            }

            LogError(e, ex);
            if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
            {
                throw e;
            }

            response.SetFail(ErrorCodeEnum.InternalExceptions);
        }

        private void HandlerException(Exception e, BaseResponse response)
        {
            if (e.GetType() == typeof(RpcException) &&
                ((RpcException)e).StatusCode == global::Grpc.Core.StatusCode.Unauthenticated)
            {
                response.SetFail(ErrorCodeEnum.Unauthorized);
                return;
            }

            string ex;
            if (e.GetType() == typeof(TMQException))
            {
                ex = ((TMQException)e).Message;
                LogError(e, ex);
                response.SetFail(ex);
                return;
            }

            if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
            {
                ex = e.Data[Constant.ErrorCodeEnum].AsString();
            }
            else
            {
                ex = e.Message;
            }

            LogError(e, ex);
            if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
            {
                throw e;
            }

            response.SetFail(ErrorCodeEnum.InternalExceptions);
        }

        #region LogError

        protected void LogError(Exception exception, string message)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseApiController Exception {Message}", message);
            }
        }

        protected void LogError(Exception exception, string message, params object?[] args)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseApiController Exception {Message}", [.. (object[])[message], .. args]);
            }
        }

        protected void LogError(Exception exception, params object?[] args)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseApiController Exception {Message}", args);
            }
        }

        protected void LogError(Exception exception)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseApiController Exception {Message}", exception.Message);
            }
        }

        protected void LogError(IEnumerable<string>? message)
        {
            if (message == null)
            {
                return;
            }

            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError("BaseApiController Error {Message}", message);
            }
        }

        protected void LogError(string? message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError("BaseApiController Error {Message}", message);
            }
        }

        protected void LogWarning(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError("BaseApiController Warning {Message}", message);
            }
        }

        protected void LogWarning(string message, params object?[] args)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogWarning("BaseController Warning {Message}", [.. (object[])[message], .. args]);
            }
        }

        protected void LogInformation(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogInformation("BaseApiController Information {Message}", message);
            }
        }

        #endregion
    }

    public class BaseApiWithCacheController(
        ILogger<BaseApiWithCacheController> logger,
        ContextService contextService,
        ICacheService cacheService
    )
        : BaseApiController(logger, contextService)
    {
        private static readonly bool IsCacheEnable = ConfigSettingEnum.CacheEnable.GetConfig().AsInt() == 1;

        private static readonly bool EnableUpdateCacheFromWorker =
            ConfigSettingEnum.EnableUpdateCacheFromWorker.GetConfig().AsInt() == 1;

        private static readonly bool IsLoadCache = ConfigSettingEnum.IsLoadCache.GetConfig().AsInt() == 1;

        private static readonly string RabbitMqExChangeFERequest =
            ConfigSettingEnum.RabbitMqExChangeFERequest.GetConfig().AsEmpty();

        private static readonly string RabbitMqRoutingFERequest =
            ConfigSettingEnum.RabbitMqRoutingFERequest.GetConfig().AsEmpty();

        #region CacheProcess

        public async Task<T?> CacheProcess<T>(
            Func<(string Key, string? Value)[]> funParameters,
            Func<Task<T?>> getModel,
            int minuteCacheExpire = 2,
            int maxMinuteCacheExpire = 30 * 24 * 60, bool isCacheStale = false)
        {
            maxMinuteCacheExpire = minuteCacheExpire + 1;
            var parameters = funParameters();
            return await CacheProcess(parameters, getModel, minuteCacheExpire, maxMinuteCacheExpire, isCacheStale);
        }

        private async Task<T?> CacheProcess<T>(
            (string Key, string? Value)[] parameters,
            Func<Task<T?>> getModel,
            int minuteCacheExpire, int maxMinuteCacheExpire, bool isCacheStale)
        {
            var startTime = Stopwatch.GetTimestamp();
            Exception? exceptionTracking = null;
            string? keyCacheTracking = null;
            var dtNow = Extension.GetCurrentDate();
            var requestUrlTracking = HttpContext.Request.GetDisplayUrl();
            var refererUrl = HttpContext.Request.GetTypedHeaders()?.Referer?.OriginalString;
            if (!string.IsNullOrEmpty(refererUrl))
            {
                requestUrlTracking = $"{refererUrl} - {requestUrlTracking}";
            }

            var requestUrl = HttpContext.Request.GetEncodedPathAndQuery();
            var requestPath = HttpContext.Request.Path;
            try
            {
                if (!IsCacheEnable)
                {
                    var modelResultNoCache = await getModel();
                    if (modelResultNoCache == null)
                    {
                        return default;
                    }

                    return modelResultNoCache;
                }

                StringBuilder keyBuilder = new StringBuilder("CACHE_API");
                if (parameters.Length > 0)
                {
                    parameters = parameters.OrderBy(p => p.Key).ToArray();
                    foreach (var parameter in parameters)
                    {
                        keyBuilder.AppendFormat("{0}_{1}", parameter.Key, parameter.Value.AsEmpty());
                    }
                }

                string key = keyBuilder.ToString();
                var result = await cacheService.Get<CacheObject<T>>(key);
                if (result != null)
                {
                    if (!(result.ExpiredDate <= dtNow))
                    {
                        HttpContext.Response.Headers.TryAdd(Constant.IsCacheHeader,
                            $"{result.CreatedDate}-{minuteCacheExpire}p-1");
                        return result!.Model;
                    }
                    else if (IsLoadCache == false && isCacheStale && EnableUpdateCacheFromWorker &&
                             RabbitMqExChangeFERequest.Length > 0)
                    {
                        try
                        {
                            
                        }
                        catch (Exception e)
                        {
                            LogError("API CacheProcess EnableUpdateCacheFromWorker send to rabbitmq error");
                            LogError(e, e.Message);
                        }

                        HttpContext.Response.Headers.TryAdd(Constant.IsCacheHeader,
                            $"{result.CreatedDate}-{minuteCacheExpire}p-0");
                        return result.Model;
                    }
                }

                var modelResult = await getModel();
                result = new CacheObject<T>()
                {
                    Model = modelResult == null ? default : modelResult,
                    CreatedDate = dtNow,
                    ExpiredDate = dtNow.AddMinutes(minuteCacheExpire),
                    MaxExpiredDate = dtNow.AddMinutes(maxMinuteCacheExpire),
                    HttpMethod = HttpContext.Request.Method,
                    RequestUrl = requestUrl,
                    CacheCreatedDate = dtNow,
                    IsLogin = null,
                    CacheControl = null,
                    ContentType = null,
                    HasPermission = true,
                    KeyCache = key,
                    RequestPath = HttpContext.Request.Path,
                    CacheTimeValid = minuteCacheExpire,
                    HttpStatusCode = null
                };
                bool isSet = await cacheService.Set(result, key, TimeSpan.FromMinutes(maxMinuteCacheExpire));
                if (!isSet)
                {
                    LogError($"Can not set cache api:{key} ");
                }

                HttpContext.Response.Headers.TryAdd(Constant.IsCacheHeader, $"{result.CreatedDate}-{minuteCacheExpire}p-0");
                return result.Model;
            }
            catch (Exception exception)
            {
                LogError(exception);
                exceptionTracking = exception;
                return default;
            }
            finally
            {
                var executeTime = Stopwatch.GetElapsedTime(startTime);
                ContextService.LogAction.Tracking(
                    parameters,
                    minuteCacheExpire,
                    executeTime,
                    exceptionTracking,
                    keyCacheTracking,
                    requestUrlTracking,
                    requestPath,
                    HttpContext.Connection.Id,
                    HttpContext.TraceIdentifier,
                    null);
            }
        }

        #endregion
    }
}
