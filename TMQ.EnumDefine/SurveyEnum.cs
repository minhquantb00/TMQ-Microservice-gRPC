using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.EnumDefine
{
    public enum SurveyEnum
    {

    }

    public enum FlexibleEnum
    {
        Yes = 1,
        No = 0
    }

    public enum TicketRequestTypeEnum
    {
        [Display(Name = "Quản lý ý kiến thắc mắc của khách hàng")]
        Inquiry = 1,
        [Display(Name = "Quản lý ý kiến không hài lòng/khiếu nại của khách hàng")]
        Complaint = 2
    }

    public enum TicketRequestMappingTypeEnum
    {
        Reply = 1,
        Attachment = 2,
    }

    public enum TicketRequestCategoryEnum
    {
        ProductConsultation = 1,        // Tư vấn sản phẩm
        QualityComplaint = 2,           // Phản ánh chất lượng sản phẩm, bán hàng, dịch vụ
        AppInquiry = 3,                 // Thắc mắc về ứng dụng
        OtherTicketRequest = 4               // Góp ý và thắc mắc khác
    }

    public enum SurveyMediaTypeEnum
    {
        Image = 1,
        Video = 2,
    }

    public enum TicketMediaTypeEnum
    {
        Image = 1,
        Video = 2,
    }

    public enum TicketRequestStatusEnum
    {
        Deleted = -1,
        Pending = 1,
        Processing = 2,
        Finished = 3,
    }

    public enum SurveyCampaignTypeEnum
    {
        Standard = 1,
        Loop = 2
    }

    public enum SurveyCampaignStatusEnum
    {
        Deleted = -1,
        Active = 1,
        New = 2,
        Paused = 3,
        Completed = 4,
    }

    public enum SurveyServiceTypeEnum
    {
        Buy = 1,
        Repair = 2,
        DrivingTest = 3
    }

    public enum SurveyTargetTypeEnum
    {
        OneTimeAll = 1, // Toàn bộ user trên hệ thống
        ServiceEvent = 2, // sau mua hàng, lái thử, bảo dưỡng ....
        Group = 3,
        User = 4
    }

    public enum SurveyGroupTypeEnum
    {
        Normal = 1,
        Vip = 2,
        Potential = 3,
        Other = 4
    }

    public enum SurveyQuestionTypeEnum
    {
        SingleChoice = 1,      // Câu hỏi trắc nghiệm chọn một
        MultipleChoice = 2,    // Câu hỏi trắc nghiệm nhiều lựa chọn
        Text = 3,              // Câu hỏi mở (nhập text)
        YesNo = 4,             // Câu hỏi Có/Không
        Rating = 5,            // Câu hỏi đánh giá theo thang điểm (1-5, 1-10)
        Ranking = 6,           // Câu hỏi sắp xếp ưu tiên
        Matrix = 7,            // Câu hỏi ma trận (đánh giá nhiều tiêu chí)
        Date = 8,              // Câu hỏi chọn ngày tháng
        Numeric = 9            // Câu hỏi nhập số
    }
}
