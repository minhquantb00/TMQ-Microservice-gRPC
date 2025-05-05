using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using TMQ.Common;
using TMQ.Config;

namespace TMQ.GoogleService
{
    public class FirebaseMessagingClient : IFirebaseMessagingClient
    {
        private readonly FirebaseMessaging _client;
        private readonly FirebaseMessaging _clientForCMS;
        private readonly ILogger<FirebaseMessagingClient> _logger;
        private readonly string _environment = ConfigSettingEnum.DataProtectionRedisKey.GetConfig();

        public FirebaseMessagingClient(ILogger<FirebaseMessagingClient> logger)
        {
            _logger = logger;
            var app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging"),
            });
            _client = FirebaseMessaging.GetMessaging(app);

            var cmsApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("vms-account-firebase-adminsdk.json")
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging"),
            }, "MOBILEVMS");
            _clientForCMS = FirebaseMessaging.GetMessaging(cmsApp);
        }

        public async Task SubscribeToTopic(string[] fcmTokens, string topic)
        {
            if (fcmTokens is not { Length: > 0 })
            {
                throw new Exception("FcmToken is not null or empty");
            }

            if (string.IsNullOrEmpty(topic))
            {
                throw new Exception("topic is not null or empty");
            }

            topic = BuildTopicName(topic);
            _logger.LogInformation("SubscribeToTopic: {Topic} with FCM: {JsonSerializeObject} ", topic,
                Common.Serialize.JsonSerializeObject(fcmTokens));
            var result = await _client.SubscribeToTopicAsync(fcmTokens, topic);
            _logger.LogInformation("SubscribeToTopic result.SuccessCount: {SuccessCount}", result.SuccessCount);
            if (result.FailureCount > 0)
            {
                _logger.LogError("SubscribeToTopic result.FailureCount: {ResultFailureCount}", result.FailureCount);
                _logger.LogError("SubscribeToTopic FailureCount: {Topic} with FCM: {JsonSerializeObject} ", topic,
                    Common.Serialize.JsonSerializeObject(fcmTokens));
            }

            if (result.Errors?.Count > 0)
            {
                _logger.LogError("SubscribeToTopic result.Errors: {ResultFailureCount}",
                    Common.Serialize.JsonSerializeObject(result.Errors));
                _logger.LogError("SubscribeToTopic Errors: {Topic} with FCM: {JsonSerializeObject} ", topic,
                    Common.Serialize.JsonSerializeObject(fcmTokens));
            }
        }

        public async Task UnsubscribeFromTopicAsync(string[] fcmTokens, string topic)
        {
            if (fcmTokens is not { Length: > 0 })
            {
                throw new Exception("FcmToken is not null or empty");
            }

            if (string.IsNullOrEmpty(topic))
            {
                throw new Exception("topic is not null or empty");
            }

            topic = BuildTopicName(topic);

            _logger.LogInformation("UnsubscribeFromTopicAsync: {Topic} with FCM: {JsonSerializeObject} ", topic,
                Common.Serialize.JsonSerializeObject(fcmTokens));
            var result = await _client.UnsubscribeFromTopicAsync(fcmTokens, topic);
            _logger.LogInformation("UnsubscribeFromTopicAsync result.SuccessCount: {SuccessCount}", result.SuccessCount);
            if (result.FailureCount > 0)
            {
                _logger.LogError("UnsubscribeFromTopicAsync result.FailureCount: {ResultFailureCount}",
                    result.FailureCount);
            }

            if (result.Errors?.Count > 0)
            {
                _logger.LogError("UnsubscribeFromTopicAsync result.Errors: {ResultFailureCount}",
                    Common.Serialize.JsonSerializeObject(result.Errors));
            }
        }

        public async Task<string> Send(string fcmToken, string title, string content, string? imageUrl,
            Dictionary<string, string> data, bool isCMSNotify)
        {
            if (string.IsNullOrEmpty(fcmToken))
            {
                throw new Exception("FcmToken is not null or empty");
            }

            _logger.LogInformation($"Send Notify: {title}");
            var message = new Message()
            {
                Token = fcmToken,
                Notification = new Notification()
                {
                    Body = content,
                    Title = title,
                    ImageUrl = imageUrl
                },
                Data = data,
            };
            try
            {
                var client = isCMSNotify ? _clientForCMS : _client;
                var result = await client.SendAsync(message);
                _logger.LogInformation($"Send Notify result: {result}");
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Send Notify Exception: {Common.Serialize.JsonSerializeObject(e)}");
                return e.Message;
            }
        }

        public async Task<BatchResponse> Send(string[] fcmTokens, string title, string content, string? imageUrl,
            Dictionary<string, string> data, bool isCMSNotify)
        {
            if (fcmTokens.Any(string.IsNullOrEmpty))
            {
                throw new Exception("FcmToken is not null or empty");
            }

            var message = new MulticastMessage()
            {
                Tokens = fcmTokens,
                Notification = new Notification()
                {

                    Body = content,
                    Title = title,
                    ImageUrl = imageUrl
                },
                Data = data,
            };
            try
            {
                var client = isCMSNotify ? _clientForCMS : _client;
                var result = await client.SendMulticastAsync(message);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<string> SendToTopic(string topic, string title, string content, string? imageUrl,
            Dictionary<string, string> data, bool isCMSNotify)
        {
            if (string.IsNullOrEmpty(topic))
            {
                throw new Exception("Topic is not null or empty");
            }

            topic = BuildTopicName(topic);
            var message = new Message()
            {
                Topic = topic,
                Notification = new Notification()
                {
                    Body = content,
                    Title = title,
                    ImageUrl = imageUrl
                },
                Data = data,
                //Condition = topic
            };
            try
            {
                var client = isCMSNotify ? _clientForCMS : _client;
                var result = await client.SendAsync(message);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("SendToTopic Data: {Message}",
                    Common.Serialize.JsonSerializeObject(message));
                _logger.LogError("SendToTopic result.Errors: {ResultFailureCount}",
                    Common.Serialize.JsonSerializeObject(e));
                return e.Message;
            }
        }

        public async Task<string> SendToTopic(string[] topics, string title, string content, string? imageUrl,
            Dictionary<string, string> data, bool isCMSNotify)
        {
            if (topics == null || topics.Length <= 0)
            {
                throw new Exception("Topics is not null or empty");
            }

            topics = topics.Select(BuildTopicName).ToArray();
            _logger.LogInformation("SendToTopic:{AsArrayJoin}", topics.AsArrayJoin());
            var topic = string.Join(" || ", topics.Select(p => $"'{p}' in topics"));
            var message = new Message()
            {
                //Topic = topic,
                Notification = new Notification()
                {
                    Body = content,
                    Title = title,
                    ImageUrl = imageUrl
                },
                Data = data,
                Condition = topic
            };
            try
            {
                var client = isCMSNotify ? _clientForCMS : _client;
                var result = await client.SendAsync(message);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("SendToTopic Data: {Message}",
                    Common.Serialize.JsonSerializeObject(message));
                _logger.LogError("SendToTopic result.Errors: {ResultFailureCount}",
                    Common.Serialize.JsonSerializeObject(e));
                return e.Message;
            }
        }

        public async Task<string> SendAll(string title, string content, string? imageUrl,
            Dictionary<string, string> data, bool isCMSNotify)
        {
            var message = new Message()
            {
                Topic = "/topics/all",
                Notification = new Notification()
                {
                    Body = content,
                    Title = title,
                    ImageUrl = imageUrl
                },
                Data = data,
            };
            try
            {
                var client = isCMSNotify ? _clientForCMS : _client;
                var result = await client.SendAsync(message);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("SendAll Data: {Message}",
                    Common.Serialize.JsonSerializeObject(message));
                _logger.LogError("SendAll result.Errors: {ResultFailureCount}",
                    Common.Serialize.JsonSerializeObject(e));
                return e.Message;
            }
        }

        private string BuildTopicName(string topic)
        {
            return $"{_environment}_{topic}";
        }
    }
}
