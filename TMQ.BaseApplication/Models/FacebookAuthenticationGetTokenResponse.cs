namespace TMQ.BaseApplication.Models
{
    public record FacebookAuthenticationGetTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
    public class FacebookAuthenticationAccountInfoResponse
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string name_format { get; set; }
        public string short_name { get; set; }
        public string email { get; set; }
    }
}
