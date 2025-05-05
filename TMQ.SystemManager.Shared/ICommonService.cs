using System.ServiceModel;
using TMQ.SystemCommands.Queries;

namespace TMQ.SystemManager.Shared
{
    [ServiceContract]
    public interface ICommonService
    {
        [OperationContract]
        Task<string> GetNextCode(GetNextCodeQuery query);

        [OperationContract]
        Task<string[]> GetNextMultipleCode(GetNextCodeQuery query);
    }
}
