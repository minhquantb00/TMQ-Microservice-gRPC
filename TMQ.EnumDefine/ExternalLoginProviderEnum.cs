using System.ComponentModel.DataAnnotations;

namespace TMQ.EnumDefine
{
    public enum ExternalLoginProviderEnum
    {
        Facebook = 1,
        Google = 2,
        Apple = 4,
        LocalId = 8,
        Microsoft = 16,
        Zalo = 32,
    }

    public enum LoginTypeEnum
    {
        CMSWeb = 1,
        CMSApp = 2,
        CustomerApp = 3,
    }

    public enum AccountStatusEnum
    {
        Active = 1,
        InActive = 2,
        Deleted = 3
    }

    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum AccountTypeEnum
    {
        CustomerApp = 1,
        OldSystem = 2,
        Local = 3,
    }

    [Flags]
    public enum OtpTypeEnum
    {
        OTPByEmail = 1,
        OTPBySMS = 2,
        OTPByApp = 4,
        SmartOTP = 8,
    }

    public enum AccountSettingEnum
    {
        Language = 1,
        Partner = 2,
        DateFormat = 3,
        NumberFormat = 4,
        Dealer = 5,
        FormDisplaySetting = 6,
        TrackingApplication = 7
    }

    public enum RelationshipEnum
    {
        DocThan = 1,
        HenHo = 2,
        DaKetHon = 3,
        ChungSong = 4,
        ChungSongCoDangKy = 5,
        DaLyThan = 6
    }

    public enum DepartmentObjectMappingTypeEnum
    {
        User = 1,
        Role = 2,
        Partner = 4,
        Dealer = 5,
        Department = 6,
        Other = 100,
        CurrentUser = 1000,
    }

    public enum DepartmentAttributeEnum
    {
    }

    public enum StatusEnum
    {
        Deleted = -1,
        Active = 1,
        New = 2,
        InActive = 3
    }

    public enum ActivityType
    {
        Online = 1,
        Offline = 2
    }


    public enum ActivityStatus
    {
        Active = 1,      // Đang dùng (lần online gần nhất hợp lệ)
        Expired = 2      // Quá lâu không online
    }


    public enum FriendShipStatus
    {
        DongY = 1,
        TuChoi = 2
    }
    public enum AddVehicleStatusEnum
    {
        Deleted = -1,
        Active = 1,
        New = 2,
        InActive = 3,
        Approved = 4,
        Rejected = 5,
    }

    public enum OwnerTypeEnum
    {
        Owner = 1,
        Other = 2
    }

    public enum UserVehicleAttributeEnum
    {
        NhacLich = 1,
        BaoHiemThanVo = 2,
        BaoHiemTNDSBatBuoc = 3,
        BaoHiemTNDSTuNguyen = 4,
        BaoHiemTaiNanNguoiNgoiTrenXe = 5,
        DangKiem = 6,
        BaoHanhTieuChuan = 7,
        BaoHanhGiaHan = 8,
        ThongTinHopDong = 9,
        ThongTinCoBanXe = 10,
    }

    public enum NhacLichEnum
    {
        [Display(Name = "Trước 1 ngày")] OneDay = 1,
        [Display(Name = "Trước 5 ngày")] FiveDays = 2,
        [Display(Name = "Trước 1 tuần")] OneWeek = 3,
        [Display(Name = "Trước 2 tuần")] TwoWeeks = 4,
        [Display(Name = "Trước 1 tháng")] OneMonth = 5
    }

    public enum MenuTypeEnum
    {
        Link = 1,
        Category = 2,
        Brand = 3,
        OutSystem = 4,
        Iframe = 5,
        CustomerApp = 6
    }

    public enum MenuPosition
    {
        CMSMenu = 1,
        MenuHeader = 2,
        MegaMenu = 3,
        MenuFooter = 4,
        MenuUser = 5,
        MenuHeaderSport = 6,
        MenuHeaderVideo = 7,
        MenuLeftVideo = 8,
        HashTagTopHeader = 9,
        MenuHeaderVnnEn = 10,
        MenuFooterVnnEn = 11,
        MenuHeaderDHD = 12,
        MenuInfonet = 13,
        MenuICTNews = 14,
        PMSMenu = 15,
        MenuXeHeader = 16,
        MenuXeFooter = 17,
        MenuGNNHeader = 18,
        MenuGNNFooter = 19,
        Menu997NewsHeader = 20,
        Menu997NewsFooter = 21,
        CMSMobile = 22,
        Test = 23,
        MegaMenuRps = 24,
        PMSBDSMenu = 25,
        MenuBDSHeader = 26,
        MenuBDSFooter = 27,
        CMSDataMenu = 30,
        TrackingCMSMenu = 31,
        TrackingHeatmapUserMenu = 32,
        MenuHeader2Sao = 33,
        MenuMega2Sao = 34,
        MenuFooter2Sao = 35,
        MenuHashTagTopHeader2Sao = 36,
        GoodLink = 37,
        CustomerAppInfo = 38,
        CustomerAppUtility = 39,
        CustomerAppWelcome = 40,
    }

    [Flags]
    public enum SystemOptionEnum
    {
        VMSV1 = 1,
        VMSV2 = 2,
        VMSV3 = 3,
        PMSV1 = 4,
        PMSV2 = 5,
        PMSV3 = 6,
        TRACKING = 7,
        NewsManager = 8,
        NewsOnlineManager = 9,
    }

    public enum LanguageTypeEnum
    {
        AdminSystem = 1,
        AccountSystem = 2,
        CustomerApp = 3,
        PMS = 5,
        MMS = 6,
        Tracking = 7
    }

    // public enum EventTypeEnum
    //     {
    //         //Default = 0,
    //         Email = 1,
    //         SMS = 2,
    //         Cache = 3,
    //         Comment = 4,
    //         News = 5,
    //         Files = 6,
    //         Order = 7,
    //         Account = 8,
    //         FileConvert = 9,
    //     }

    // public enum StatusEnum
    // {
    //     [Display(Name = "Đã xóa")] Deleted = -1,
    //     [Display(Name = "Đang hoạt động")] Active = 1,
    //     [Display(Name = "Mới tạo")] New = 2,
    //     [Display(Name = "Không hoạt động")] InActive = 3
    // }

    public enum ConfigTypeEnum
    {
        Other = 0,
        TemplateRenderer = 1
    }

    #region all

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
        IdNullException = 505,
        CurrentWebsiteNullException = 506,
        CurrentCompanyNullException = 507,
        PermissionDeny = 403,
        AntiXss = 502,
        InternalExceptionsNotDefine = 508
    }

    // public enum AccountStatus
    // {
    //     Active = 1,
    //     InActive = 2,
    //     Deleted = 3
    // }

    public enum SpecialPositionEnum
    {
        EventHomePageBox = 1,
        HotEventHomePageBox = 2
    }

    public enum VerticalAlign
    {
        Top = 1,
        Center = 2,
        Bottom = 3
    }

    public enum HorizontalPosition
    {
        Left = 1,
        Center = 2,
        Right = 3
    }

    public enum BoxEventType
    {
        Basic = 1,
        HaveMenu = 2
    }

    public enum BoxEventContentType
    {
        Category = 1,
        Tag = 2,
        Series = 3,
        Event = 4
    }

    public enum StyleSpecialPositionEnum
    {
        Basic = 1,
        Custom = 2
    }

    // public enum StatusEnum
    // {
    //     [Display(Name = "Đã xóa")] Deleted = -1,
    //     [Display(Name = "Đang hoạt động")] Active = 1,
    //     [Display(Name = "Mới tạo")] New = 2,
    //     [Display(Name = "Không hoạt động")] InActive = 3
    // }

    public enum CalendarCycleType
    {
        Monthly = 1,
        Annually = 2,
        Fixed = 4
    }

    public enum CalendarSolarLunarType
    {
        Solar = 1,
        Lunar = 2
    }

    public enum CalendarUrlEventType
    {
        Url = 1,
        Event = 2,
        Series = 4,
        Tag = 8,
    }

    public enum KpiArticleOptionEnum
    {
        Active = 1,
        InActive = 3
    }

    public enum StatusAudioEnum
    {
        Active = 1,
        InActive = 2
    }

    public enum ActiveStatusEnum
    {
        New = 0,
        Approved = 1,
        Rejected = 2,
        Changed = 3,
        Cancel = -1,
    }

    public enum GoogleDriveStatusEnum
    {
        New = 1,
        Processed = 2,
        Success = 4,
        Fail = 8
    }

    // [Flags]
    // public enum VideoStatusEnum
    // {
    //     New = 1,
    //     Processing = 2,
    //     Success = 4,
    //     Fail = 8,
    //     Delete = 16
    // }
    //
    // [Flags]
    // public enum ImageGifStatusEnum
    // {
    //     New = 1,
    //     Processing = 2,
    //     Success = 4,
    //     Fail = 8,
    //     Delete = 16
    // }

    public enum TypeFormatEnum
    {
        DAY = 1,
        MONTH = 2,
        YEAR = 3
    }

    public enum TypeChartEnum
    {
        PaidAdvanceCount = 0,
        PaidAdvanceAmount = 1
    }

    public enum TypeChartUserEnum
    {
        ALL = 0,
        USER = 1,
        PREMIUM = 2,
        VNPOST = 3,
        REFUND = 4,
        FREE = 5,
        NOTFREE = 6
    }

    public enum TypeApexChartEnum
    {
        Histogram = 1,
        Bar = 2,
        Line = 3,
        Area = 4,
        Donut = 5,
        PolarArea = 6,
        Pie = 7,
        ColumnAndLine = 8,
        StackedBar = 9
    }

    public enum TypeDataTableEnum
    {
        TableType1 = 1,
        TableType2 = 2,
        TableType3 = 3,
        TableType4 = 4,
        TableType5 = 5,
        TableType6 = 6,
        TableType7 = 7,
        TableType8 = 8
    }

    public enum TypeSearchEnum
    {
        News = 1,
        WikiArticle = 2,
        SportFootballPlayer = 3,
        SportTeams = 4,
        SportFixtures = 5
    }

    public enum TeamsTypeEnum
    {
        Active = 1,
        InActive = 2
    }

    public enum PlayerTypeEnum
    {
        Active = 1,
        InActive = 2
    }

    public enum ScorePredictionVoteValue
    {
        HomeWin = 1,
        Draw = 2,
        AwayWin = 3
    }

    public enum LikeHistoryType
    {
        NewsComment = 1,
        NewsLike = 2,
        NewsBookmark = 3,
        AuthorLike = 4,
        CakeLike = 5,
        FlowerLike = 6,
        GiftLike = 7,
        ProductComment = 8,
        ProductLike = 9,
        ProductBookmark = 10,
        ProductManufacturerComment = 11,
        ProductManufacturerLike = 12,
        ProductManufacturerBookmark = 13,
        ProductVendorComment = 14,
        ProductVendorLike = 15,
        ProductVendorBookmark = 16,
        ProductAttributeValueComment = 17,
        ProductAttributeValueLike = 18,
        ProductAttributeValueBookmark = 19,
        VeryLike = 20,
        Happy = 21,
        Surprise = 22,
        Sad = 23,
    }

    public enum ChartCommentType
    {
        DateTime = 1,
        Category = 2,
    }

    public enum EducationExamTypeEnum
    {
        HighSchoolExam = 1,
        NationalHighSchoolExam = 2,
        CollegeExam = 3,
    }

    public enum EducationSchoolType
    {
        HighSchool = 1,
        University = 2,
        College = 3,
    }

    public enum EducationTypeOfTraining
    {
        Default = 0,
        College = 3,
        University = 2
    }

    // public enum DepartmentAttributeEnum
    // {
    //     ////[Display(Name = "Admin")]
    //     Administrator = 1,
    //
    //     ////[Display(Name = "PhoBan")]
    //     PhoBan = 2,
    //
    //     ////[Display(Name = "TruongBan")]
    //     TruongBan = 3,
    //
    //     ////[Display(Name = "ThuKyToaSoan")]
    //     ThuKyToaSoan = 4,
    //
    //     ////[Display(Name = "PhoTongThuKyToaSoan")]
    //     PhoTongThuKyToaSoan = 5,
    //
    //     ////[Display(Name = "TongThuKyToaSoan")]
    //     TongThuKyToaSoan = 6,
    //
    //     ////[Display(Name = "PhoTongBienTap")]
    //     PhoTongBienTap = 7,
    //
    //     ////[Display(Name = "TongBienTap")]
    //     TongBienTap = 8,
    //
    //     ////[Display(Name = "TruongbanTruyenThong")]
    //     TruongNhomDuAn = 9,
    //
    //     // Phóng viên
    //     NewsReporter = 10,
    //
    //     // Biên tập viên
    //     NewsEditor = 11,
    //
    //     ////[Display(Name = "BienTapTruyenThong")]
    //     BienTapVienDuAn = 12,
    //
    //     ////[Display(Name = "GiamDocSXND")]
    //     GiamDocSXND = 13,
    //
    //     ////[Display(Name = "SEO")]
    //     SEO = 14,
    //
    //     ////[Display(Name = "SanPham")]
    //     SanPham = 15,
    //
    //     ////[Display(Name = "TongHopNhuanBut")]
    //     TongHopNhuanBut = 16,
    //
    //     ////[Display(Name = "TruongPhongTCHC")]
    //     TruongPhongTCHC = 17,
    //
    //     // Cong tac vien
    //     CongTacVien = 18,
    //
    //     // Vietnamnet Content User
    //     VnnContentUser = 19,
    //
    //     // Quền tick hiển thị là cây bút
    //     TickDisplayAsPseudonym = 20,
    //
    //     ////[Display(Name = "GiamdocMKT")]
    //     GiamdocMKT = 21,
    //
    //     // Quyền cấu hình link xem trước
    //     ConfigPreviewSetting = 22,
    //
    //     // Quyền chọn tìm kiếm theo ngày xuất bản
    //     KPIAllowChoosePublishDate = 23,
    //
    //     // Quyền chọn tìm kiếm theo website
    //     KPIAllowChooseWebsite = 24,
    //
    //     ThuKyXuatBan = 25,
    //
    //     TruongBanVideo = 26,
    //
    //     ThuKyXuatBanVideo = 27,
    //
    //     BienTapVienVideo = 28,
    //
    //     SEOLeader = 29,
    //
    //     ToChucHanhChinh = 30,
    //     PhanTichDuLieu = 31
    // }

    // public enum DepartmentObjectMappingTypeEnum
    // {
    //     User = 1,
    //     Role = 2,
    //     Category = 3,
    //     Company = 4,
    //     Zone = 5,
    //     WikiCategory = 6,
    //     NewsDisplayType = 7,
    //     ProductCategory = 8,
    //     Website = 9,
    //     AccountSystem = 10,
    //     Department = 11,
    //     DepartmentAttribute = 12,
    //     NewsStatus = 13,
    //     NewsActionType = 14,
    //     NewsOption = 15,
    //     Other = 16,
    //     CurrentUser = 17,
    //     Options = 18
    // }

    public enum UserSavedMappingTypeEnum
    {
        News = 1,
    }

    public enum DepartmentDeleteStateEnum
    {
        Hidden = 1,
        Display = 0,
    }

    public enum DepartmentObjectMappingTitleEnum
    {
        Staff = 1,
        Leader = 2,
        CenterLeader = 3,
        Storage = 4
    }

    public enum ESUserDepartmentMappingTypeEnum
    {
        User = 1,
        DepartmentMapping = 2
    }

    // public enum GenderEnum
    // {
    //     Male = 1,
    //     Female = 2,
    //     Other = 3
    // }

    [Flags]
    public enum NotificationPlatformEnum
    {
        All = 1,
        Mobile = 2,
        WebAdmin = 4,
        Email = 8,
        SMS = 16,
        WebCustomer = 32,
    }

    public enum NotifyActionTypeEnum
    {
        Text = 1,
        Html = 2,
        OTP = 3,
        System = 4,
        MyArticleApproved = 5,
        MyArticleChanged = 6,
        MyArticleMissingSeo = 7,

        //MyArticleDeny = 8,
        MyArticlePublished = 9,
        ArticleSendToEditor = 10,
        ArticleSendToPublish = 11,
        ArticleAssignToMe = 12,
        ArticleHasDiscuss = 13,
        ArticleHasComment = 14,
        MyRoleChanged = 15,
        ImpersonateFrom = 16,
        ImpersonateTo = 17,
        ImpersonateExpired = 18,
        CommentNeedApprove = 19,
        SyncNewsToEs = 20,
        Lock = 21,
        UnLock = 22,
        VideoToGif = 23,
        DashboardWidgetChange = 24,
        LoginNotify = 25,
        MobileCategory = 26,
        MobileArticle = 27,
        UserSignIn = 28,
        MessageSent = 29,
        MessageReceived = 30,

        UserAddToDepartment = 31,
        UserRemoveFromDepartment = 32,
        MyArticleDenyFromEditor = 33,
        MyArticleDenyFromPublish = 34,
        MyArticleEditing = 35,
        CommentChangeStatus = 36,
        VideoAddWatermark = 37,
        VbeeAudioConvertSuccess = 38,
        Podcast = 39,
        LotteryTrial = 40,
        LotteryTrialExpired = 41,
        ArticlePublish = 42,
        ArticleUnPublish = 43,
        ArticleChange = 44,
        UserActivityStart = 45,
        UserActivityEnd = 46,
        ContentOnlineInterviewQuestionAndAnswerReplace = 47,
        ContentOnlineInterviewGuestAdd = 48,
        ContentOnlineInterviewGuestChange = 49,
        ContentOnlineInterviewGuestStatusChange = 50,
        ContentOnlineInterviewQuestionSort = 51,
        ContentOnlineInterviewQuestionAndAnswerPublish = 52,
        ContentOnlineInterviewQuestionAndAnswerUnPublish = 53,
        ContentOnlineInterviewQuestionAndAnswerRemove = 54,
        MessageChange = 55,
        MessageRemove = 56,
        CommentApproved = 57,
        CommentReplyApproved = 58,
        CommentEmotionReact = 59,
        PremiumOrderSuccess = 60,
        PremiumExpireNotice = 61,
    }

    [Flags]
    public enum NotificationMessageStatus
    {
        NotSend = 0,
        Sent = 1,
        Read = 2,
        Important = 4,
        Remove = 8,
        OK = 16,
        Cancel = 32
    }

    public enum NotificationStatusEnum
    {
        Delete = -1,
        New = 1,
        Approved = 2,
        Cancel = 3,
        Sending = 4,
        Sent = 5
    }

    public enum CenterTypeEnum
    {
        Default = 0,
    }

    // [Flags]
    // public enum OtpTypeEnum
    // {
    //     LoginByPhone = 1,
    //     EmailConfirmed = 2,
    //     PhoneNumberConfirmed = 4,
    //     OTPByEmail = 8,
    //     OTPBySMS = 16,
    //     OTPByApp = 32,
    //     SmartOTP = 64,
    //     QR = 128
    // }

    public enum SMSTypeEnum
    {
        SendOtp = 1,
        Other = 2
    }

    public enum EmailTypeEnum
    {
        SendOtp = 1,
        Other = 2,
        Contact = 3,
        Notify = 4,
        Marketing = 5,
        NewspaperPrint = 6,
        ApproveArticle = 7
    }

    // public enum LoginTypeEnum
    // {
    //     Web = 1,
    //     App = 2,
    //     Google = 3,
    //     Facebook = 4,
    //     Apple = 5,
    //     AppGoogle = 6,
    //     AppFacebook = 7,
    //     AppApple = 8,
    //     AppVnn = 9
    // }

    [Flags]
    public enum SyncStatusEnum
    {
        Return = -1,
        New = 1,
        Success = 2,
        Fail = 4,
        Retry = 8,
        Cancel = 16,
        NewWaitApprove = 32,
        Approved = 64,
    }

    public enum HttpClientNameEnum
    {
        Default,
        Retry,
        NotificationActor,
        GoogleAPI,
        FacebookAPI,
        AppleAPI,
        LDAP,
        VNNGrpc,
        SMS,
        ViettelToken,
        Proxy,
        NginxCache,
        ReloadCache,
        Crawler,
        Crawler_BatDongSan_Com_VN,
        DownloadImage,
        ServerCertificateCustomValidation,
        EsReport
    }

    [Flags]
    public enum ForgotPasswordStatusEnum
    {
        New = 1,
        Send = 2,
        Expired = 4,
        Used = 8
    }

    // public enum ExternalLoginProviderEnum
    // {
    //     Facebook = 1,
    //     Google = 2,
    //     Apple = 3,
    //     VinID = 4,
    //     SmsViettel = 5,
    //     SmsVinaPhone = 6,
    //     SmsMobiPhone = 7,
    //     VnnId = 8
    // }

    public enum AddressTypeEnum
    {
        ShippingAddress = 1,
        BillingAddress = 2,
        HomeAddress = 3,
        BusinessAddress = 4,
        ContractAddress = 5,
        RecipientAddress = 6,
        OtherAddress = 7,
        AuthorAddress = 8,
        DefaultAddress = 9,
    }

    public enum GroupTypeEnum
    {
        Default = 0,
        Administrator = 1,
        EditorialSecretary = 2, // Thư ký tòa soạn
        SectionSecretary = 3, // Thư ký chuyên mục,
        Reporter = 4, // Phóng viên,
        VnPost = 5, // Đối tác VNPOST
        VnPostCustomer = 6, // khách hàng do vnpost giới thiệu
        Pseudonym = 7, // Bút danh 
        Collaborators = 8, // Cong tac vien
    }

    public enum NewsUserGroupTypeEnum
    {
        Pseudonym = 7, // Bút danh 
        Collaborators = 8, // Cong tac vien
    }

    [Flags]
    public enum OrderStatusEnum : long
    {
        //[Display(Name = "Mới")]
        Created = 1,

        //[Display(Name = "Khách hàng xác nhận lấy hàng")]
        CustomerConfirm = 2,

        //[Display(Name = "Khách hàng xác nhận không lấy hàng")]
        CustomerCancel = 4,

        //[Display(Name = "Đang thanh toán")]
        Paying = 8,

        //[Display(Name = "Đã thanh toán")]
        PayCompleted = 16,

        //[Display(Name = "Bắt đầu xử lý giao nhận")]
        WaitAccountingConfirm = 32,

        //[Display(Name = "Hủy giao nhận")]
        WarehouseCancel = 64,

        //[Display(Name = "Đã giao cho KH")]
        Delivered = 128,

        //[Display(Name = "NVGN đã nộp tiền cho thu ngân")]
        PaidAtCashier = 256,

        //[Display(Name = "Đã đóng")]
        Completed = 512,

        //[Display(Name = "Hủy")]
        Cancel = 1024,

        // [Display(Name = "Trả hàng")]
        Return = 2048,

        //[Display(Name = "Chuyển qua đơn hàng mới")]
        MoveToNewOrder = 4096,

        //[Display(Name = "Kế toán xác nhận")]
        AccountingConfirm = 8192,

        //[Display(Name = "Xuất hóa đơn")]
        InvoiceBilling = 16384,
    }

    [Flags]
    public enum OrderConfigOptionEnum : long
    {
        TakeVat = 1,
        IsLogin = 2,
        HasDeliveryCharges = 4,
        Decay = 8,
        FromAdmin = 16
    }

    public enum DeviceTypeEnum
    {
        Web = 1,
        Wap = 2,
        MobileAppAndroid = 4,
        MobileAppIos = 8,
        Admin = 16,
        Tablet = 32
    }

    public enum PageDeviceTypeEnum
    {
        Desktop = 1,
        Mobile = 2,
        Tablet = 3,
        AMP = 4
    }

    [Flags]
    public enum PaidAdvanceStatusEnum
    {
        Created = 1,
        Paying = 2,
        PayCompleted = 4,
        WaitAccountingConfirm = 8,
        Cancel = 16,
        Return = 32,
        MoveToNewOrder = 64,
        PayFail = 128,
        Debt = 256,
        DebtCollected = 512,
    }

    public enum OrderTypeEnum
    {
        Default = 0,
        Article = 1
    }

    public enum CustomerTypeEnum
    {
        VIP1 = 1,
        VIP2 = 2
    }

    [Flags]
    public enum OrderItemConfigOptionEnum : long
    {
        ReturnActive = 1,
        IsInstallment = 2,
        IsConsignment = 4
    }

    [Flags]
    public enum ProductConfigOption : long
    {
        NotAllowCustomerReviews = 1,

        //Tích vào điều khoản sử dụng
        HasUserAgreement = 2,
        DisplayStockAvailability = 4,
        DisplayStockQuantity = 8,
        AllowBackInStockSubscriptions = 16,
        NotReturnable = 32,
        DisableBuyButton = 64,
        DisableWishlistButton = 128,
        AvailableForPreOrder = 256,
        SerialRequired = 512,
        InstallRequired = 1024,
        CanDeliverMultiCity = 2048,
        NotTrackingInventory = 4096,
        IllustrativePhoto = 8192,
        InformationIsBeingUpdated = 16384,
        NotDisplayOnListing = 32768,
        DisplayOnListing = 65536
    }

    public enum ProductOrderTransactionStatusEnum
    {
        Reserved = 1,
        UnReserved = 2,
        Purchase = 3
    }

    public enum RepairTicketPaymentTypeEnum
    {
        //[Display(Name = "Tiền mặt")]
        Cash = 1,

        //[Display(Name = "Chuyển khoản")]
        Bank = 2,

        //[Display(Name = "POS")]
        Pos = 3
    }

    public enum PaidAdvanceTransactionStatusEnum
    {
    }

    public enum OrderReasonType
    {
        ReturnOrder = 1
    }

    public enum NewEventTypeEnum
    {
        // [Display(Name = "Video")] Video = 1,
        //
        // [Display(Name = "Định danh quảng cáo")]
        // AdsIdentity = 2048,
    }

    public enum SeriesConfigOptionsEnum
    {
        AdsIdentity = 2048,
    }

    public enum TagConfigOptionEnum
    {
        // [Display(Name = "Định danh quảng cáo")]
        // AdsIdentity = 2048,
    }

    public enum TagOptionEnum
    {
        [Display(Name = "Không khóa")] Normal = 1,
        [Display(Name = "Khóa")] Lock = 2,
    }

    [Flags]
    public enum ReturnOrderStatusEnum : long
    {
        //[Display(Name = "Mới")]
        Created = 1,

        //[Display(Name = "Duyệt hoàn")]
        Approved = 2,

        //[Display(Name = "Từ chối duyệt hoàn")]
        Rejected = 4,

        //[Display(Name = "Kế toán hoàn tiền xong")]
        Refund = 8,

        //[Display(Name = "Đã đóng")]
        Closed = 16,
    }

    [Flags]
    public enum ReturnOrderItemStatusEnum : long
    {
        Active = 1,
        Remove = 2,
        Approved = 4,
        Rejected = 8
    }

    [Flags]
    public enum ProductStatusEnum
    {
        New = 1,
        Approve = 2,
        Reject = 4,
        Processed = 8,
        Cancel = 16,
        UnActive = 32,
        Changed = 64,
        Publish = 128,
        UnPublish = 256,
        MissCache = 512,
        Deleted = -1,
    }

    [Flags]
    public enum PaidAdvanceTypeEnum
    {
        Cash = 1,
        ATM = 2,
        VISA = 4,
        GIFTCODE = 8,
        QRCODE = 16,
        POS = 32,
        MPOS = 64,
        BankTranfer = 128,
        COD = 256,
        Gift = 512,
        VNPost = 1024,
        AppleIAP = 2048,
        Type997 = 4096,
        QRVNPAY = 8192,
    }

    [Flags]
    public enum RefundTypeEnum
    {
        // ReSharper disable once InconsistentNaming
        Cash = 1,

        // ReSharper disable once InconsistentNaming
        ATM = 2,

        // ReSharper disable once InconsistentNaming
        VISA = 4,

        // ReSharper disable once InconsistentNaming
        GIFTCODE = 8,

        // ReSharper disable once InconsistentNaming
        QRCODE = 16,
        POS = 32,
        MPOS = 64,
        BankTranfer = 128,
    }

    public enum ProductTypeEnum
    {
        [Display(Name = "Bình thường")] Normal = 1,

        //[Display(Name = "Tươi sống")]
        //FreshFood = 2,
        //[Display(Name = "Cồng kềnh")]
        //Bulky = 3,
        //[Display(Name = "Rau")]
        //Vegetable = 4,
        //[Display(Name = "Thẻ online")]
        //Online = 5,
        //[Display(Name = "Dịch vụ")]
        //Deal = 6,
        PremiumAccount = 7,
        [Display(Name = "Ôtô")] Car = 8,
        [Display(Name = "Xe máy")] Motor = 9,
        [Display(Name = "Dự án")] EstateProject = 10,
        [Display(Name = "Sản phẩm dự án")] EstateProjectProduct = 11,
        [Display(Name = "Thiết kế")] EstateArchitectureNews = 12,

        [Display(Name = "Giá vật liệu xây dựng")]
        EstateConstructionMaterialNews = 13,
        [Display(Name = "Tin rao")] EstateBrokerNews = 14,
        [Display(Name = "Quy hoạch")] EstateSchemeNews = 15,
        [Display(Name = "Pháp lý")] EstateLegalNews = 16,
        [Display(Name = "Giá đất")] EstatePriceNews = 17,
        [Display(Name = "Bảng xếp hạng")] EstateProductZoneNews = 18,
        [Display(Name = "Khảo sát")] EstateProductVote = 19
    }

    public enum ProductMappingObjectTypeEnum
    {
        Article = 1,
        Address = 2,
        Product = 3
    }

    public enum ProductMappingTypeEnum
    {
    }

    public enum ProductAttributeType
    {
        [Display(Name = "Chọn một giá trị")] SelectOneValue = 2,
        [Display(Name = "Mô tả ngắn")] ShortText = 4,
        [Display(Name = "Mô tả dài")] LongText = 6
    }

    [Flags]
    public enum AttributeCategoryConfigOptionEnum
    {
        HiddenOnProductDetail = 1,
        HiddenOnPMS = 2,
        ShortTextToSelectOneValue = 4,
        ShortTextToSelectOneValueAppendManufacturerName = 8,
        ShortTextFormatDateTime = 16,
        ShortTextFormatInteger = 32,
        ShortTextFormatDecimal = 64,
        SelectCheckbox = 128,
        SelectMulti = 256
    }

    public enum VendorProductStatusEnum
    {
        Active = 1,
        Deleted = -1,
        New = 3,
        InActive = 2,
        Changed = 4
    }

    public enum WarehouseProductMappingStatusEnum
    {
        New = 0,
        Active = 1,
        Stop = 2,
        ChangeSellPrice = 3
    }

    public enum ProductMediaTypeEnum
    {
        [Display(Name = "Ảnh")] Image = 1,
        [Display(Name = "Video")] Video = 2,
        [Display(Name = "Tài liệu pháp lý")] LegalDocument = 3,

        [Display(Name = "Tài liệu mô tả quy hoạch")]
        SchemeDocument = 4,

        [Display(Name = "Tài liệu thuyết minh quy hoạch")]
        SchemeDescriptionDocument = 5,

        [Display(Name = "Tài liệu quy định quản lý theo đồ án")]
        SchemeManagementDocument = 6,
        [Display(Name = "Tài liệu giá VLXD")] ContructionMaterialPriceDocument = 7,
        [Display(Name = "Biểu đồ")] Chart = 8
    }

    // public enum MenuPosition
    // {
    //     CMSMenu = 1,
    //     MenuHeader = 2,
    //     MegaMenu = 3,
    //     MenuFooter = 4,
    //     MenuUser = 5,
    //     MenuHeaderSport = 6,
    //     MenuHeaderVideo = 7,
    //     MenuLeftVideo = 8,
    //     HashTagTopHeader = 9,
    //     MenuHeaderVnnEn = 10,
    //     MenuFooterVnnEn = 11,
    //     MenuHeaderDHD = 12,
    //     MenuInfonet = 13,
    //     MenuICTNews = 14,
    //     PMSMenu = 15,
    //     MenuXeHeader = 16,
    //     MenuXeFooter = 17,
    //     MenuGNNHeader = 18,
    //     MenuGNNFooter = 19,
    //     Menu997NewsHeader = 20,
    //     Menu997NewsFooter = 21,
    //     CMSMobile = 22,
    //     Test = 23,
    //     MegaMenuRps = 24,
    //     PMSBDSMenu = 25,
    //     MenuBDSHeader = 26,
    //     MenuBDSFooter = 27,
    //     CMSDataMenu = 30,
    //     TrackingCMSMenu = 31,
    //     TrackingHeatmapUserMenu = 32,
    //     MenuHeader2Sao = 33,
    //     MenuMega2Sao = 34,
    //     MenuFooter2Sao = 35,
    //     MenuHashTagTopHeader2Sao = 36,
    //     GoodLink = 37
    // }

    [Flags]
    public enum NewsStatusEnum
    {
        // tạo mới ở trạng thái tạo mới
        [Display(Name = "Nháp")] New = 1,

        // Phóng viên xong chuyển cho BTV, chờ biên tập
        //Chờ biên tập
        [Display(Name = "Gửi biên tập")] SendToEditor = 2,

        // BTV nhận bài và biên tập bài
        [Display(Name = "Đang biên tập")] Editing = 4,

        //BTV biên tập xong chuyển cho trưởng ban, Chờ xuất bản
        [Display(Name = "Gửi xuất bản")] SendToPublish = 8,

        // Chờ thiết kế
        WaitForDesign = 16,

        // Chờ Phó TBT phụ trách duyệt
        WaitForDeputyEditorInChiefManager = 33554432,

        // TB hoặc TKXB: Gửi TKTS xuất bản => Chờ TKTS xuất bản
        [Display(Name = "Gửi TKTS")] SendToSecretaryPublish = 32,

        // Trưởng ban publish
        [Display(Name = "Xuất bản")] Publish = 64,

        PublishMultipleWebsite = 128,

        PublishedMultipleWebsite = 256,

        // Đánh dấu bài trả lại
        IsReturn = 512,

        // TB hạ bài
        [Display(Name = "Hạ xuất bản")] UnPublish = 1024,

        // Phóng viên trả bài
        [Display(Name = "Phóng viên trả bài")] RemoveFromReporter = 2048,

        //BTV trả bài
        [Display(Name = "BTV trả bài")] RemoveFromEditor = 4096,

        // TB trả bài
        [Display(Name = "Trưởng ban trả bài")] RemoveFromPublish = 8192,

        // bài bị chuyển loại hiển hị
        [Display(Name = "Chuyển loại hiển thị")]
        DisplayTypeChanged = 16384,

        // bài đã được 1 lần publish
        [Display(Name = "Đã xuất bản 1 lần")] Published = 32768,

        //Bài bị xóa
        [Display(Name = "Xóa")] Removed = 65536,

        //Chờ Lãnh đạo ban duyệt
        WaitForHeadOfSection = 131072,

        //Chờ phân loại
        WaitForClassify = 262144,

        //Chờ đọc soát
        WaitForProofread = 524288,

        //Chờ Trưởng kíp duyệt
        WaitForShiftLeader = 1048576,

        //Chờ TB TKTS duyệt
        WaitForSecretaryPublish = 2097152,

        //Chờ PTBT trực duyệt
        WaitForDeputyEditorInChief = 4194304,

        //Chờ TBT duyệt
        WaitForEditorInChief = 8388608,

        //Chờ TKTS tổng hợp duyệt
        WaitForEditorialSecretary = 16777216,
    }

    public enum NewsActionTypeEnum
    {
        //ReCallFromReporter = 1,
        // Phóng viên xong chuyển cho BTV
        SendToEditor = 2,
        EditorPick = 3,

        //ReCallFromEditor = 4,
        SendToPublish = 5,

        //CheckForPublish = 6,
        //Gửi trưởng ban TKTS
        SendToSecretaryPublish = 7,

        //ReCallFromPublish = 8,
        Publish = 9,

        //Trả lại BTV
        DenyFromEditor = 10,
        DenyFromPublish = 11,
        DenyFromPublishToReporter = 12,
        UnPublish = 13,
        RemoveFromReporter = 14,
        RemoveFromEditor = 15,
        RemoveFromPublish = 16,
        RePublish = 17,
        PublishDirect = 18,
        ReSendEditor = 19,
        Remove = 20,

        // gửi cho trưởng ban, Gửi lãnh đạo ban
        SendToHeadOfSection = 21,

        // gửi phân loại
        SendToClassify = 22,

        // gửi đọc và soát lỗi
        SendToProofread = 23,

        // gửi trưởng ca, trưởng kíp
        SendToShiftLeader = 24,

        // gửi thư ký tòa soạn
        SendToEditorialSecretary = 25,

        // gửi phó tổng biên tập trực
        SendToDeputyEditorInChief = 26,

        // gửi tổng biên tập
        SendToEditorInChief = 27,

        // Trả lại lãnh đạo ban
        DenyToHeadOfSection = 28,

        // Trả lại phân loại
        DenyToClassify = 29,

        // Trả lại đọc soát
        DenyToProofread = 30,

        //Trả lại biên tập
        DenyToEditor = 31,

        //Trả lại trưởng kíp
        DenyToShiftLeader = 32,

        // Trả lại trưởng ban TKTS
        DenyToSecretaryPublish = 33,

        //Trả lại Phó TBT
        DenyToDeputyEditorInChief = 34,

        //Trả lại TBT
        DenyToEditorInChief = 35,

        //Trả lại phóng viên
        DenyToReporter = 36,

        //Gửi xuất bản trực tuyến
        CopyToOnline = 37,

        // Trả lại TKTS phân loại
        DenyToEditorialSecretary = 38,

        //  Gửi thiết kế
        SendToDesign = 39,

        //  Gửi in
        SendToPrint = 40,

        // Trả lại thiết kế
        DenyToPrint = 41,

        // gửi phó tổng biên tập phụ trách
        SendToDeputyEditorInChiefManager = 42,

        //  Trả lại thiết kế: Bấm để chuyển trạng thái về Chờ thiết kế
        DenyToDesign = 43,

        //Trả lại Phó TBT phụ trách
        DenyToDeputyEditorInChiefManager = 44,
        OnlineInterviewGuestAddOrChange = 45,
        OnlineInterviewGuestAddOrChangeCreateByOther = 46,
        ContentOnlineInterviewQuestionAndAnswerReplace = 47,
        ContentOnlineInterviewQuestionAndAnswerReplaceCreateByOther = 48,
        ContentOnlineInterviewQuestionAndAnswerPublish = 49,
        ContentOnlineInterviewQuestionAndAnswerUnPublish = 50,
    }

    public enum NewsDisplayTypeEnum
    {
        [Display(Name = "Bài thường")] Normal = 1,
        [Display(Name = "Bài nhiều trang")] MultiplePage = 2,
        [Display(Name = "Bài ảnh")] Image = 3,
        [Display(Name = "Bài video")] Video = 4,
        [Display(Name = "Bài live")] Live = 5,
        [Display(Name = "Bài trắc nghiệm")] Quiz = 6,
        [Display(Name = "Bài link ngoài")] OutSystem = 7,
        [Display(Name = "Bài Infographic")] Infographic = 8,

        [Display(Name = "Bài giao lưu trực tuyến")]
        OnlineInterview = 10,

        [Display(Name = "Bài EMagazineRotateCover")]
        EMagazineRotateCover = 11,
        [Display(Name = "Bài Longform")] EMagazineLongform = 12,

        [Display(Name = "Bài EMagazineStoryScroll")]
        EMagazineStoryScroll = 13,

        [Display(Name = "Bài EMagazineFlipPage")]
        EMagazineFlipPage = 14,

        [Display(Name = "Bài EMagazinePageSlide")]
        EMagazinePageSlide = 15,

        [Display(Name = "Bài EMagazineCustom")]
        EMagazineCustom = 16,
        [Display(Name = "Bài Custom")] Custom = 17,
        [Display(Name = "Bài HtmlContent")] HtmlContent = 18,
        [Display(Name = "Bài FullImages")] FullImages = 19,
        [Display(Name = "Bài MoveToIctNews")] MoveToIctNews = 20,

        [Display(Name = "Bài xuất bản nhiều website")]
        PublishMultipleWebsite = 21,
        [Display(Name = "Bài Podcast")] Podcast = 22,
        [Display(Name = "Số báo giấy")] Newspaper = 30,

        [Display(Name = "Bài chi tiết báo giấy")]
        NewspaperDetail = 31,

        [Display(Name = "Bài báo in xem online")]
        NewspaperOnline = 32,
        [Display(Name = "Bài bói Tarot")] Tarot = 33
    }

    public enum NewsHistoryTypeEnum
    {
        History = 1,
        AutoSave = 2,
        AutoSaveV2 = 20
    }

    public enum NewsHistoryActionTypeEnum
    {
        Add = 1,
        Update = 2,
        StatusChange = 3,
        Draft = 4,
        AutoSave = 5,
        AddOrRemoveToEvent = 6,
        AddOrRemoveToSeries = 7,
        AddOrRemoveToTag = 8,
        AddOrRemoveToProject = 9,
        ChangeAudio = 10,
        ChangeNotInTopView = 11,
        DisplayTypeChange = 12,
        OutSystemCheckLinkActiveByDomainEvent = 13,
        NewsUpdateChildrenPodcastEvent = 14,
        NewsSyncToESEvent = 15,
        MoveNewsTo997Event = 16,
        ChangeProjectSettings = 17,
        ChangeGroupVoteSettings = 18,
        AddOrRemoveToWiki = 19,
        AutoSaveV2 = 20
    }

    public enum NewsDisplayTypeChangeOptionEnum
    {
        Copy = 1,
        Replace = 2
    }

    [Flags]
    public enum NewsOptionEnum : long
    {
        [Display(Name = "Bài PR")] PR = 4,
        [Display(Name = "Bài Premium")] Fee = 8,
        [Display(Name = "Audio")] Audio = 1,
        [Display(Name = "Ẩn quảng cáo")] Ads = 2,
        [Display(Name = "Ẩn trên trang")] Hidden = 16,
        [Display(Name = "Ẩn box bình chọn")] PollHidden = 32,
        [Display(Name = "Social Mark")] SocialTag = 64,
        [Display(Name = "NotInTopView")] NotInTopView = 128,
        [Display(Name = "Readers")] Readers = 256,
        [Display(Name = "Live")] Live = 512,
        [Display(Name = "FullImages")] FullImages = 1024,

        [Display(Name = "Định danh quảng cáo")]
        AdsIdentity = 2048,

        [Display(Name = "HiddenInWebsite")] HiddenInWebsite = 4096,

        [Display(Name = "NewsPublishMultipleWebsite")]
        NewsPublishMultipleWebsite = 8192,

        //AdsTag = 16384,
        //Video = 32768,
        [Display(Name = "Multimedia")] Multimedia = 65536,
        [Display(Name = "PodcastClone")] PodcastClone = 131072,
        [Display(Name = "CustomersPreview")] CustomersPreview = 262144,

        [Display(Name = "Thông tin và truyền thông")]
        InfoAndCommunications = 524288,
        [Display(Name = "NotPublish")] NotPublish = 1048576,

        [Display(Name = "Bài đặc cách lượt view")]
        BypassCheckViewKpi = 2097152,

        [Display(Name = "Tin bài về hoạt động của lãnh đạo Đảng và Nhà nước")]
        NewsOfHeadLeaders = 4194304,

        [Display(Name = "Tin bài về hoạt động của lãnh đạo địa phương")]
        NewsOfLocalLeaders = 8388608,

        [Display(Name = "Tin bài phục vụ Bộ Thông tin và truyền thông")]
        NewsOfInfoAndCommunications = 16777216,

        [Display(Name = "Tin bài bạn đọc ủng hộ, trao quà của ban Bạn đọc")]
        NewsOfCharityReaders = 33554432,

        [Display(Name = "Tin bài đặc biệt theo yêu cầu của lãnh đạo tòa soạn")]
        NewsOfPublisherLeaders = 67108864
    }

    [Flags]
    public enum NewsCategoryEnum
    {
        CmsHidden = 1,
        InfoAndCommunications = 2,
        NewsPublishMultipleWebsiteHidden = 4,
        UnActiveInWebsite = 8,
        WordCountImageCaption = 16,
        ArticleSearch = 32,
        PromotionArticleCategory = 64,
        InstructionArticleCategory = 128,
        FinanceArticleCategory = 256,
        InsuranceArticleCategory = 512,
    }

    public enum NewsAdminSortTypeEnum
    {
        [Display(Name = "Tăng dần")] Asc = 1,
        [Display(Name = "Giảm dần")] Desc = 2
    }

    public enum FEDateRangeTypeEnum
    {
        [Display(Name = "1 ngày qua")] Day = 1,
        [Display(Name = "1 tuần qua")] Week = 2,
        [Display(Name = "1 tháng qua")] Month = 3,
        [Display(Name = "1 năm qua")] Year = 4
    }

    public enum FENewsDisplayTypeEnum
    {
        [Display(Name = "Bài thường")] Normal = 1,
        [Display(Name = "Bài ảnh")] Image = 2,
        [Display(Name = "Bài video")] Video = 3,
        [Display(Name = "Bài Podcast")] Podcast = 4,
        [Display(Name = "Bài EMagazine")] EMagazine = 5,
        [Display(Name = "Bài StoryScroll")] StoryScroll = 6,
        [Display(Name = "Bài Infographic")] Infographic = 7
    }

    public enum NewsAdminSortFieldEnum
    {
        Default = 0,
        CreateDate = 1,
        PublishDate = 2,
        PageView = 3,
        Position = 4,
        Comment = 5,
        Event = 6,
        Series = 7,
        UpdatedDate = 8,
        PageView12Hour = 9,
        PageView3Hour = 10,
        PageView6Hour = 11,
        PageView9Hour = 12,
        Wiki = 13,
    }

    public enum AuthorPositionEnum
    {
        [Display(Name = "Tác giả")] Author = 1,
        [Display(Name = "Tác giả ảnh")] Image = 2,
        [Display(Name = "Biên tập viên")] Editor = 3
    }

    public enum VnpayHttpTypeEnum
    {
        Request = 1,
        Response = 2
    }

    // public enum AudioTypeEnum
    // {
    //     NuMienBac = 1,
    //     NuMienNam = 2,
    //     NamMienBac = 3,
    //     NamMienNam = 4,
    //     NamMienTrung = 5,
    //     NuMienTrung = 6,
    // }

    public enum WikiOptionEnum
    {
        DiseaseHot = 1,
        DiseaseInfectious = 2,
        DiseaseDeficiency = 4,
        DiseaseHereditary = 8,
        DiseasePhysiological = 16,
        Politicians = 32
    }

    [Flags]
    public enum NewsConfigOption
    {
        AllowComment = 1,
    }

    public enum NewsQuizTypeEnum
    {
        Normal = 1,
        Image = 2,
    }

    public enum NewsQuizAnswerTypeEnum
    {
        SingleCorrectAnswer = 1,
        MultipleCorrectAnswer = 2
    }

    public enum AutoPayStatusEnum
    {
        Auto = 1,
        Manual = 2,
        Confirm = 3
    }

    // public enum LanguageTypeEnum
    // {
    //     AdminSystem = 1,
    //     AccountSystem = 2,
    //     FESport = 3,
    //     PMS = 5,
    //     MMS = 6,
    //     Tracking = 7
    // }

    public enum CommentStatusEnum
    {
        New = 0,
        Approved = 1,
        Rejected = 2,
        Changed = 3,
        Pinned = 4,
        Cancel = -1,
    }

    public enum CommentSortTypeEnum
    {
        Like = 0,
        CreatedDateAsc = 1,
        CreatedDateDesc = 2
    }

    // public enum PageSourceType
    // {
    //     All = 0,
    //     Article = 1,
    //     Product = 2,
    //     ProductManufacturer = 3,
    //     ProductVendor = 4,
    //     ProductAttributeValue = 5,
    //     ProductEstate = 6
    // }

    public enum RateStatusEnum
    {
        Active = 1,
        InActive = 3,
        Delete = -1
    }

    public enum SeriesMappingTypeEnum
    {
        Category = 1,
        News = 2
    }

    public enum EventMappingTypeEnum
    {
        Category = 1,
        News = 2
    }

    public enum CrawlNewsValueByEnum
    {
        ContentText = 1,
        AttributeText = 2,
        ContentHtml = 3,
    }

    public enum ElementSelectorGroupEnum
    {
        Title = 1,
        Description = 2,
        Content = 3,
        Avatar = 4,
        Tag = 5,
        RemoveElement = 6,
        PublishDate = 7,
        Author = 8
    }

    public enum VoteTypeEnum
    {
        SingleChoice = 0,
        MultiChoice = 1,
        Readonly = 2,
        Quiz = 3,
        QuizGroup = 4
    }

    public enum VoteDisplayConfigEnum
    {
        [Display(Name = "Tất cả")] All = 0,
        [Display(Name = "Trang chủ")] Home = 1,
        [Display(Name = "Chuyên mục")] Category = 2,
        [Display(Name = "Chủ đề")] Tags = 3,
        [Display(Name = "Sự kiện")] Events = 4,
        [Display(Name = "Tuyến bài")] Series = 5,
        [Display(Name = "Tin bài")] News = 6,
        DetailNews = 7,
        Quiz = 8
    }

    public enum VoteFromEnum
    {
        News = 1,
        Category = 2
    }

    public enum GroupVoteStatusEnum
    {
        Delete = -1,
        All = 0,
        Active = 1,
        InActive = 3,
    }

    public enum LuckyDrawRewardTypeEnum
    {
        //Viettel = 1,
        Vinaphone = 2,
        Mobifone = 3,
    }

    public enum LuckyDrawDateStatusEnum
    {
        NotStarted = 1,
        Happenning = 2,
        Finished = 3,
    }

    public enum GroupVoteSortFieldEnum
    {
        CreatedDate = 1,
        MemberJoin = 2
    }

    public enum TopicActionEnum
    {
        Create = 1,
        Change = 2,
        Active = 3,
        UnActive = 4,
        SetHotInHomePage = 5,
        UnSetHotInHomePage = 6,
        SetHotInCategory = 7,
        UnSetHotInCategory = 8,
        Delete = 9
    }

    // [Flags]
    // public enum FileSystemTypeEnum
    // {
    //     AllImage = 0,
    //     Folder = 1,
    //     Image = 2,
    //     Video = 4,
    //     Pdf = 8,
    //     Gif = 16,
    //     Audio = 32,
    //     Panorama = 64
    // }
    //
    // public enum ImageKeywordTypeEnum
    // {
    //     All = 0,
    //     Person = 1,
    //     Address = 2,
    //     Event = 3,
    //     Note = 4,
    //     Name = 5,
    // }
    public enum FilePathEnum
    {
        Image = 2,
        Video = 4,
        Audio = 32,
        GoogleDrive = 64
    }

    // public enum FileStyleEnum
    // {
    //     All = 0,
    //     Normal = 1,
    //     Watermark = 2
    // }
    //
    // public enum FileLicenseEnum
    // {
    //     All = 0,
    //     Normal = 1,
    //     License = 2
    // }
    //
    // public enum FileSystemFolderEnum
    // {
    //     All = 0,
    //     Image = 1,
    //     Video = 2,
    //     Audio = 3,
    //     GoogleDrive = 4
    // }

    [Flags]
    public enum SeoSettingEnum
    {
        NoIndex = 1,
        NoFollow = 2
    }

    public enum MediaTypeEnum
    {
        NewsAvatar = 1,
        NewsFacebookAvatar = 2,
        NewsFacebookShare = 3,
        TagCover = 4,
        SeriesCover = 5, //Ảnh cover Desktop 
        SeriesAvatar = 11,
        SeriesCoverMobile = 12, //Ảnh cover Mobile
        SeriesFacebookShare = 13, //Ảnh share Facebook:
        EventCover = 6, //Ảnh cover Desktop
        EventAvatar = 7,
        EventCoverMobile = 9, //Ảnh cover Mobile
        EventFacebookShare = 10, //Ảnh share Facebook:
        ArticleContent = 8,
        PodcastCoverDesktop = 14,
        PodcastCoverMobile = 15,
        NewsCategoryAvatar = 16,
        NewsCategoryFacebookShare = 17,
        FileInContent = 18,
        NewspaperPdf = 19,
        TarotImageFront = 20,
        TarotImageBack = 21,
    }

    public enum VehicleMediaEnum
    {
        InspectionFront = 1,
        InspectionBack = 2,
        CarRegistrationFront = 3,
        CarRegistrationBack = 4,
    }

    public enum HttpProtocolEnum
    {
        Https = 1,
        Http = 2,
    }

    // public enum GoogleFileChecksumEnum
    // {
    //     Md5 = 1,
    //     Sha1 = 2,
    //     Sha256 = 3,
    // }

    public enum ComponentTypeEnum
    {
        Default = 0,
        Html = 1,
        NewsList = 2,
        NewsDetail = 3,
        Article = 100,
        Layout = 5,
        Pseudonym = 6,
        PseudonymDetail = 7,
        WikiProfiles = 8,
        CategoryCustom = 9,
        Category = 10,
        NewsEvents = 11,
        ArticleSearch = 12,
        SeriesList = 13,
        TagsDetail = 14,
        SeriesDetail = 15,
        EventDetail = 19,
        WikiArticleDetail = 20,
        PartialView = 103,
        CKEditorImages = 1000,
        WikiDiseaseDetail = 21,
        EventPage = 1001,
        Menus = 23,
        EventByZone = 24,
        ArticleByCategoryGroups = 25,
        ArticlesByProvince = 26,
        Sitemap = 27,
        ArticlesByZone = 28,
        Wikis = 29,
        WikiProfilesTopHome = 30,
        ArticlesByWiki = 31,
        EducationScores = 32,
        EducationStudentScores = 33,
        EducationScoreReport = 34,
        EducationMajorGroup = 35,
        EducationMajor = 36,
        University = 37,
        UniversityDetail = 38,
        UniversityScore = 39,
        UniversityGroup = 40,
        LayoutByArticle = 41,
        MappingConfig = 101,
        SpecialPosition = 10000,
        SpecialPositionEvent = 10001
    }

    public enum PageTypeEnum
    {
        Home = 1,
        NewsCategory = 2,
        NewsDetail = 3,
        LandingPage = 4,
        SpecialEvent = 5,
        ArticlePreview = 6,
        Is404 = 7,
        IsOldVersion = 8,
        NewsCategoryCustom = 9,
        Tag = 10,
        EducationStudentScoresDetail = 11,
        Is301 = 12,
        Is301Rule = 13,
        Premium = 14,
        PremiumSpecial = 15,
        WebsiteTemplate = 100,
    }

    public enum ComponentConditionTypeEnum
    {
        Category = 1,

        //Author = 2,
        //News = 5,
        //Series = 6,
        Tag = 7,
        Event = 8,

        //Type = 9,
        DisplayType = 10,

        //Source = 11,
        //PublishDate = 12,
        //Sticker = 13,
        //Icon = 14,
        //Province = 15,
        //SubEvent = 16,
        //View = 17,
        //CategoryMain = 18,
        //CategorySub = 19,
        Menu = 21,
        Zone = 22,
        //Wiki = 23,
    }

    public enum LiveMediaTypeEnum
    {
        Normal = 1,
        Image = 2,
        Video = 3,
    }

    // public enum AccountSettingEnum
    // {
    //     Language = 1,
    //     Company = 2,
    //     DateFormat = 3,
    //     NumberFormat = 4,
    //     Website = 5,
    //     FormDisplaySetting = 6,
    //     TrackingApplication = 7
    // }

    [Flags]
    public enum GiftCodeCampaignConfigOptionEnum
    {
        AllowPaymentOnCheckout = 1,
        IsApplyWithPromotion = 2,
        IsRequireOtp = 4,
        PublicOnFe = 8,
        FromVnPost = 16,
    }

    public enum GiftCodeCampaignStatus
    {
        New = 1,
        Active = 2,
        Stop = 3,
        Cancel = 4,
    }

    public enum GiftCodeConditionTypeEnum
    {
        Category = 1,
        Manufacturer = 2,
        Vendor = 3,
        Warehouse = 4,
        Attribute = 5,
        Price = 6,
        Quantity = 7,
        Product = 8,

        PaymentType = 9,
        DeviceType = 10,

        Location = 11,
        Customer = 15,
    }

    public enum GiftCodeGroupStatus
    {
        New = 1,
        Active = 2,
        Stop = 3,
        Cancel = 4
    }

    public enum GiftCodeTypeEum
    {
        Percent = 1,
        Amount = 2
    }

    public enum GiftCodeGroupTypeEnum
    {
        // [Display(Name = "(n) mã, mỗi mã 1 lần gắn với 1 định danh")]
        OnceUsed = 1,

        // [Display(Name = "(n) mã, mỗi mã (m) lần với 1 định danh")]
        OnceUsedByAccount = 2,

        //[Display(Name = "1 mã, (m) lần với (z) định danh")]
        RepeatedUse = 3,
    }

    public enum GiftCodeGroupTypeDescriptionEnum
    {
        // [Display(Name = "(n) mã, mỗi mã 1 lần gắn với 1 định danh")]
        OnceUsedDescription = 1,

        // [Display(Name = "(n) mã, mỗi mã (m) lần với 1 định danh")]
        OnceUsedByAccountDescription = 2,

        //[Display(Name = "1 mã, (m) lần với (z) định danh")]
        RepeatedUseDescription = 3,
    }

    public enum GiftCodeUsedStatus
    {
        Used = 1,
        Holding = 2,
        Cancel = 3
    }

    public enum GiftCodeCalendarModeEnum
    {
        FullTime = 1,
        ByCalendar = 2
    }

    public enum CommentObjectTypeEnum
    {
        News = 1,
        Category = 2,
        ViewUrl = 3
    }

    public enum SpecialEventFilterEnum
    {
        All = -1,
        Normal = 0,
        Special = 1
    }

    public enum AttributeType
    {
        DropdownList = 1,

        //RadioList = 2,
        //Checkboxes = 3,
        TextBox = 4,

        //MultilineTextbox = 5,
        Datepicker = 6,
        FileUpload = 7,
        ColorSquares = 8,

        //ImageSquares = 9,
        //ReadonlyCheckboxes = 10,
        Timeline = 11,
        GalleryImage = 12,
        EditorBox = 13,
        Location = 14,
    }

    public enum WikiMediaTypeEnum
    {
        ArticleAvatar = 1,
        ArticleFacebookAvatar = 2,
        ArticleFacebookShare = 3,
        TagCover = 4,
        SeriesCover = 5,
        EventCover = 6,
        EventAvatar = 7
    }

    public enum WikiAttributeTypeEnum
    {
        Default = 0,
        BirthDay = 1,
        Job = 2,
        BirthPlace = 3,
        Position = 4,
        Aliases = 5,
        DiseaseName = 6,
        DiseaseScienceName = 7,
        DiseaseAge = 8,
        DiseaseGender = 9,
        DiseasePopularity = 10,
        LocationBirthPlace = 11,
        LocationHomeTown = 12,
        CurrentPosition = 13,
    }

    public enum ZonePositionType
    {
        Article = 1,
        Other = 0,
        Event = 2,
        Series = 3,
        Product = 4,
        Manufacturer = 5,
        Vendor = 6,
        Attribute = 7
    }

    public enum LockActionEnum
    {
        ZonePosition = 1
    }

    public enum SignalRMethodEum
    {
        OnLogin = 1,
        OnZoneDisplayConfigLockOrUnLock = 2
    }

    public enum JsonLdTypeEnum
    {
        Static = 1,
        Auto = 2,
        Text = 3,
        Number = 4,

        // MultipleText = 5,
        // MultipleNumber = 6,
        Image = 7,
        Video = 8,
        Date = 9,
        DateTime = 10,
        RangeDate = 11,
        RangeDateTime = 12,
        SelectOne = 13,
        SelectMultiple = 14,
    }

    public enum JsonLdGroupTypeEnum
    {
        Value = 1,
        Object = 2,
        Array = 3
    }

    public enum SocialNetworkEnum
    {
        GooglePlus = 1,
        Facebook = 2,
        Twitter = 3
    }

    public enum RedirectUrlEnum
    {
        Code300 = 300,
        Code301 = 301,
        Code302 = 302,
        Code304 = 304,
        Code307 = 307,
        Code308 = 308,
    }

    public enum WebsiteSettingTypeEnum
    {
        SocialTag = 1,
        Ga = 2,
        GaId = 3,
        MaxPageSize = 4,
        CdnDomain = 5,
        CdnDomainSystem = 6,
        CmsDomain = 7,
        IsPms = 8,
        IsAccountSystem = 9,
        CustomerAccountSystemId = 10,
        EmployeeAccountSystemId = 11,
        DomainsAccept = 12,
        ArticleConnectionString = 100,
        CdnDomainEmbed = 101,
        CdnDomainEmbedPublic = 102,
        CdnDomainVideoPublic = 103,
        CdnDomainVideoPrivate = 104,
        CdnDomainPublic = 105,
        CdnDomainPublicNotCopy = 106,
        ImageSizeLimit = 110,
        FileMaxWith = 111,
        FileMinSize = 112,
        FileMinWidth = 113
    }

    [Flags]
    public enum LeagueStatusEnum
    {
        New = 0,
        Active = 1,
        DisplayInSportHomePage = 2,
        DisplayInMenuSportHomePage = 4,
        DisplayInSchedulePage = 8,
    }

    [Flags]
    public enum SeasonStatusEnum
    {
        New = 0,
        Active = 1,
        DisplayInSportHomePage = 2,
        DisplayInSchedulePage = 8,
        DisplayInCompetitionResultPage = 16,
        DisplayInStandingPage = 32,
    }

    [Flags]
    public enum LeaguesDisplayEnum
    {
        [Display(Name = "Menu giải đấu")] DisplayInMenuSportHomePage = 1,
        [Display(Name = "Trang chủ dữ liệu")] DisplayInSportHomePage = 2,
        [Display(Name = "Lịch thi đấu tổng")] DisplayInSchedulePage = 4,
        [Display(Name = "Kết quả tổng")] DisplayInCompetitionResultPage = 8,
        [Display(Name = "Bảng xếp hạng tổng")] DisplayInStandingPage = 16,
    }

    public enum LeagueMenuTypeEnum
    {
        Schedule = 1,
        CompetitionResult = 2,
        TopScorer = 3,
        Home = 4,
        Standing = 5
    }

    public enum TemplateFootballTypeEnum
    {
        Schedule = 1,
        CompetitionResult = 2,
        Standing = 3,
        TopScorer = 4,
        HeadToHead = 5,
    }

    public enum ZoneObjectTypeEnum
    {
        Default = 0,
        EventSpecial = 1,
        Category = 2,
        Video = 3,
        Fee = 4,
        HomePage = 5,
        Event = 6,
        Series = 7,
        Province = 8,
        SpecialPosition = 9
    }

    public enum MobilePageEnum
    {
        HomePage = 1,
        Category = 2,
        VideoHomePage = 3,
        VideoCategory = 4,
        PremiumHomePage = 5
    }

    public enum ImageTypeEnum
    {
        ArticleAvatar = 1,
        ArticleContent = 2
    }

    public enum LockTypeEnum
    {
        ArticleEdit = 1
    }

    public enum ShortLinkEnum
    {
        SL301 = 1,
        Config = 2
    }

    public enum StatusFilterTypeEnum
    {
        All = 0,
        Active = 1,
        InActive = 2
    }

    public enum TemplateGroupTypeEnum
    {
        SpecialEventMenu = 1,
        SpecialEventArticleTop = 2,
        SpecialEventArticleList = 3,
        SpecialEventVideoList = 4,
        SpecialEventSub = 5,
        SpecialEventFootBall = 6,
        LayoutByArticle = 7,
        EMagazineEditor = 8,
        SpecialPosition = 9
    }

    [Flags]
    public enum MessageUserStateEnum
    {
        OffLine = 0,
        Available = 1,
        Busy = 2,
        DoNotDisturb = 4,
        BeRightBack = 8,
        AppearAway = 16,
        AppearOffline = 32
    }

    public enum ChannelTypeEnum
    {
        Company = 1,
        Department = 2,
        Group = 3,
        Thread = 4
    }

    public enum RoomTypeEnum
    {
        Article = 1,
        Chat = 2
    }

    public enum RoomMembersAddOptionEnum
    {
        IncludeMessage = 1
    }

    [Flags]
    public enum RoomMemberMappingOptionEnum : long
    {
        HeadOfDepartment = 1,
        DeputyHeadOfDepartment = 2,
        Member = 4
    }

    public enum MessageStateEnum
    {
        Sent = 1,
        Received = 2,
        Read = 3
    }

    public enum NewsActionStatusEnum
    {
        IsReturn = 1,
        IsUnPublish = 2
    }

    public enum NewsReturnReceiverEnum
    {
        Reporter = 1,
        Editor = 2
    }

    public enum KPISettingTypeEnum
    {
        Budget = 1,
        View = 2,
        ArticleType = 3,
        ArticleOption = 4,
        Media = 5,
        VideoSelfProduced = 6,
        VideoRecover = 7,
        RatioByView = 8
    }

    public enum KPIArticleOptionEnum
    {
        Quick = 1,
        Slow = 2,
        Quality = 3,
        Monopoly = 4
    }

    public enum KPIResultTypeEnum
    {
        NewsType = 1,
        NewsOption = 2,
        NewsImage = 3,
        Author = 4,
        View = 5,
        Video = 6,
        BudgetCategory = 7
    }

    public enum KPIArticleMappingTypeEnum
    {
        NewsAuthor = 1, // Tac Gia bai viet
        MarkedAuthor = 2, // Tac gia duoc cham KPI
        Project = 3, // Project cua bai viet
    }

    public enum KPIRewardFilterStatusEnum
    {
        All = 0,
        Pending = 1,
        Finished = 2
    }

    public enum KPIReportTypeEnum
    {
        Detail = 1,
        DetailAuthor = 2,
        DetailProject = 3,
        Author = 4,
        Category = 5,
        Budget = 6
    }

    public enum WikiSortFieldEnum
    {
        CreatedDate = 1,
        UpdatedDate = 2,
        PageView = 3,
    }

    public enum UrlTypeEnum
    {
        ArticleDetail = 1,
        VideoArticleDetail = 2,
        PremiumArticleDetail = 3,
        EnArticleDetail = 11,

        Tag = 4,
        EnTag = 5,
        Event = 6,
        VideoEvent = 7,

        Author = 8,
        Search = 9,
        VideoSearch = 10,
        WikiDetail = 12,
        FootballPlayer = 13,
        FootballTeam = 14,

        MappingCategory = 100,
        Missing = 101,
        VideoCategory = 102,


        PremiumCategory = 105,
        UrlRedirect = 106,
        UrlFullRedirect = 107,
        //UrlDownloadByProxy = 108,

        //        UrlMappingByRule = 109,
        Url404 = 110,

        //UrlDownloadByProxyAndQuery = 111,
        Pseudonym = 112,
        PremiumUser = 113,
        VideoUser = 114,
        VietNamNetUser = 115,
        CategoryPaging = 116,
    }

    [Flags]
    public enum KPIArticleStatusEnum
    {
        New = 1,
        HeadOfDepartmentFinish = 2,
        EditorialSecretaryFinish = 4,
        DeputyEditorInChiefFinish = 8,
        NewsChanged = 16,
    }

    public enum KPIArticleStatusTypeEnum
    {
        Waiting = 1,
        Edited = 2,
        Finished = 3
    }

    public enum KPIHistoryTypeEnum
    {
        Default = 0,
        HeadOfDepartmentMarkByIds = 1, // Trưởng chấm nhanh
        EditorialSecretaryMarkByIds = 2, // TKTS chấm nhanh
        DeputyEditorInChiefMarkByIds = 3, // Phó tổng chấm nhanh
        HeadOfDepartmentConfirm = 4, // Trưởng ban Chấm chi tiet
        EditorialSecretaryConfirm = 5, // TKTS cham chi tiet
        DeputyEditorInChiefConfirm = 6, // Pho Tong Chấm chi tiet
        PeriodAdd = 7, // Thêm kỳ
        PeriodChange = 8, // Sửa kỳ
        PeriodStatusChange = 9, // Thay đổi status kỳ
        RewardAdd = 10, // Thêm Thưởng phạt theo bài,
        SettingBudgetUpdate = 11, // Các màn setting giá cho kpi
        SettingArticleOptionAdd = 12,
        SettingArticleOptionChange = 13,
        SettingNewsTypeAdd = 14,
        SettingNewsTypeChange = 15,
        SettingMediaImageAddOrChange = 16,
        SettingVideoKpiChange = 17,
        SettingRemoveCondition = 18,
        SettingStatusChange = 19,
        SettingViewCategoriesChange = 20,
        SettingVideoKpiAdd = 21,
        SettingKpiRatioByViewChange = 22,
    }

    public enum CovidLocationTypeEnum
    {
        ByDate = 0,
        Internal = 1,
        World = 2,
        InternalToday = 3,
        WorldToday = 4,
        ByCountry = 5,
        WorldByDate = 6
    }

    public enum ZonePositionDisplayTypeEnum
    {
        Active = 1,
        Timer = 2,
        Pin = 3
    }

    public enum ZonePositionActionTypeEnum
    {
        Change = 1,
        Remove = 2,
    }

    public enum KPIResultUserTypeEnum
    {
        EditorialSecretary = 1,
        DeputyEditorInChief = 2,
        HeadOfDepartment = 3
    }

    public enum KPIResultStatusEnum
    {
        Deleted = -1,
        New = 0,
        Active = 1,
        Reject = 2,
    }

    public enum KPIPeriodStatusEnum
    {
        All = -1,
        New = 1,
        Active = 2,
        End = 3,
        DeActive = 4,
    }

    public enum IconConfigTypeEnum
    {
        Title = 1,
        Avatar = 2
    }

    public enum IconConfigAvatarPositionEnum
    {
        Default = 0,
        Image = 1,
        Video = 2,
        Podcast = 3,
        Chart = 4,
        Infographic = 5,
        PhotoStory = 6,
        Emagazine = 7
    }

    public static class EnumExtension
    {
        public static string ToTextByEnum<T>(this long value)
        {
            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .FirstOrDefault(v => (int)(object)v == value)
                .ToString();
        }

        public static string ToTextByEnum<T>(this int value)
        {
            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .FirstOrDefault(v => (int)(object)v == value)
                .ToString();
        }
    }

    public enum SeaGameCode
    {
        SeaGame31 = 31
    }

    public enum SeaGameMedalEnum
    {
        Gold = 1,
        Silver = 2,
        Bronze = 3
    }

    public enum CategoryLogoTypeEnum
    {
        ImageUrl = 1,
        IconUrl = 2
    }

    public enum SlugTypeEnum
    {
        Category = 1,
        Manufacturer = 2,
        Vendor = 3
    }

    [Flags]
    public enum VendorTypeEnum
    {
        [Display(Name = "Nhà cung cấp")] SupplierProduct = 1,
        [Display(Name = "Đại lý")] Merchant = 2,
        [Display(Name = "Chủ đầu tư")] EstateInvestor = 4,
        [Display(Name = "Đơn vị thiết kế")] EstateArchitect = 8,
        [Display(Name = "Đơn vị môi giới")] EstateBroker = 16,
        [Display(Name = "Công ty xây dựng")] EstateConstructor = 32
    }

    public enum VendorSubTypeEnum
    {
        [Display(Name = "Công ty")] Company = 1,
        [Display(Name = "Cá nhân")] Individual = 2
    }

    public enum VendorMappingObjectTypeEnum
    {
        Article = 1,
        Address = 2
    }

    public enum VendorMappingTypeEnum
    {
    }

    public enum VendorMediaTypeEnum
    {
        Image = 1,
        Video = 2
    }

    public enum CommissionStatusEnum
    {
        [Display(Name = "Duyệt")] Active = 1,
        [Display(Name = "Xóa")] Deleted = -1,
        [Display(Name = "Tạo mới")] New = 3,
        [Display(Name = "Thay đổi")] Change = 2,
        [Display(Name = "Ngừng bán")] UnSell = 4
    }

    public enum CommissionChangeStatusTypeEnum
    {
        New = 1,
        Active = 2,
        Stop = 3
    }

    public enum ProductGroupConfigTypeEnum
    {
        Category = 1,
        Manufacturer = 2,
        Vendor = 3,
        Warehouse = 4,
        Attribute = 5,
        Price = 6,
        Quantity = 7,
        Product = 8
    }

    public enum WarehouseTypeEnum
    {
        [Display(Name = "InHouse")] InHouse = 1,
        [Display(Name = "Vendor")] Vendor = 2
    }

    public enum WarehouseVendorInventoryChangeStatusTypeEnum
    {
        Approve = 1
    }

    public enum WarehouseWorkingTimeEnum
    {
        [Display(Name = "Bình thường")] Normal = 1,
        [Display(Name = "Nghỉ trưa")] Lunch = 2
    }

    public enum WarehouseHolidayTimeEnum
    {
        [Display(Name = "Cả ngày")] AllDay = 1,
        [Display(Name = "Theo ca")] Shift = 2
    }

    public enum ProductImeiStatusEnum
    {
        New = 1,
        Cancel = 2,
        Used = 3
    }

    public enum EducationGetByType
    {
        School = 1,
        Major = 2,
        Scores = 3
    }

    public enum CacheKey
    {
        [Display(Name = "Education.SchoolYear")]
        SchoolYear,
    }

    public enum ProductSortFieldEnum
    {
        [Display(Name = "Tên sản phẩm")] Name = 0,
        [Display(Name = "Ngày tạo")] CreateDate = 1,

        [Display(Name = "Ngày cập nhật giá thị trường")]
        SellPriceUpdatedDate = 2,
        [Display(Name = "Lượt xem")] PageView = 3,
        [Display(Name = "Vị trí")] Position = 4,
        [Display(Name = "Bình luận")] Comment = 5,
        [Display(Name = "Ngày phê duyệt")] ApprovedDate = 6,
        [Display(Name = "Giá niêm yết")] ListedPrice = 7,
        [Display(Name = "Giá thị trường")] SellPrice = 8,
        [Display(Name = "Tìm nhiều")] TopSearch72Hour = 9,
        [Display(Name = "Xem nhiều")] TopView1Month = 10,
        [Display(Name = "Đánh giá")] Rating = 11,
        [Display(Name = "Tên hãng xe")] ManufacturerName = 12,
        [Display(Name = "Thuộc tính")] Attribute = 13,
        [Display(Name = "Ngày cập nhật")] UpdatedDate = 14,
        [Display(Name = "Score")] Score = 15,
        [Display(Name = "Ngày xuất bản")] PublishDate = 16
    }

    public enum VendorSortFieldEnum
    {
        Name = 1,
        CreateDate = 2,
        UpdatedDate = 3,
        LastChange = 4,
        View = 5
    }

    public enum ProductAttributeStatusEnum
    {
        Deleted = -1,
        New = 1,
        Active = 2,
        InActive = 4,
        Display = 8,
    }

    public enum ProductSettingTypeEnum
    {
        Price = 1,
        CarRegistrationFeeByProvince = 2,
        Formula = 3,
        ConstructionCostFormula = 4
    }

    public enum ProductSettingPriceTypeEnum
    {
        //PHIDUONGBO, BAOHIEM, PHIDANGKIEMBIENSO, PHIDANGKIEM
        Const = 1,

        // gia niem yết
        ListedPrice = 2,

        //PHITRUOCBA
        CarRegistrationFeeByProvince = 3,

        //Chi phí
        Cost = 4,

        //Tổng số tiền cho vay
        TotalLoanAmount = 5,

        //Thời hạn cho vay tối đa
        MaxLoanTerm = 6,

        //Lãi suất
        InterestRate = 7,

        //Lãi suất tối đa
        MaxInterestRate = 8
    }

    [Flags]
    public enum ProductAttributeConfigOptionEnum : long
    {
        [Display(Name = "Năm sản xuất")] YearOfManufacture = 1,
        [Display(Name = "Mẫu xe")] CarSerial = 2,
        [Display(Name = "Kiểu dáng")] CarType = 4,
        [Display(Name = "Phân khúc")] CarSegment = 8,
        [Display(Name = "Hãng xe")] CarManufacturer = 16,

        [Display(Name = "Khoảng giá niêm yết")]
        CarListedPriceRange = 32,

        [Display(Name = "Khoảng giá thị trường")]
        CarSellPriceRange = 64,
        [Display(Name = "Màu sắc")] Color = 128,
        [Display(Name = "Phiên bản")] Version = 256,

        [Display(Name = "Phí đăng ký biển số")]
        CarLicensePlateRegistrationFee = 512,

        [Display(Name = "Phí trước bạ")] CarRegistrationFee = 1024,

        [Display(Name = "Phí bảo hiểm trách nhiệm dân sự")]
        CivilLiabilityInsurancePremium = 2048,
        [Display(Name = "Phí đường bộ")] CarRoadFees = 4096,
        [Display(Name = "Phí đăng kiểm")] CarRegistryFee = 8192,
        [Display(Name = "Mẫu xe và năm")] CarSerialAndYear = 16384,
        [Display(Name = "Loại bảng xếp hạng")] ProductZoneType = 32768,
        [Display(Name = "Tiện ích dự án")] EstateProjectUtility = 65536,
        [Display(Name = "Nội thất bàn giao")] EstateHandoverFurniture = 131072,
        [Display(Name = "Tiện ích khác")] EstateProjectUtilityOther = 262144,
        [Display(Name = "Loại bất động sản")] EstateProjectProductType = 524288,

        [Display(Name = "Phong cách thiết kế")]
        EstateArchitectureStyle = 1048576,
        [Display(Name = "Mức giá")] EstateSellPriceRange = 2097152,
        [Display(Name = "Diện tích")] EstateProductAreaRange = 4194304,
        [Display(Name = "Hướng")] EstateHouseOrientation = 8388608,
        [Display(Name = "Số phòng ngủ")] EstateNumberOfBedRoom = 16777216,
        [Display(Name = "Loại dự án")] EstateProjectType = 33554432,
        [Display(Name = "Tiến độ dự án")] EstateProjectProgressStatus = 67108864,
        [Display(Name = "Pháp lý")] EstateProjectLegal = 134217728,
        [Display(Name = "Tổng diện tích")] EstateProjectTotalArea = 268435456,
        [Display(Name = "Tổng số tòa")] EstateProjectTotalBlock = 536870912,
        [Display(Name = "Tổng số căn")] EstateProjectTotalRoom = 1073741824,
        [Display(Name = "Ngày khởi công")] EstateProjectBeginDate = 2147483648,
        [Display(Name = "Ngày hoàn thiện")] EstateProjectFinishDate = 4294967296,
        [Display(Name = "Giá bán thấp nhất")] EstateSellPriceFrom = 8589934592,
        [Display(Name = "Giá bán cao nhất")] EstateSellPriceTo = 17179869184,
        [Display(Name = "Giá")] EstateSellPrice = 34359738368,
        [Display(Name = "Diện tích (m2)")] EstateProductArea = 68719476736,

        [Display(Name = "Diện tích thông thủy (m2)")]
        EstateProductCarpetArea = 137438953472,
        [Display(Name = "Diện tích đất (m2)")] EstateProductLandArea = 274877906944
    }

    public enum TrackingTypeEnum
    {
        NewsView = 0,
        ProductView = 1,
        VendorView = 2,
        ManufacturerView = 3,
        ProductSearch = 4,
        AttributeValueView = 5,
        ProductComment = 6,
        ManufacturerComment = 7,
        VendorComment = 8,
        AttributeValueComment = 9,
        ProductRating = 10,
        ManufacturerRating = 11,
        VendorRating = 12,
        AttributeValueRating = 13
    }

    public enum TrackingServiceGroupEnum
    {
        News = 1,
        Product = 2
    }

    public enum WarehouseProductMappingConfigOptionEnum : long
    {
        FullBox = 1
    }

    public enum InventoryStatusEnum
    {
        New = 1,
        Approved = 2,
        Processing = 3,
        Done = 4,
        Cancel = 5,
        Remove = -1,
        Changed = 6,
    }

    public enum InventoryTypeEnum
    {
        In = 1,
        Out = 2,
        Inventory = 3
    }

    public enum ConditionOfProductEnum
    {
        Good = 1,
        Broken = 2,
        Unclear = 3
    }

    public enum ReasonTypeEnum
    {
        InventoryCancel = 1
    }

    public enum InventoryDetailStatusEnum
    {
        Active = 1,
        Remove = 2
    }

    public enum NewsCopyTypeEnum
    {
        Root = 0,
        [Display(Name = "Bài link ngoài")] Is301 = 1,
        [Display(Name = "Bài copy")] IsCopy = 2,
    }

    public enum LogCollectionEnum
    {
        Action = 1,
        TrackingComponent = 2,
        TrackingPage = 3
    }

    [Flags]
    public enum SourceSettingEnum
    {
        PlanEspecially = 1,
        IsClone = 2
    }

    public enum BonusTypeEnum
    {
        NewsTracking = 1
    }

    public enum NewsPodcastCoverTypeEnum
    {
        [Display(Name = "Cover tối")] Dark = 1,
        [Display(Name = "Cover sáng")] Light = 2,
        // Custom = 3,
    }

    [Flags]
    public enum PageSettings : long
    {
        IsResponsive = 1,
        KeyCacheValidQuerystring = 2,
        EditSettingEnable = 4
    }

    public enum NewsChangeAttribute
    {
        [Display(Name = "Tiêu đề")] Title = 1,
        [Display(Name = "Tiêu đề phụ")] Title2,
        [Display(Name = "Mô tả")] Description,
        [Display(Name = "Nội dung")] Content,
        [Display(Name = "Từ khóa")] Tags,
        [Display(Name = "Loại hiển thị")] DisplayType,
        [Display(Name = "Nguồn tin")] Source,
        [Display(Name = "Tin liên quan")] RelatedNews,
        [Display(Name = "Thông tin SEO")] SeoOptions,
        [Display(Name = "Giờ xuất bản")] PublishDate,
        [Display(Name = "Tin địa phương")] Province,
        [Display(Name = "Thông tin Facebook")] FacebookOptions,
        [Display(Name = "Vị trí theo vùng")] ZonePosition,
        [Display(Name = "Cài đặt khác")] NewsOptions = 14,
        [Display(Name = "Chuyên mục")] Category,
        [Display(Name = "Dự án")] Project,
        [Display(Name = "Loại tin bài")] NewsType,
        [Display(Name = "Wiki")] WikiArticle,
        [Display(Name = "Vị trí chuyên mục")] CategoryPosition,
        [Display(Name = "Video liên quan")] RelatedVideos,
        [Display(Name = "Ghi chú")] QuickNote,
        [Display(Name = "Trạng thái")] NewsStatus,
        [Display(Name = "Tuyến bài")] Series,
        [Display(Name = "Sự kiện")] Events,
        [Display(Name = "Media")] Medias,
        [Display(Name = "Tác giả")] AuthorOption,
        [Display(Name = "Icon")] Icon
    }

    public enum NotificationConditionTypeEnum
    {
        Department = 1,
        User = 2,
        NotificationGroup = 3,
    }

    public enum NotifyContentSourceEnum
    {
        Text = 1,
        Url = 2
    }

    [Flags]
    public enum NotifyOptionEnum
    {
        SendToFCM = 1,
    }

    public enum ProductAttributeValueTypeEnum
    {
        Text = 1,
        Color = 2,

        [Display(Name = "Phí đăng ký biển số")]
        CarLicensePlateRegistrationFee = 3,
        [Display(Name = "Phí trước bạ")] CarRegistrationFee = 4,

        [Display(Name = "Phí bảo hiểm trách nhiệm dân sự")]
        CivilLiabilityInsurancePremium = 5,
        [Display(Name = "Phí đường bộ")] CarRoadFees = 6,
        [Display(Name = "Phí đăng kiểm")] CarRegistryFee = 7,
        [Display(Name = "Product")] EstateProduct = 8,
        [Display(Name = "Vendor")] EstateVendor = 9
    }

    public enum GPTModels
    {
        [Display(Name = "VNN-openai-model-turbo-0301")]
        Gpt35,
        [Display(Name = "gpt-4-0314")] Gpt4,
    }

    public enum LotteryEnum
    {
        Lottery = 1,
        LuckyNumber = 2
    }

    public enum BiddingStatusEnum // Trạng thái gói thầu
    {
        [Display(Name = "Chưa phát hành")] Draft = 1,
        [Display(Name = "Đang phát hành")] Releasing,
        [Display(Name = "Đang xử lý")] Processing,
        [Display(Name = "Đã mở thầu")] Opened,
        [Display(Name = "Đã đóng thầu")] Closed,
        [Display(Name = "Đã hủy thầu")] Cancelled,

        [Display(Name = "Đã phê duyệt kết quả")]
        ResultApproved,

        [Display(Name = "Đã thông báo kết quả LCNT")]
        NotifiedResult,
        [Display(Name = "Không xác định")] Undefined,
    }

    public enum ManagerBiddingStatusEnum // Trạng thái xử lý gói thầu
    {
        [Display(Name = "Chưa xử lý")] Draft = 0,
        [Display(Name = "Đang xử lý")] Processing,
        [Display(Name = "Đã xử lý")] Completed,
    }

    public enum CrawlDetailStatusEnum // Trạng thái update chi tiết
    {
        [Display(Name = "Chưa xử lý")] Draft = 0,
        [Display(Name = "Đã xử lý")] Completed,

        [Display(Name = "Không lấy được chi tiết")]
        CannotProcess,
    }

    public enum ContractorSelectionMethodEnum // Hình thức lựa chọn nhà thầu
    {
        [Display(Name = "Chỉ định thầu")] DirectAppointment = 1,

        [Display(Name = "Chào hàng cạnh tranh")]
        CompetitiveBidding,
        [Display(Name = "Đấu thầu rộng rãi")] OpenBidding,
        [Display(Name = "Chỉ định đối tác")] DirectPartner,
        [Display(Name = "EPC")] EPC,

        [Display(Name = "Ký hợp đồng mở rộng")]
        ExtendedContract,

        [Display(Name = "Ký hợp đồng trực tiếp")]
        DirectContract,

        [Display(Name = "Lựa chọn đối tác chiến lược")]
        StrategicPartnerSelection,
        [Display(Name = "Mua sắm nhỏ lẻ")] SmallScaleProcurement,
        [Display(Name = "Mua sắm trực tiếp")] DirectProcurement,
        [Display(Name = "Tự thực hiện")] SelfPerformance,
        [Display(Name = "Đấu thầu hạn chế")] RestrictedBidding,

        [Display(Name = "Chào hàng cạnh tranh rút gọn")]
        ShortCompetitiveBidding,
    }

    public enum BiddingMethodEnum // Hình thức dự thầu
    {
        [Display(Name = "Qua mạng")] Online = 1,
        [Display(Name = "Không qua mạng")] Offline
    }

    public enum BiddingCrawlSourceEnum // Nguồn crawl đấu thầu
    {
        [Display(Name = "MuasamcongMpi")] MuasamcongMpi = 1,
        [Display(Name = "Viettel")] Viettel,
    }

    public enum BiddingNotifyStatus
    {
        [Display(Name = "Chưa gửi")] NotSent,
        [Display(Name = "Đã gửi")] Sent
    }

    public enum IndexToAIStatusEnum
    {
        NotIndex = 0,
        Complete = 1
    }

    public enum WebsiteCrawlerStatusEnum
    {
        Running = 1,
        Idle = 2,
        Starting = 3,
        Delay = 4
    }

    public enum WebsiteCrawlerFrequencyTypeEnum
    {
        Day = 1,
        Hour = 2,
        Minute = 3
    }

    public enum CrawlerPageTypeEnum
    {
        Home = 1,
        List = 2,
        Detail = 3,
        Other = 4
    }

    public enum MapMouseModeEnum
    {
        Dark = 1,
        Light = 2
    }

    public enum MapNotationPositionEnum
    {
        [Display(Name = "Left")] Left = 1,
        [Display(Name = "Center")] Center = 2,
        [Display(Name = "Right")] Right = 3
    }

    public enum MapNotationVerticalPositionEnum
    {
        [Display(Name = "Top")] Top = 1,
        [Display(Name = "Middle")] Middle = 2,
        [Display(Name = "Bottom")] Bottom = 3
    }

    public enum ArticleCompareActionEnum
    {
        Add = 1,
        Change = 2,
        Remove = 3,
        NotChange = 4
    }

    [Flags]
    public enum ArticleExtractorStatusEnum
    {
        New = 1,
        CrawlerChange = 2,
        //4
        //8
    }

    [Flags]
    public enum ElementAnchorStatusEnum
    {
        HostInvalid = 1,
        ProtocolInvalid = 2,
        Duplicate = 4,
        EmptyHref = 8
    }

    public enum CampaignFieldTypeEnum
    {
        Text = 1,
        Number = 2,
        Email = 3,
        PhoneNumber = 4,
        RangeNumber = 5,
        RichText = 6,
        SelectOne = 7,
        SelectMultiple = 8,
        Radio = 9,
        CheckBox = 10,
        CheckBoxMultiple = 11,
        Date = 12,
        DateRange = 13
    }

    public enum ProductSettingFloorTypeEnum
    {
        [Display(Name = "Hầm")] Ham = 1,
        [Display(Name = "Sàn trệt")] Tret,
        [Display(Name = "Lửng")] Lung,
        [Display(Name = "Lầu 1")] Lau1,
        [Display(Name = "Lầu 2")] Lau2,
        [Display(Name = "Lầu 3")] Lau3,
        [Display(Name = "Sân thượng")] SanThuong,
        [Display(Name = "Mái")] Mai,
        [Display(Name = "Ô thông tầng")] OThongTang,
        [Display(Name = "Móng")] Mong,
        [Display(Name = "Sân vườn")] SanVuon,
        [Display(Name = "Lầu 4")] Lau4,
        [Display(Name = "Lầu 5")] Lau5,
        [Display(Name = "Lầu 6")] Lau6,
        [Display(Name = "Lầu 7")] Lau7,
        [Display(Name = "Lầu 8")] Lau8,
        [Display(Name = "Lầu 9")] Lau9,
        [Display(Name = "Lầu 10")] Lau10,
        [Display(Name = "Lầu 11")] Lau11,
        [Display(Name = "Lầu 12")] Lau12,
        [Display(Name = "Lầu 13")] Lau13,
        [Display(Name = "Lầu 14")] Lau14,
        [Display(Name = "Lầu 15")] Lau15
    }

    public enum SeoBackLinkTypeEnum
    {
        Internal = 1,
        External = 2,
        Other = 3
    }

    public enum ProductInterestOfLoadTypeEnum
    {
        [Display(Name = "Dư nợ giảm dần")] LoanWithDecliningBalance = 1,
        [Display(Name = "Trả lãi chia đều")] LoanWithPayEqually,
    }

    public enum ArticlesSearchObjectTypeEnum
    {
        Product = 1,
        Vendor = 2
    }

    public enum RoleActionMappingAttributeTypeEnum
    {
        Department = 1,
        DepartmentAttribute = 2,
        NewsCategory = 3
    }

    public enum QRTypeEnum
    {
        Login = 1,
        VerifyTwoFactor = 2
    }

    public enum UserActivityTypeEnum
    {
        Article = 1
    }

    [Flags]
    public enum UserActivityStateEnum
    {
        Read = 1,
        Write = 2,
        HasModified = 4,
    }

    public enum MapColumnTypeEnum
    {
        Number = 1,
        Text = 2
    }

    public enum WebsiteDomainTypeEnum
    {
        Primary = 1,
        Api = 2,
        Account = 3,
        Comment = 4,
        CdnUpload = 5,
        Embed = 6,
        CssAndJsFile = 7,
        ImageAndVideoFile = 8
    }

    public enum AccessPermissionTypeEnum
    {
        All = 0,
        Read = 1,
        Write = 2
    }

    public enum AccessPermissionDomainEnum
    {
        [Display(Name = "Account")] Account = 1,
        [Display(Name = "Notification")] Notification = 2,
        [Display(Name = "Order")] Order = 3,
        [Display(Name = "Location")] Location = 4,
        [Display(Name = "File")] File = 5,
        [Display(Name = "News")] News = 6,
        [Display(Name = "Comment")] Comment = 7,
        [Display(Name = "Product")] Product = 8,
        [Display(Name = "System")] System = 9,
        [Display(Name = "CrawlNews")] CrawlNews = 10,
        [Display(Name = "Polls")] Polls = 11,
        [Display(Name = "Sports")] Sports = 12,
        [Display(Name = "Wiki")] Wiki = 13,
        [Display(Name = "UtilityInfo")] UtilityInfo = 14,
        [Display(Name = "AssistantNews")] AssistantNews = 15,
        [Display(Name = "Chart")] Chart = 16,
        [Display(Name = "Search")] Search = 17,
        [Display(Name = "Tracking")] Tracking = 18,
        [Display(Name = "GoogleHook")] GoogleHook = 19,
        [Display(Name = "ViettelAiHook")] ViettelAiHook = 20,
        [Display(Name = "DocumentComment")] DocumentComment = 21,
        [Display(Name = "Message")] Message = 22,
        [Display(Name = "KPI")] KPI = 23,
        [Display(Name = "Convert")] Convert = 24,
        [Display(Name = "Log")] Log = 25,
        [Display(Name = "Education")] Education = 26,
        [Display(Name = "Warehouse")] Warehouse = 27,
        [Display(Name = "Report")] Report = 28,
        [Display(Name = "Bidding")] Bidding = 29,
        [Display(Name = "Project")] Project = 30
    }

    public enum AccessPermissionModelEnum
    {
        [Display(Name = "Account.User")] AccountUser = 100,
        [Display(Name = "KPI.KPISetting")] KPISetting = 2300
    }

    public enum WebsiteConfigTypeEnum
    {
        Group = 1,
        Item = 2
    }

    public enum WebsiteConfigFieldTypeEnum
    {
        Text = 1,
        TextArea = 2,
        Image = 3,
        CheckBox = 4,
        Menu = 5
    }

    [Flags]
    public enum PlaywrightSettingEnum : long
    {
        ScrollToBottom = 1
    }

    public enum WebsiteInitOverviewActionTypeEnum
    {
        InitIfNotExist = 1,
        InitOverride = 2,
        InitOverrideAndRemove = 3,
    }

    public enum WebsiteInitOverviewTypeEnum
    {
        Page = 1,
        Component = 2,
        Config = 3,
        Active = 4
    }

    public enum WebsiteDataTypeEnum
    {
        All = 0,
        Website = 1,
        Zone = 2,
        Page = 3,
        Component = 4,
        Menu = 5,
        WebsiteConfig = 6,
    }

    public enum GoogleDriveActionEnum
    {
        Create = 1,
        Edit = 2,
        Move = 3,
        Rename = 4,
        Delete = 5,
        Restore = 6
    }

    public enum AgencyMediaTypeEnum
    {
        [Display(Name = "Ảnh")] Image = 1,
        [Display(Name = "Video")] Video = 2,
        [Display(Name = "Tài liệu")] Document = 3,
    }

    public enum AgencyRepresentativeGroupTypeEnum
    {
        [Display(Name = "BTV")] Btv = 1,
        [Display(Name = "Khách hàng")] Customer = 2,
    }

    public enum TrackingPageTypeEnum
    {
        [Display(Name = "Bài viết")] Article = 1,
        [Display(Name = "Chuyên mục")] Category = 2,
        [Display(Name = "Sự kiện")] Event = 3,
        [Display(Name = "Tuyến bài")] Series = 4,
        [Display(Name = "Chủ đề")] Tag = 5,
        [Display(Name = "Khác")] Other = 6
    }

    [Flags]
    public enum ZoneOptionEnum
    {
        VideoAuto = 1,
        EventAuto = 2,
        SeriesAuto = 4,
        PinSupport = 8,
    }

    public enum AssistantNewsPartnerEnum
    {
        VnnAI = 1,
        OpenAI = 2,
        CocCocAI = 3
    }

    public enum AssistantNewsPartnerParamEnum
    {
        url = 1,
        method = 2,
        authorization = 3,
        model = 4,
        system_content = 5,
        temperature = 6,
        max_tokens = 7,
        top_p = 8,
        frequency_penalty = 9,
        presence_penalty = 10,
        cache_minute = 11,
        cache_version = 12
    }

    public enum AssistantNewsWordTypeEnum
    {
        typos = 1,
        suggest = 2
    }

    public enum NewspaperTemplateTypeEnum
    {
        Color = 1,
        Album = 2,
        Document = 3,
        Page = 4,
        Font = 5
    }

    public enum NewspaperTemplateOptionEnum
    {
        Default = 1,
    }

    public enum NewsTypeEnum
    {
        Article = 1, // báo điện tử
        Newspaper = 2 // báo in
    }

    [Flags]
    public enum NewspaperPageOptionEnum : long
    {
        Lock = 1,
        DesignCompleted = 2,
        Publish = 4,
        Hidden = 8
    }

    public enum NewspaperPageExportFileTypeEnum
    {
        Image = 1,
        Pdf = 2
    }

    public enum NewspaperPagePageIndexTypeEnum
    {
        First = 1,
        Last = 2,
        Other = 3
    }

    public enum NewspaperTabTypeEnum
    {
        Type = 1,
        Type2 = 2
    }

    public enum NewspaperUnitEnum
    {
        Px = 1,
        Cm = 2
    }

    public enum ArticleCompareViolatorEnum
    {
        VietNamNet = 1,
        Partner = 2
    }

    // public enum MessageTypeEnum
    // {
    //     Sender = 1,
    //     Receiver = 2
    // }

    public enum HistoryStatusEnum
    {
        Unknown = -1,
        NoChange = 0,
        Changed = 1,
    }

    public enum HistoryItemFETypeEnum
    {
        ItemString = 1,
        ItemArray = 2,
        ItemOptions = 3,
        ItemObject = 4,
        ItemZonePosition = 5,
        ItemArticlePositions = 6,
        ItemNewsContent = 7,
        ItemNewsEvent = 8,
        ItemNewsSeries = 9,
        ItemChecked = 10,
        ItemArrayOptions = 11,
        ItemSwitch = 12,
        ItemHomeZonePosition = 13,
        ItemMessage = 14,
        ItemMessages = 15,
        ItemHtml = 16,
        ItemWikiAttribute = 17,
        ItemWikiAttributeHtml = 18,
        ItemWikiAttributeTimeLine = 19,
        ItemWikiAttributeGallery = 20,
        ItemWikiAttributeLocation = 21,
        ItemVoteStyle = 22,
        ItemVoteType = 23,
        ItemCampaignTemplate = 24,
        ItemCampaignDataRemove = 25,
        ItemVoteExtra = 26,
    }

    public enum NewsContentRichMediaTypeEnum
    {
        [Display(Name = "Bảng biểu")] Table = 1,
        [Display(Name = "Biểu đồ")] Chart = 2,
        [Display(Name = "Wiki")] Wiki = 3,
        [Display(Name = "Bản đồ")] Map = 4,

        [Display(Name = "Audio trong nội dung")]
        AudioInContent = 5,
        [Display(Name = "Podcast")] Podcast = 6,

        [Display(Name = "Video trong nội dung")]
        VideoInContent = 7,
        [Display(Name = "Video")] Video = 8,
        [Display(Name = "PDF")] Pdf = 9,
        [Display(Name = "Menu xem nhanh")] QuickMenu = 10,
        [Display(Name = "Ảnh panorama")] Panorama = 11
    }

    public enum DepartmentMappingOptionEnum : long
    {
        CanTickDisplayAsPseudonym = 1,
        CanEditPremiumSetting = 2,
        ShowPreviewSettingBox = 3,
        ChangeTitleLengthLimit = 4,

        // ChangeHashTagTopHeader = MenuPosition.HashTagTopHeader, //9
        // ChangeHashTagMenuHeaderVideo = MenuPosition.MenuHeaderVideo, //7
        // ChangeHashTagMenuLeftVideo = MenuPosition.MenuLeftVideo, //8,
        ReadArticleHistory = 11,
        ReadEventHistory = 12,
        ReadSeriesHistory = 13,
        ReadCategoryHistory = 14,
        ReadTagHistory = 15,
        ReadVoteHistory = 16,
        ReadSurveyHistory = 17,
        ReadCollectData = 18,
        ReadWikiArticleHistory = 19,
        //ChangeMenuGoodLink = MenuPosition.GoodLink, //GoodLink = 37
        //ChangeHashTagTopHeader2Sao = MenuPosition.MenuHashTagTopHeader2Sao, //36,
    }

    public enum NewsContentMappingTypeEnum
    {
        Video = 1,
        Image = 2,
        Wiki = 3,
        Article = 4,
        Audio = 5,
        Chart = 6,
        Map = 7,
        TableData = 8,
        Panorama = 9
    }

    public enum ArticleIntegrationStatusEnum
    {
        New = 1,
        Processing = 2,
        Success = 4,
        Fail = 8,
    }

    public enum NewsContentTarotDisplayTypeEnum
    {
        Stack = 1,
        StackInTurn = 2
    }

    [Flags]
    public enum NewsContentTarotOptionEnum
    {
        ShareFrontImage = 1,
    }

    [Flags]
    public enum NewsTagMappingOptionEnum
    {
        Special = 1
    }

    #endregion

    #region EventBus


    public enum EventStatusEnum
    {
        New = 0,
        Success = 1,
        Fail = -1,
        Retry = 2
    }

    public enum KeyCacheTypeEnum
    {
        Component = 1,
        NewsCategory = 2,
        News = 3,
        Event = 4,
        MenuPosition = 6,
        NickName = 7,
        Series = 8,
        Tag = 9,
        Wiki = 10,
        Zone = 11,
        Fixture = 12,
        League = 13,
        Standing = 14,
        Season = 15,
        Player = 16,
        Team = 17,
        PlayerStatistic = 18,
        Sitemap = 19,
        Rss = 20,
        Chart = 21,
        Video = 22,
        Website = 23,
        ProductCategory = 25,
        ProductManufacturer = 26,
        ProductAttributeValueCostSetting = 27,
        ProductAttributeCostSetting = 28,
        AttributeCategoryMappingsLoad = 29,

        ProductAttributeValueHasOption = 30,
        ProductAttributeHasOption = 31,
    }

    #endregion

    public enum ShardingTypeEnum
    {
        Account = 1,
        Dealer = 2
    }

    public enum ShardingMappingTypeEnum
    {
        Account = 1,
        Dealer = 2
    }

    public enum EncryptTypeEnum
    {
        Default = 1,
    }

    public enum UserDealerMappingTypeEnum
    {
        Service = 1,
        Sale = 2
    }
}
