using System.Diagnostics;
using TMQ.Common;

namespace TMQ.BaseApplication.Models
{
    public class TrackingProcessTimeModel
    {
        public TrackingProcessTimeModel()
        {
        }

        public TrackingProcessTimeModel(string functionName)
        {
            FunctionName = functionName;
            Models = new List<TrackingProcessTimeDetailModel>();
            Priority = 0;
            StartTime = Stopwatch.GetTimestamp();
        }

        public string FunctionName { get; private set; }
        public int Priority { get; private set; }
        public long ProcessTimeTotal => Models?.Sum(p => p.ProcessTime) ?? 0;
        public List<TrackingProcessTimeDetailModel> Models { get; private set; }
        public long StartTime { get; private set; }

        public void Add(long startTime, string key, object? description)
        {
            TimeSpan processTime = Stopwatch.GetElapsedTime(startTime);
            Models.Add(new TrackingProcessTimeDetailModel(Priority++, key, processTime, description));
        }

        public void AddThenResetStartTime(string key, object? description)
        {
            var endTime = Stopwatch.GetTimestamp();
            TimeSpan processTime = Stopwatch.GetElapsedTime(StartTime, endTime);
            Models.Add(new TrackingProcessTimeDetailModel(Priority++, key, processTime, description));
            StartTime = endTime;
        }
    }

    public class TrackingProcessTimeDetailModel
    {
        public TrackingProcessTimeDetailModel()
        {
        }

        public TrackingProcessTimeDetailModel(int priority, string key, TimeSpan processTime, object? description)
        {
            Priority = priority;
            Key = key;
            ProcessTime = processTime.TotalMilliseconds.DoubleAsLong();
            Description = description;
        }

        public int Priority { get; set; }
        public string Key { get; set; }
        public long ProcessTime { get; set; }
        public object? Description { get; set; }
    }
}
