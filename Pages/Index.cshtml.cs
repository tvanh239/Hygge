using Hygge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hygge.Pages
{
	public class IndexModel : PageModel
    {
        #region properties
        /// <summary> Manage sign in  </summary>
        private readonly SignInManager<HyggeUser> _signInManager;
        /// <summary>Manages user in a persistence store</summary>
        private readonly UserManager<HyggeUser> _userManager;
        /// <summary>The avatar of the user</summary>
        public string _url_img {  get; set; }
        
        #endregion
        public IndexModel(SignInManager<HyggeUser> signInManager, UserManager<HyggeUser> userManager)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _url_img = String.Empty;

        }

		public void  OnGet()
		{

        }
	}
}
