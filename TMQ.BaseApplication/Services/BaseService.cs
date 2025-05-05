using System.Diagnostics;
using TMQ.BaseCommands;
using TMQ.BaseEvents;
using TMQ.Common;
using TMQ.Config;
using TMQ.EventBus;
using TMQ.Exceptions;

namespace TMQ.BaseApplication.Services
{
    public class BaseService(
    ILogger<BaseService> logger,
    ContextService contextService) : BaseEventHandler(logger)
    {
        private readonly RabbitMqConnectionPool _rabbitMqConnectionPool = contextService.RabbitMqConnectionPool;

        #region ProcessCommand

        protected async Task<BaseCommandResponse> ProcessCommand(Func<BaseCommandResponse, Task> processFunc)
        {
            BaseCommandResponse response = new BaseCommandResponse();
            try
            {
                await processFunc(response);
                await NotifyEvent();
                await Trigger();
            }
            catch (TMQException e)
            {
                response.SetFail(e.Message);
                LogError(e, e.Message);
            }
            catch (Exception e)
            {
                if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                    Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
                {
                    response.SetFail((ErrorCodeEnum)(e.Data[Constant.ErrorCodeEnum] ??
                                                     ErrorCodeEnum.InternalExceptionsNotDefine));
                }
                else
                {
                    if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
                    {
                        response.SetFail(e.Message);
                    }
                    else
                    {
                        response.SetFail(ErrorCodeEnum.InternalExceptions);
                    }
                }

                LogError(e, e.Message);
            }

            return response;
        }

        protected async Task<BaseCommandResponse> ProcessCommand(BaseCommand request,
            Func<BaseCommandResponse, Task> processFunc)
        {
            BaseCommandResponse response = new BaseCommandResponse();
            try
            {
                await processFunc(response);
                await NotifyEvent();
                await Trigger();
            }
            catch (TMQException e)
            {
                response.SetFail(e.Message);
                LogError(e, e.Message);
            }
            catch (Exception e)
            {
                if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                    Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
                {
                    response.SetFail((ErrorCodeEnum)(e.Data[Constant.ErrorCodeEnum] ??
                                                     ErrorCodeEnum.InternalExceptionsNotDefine));
                }
                else
                {
                    if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
                    {
                        response.SetFail(e.Message);
                    }
                    else
                    {
                        response.SetFail(ErrorCodeEnum.InternalExceptions);
                    }
                }

                LogError(e, e.Message);
            }

            return response;
        }

        protected async Task<BaseCommandResponse<T>> ProcessCommand<T>(Func<BaseCommandResponse<T>, Task> processFunc)
        {
            BaseCommandResponse<T> response = new BaseCommandResponse<T>();
            try
            {
                await processFunc(response);
                await NotifyEvent();
                await Trigger();
            }
            catch (TMQException e)
            {
                response.SetFail(e.Message);
                LogError(e, e.Message);
            }
            catch (Exception e)
            {
                if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                    Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
                {
                    response.SetFail((ErrorCodeEnum)(e.Data[Constant.ErrorCodeEnum] ??
                                                     ErrorCodeEnum.InternalExceptionsNotDefine));
                }
                else
                {
                    if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
                    {
                        response.SetFail(e.Message);
                    }
                    else
                    {
                        response.SetFail(ErrorCodeEnum.InternalExceptions);
                    }
                }

                LogError(e, e.Message);
            }

            return response;
        }

        protected async Task<BaseCommandResponse<T>> ProcessCommand<T>(BaseCommand request,
            Func<BaseCommandResponse<T>, Task> processFunc)
        {
            BaseCommandResponse<T> response = new BaseCommandResponse<T>();
            try
            {
                await processFunc(response);
                await NotifyEvent();
                await Trigger();
            }
            catch (TMQException e)
            {
                response.SetFail(e.Message);
                LogError(e, e.Message);
            }
            catch (Exception e)
            {
                if (e.Data.Contains(Constant.ErrorCodeEnum) &&
                    Enum.TryParse(e.Data[Constant.ErrorCodeEnum].AsString(), out ErrorCodeEnum _))
                {
                    response.SetFail((ErrorCodeEnum)(e.Data[Constant.ErrorCodeEnum] ??
                                                     ErrorCodeEnum.InternalExceptionsNotDefine));
                }
                else
                {
                    if (ConfigSettingEnum.IsDevEnvironment.GetConfig().AsInt() == 1)
                    {
                        response.SetFail(e.Message);
                    }
                    else
                    {
                        response.SetFail(ErrorCodeEnum.InternalExceptions);
                    }
                }

                LogError(e, e.Message);
            }

            return response;
        }

        protected async Task ProcessEvent(Func<Task> processFunc)
        {
            try
            {
                await processFunc();
                await NotifyEvent();
                await Trigger();
            }
            catch (TMQException e)
            {
                LogError(e, e.Message);
            }
            catch (Exception e)
            {
                if (e.Data.Contains(Constant.ErrorCodeEnum))
                {
                    var t = e.Data[Constant.ErrorCodeEnum];
                    LogError($"ProcessEvent Exception {t}");
                }

                LogError(e);
            }
        }

        #endregion

        #region NotifyEvent

        private async Task NotifyEvent()
        {
            var events = EventsGet();

            if (!(events.Length > 0)) return;
            await _rabbitMqConnectionPool.Send(
                (ConfigSettingEnum.RabbitMqExChange.GetConfig(), ConfigSettingEnum.RabbitMqRoutingRoot.GetConfig()),
                events);
            EventRemoveAll();
        }

        public async Task NotifyWebsocket(IEvent @event)
        {
            if (string.IsNullOrEmpty(@event.Publisher))
            {
                var stackTrace = new StackTrace(new StackFrame(1, false));
                @event.Publisher =
                    $"{Environment.MachineName} - {CommonUtility.GetFullMethodName(stackTrace.GetFrame(0)?.GetMethod())}";
            }

            await _rabbitMqConnectionPool.Notify(ConfigSettingEnum.RabbitMqExChangeNotify.GetConfig(), @event);
        }

        protected async Task Trigger(IEvent @event)
        {
            if (string.IsNullOrEmpty(@event.Publisher))
            {
                var stackTrace = new StackTrace(new StackFrame(1, false));
                @event.Publisher =
                    $"{Environment.MachineName} - {CommonUtility.GetFullMethodName(stackTrace.GetFrame(0)?.GetMethod())}";
            }

            await _rabbitMqConnectionPool.NotifyTrigger(ConfigSettingEnum.RabbitMqExChangeTrigger.GetConfig(), [@event]);
        }

        private async Task Trigger()
        {
            var events = EventsGet(true);
            if (!(events.Length > 0)) return;
            await _rabbitMqConnectionPool.NotifyTrigger(ConfigSettingEnum.RabbitMqExChangeTrigger.GetConfig(), events);
            EventRemoveAll(true);
        }

        protected void LogErrorElastic(string message)
        {
            if (message.Contains(Constant.ElasticSearchNotFoundMessage))
            {
                LogWarning(message);
            }
            else
            {
                LogError(message);
            }
        }

        #endregion
    }
}
