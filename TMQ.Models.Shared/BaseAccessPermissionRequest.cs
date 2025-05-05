namespace TMQ.Models.Shared
{
    public class BaseAccessPermissionRequest
    {
        public string ObjectId { get; set; }
        public string[] ObjectIds { get; set; }
        public int ObjectDomain { get; set; }
        public int ObjectModel { get; set; }
    }
}
