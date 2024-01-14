//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : Register.cshtml.cs   　　　                        　        *
//* Function     : Register Account                                            *
//* Create       : VietAnh 2024/01/14                                          *
//*****************************************************************************
#nullable disable


using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Hygge.Data;
using Hygge.Service;
namespace Hygge.Areas.Identity.Pages.Account
{
    
    /// <summary> Register Account </summary>
    public class RegisterModel : PageModel
    {
        #region Define Class
        /// <summary>  The Input Class </summary>
        public class InputModel
        {
            #region properties
            /// <summary> The input email  </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>The password of input</summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>Confirm password of input</summary>
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
            [RegularExpression(@"^\d{10,}$", ErrorMessage = "Please enter at least 10 digits")]
            [StringLength(15, MinimumLength = 10, ErrorMessage = "10-15 digits")]
            public required string Phone { get; set; }
            /// <summary>The name of user</summary>
            [StringLength(60, MinimumLength = 1)]
            [Required(ErrorMessage = "Required")]
            public string Name { get; set; }
            #endregion
        }
        #endregion

        #region properties
        /// <summary>Manages user in sign in</summary>
        private readonly SignInManager<HyggeUser> _signInManager;
        /// <summary>Manages user in a persistence store</summary>
        private readonly UserManager<HyggeUser> _userManager;
        /// <summary>The management of the user account</summary>
        private readonly IUserStore<HyggeUser> _userStore;
        /// <summary>The management of the user email address </summary>
        private readonly IUserEmailStore<HyggeUser> _emailStore;
        /// <summary>Log </summary>
        private readonly ILogger<RegisterModel> _logger;
        /// <summary>The app setting </summary>
        private readonly IConfiguration Configuration;
        /// <summary> Input in view </summary>
        [BindProperty]
        public InputModel Input { get; set; }
        /// <summary> The url which can return after action </summary>
        public string ReturnUrl { get; set; }
        /// <summary>  The other Login   </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        #endregion

        #region functions
        /// <summary>
        /// Initialization function
        /// </summary>
        /// <param name="userManager">Manages user in sign in</param>
        /// <param name="userStore">Manages user in a persistence store</param>
        /// <param name="signInManager">The management of the user account</param>
        /// <param name="logger">Log</param>
        /// <param name="configuration">The app setting</param>
        public RegisterModel(UserManager<HyggeUser> userManager, IUserStore<HyggeUser> userStore, SignInManager<HyggeUser> signInManager, ILogger<RegisterModel> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            Configuration = configuration;

        }
        /// <summary>
        /// When get view
        /// </summary>
        /// <param name="returnUrl">The url after action</param>
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
        /// <summary>
        /// Crete user
        /// </summary>
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
        /// <summary>
        /// Get Email Address
        /// </summary>
        private IUserEmailStore<HyggeUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<HyggeUser>)_userStore;
        }
        #endregion
    }
}
