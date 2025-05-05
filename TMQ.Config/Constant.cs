using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;

namespace TMQ.Config
{
    public class Constant
    {
        public const string CorsPolicy = "TYTCorsPolicy";

        public static readonly int RedisDbIdDataProtectionKey =
            ConfigSettingEnum.DataProtectionRedisDbId.GetConfig().AsInt(8);

        public const string ErrorCodeEnum = "ErrorCode";
        public const string System = "System";
        public static readonly string CookieAuthenName = ConfigSettingEnum.CookieAuthenName.GetConfig();
        public static readonly string CookieDomain = ConfigSettingEnum.CookieDomain.GetConfig();
        public const string ESProductPublishIndexName = "productpublish";
        public const string ESProductIndexName = "product";
        public const string EsFileIndexName = "file";
        public const string ImageFolderId = "2";
        public const string ESLogHistory = "loghistory";
        public const string ESUserIndexName = "users";
        public const string ESCampaignCollectUserInfo = "CampaignCollectUserInfo";
        public const string ESGroupVote = "groupVote";
        public const string ESVote = "vote";
        public const int PageSize = 30;

        public const string NotificationNotificationId = "NotificationId";
        public const string Notification = "Notification";

        public const string NotificationGroupIdAllMobile = "ALLMOBILE";
        public const string NotificationGroupIdAllWeb = "ALLWEB";

        public const string NotificationGroupIdPodcastMobile = "PODCASTMOBILE";
        public const string NotificationGroupIdPodcastWeb = "PODCASTWEB";
        public const string NotificationGroupIdKQXSTrial = "KQXSTrial";
        public const int MaxPageSize = int.MaxValue;

        public const string ESTotalCommentIndexName = "totalComment";
        public const int EventSortPageSize = 50;
        public const int SeriesSortPageSize = 50;
        public const int WikiSortPageSize = 50;

        public const string DealerIdNotConfig = "DealerIdNotConfig";
        public const string PartnerIdNotConfig = "PartnerIdNotConfig";
        public const string DealerIdNotDefine = "DealerIdNotDefine";
        public const string PartnerIdNotDefine = "PartnerIdNotDefine";

        public const string ArticlePrefixToOnline = "T";
        public const string ArticlePrefixFromOnline = "F";

        public static readonly string[] PermissionIdsNotDefine = [(int.MaxValue * (-1)).ToString()];
        public static readonly string PermissionIdNotDefine = (int.MaxValue * (-1)).ToString();

        public const string ESEventIndexName = "events";
        public const string ESTagsIndexName = "tags";
        public const string ESSeriesIndexName = "series";
        public const string ESNewIndexName = "news";
        public const string ESNewTagExtendIndexName = "newstagextend";
        public const string ESNewsPublishIndexName = "newspublish";
        public const string ESNewsContentMapping = "newscontentmapping";
        public const string ElasticSearchNotFoundMessage = "404 (Not Found)";
        public const string UserActivityGroupSignalA = "UserActivityGroup_{0}_{1}_{2}";
        public static readonly string ImageThumpHashKey = ConfigSettingEnum.ImageThumpHashKey.GetConfig();
        public const string NotifyTargetUrl = "NotifyTargetUrl";
        public const string PermissionActionKey = "PermissionActionKey";
        public const string DateFormat = "dd/MM/yyyy";
        public const string IsCacheHeader = "iscache";
    }
}
