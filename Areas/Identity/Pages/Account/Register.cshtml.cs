// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Hygge.Data;
using Hygge.Service;
namespace Hygge.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<HyggeUser> _signInManager;
        private readonly UserManager<HyggeUser> _userManager;
        private readonly IUserStore<HyggeUser> _userStore;
        private readonly IUserEmailStore<HyggeUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IConfiguration Configuration;

        public RegisterModel( UserManager<HyggeUser> userManager,  IUserStore<HyggeUser> userStore, SignInManager<HyggeUser> signInManager,  ILogger<RegisterModel> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            Configuration = configuration;

        }

        /// <summary>
        ///    
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///   
        /// </summary>
        public class InputModel
        {
			/// <summary>
			///  
			/// </summary>
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			/// <summary>
			///  
			/// </summary>
			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			/// <summary>
			///   
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			/// <summary>Birthdate of user</summary>
			[DataType(DataType.Date)]
			[Required(ErrorMessage = "Required")]
			[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
			public DateTime? BirthDate { get; set; }

			/// <summary>The phone of user</summary>
			[Required(ErrorMessage = "Required")]
			[RegularExpression(@"^\d{10,}$", ErrorMessage = "10桁以上入力してください")]
			[StringLength(15, MinimumLength = 10, ErrorMessage = "10 ～ 15 桁")]
			public required string Phone { get; set; }

			[StringLength(60, MinimumLength = 1)]
			[Required(ErrorMessage = "Required")]
			public string Name { get; set; }
		}


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
				user.BirthDate = Input.BirthDate;
				user.Name = Input.Name;
				user.Phone = Input.Phone;
				user.CreateDate = DateTime.Now;
				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                   var emailService = new EmailService(Configuration);
                    var isSuccess = emailService.SendMailRegister(Input.Email,
                        $"{HtmlEncoder.Default.Encode(callbackUrl)}");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private HyggeUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<HyggeUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(HyggeUser)}'. " +
                    $"Ensure that '{nameof(HyggeUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<HyggeUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<HyggeUser>)_userStore;
        }
    }
}
