namespace TMQ.SystemRepository
{
    public interface ICommonRepository
    {
        Task<long> GetNextValueForSequence(string pathName);
        Task<long[]> GetNextMultipleValueForSequence(string pathName, int totalValue);
        Task CreateSequence(string pathName);
        Task<(long Min, long Max)> GetNextValueForSequence(string pathName, int totalValue);
    }
}
