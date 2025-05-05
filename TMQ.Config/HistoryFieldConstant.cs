using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Config
{
    public record HistoryFieldConstant
    {
        public const string FieldHistoryKey = "historyKey";

        public record NewsManager
        {
            public const string NewsCategoryHistoryName = "NewsManager.NewsCategory";
            public const string NewsHistoryName = "NewsManager.News";
            public const string EventHistoryName = "NewsManager.Event";
            public const string SeriesHistoryName = "NewsManager.Series";
            public const string TagsHistoryName = "NewsManager.Tags";
        }

        public record WikiManager
        {
            public const string ArticleHistoryName = "WikiManager.Article";
            public const string CategoryHistoryName = "WikiManager.Category";
        }

        public record PollsManager
        {
            public const string VoteHistoryName = "PollsManager.Vote";
            public const string GroupVoteHistoryName = "PollsManager.GroupVote";
            public const string CampaignHistoryName = "PollsManager.Campaign";
        }
    }
}
