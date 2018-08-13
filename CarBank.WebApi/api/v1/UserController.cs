using CarBank.WebApi.Helpers;
using CarBank.WebApi.Models;
using CarBank.WebApi.Models.AccountViewModels;
using CarBank.WebApi.Services;
using CarBank.WebApi.Services.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBank.WebApi.api.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;
        private readonly IJwtFactory _jwtFactory;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<UserController> logger,
            ICustomerService customerService,
            IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _customerService = customerService;
            _jwtFactory = jwtFactory;
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("login")]
        public async Task<AuthenticationResponse<LoginResponse>> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var _claimIdentityHelper = new ClaimIdentityHelper();
                    var identity = await _claimIdentityHelper.GetClaimsIdentity(model.Email, model.Password,true, _jwtFactory, _userManager);
                    string jwt = string.Empty;
                    if (identity != null)
                    {
                        jwt = await TokenGenerator.GenerateJwt(identity, _jwtFactory, model.Email, new Services.JWT.JwtIssuerOptions(), new JsonSerializerSettings { Formatting = Formatting.Indented });

                    }
                    return new AuthenticationResponse<LoginResponse>() { StatusCode = System.Net.HttpStatusCode.OK, Value = new LoginResponse() { Result = result,Jwt = jwt} };
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new AuthenticationResponse<LoginResponse>() { StatusCode = System.Net.HttpStatusCode.OK, Errors = new List<string>() { "Account is Locked" }, Value = new LoginResponse() { Result = new Microsoft.AspNetCore.Identity.SignInResult() } };
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt.");
                    return new AuthenticationResponse<LoginResponse>() { StatusCode = System.Net.HttpStatusCode.OK, Errors = new List<string>() { "Invalid login attempt." }, Value = new LoginResponse() { Result = new Microsoft.AspNetCore.Identity.SignInResult() } };
                }
            }

            _logger.LogWarning("Model State validation failed");
            return new AuthenticationResponse<LoginResponse>() { StatusCode = System.Net.HttpStatusCode.OK, Errors = new List<string>() { "Please provide valid credentials, Inavlid login!" }, Value = new LoginResponse() { Result = new Microsoft.AspNetCore.Identity.SignInResult() } };
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("register")]
        public async Task<AuthenticationResponse<CreateProfileResponse>> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                long? customerId = 0; string callbackUrl = string.Empty;
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    // Create customer
                    var customer = new Customer() { FirstName = model.FirstName, MiddleName = model.MiddleName, LastName = model.LastName, IdentityId = user.Id };
                    customerId = await _customerService.CreateCustomer(customer);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                }

                return new AuthenticationResponse<CreateProfileResponse>() { StatusCode = System.Net.HttpStatusCode.OK, Value = new CreateProfileResponse() { User = user, CustomerId = customerId, ConfirmEmailCallBackUrl = callbackUrl }, Errors = result.Errors?.Select(e => e.Description)?.ToList() };

            }
            // If we got this far, validation failed
            return new AuthenticationResponse<CreateProfileResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, Errors = new List<string>() { "An error occured !!!" } };
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) { return Unauthorized(); }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return Ok(result.Succeeded ? "ConfirmEmail" : "Error");
            // Ideally we would want to redirect to login page after the user clicks on link sent to Email Address
        }
    }
}