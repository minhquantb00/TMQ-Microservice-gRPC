using ProtoBuf.Serializers;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;

namespace TMQ.BaseEvents
{
    [ProtoContract]
    public record EventBusMessage
    {
        public EventBusMessage()
        {

        }

        public EventBusMessage(string topicName, string messageId, object body, SerializeTypeEnum serializeType, string? publisher)
        {
            TopicName = topicName;
            SerializeType = serializeType;
            if (body != null)
            {
                BodyType = body.GetType().AssemblyQualifiedName;
                switch (SerializeType)
                {
                    case SerializeTypeEnum.Json:
                        {
                            JsonBody = Serialize.JsonSerializeObject(body);
                            break;
                        }
                    case SerializeTypeEnum.Protobuf:
                        {
                            ProtobufBody = Serialize.ProtoBufSerialize(body);
                            break;
                        }
                    case SerializeTypeEnum.Byte:
                        {
                            ByteBody = (byte[])body;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Body type invalid");
                        }
                }
            }
            else
            {
                throw new Exception("Body type invalid");
            }

            CorrelationId = CommonUtility.GenerateGuid();
            MessageId = messageId;
            CreatedDate = Extension.GetCurrentDate();
            Publisher = publisher;
        }
        [ProtoMember(1)] public required string MessageId { get; set; }
        [ProtoMember(2)] public TimeSpan? Delay { get; set; }
        [ProtoMember(3)] public TimeSpan? TimeToLive { get; set; }
        [ProtoMember(4)] public required string CorrelationId { get; set; }
        [ProtoMember(5)] public required SerializeTypeEnum SerializeType { get; set; }
        [ProtoMember(6)] public required int Version { get; set; }
        [ProtoMember(7)] public string? BodyType { get; set; }
        [ProtoMember(8)] public DateTime CreatedDate { get; set; }
        [ProtoMember(9)] public DateTime? ProcessDate { get; set; }
        [ProtoMember(10)] public required string TopicName { get; set; }
        [ProtoMember(11)] public byte[]? ProtobufBody { get; set; }
        [ProtoMember(12)] public string? JsonBody { get; set; }
        [ProtoMember(13)] public byte[]? ByteBody { get; set; }
        [ProtoMember(14)] public string? EventId { get; set; }
        [ProtoMember(15)] public EventTypeEnum EventType { get; set; }
        [ProtoMember(16)] public long? ExecuteTime { get; set; }
        [ProtoMember(17)] public EventStatusEnum? Status { get; set; }
        [ProtoMember(18)] public string? Error { get; set; }
        [ProtoMember(19)] public DateTime? FinishDate { get; set; }
        [ProtoMember(20)] public long? SendTime { get; set; }
        [ProtoMember(21)] public string? Publisher { get; set; }
        [ProtoMember(22)] public string? Consumer { get; set; }
    }
}
