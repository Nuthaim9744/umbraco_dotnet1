





using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using zudioclone.models.Models.Viewmodels;
using zudioclone.Core.Services.Interface;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Security;

namespace zudioclone.Core.Controllers
{
    public class AccountController : SurfaceController
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IAccountService accountService,
            ILogger<AccountController> logger)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _accountService = accountService;
            _logger = logger;
        }




        // Show Register Page
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            ViewBag.RegisterModel = new RegisterViewModel();

            return CurrentUmbracoPage();
        }

        // Handle Registration
        [HttpPost]
        [ValidateAntiForgeryToken]



        public async Task<IActionResult> HandleRegisterMember(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return View("Register", model);
                //return BadRequest(ModelState);

                //return CurrentUmbracoPage();
                return CurrentUmbracoPage();

            }



            try
            {
                var result = await _accountService.RegisterMemberAsync(model);

                if (result.Succeeded)
                {
                    return Redirect("/login");
                }
                //return BadRequest(result.Errors);
                else
                {
                    ModelState.AddModelError(string.Empty, "registration attempt failed");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


                //return View("Register", model);
            }

            catch (Exception ex)
            {
                // Log the exception details (assuming you have a logging mechanism)
                _logger.LogError(ex, "An error occurred while registering the member.");

                // Add a generic error message to the ModelState
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");

            }



            return CurrentUmbracoPage();
        }




        // Show Login Page
        [HttpGet]
        public IActionResult Login()
        {
            var model=new LoginViewModel();

            //return View(); 

            return CurrentUmbracoPage();
        }

        // Handle Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleLoginMember(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return View("Login", model);

                return CurrentUmbracoPage();
            }
            try
            {

                var result = await _accountService.LoginMemberAsync(model);

                if (result.Succeeded)
                {
                    return Redirect("/");
                }

            }
            catch (Exception ex)
            {

                ex.Message.ToString();
                throw;

            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //return View("Login", model);

            return CurrentUmbracoPage();
        }




        // Handle Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutMemberAsync();
            return Redirect("/");
        }
    }
}





