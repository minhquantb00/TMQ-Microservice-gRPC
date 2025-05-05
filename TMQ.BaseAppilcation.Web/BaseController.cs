
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TMQ.BaseApplication.Services;
using TMQ.Common;
using TMQ.Config;
using TMQ.Exceptions;

namespace TMQ.BaseAppilcation.Web
{
    public class BaseController(ILogger<BaseController> logger, ContextService contextService) : Controller
    {
        protected readonly ContextService ContextService = contextService;

        protected async Task<IActionResult> ProcessRequest(Func<Task<IActionResult>> processFunc)
        {
            try
            {
                return await processFunc();
            }
            catch (Exception e)
            {
                return HandlerException(e);
            }
        }

        private IActionResult HandlerException(Exception e)
        {
            if (e.GetType() == typeof(RpcException) && ((RpcException)e).StatusCode == Grpc.Core.StatusCode.Unauthenticated)
            {
                Response.StatusCode = HttpStatusCode.Unauthorized.AsEnumToInt();
                return Content(ErrorCodeEnum.Unauthorized.ToString());
            }

            string ex;
            if (e.GetType() == typeof(TMQException))
            {
                ex = ((TMQException)e).Message;
            }
            else
            {
                if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                    Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
                {
                    ex = e.Data[Constant.ErrorCodeEnum].AsString();
                }
                else
                {
                    ex = e.Message;
                }
            }

            LogError(e, ex);
            if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
            {
                throw e;
            }

            Response.StatusCode = 500;
            return Content(ErrorCodeEnum.InternalExceptions.ToString());
        }

        #region LogError

        protected void LogError(Exception exception, string message)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseController Exception {Message}", message);
            }
        }

        protected void LogError(Exception exception, string message, params object?[] args)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseController Exception {Message}", [.. (object[])[message], .. args]);
            }
        }

        protected void LogError(Exception exception, params object?[] args)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseController Exception {Message}", args);
            }
        }

        protected void LogError(Exception exception, object? arg)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, $"BaseController Exception {exception.Message} {Serialize.JsonSerializeObject(arg)}");
            }
        }

        protected void LogError(Exception exception)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogError(exception, "BaseController Exception {Message}", exception.Message);
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
                logger.LogError("BaseController Error {Message}", message);
            }
        }

        protected void LogError(string message, object? arg)
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
                logger.LogError($"BaseController Error {message} {Serialize.JsonSerializeObject(arg)}");
            }
        }

        protected void LogError(string message)
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
                logger.LogError("BaseController Error {Message}", message);
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
                logger.LogWarning("BaseController Warning {Message}", message);
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

        protected void LogWarning(string message, object? arg)
        {
            using (logger.BeginScope(new Dictionary<string, object>
               {
                   { "RefererUrl", ContextService.RefererUrl() }
               }))
            {
                logger.LogWarning($"BaseController Warning {message} {Serialize.JsonSerializeObject(arg)}");
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
                logger.LogInformation("BaseController Information {Message}", message);
            }
        }

        #endregion
    }
}
