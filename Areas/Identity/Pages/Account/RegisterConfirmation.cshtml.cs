//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : RegisterConfirmation.cshtml.cs   　　　                     *
//* Function     : Confirmation Account                                       *
//* Create       : VietAnh 2024/01/14                                         *
//*****************************************************************************
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Hygge.Data;

namespace Hygge.Areas.Identity.Pages.Account
{
    /// <summary> The model for register confirm </summary>
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        #region properties
        /// <summary> Manages user account </summary>
        private readonly UserManager<HyggeUser> _userManager;
        /// <summary> Manages email (Currently not in use) </summary>
        private readonly IEmailSender _sender;

        /// <summary> Email Account </summary>
        public string Email { get; set; }

        /// <summary>   Can display confirm screen? (true : can, false:can't  </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>  The email confirm url  </summary>
        public string EmailConfirmationUrl { get; set; }
        #endregion

        #region functions
        /// <summary>
        /// Init function
        /// </summary>
        /// <param name="userManager">Manage account</param>
        /// <param name="sender">Manage Email (Currently not in use)</param>
        public RegisterConfirmationModel(UserManager<HyggeUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }
        /// <summary>
        /// The event when get the view
        /// </summary>
        /// <param name="email">The email account Input</param>
        /// <param name="returnUrl">The url which can return</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = false;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
        #endregion
    }
}
