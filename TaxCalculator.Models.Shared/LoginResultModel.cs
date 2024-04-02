namespace TaxCalculator.Models.Shared
{
    public class LoginResultModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
