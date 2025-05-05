using TMQ.EnumDefine;

namespace TMQ.BaseApplication.Models
{
    public class FileModel
    {
        public string? Id { get; set; }
        public string? FullUrl { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? FolderId { get; set; }
        public string? HostName { get; set; }
        public FileTypeEnum? Type { get; set; }
        public long? Size { get; set; }
        public string? Extension { get; set; }
        public string? ContentType { get; set; }
    }
}
