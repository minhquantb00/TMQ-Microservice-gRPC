using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.EnumDefine
{
    [Flags]
    public enum FileSystemTypeEnum
    {
        AllImage = 0,
        Folder = 1,
        Image = 2,
        Video = 4,
        Pdf = 8,
        Gif = 16,
        Audio = 32,
    }

    [Flags]
    public enum FileTypeEnum
    {
        Undefined = 0,
        Image = 1,
        Video = 2,
        Document = 16,
        Other = 32
    }

    public enum FileLicenseEnum
    {
        All = 0,
        Normal = 1,
        License = 2
    }

    public enum FileSystemFolderEnum
    {
        All = 0,
        Image = 1,
        Video = 2,
        Audio = 3,
        GoogleDrive = 4
    }

    public enum PageSourceType
    {
        All = 0,
        Article = 1,
        Product = 2,
        ProductManufacturer = 3,
        ProductVendor = 4,
        ProductAttributeValue = 5,
        ProductEstate = 6
    }

    public enum FileStyleEnum
    {
        All = 0,
        Normal = 1,
        Watermark = 2
    }



    [Flags]
    public enum VideoStatusEnum
    {
        New = 1,
        Processing = 2,
        Success = 4,
        Fail = 8,
        Delete = 16
    }

    public enum AudioTypeEnum
    {
        NuMienBac = 1,
        NuMienNam = 2,
        NamMienBac = 3,
        NamMienNam = 4,
        NamMienTrung = 5,
        NuMienTrung = 6,
    }

    [Flags]
    public enum ImageGifStatusEnum
    {
        New = 1,
        Processing = 2,
        Success = 4,
        Fail = 8,
        Delete = 16
    }

    public enum ImageSettingEnum
    {
        PersonName = 1,
        Address = 2,
        Event = 3,
        Note = 4
    }

    public enum FileDescriptionEnum
    {
        Folder = 1,
        Image = 2
    }

    public enum GoogleFileChecksumEnum
    {
        Md5 = 1,
        Sha1 = 2,
        Sha256 = 3,
    }

    public enum WatermarkPositionEnum
    {
        TopLeft = 11,
        LeftCenter = 21,
        BottomLeft = 31,
        TopCenter = 12,
        Center = 22,
        BottomCenter = 32,
        TopRight = 13,
        CenterRight = 23,
        BottomRight = 33
    }

    public enum FileDownloadTypeEnum
    {
        Image = 1,
        Pdf = 2,
        Zip = 3,
        Rar = 4
    }

    public enum FileDownloadSourceTypeEnum
    {
        Newspaper = 1,
    }

    [Flags]
    public enum FileDownloadSourceOptionEnum : long
    {
        Default = 0,
        Encryption = 1,
        Md5Checksum = 2,
        SHA1Checksum = 4,
        SHA2Checksum = 8,
        SHA2512Checksum = 16,
    }

    public enum ImageKeywordTypeEnum
    {
        All = 0,
        Person = 1,
        Address = 2,
        Event = 3,
        Note = 4,
        Name = 5,
    }

    public enum ESSortTypeEnum
    {
        [Display(Name = "Tăng dần")] Asc = 1,
        [Display(Name = "Giảm dần")] Desc = 2
    }
}
