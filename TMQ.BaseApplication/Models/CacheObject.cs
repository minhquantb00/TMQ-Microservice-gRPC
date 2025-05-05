using ProtoBuf;

namespace TMQ.BaseApplication.Models
{
    [ProtoContract]
    public class CacheObject<T>
    {
        [ProtoMember(1)] public T? Model { get; set; }
        [ProtoMember(2)] public DateTime CreatedDate { get; set; }
        [ProtoMember(3)] public DateTime ExpiredDate { get; set; }
        [ProtoMember(4)] public DateTime MaxExpiredDate { get; set; }
        [ProtoMember(5)] public string? HttpMethod { get; set; }
        [ProtoMember(6)] public string? RequestUrl { get; set; }
        [ProtoMember(7)] public string? KeyCache { get; set; }
        [ProtoMember(8)] public bool? IsLogin { get; set; }
        [ProtoMember(9)] public DateTime? CacheCreatedDate { get; set; }
        [ProtoMember(10)] public string? RequestPath { get; set; }
        [ProtoMember(11)] public string? ContentType { get; set; }
        [ProtoMember(12)] public string? CacheControl { get; set; }
        [ProtoMember(13)] public int? HttpStatusCode { get; set; }
        [ProtoMember(14)] public bool? HasPermission { get; set; }
        [ProtoMember(15)] public long CacheTimeValid { get; set; }
        [ProtoMember(16)] public string? Url { get; set; }
    }
}
