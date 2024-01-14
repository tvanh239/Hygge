//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : Login.cshtml.cs   　　　                        　          *
//* Function     : Login Account                                              *
//* Create       : VietAnh 2024/01/14                                         *
//*****************************************************************************.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Hygge.Data;

namespace Hygge.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        #region Define other class
        /// <summary> The input data class </summary>
        public class InputModel
        {
            #region properties
            /// <summary>Email Address </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary> Password  </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary> checkbox if you want save account in browser </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            #endregion
        }
        #endregion
        #region properties
        /// <summary> Manage sign in  </summary>
        private readonly SignInManager<HyggeUser> _signInManager;
        /// <summary> Log  </summary>
        private readonly ILogger<LoginModel> _logger;
        /// <summary> Input Data  </summary>
        [BindProperty]
        public InputModel Input { get; set; }
        /// <summary> Other Login </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        /// <summary> The url after action </summary>
        public string ReturnUrl { get; set; }
        /// <summary>    Error Message (Invaild, Wrong Password) </summary>
        [TempData]
        public string ErrorMessage { get; set; }
        #endregion
        #region functions
        /// <summary>
        /// Init function
        /// </summary>
        /// <param name="signInManager">manage sign in</param>
        /// <param name="logger">Log</param>
        public LoginModel(SignInManager<HyggeUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        /// <summary>
        /// When get the view
        /// </summary>
        /// <param name="returnUrl">The url which can return</param>
        /// <returns></returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        /// <summary>
        /// When click button login, event function
        /// </summary>
        /// <param name="returnUrl">The url which returns</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        #endregion
    }
}
