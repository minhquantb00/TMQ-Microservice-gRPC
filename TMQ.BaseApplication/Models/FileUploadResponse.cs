using TMQ.BaseReadModels;

namespace TMQ.BaseApplication.Models
{
    public record FileUploadResponse : BaseResponse
    {
        public string FullUrl => $"{HostName}/{Path}";
        public string FilePath => $"{Path}";
        public string RootPath { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string? FolderId { get; set; }
        public string HostName { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string TypeFile { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string OriginFilePath { get; set; }
        public string OriginFullUrl => $"{HostName}/{OriginFilePath}";
    }

    public class DeleteFileRequest
    {
        public string FileUrl { get; set; }
    }
}
