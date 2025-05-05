namespace TMQ.BaseApplication.Models
{
    public class ZaloAuthenticationResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }

    public record ZaloInfoResponse(
        bool is_sensitive,
        string name,
        string id,
        int error,
        string message,
        Picture picture
    );

    public record Picture(
        Data data
    );

    public record Data(
        string url
    );

}
