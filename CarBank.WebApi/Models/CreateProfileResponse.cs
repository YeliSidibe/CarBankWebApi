using Microsoft.AspNetCore.Identity;

namespace CarBank.WebApi.Models
{
    public class CreateProfileResponse
    {
        public ApplicationUser User             { get; set; }
        public long? CustomerId                 { get; set; }
        public string ConfirmEmailCallBackUrl   { get; set; }
    }

    public class LoginResponse
    {
        public SignInResult Result { get; set; }
        public string Jwt          { get; set; }
    }
}
