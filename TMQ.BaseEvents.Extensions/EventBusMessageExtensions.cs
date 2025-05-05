using TMQ.Common;

namespace TMQ.BaseEvents.Extensions
{
    public static class EventBusMessageExtensions
    {
        public static EventBusMessage EventBusMessageCreate(object body, string topicName, string messageId,
            SerializeTypeEnum serializeType, string? publisher)
        {
            var message = new EventBusMessage()
            {
                TopicName = topicName,
                SerializeType = serializeType,
                BodyType = body.GetType().AssemblyQualifiedName ?? string.Empty,
                CorrelationId = Guid.CreateVersion7().ToString("N"),
                MessageId = messageId,
                CreatedDate = DateTime.Now,
                Publisher = publisher,
                Version = 1
            };
            switch (serializeType)
            {
                case SerializeTypeEnum.Json:
                    {
                        message.JsonBody = Serialize.JsonSerializeObject(body);
                        break;
                    }
                case SerializeTypeEnum.Protobuf:
                    {
                        message.ProtobufBody = Serialize.ProtoBufSerialize(body);
                        break;
                    }
                case SerializeTypeEnum.Byte:
                    {
                        message.ByteBody = (byte[])body;
                        break;
                    }
                default:
                    {
                        throw new Exception("Body type invalid");
                    }
            }

            return message;
        }

        public static object? EventBusMessageToObj(this EventBusMessage message)
        {
            Type? type = Type.GetType(message.BodyType);
            if (type == null)
            {
                throw new Exception("Body type is null");
            }

            switch (message.SerializeType)
            {
                case SerializeTypeEnum.Json:
                    {
                        return Serialize.JsonDeserializeObject(message.JsonBody, type);
                    }
                case SerializeTypeEnum.Protobuf:
                    {
                        return Serialize.ProtoBufDeserialize(message.ProtobufBody, type);
                    }
                case SerializeTypeEnum.Byte:
                    {
                        return message.ByteBody;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public static byte[] EventBusMessageToBytes(this EventBusMessage message)
        {
            return Serialize.ProtoBufSerialize(message);
        }

        public static EventBusMessage EventBusMessageClone(this EventBusMessage message)
        {
            return new EventBusMessage()
            {
                BodyType = message.BodyType,
                CorrelationId = message.CorrelationId,
                CreatedDate = message.CreatedDate,
                Delay = message.Delay,
                MessageId = message.MessageId,
                ProcessDate = message.ProcessDate,
                SerializeType = message.SerializeType,
                TimeToLive = message.TimeToLive,
                Version = message.Version,
                TopicName = message.TopicName,
                ProtobufBody = message.ProtobufBody,
                JsonBody = message.JsonBody,
                ByteBody = message.ByteBody,
                EventId = message.EventId,
                EventType = message.EventType,
                ExecuteTime = message.ExecuteTime,
                Status = message.Status,
                Error = message.Error,
                FinishDate = message.FinishDate,
                SendTime = message.SendTime,
                Publisher = message.Publisher,
                Consumer = message.Consumer
            };
        }

        public static EventBusMessage CreateMessageFromQueue(byte[] bytes)
        {
            return Serialize.ProtoBufDeserialize<EventBusMessage>(bytes);
        }
    }
}
