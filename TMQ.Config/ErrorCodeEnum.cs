using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Config
{
    public enum ErrorCodeEnum
    {
        NoErrorCode = 0,
        Success = 1,
        Fail = 2,
        ErrorCommentLimit = 3,
        ErrorCommentTime = 4,
        InternalExceptions = 500,
        Unauthorized = 401,
        NullRequestExceptions = 501,
        NotExistExceptions = 503,
        UserNullException = 504,
        PermissionDeny = 403,
        AntiXss = 502,
        InternalExceptionsNotDefine = 505,
    }
}
