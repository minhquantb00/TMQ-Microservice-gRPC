using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    public enum EventTypeEnum
    {
        NotifyMessage = 1,
        Product = 2,
        ProductSyncToEs = 3,
        ProductPublish = 4,
        System = 5,
        Email = 6,
        EmailMarketing = 7,
        Notification = 8,
        NotificationMessage = 9,
        Sms = 10,
        Warehouse,
        FileRemove,
        HistorySyncToES,
        LogEvent,
        History,
        Poll,
        Cache,
        NewsHistory,
        News,
        EsNewsSyncGaEvent,
        NewsCategorySyncToAI,
        NewsPublicSyncView,
        NewsKPI,
        NewsNotification,
        NewsConvertToAudio,
        NewsImageIndexToAI,
        NewsMediaHistoryMapping,
        PodcastUpdateFromParentNewsId,
        NewsProjectKpi,
        NewsPublishChange,
        NewsPublish,
        NewsPublishToCache,
        NewsPublishToES,
        NewsPublishFlushCacheNginx,
        NewsReport,
        NewsReportSyncAllFromNewsToReport,
        NewsSentToEditor,
        NewsSyncToAI,
        NewsSyncToES,
        NewsSyncToSearch,
        NewsTrackingBonus,
        PodcastUpdateChildren,
        NewspaperPage,
        PodcastNotify,
        NewsPublishMultipleWebsite,
        UpdateArticleCrawlerReferenceId,
        EventZonePosition,
        ZonePosition,
        Account,
        FileUpload,

    }
}
