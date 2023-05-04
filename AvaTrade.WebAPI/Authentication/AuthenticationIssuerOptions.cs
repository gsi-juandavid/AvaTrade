namespace AvaTrade.WebAPI.Authentication
{
    public class AuthenticationIssuerOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
