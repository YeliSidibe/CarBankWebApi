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
        public AuthSignResult Result { get; set; }
        public string Jwt          { get; set; }
    }

    public class AuthSignResult 
    {
        //
        // Summary:
        //     Returns a flag indication whether the sign-in was successful.
        public bool Succeeded { get; set; }
        //
        // Summary:
        //     Returns a flag indication whether the user attempting to sign-in is locked out.
        public bool IsLockedOut { get; set; }
        //
        // Summary:
        //     Returns a flag indication whether the user attempting to sign-in is not allowed
        //     to sign-in.
        public bool IsNotAllowed { get; set; }
        //
        // Summary:
        //     Returns a flag indication whether the user attempting to sign-in requires two
        //     factor authentication.
        public bool RequiresTwoFactor { get; set; }
    }
}
