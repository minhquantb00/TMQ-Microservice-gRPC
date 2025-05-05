using Microsoft.Data.SqlClient;
using TMQ.BaseApplication.Services;
using TMQ.Common;
using TMQ.SystemCommands.Queries;
using TMQ.SystemManager.Shared;
using TMQ.SystemRepository;

namespace TMQ.SystemManager.Services
{
    public class CommonService(
    ICommonRepository commonRepository,
    ContextService contextService,
    ILogger<CommonService> logger)
    : BaseService(logger, contextService), ICommonService
    {
        public async Task<string> GetNextCode(GetNextCodeQuery query)
        {
            if (string.IsNullOrEmpty(query.TypeName))
            {
                query.TypeName = "Common";
            }

            string code = string.Empty;
            int numberFormat = 6;
            if (query.Number > 0)
            {
                numberFormat = query.Number;
            }

            try
            {
                long nextValue = await commonRepository.GetNextValueForSequence(query.TypeName);
                code = query.IsDigit
                    ? nextValue.ToString().PadLeft(numberFormat, '0')
                    : CommonUtility.GenerateCodeFromId(nextValue, numberFormat);
            }
            catch (SqlException e)
            {
                if (e.Message.StartsWith("Invalid object name 'Sequence"))
                {
                    await commonRepository.CreateSequence(query.TypeName);
                    long nextValue = await commonRepository.GetNextValueForSequence(query.TypeName);
                    code = query.IsDigit
                        ? nextValue.ToString().PadLeft(numberFormat, '0')
                        : CommonUtility.GenerateCodeFromId(nextValue, numberFormat);
                }
            }
            catch (Exception e)
            {
                if (e.Message.StartsWith("Invalid object name 'Sequence"))
                {
                    await commonRepository.CreateSequence(query.TypeName);
                    long nextValue = await commonRepository.GetNextValueForSequence(query.TypeName);
                    code = query.IsDigit
                        ? nextValue.ToString().PadLeft(numberFormat, '0')
                        : CommonUtility.GenerateCodeFromId(nextValue, numberFormat);
                }
                else
                {
                    e.ExceptionAddParam("CommonService.GetNextId", query.TypeName);
                    throw;
                }
            }

            if (!string.IsNullOrEmpty(query.Prefix))
            {
                code = $"{query.Prefix}{code}";
            }

            return code;
        }

        public async Task<string[]> GetNextMultipleCode(GetNextCodeQuery query)
        {
            if (string.IsNullOrEmpty(query.TypeName))
            {
                query.TypeName = "Common";
            }

            if (query.TotalValue.GetValueOrDefault() <= 0)
            {
                query.TotalValue = 1;
            }

            List<string> codes = [];
            int numberFormat = 6;
            if (query.Number > 0)
            {
                numberFormat = query.Number;
            }

            try
            {
                var nextValues =
                    await commonRepository.GetNextMultipleValueForSequence(query.TypeName, query.TotalValue ?? 1);
                if (nextValues.Length > 0)
                {
                    foreach (var nextValue in nextValues)
                    {
                        var code = query.IsDigit
                            ? nextValue.ToString().PadLeft(numberFormat, '0')
                            : CommonUtility.GenerateCodeFromId(nextValue, numberFormat);
                        if (!string.IsNullOrEmpty(query.Prefix))
                        {
                            code = $"{query.Prefix}{code}";
                        }

                        codes.Add(code);
                    }
                }
            }
            catch (SqlException e)
            {
                if (e.Message.StartsWith("Invalid object name 'Sequence"))
                {
                    await commonRepository.CreateSequence(query.TypeName);
                    var nextValues =
                        await commonRepository.GetNextMultipleValueForSequence(query.TypeName, query.TotalValue ?? 1);
                    if (nextValues.Length > 0)
                    {
                        foreach (var nextValue in nextValues)
                        {
                            var code = query.IsDigit
                                ? nextValue.ToString().PadLeft(numberFormat, '0')
                                : CommonUtility.GenerateCodeFromId(nextValue, numberFormat);
                            if (!string.IsNullOrEmpty(query.Prefix))
                            {
                                code = $"{query.Prefix}{code}";
                            }

                            codes.Add(code);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.StartsWith("Invalid object name 'Sequence"))
                {
                    await commonRepository.CreateSequence(query.TypeName);
                    var nextValues =
                        await commonRepository.GetNextMultipleValueForSequence(query.TypeName, query.TotalValue ?? 1);
                    if (nextValues.Length > 0)
                    {
                        foreach (var nextValue in nextValues)
                        {
                            var code = query.IsDigit
                                ? nextValue.ToString().PadLeft(numberFormat, '0')
                                : CommonUtility.GenerateCodeFromId(nextValue, numberFormat);
                            if (!string.IsNullOrEmpty(query.Prefix))
                            {
                                code = $"{query.Prefix}{code}";
                            }

                            codes.Add(code);
                        }
                    }
                }
                else
                {
                    e.ExceptionAddParam("CommonService.GetNextId", query.TypeName);
                    throw;
                }
            }

            return codes.ToArray();
        }
    }
}
