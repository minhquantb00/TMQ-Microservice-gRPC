namespace TMQ.BaseApplication.Models
{
    public record GoogleReCaptChaResponse
    {
        public bool? success { get; set; }
        public DateTime? challenge_ts { get; set; }
        public decimal? score { get; set; }
        public string? hostname { get; set; }
        public string? action { get; set; }
    }
}
